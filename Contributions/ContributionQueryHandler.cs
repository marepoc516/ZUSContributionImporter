using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ZUSContributionImporter.CQRS;
using Dapper;

namespace ZUSContributionImporter.Contributions
{
    public class ContributionQueryHandler : IQueryHandler<ContributionQuery, Contribution>
    {
        private readonly IConfigurationRoot _config;

        private IDbConnection Connection
        {
            get => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public ContributionQueryHandler()
        {
        }

        public ContributionQueryHandler(IConfigurationRoot config)
        {
            _config = config;
        }

        public async Task<Contribution> HandleAsync(ContributionQuery query)
        {
            string sql = "SELECT * FROM [Contributions] WHERE [ValidityBeginDate] < @ValidityDate AND [ValidityEndDate] > @ValidityDate";

            return (await Connection.QueryAsync<Contribution>(sql, query)).FirstOrDefault();
        }
    }
}