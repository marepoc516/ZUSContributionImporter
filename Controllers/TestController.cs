using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using KeesTalksTech.Utilities.Latin.Numerals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZUSContributionImporter.Common;
using ZUSContributionImporter.Contributions;
using Dapper;

namespace ZUSContributionImporter.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IConfigurationRoot _config;
        private const int headerRowsNumber = 5;
        private const int footerRowsNumber = 4;

        private IDbConnection Connection
        {
            get => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public TestController(IConfigurationRoot config)
        {
            _config = config;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<AddContributionCommand> contributions = new List<AddContributionCommand>();

            string url = _config.GetValue<string>("Urls:ZusHistoricalContributionsUrl");
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDocument = web.Load(url);

            HtmlNodeCollection tables = htmlDocument.DocumentNode.SelectNodes("//table");
            HtmlNode contributionTable = tables.Count > 1 ? tables[1] : null;
            if (contributionTable != null)
            {
                HtmlNodeCollection rows = contributionTable.SelectNodes("//tbody/tr");
                if (rows != null && rows.Count() > 0)
                {
                    var lastAddedYear = 1900;
                    foreach (HtmlNode row in rows.Skip(headerRowsNumber).Take(rows.Count() - (headerRowsNumber + footerRowsNumber)))
                    {
                        AddContributionCommand contribution = null;
                        if (TryMapZusContribution(row, lastAddedYear, out contribution))
                        {
                            lastAddedYear = contribution.ValidityBeginDate.Year;
                            contributions.Add(contribution);
                        }
                    }
                }
            }

            // using(var conn = GetOpenConnection())
            // using(var tran = conn.BeginTransaction()) {
            // https://stackoverflow.com/questions/16903289/using-for-idbconnection-idbtransaction-safe-to-use
            using (IDbConnection connection = Connection)
            using (IDbTransaction transaction = connection.BeginTransaction())
            {
                // ?????
                // wstawić o ile nie istnieje  
                var lastSynchronizationValidityEndDate = GetLastSynchronizationValidityEndDate();
                foreach (AddContributionCommand contribution in contributions.Where(contribution => !lastSynchronizationValidityEndDate.HasValue || contribution.ValidityEndDate > lastSynchronizationValidityEndDate))
                {
                    // InsertZusContribution(contribution, transaction);
                    //InsertSynchronization(dupa, transaction);
                }
            }

            return new string[] { "value1", "value2" };
        }

        private void InsertZusContribution(AddContributionCommand contribution, IDbTransaction transaction)
        {
            using (IDbConnection connection = Connection)
            {
                string query = @"INSERT INTO [dbo].[Contributions]
                                            ([ValidityBeginDate]
                                            ,[ValidityEndDate]
                                            ,[SocialInsuranceBasisOfContributionDimension]
                                            ,[PensionInsuranceCost]
                                            ,[DisabilityInsuranceCost]
                                            ,[MedicalInsuranceCost]
                                            ,[AccidentInsuranceCost]
                                            ,[LaborFundBasisCost]
                                            ,[HealthInsuranceBasisOfContributionDimension]
                                            ,[HealthInsuranceCost]
                                            ,[HealthInsuranceTaxDeductible]
                                            ,[TotalContribution])
                                        VALUES
                                        (@ValidityBeginDate,
                                        @ValidityEndDate,
                                        @SocialInsuranceBasisOfContributionDimension,
                                        @PensionInsuranceCost,
                                        @DisabilityInsuranceCost,
                                        @MedicalInsuranceCost,
                                        @AccidentInsuranceCost,
                                        @LaborFundBasisCost,
                                        @HealthInsuranceBasisOfContributionDimension,
                                        @HealthInsuranceCost,
                                        @HealthInsuranceTaxDeductible,
                                        @TotalSocialWithoutDisease)";

                connection.Execute(query, contribution, transaction);
            }
        }

        private ApplicationMonths CalculateApplicationMonths(string months)
        {
            ApplicationMonths applicationMonths = new ApplicationMonths();

            if (months.Contains(","))
            {
                applicationMonths.BeginMonth = RomanNumeral.Parse(months.Split(",").First().Trim());
                applicationMonths.EndMonth = RomanNumeral.Parse(months.Split(",").Last().Trim());
            }
            else if (months.Contains("-"))
            {
                applicationMonths.BeginMonth = RomanNumeral.Parse(months.Split("-")[0].Trim());
                applicationMonths.EndMonth = RomanNumeral.Parse(months.Split("-")[1].Trim());
            }
            else
            {
                applicationMonths.BeginMonth = RomanNumeral.Parse(months.Trim());
                applicationMonths.EndMonth = RomanNumeral.Parse(months.Trim());
            }

            return applicationMonths;
        }

        private bool TryMapZusContribution(HtmlNode row, int lastAddedYear, out AddContributionCommand contribution)
        {
            bool isSuccess = true;
            contribution = null;

            HtmlNodeCollection cells = row.SelectNodes(".//td");
            if (cells != null)
            {
                if (cells.Count == 2)
                {
                    isSuccess = false;
                }

                if (isSuccess)
                {
                    int year = 1900;
                    bool isRowWithYearValue = false;

                    if (cells.Count == 20)
                    {
                        year = Convert.ToInt32(cells[0].InnerText);
                        isRowWithYearValue = true;
                    }
                    else if (cells.Count == 19)
                    {
                        year = lastAddedYear;
                        isRowWithYearValue = false;
                    }

                    ApplicationMonths applicationMonths = CalculateApplicationMonths(cells[isRowWithYearValue ? 1 : 0].InnerText);

                    contribution = new AddContributionCommand
                    {
                        ValidityBeginDate = new DateTime(year, applicationMonths.BeginMonth, 1),
                        ValidityEndDate = new DateTime(year, applicationMonths.EndMonth, DateTime.DaysInMonth(year, applicationMonths.EndMonth)),
                        SocialInsuranceBasisOfContributionDimension = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 2 : 1].InnerText),

                        PensionInsuranceCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 4 : 3].InnerText),
                        DisabilityInsuranceCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 6 : 5].InnerText),
                        MedicalInsuranceCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 8 : 7].InnerText),
                        AccidentInsuranceCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 10 : 9].InnerText),

                        LaborFundBasisCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 14 : 13].InnerText),

                        HealthInsuranceBasisOfContributionDimension = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 15 : 14].InnerText),
                        HealthInsuranceCost = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 17 : 16].InnerText),
                        HealthInsuranceTaxDeductible = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 18 : 17].InnerText),

                        TotalContribution = ConvertExtensions.ToDecimalWithTrim(cells[isRowWithYearValue ? 19 : 18].InnerText)
                    };
                }
            }
            else
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public DateTime? GetLastSynchronizationValidityEndDate()
        {
            using (IDbConnection connection = Connection)
            {
                string query = "SELECT TOP 1 [ValidityEndDate] FROM [dbo].[SynchronizationLogs] WHERE [IsSuccess] = 1 ORDER BY SynchronizationLogId DESC";
                var validityEndDate = connection.Query<DateTime>(query);

                return validityEndDate.Count() > 0 ? validityEndDate.Single() : (DateTime?)null;
            }
        }
    }

    class ApplicationMonths
    {
        public int BeginMonth { get; set; }
        public int EndMonth { get; set; }
    }
}
