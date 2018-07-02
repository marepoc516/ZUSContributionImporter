using System;
using System.Threading.Tasks;
using ZUSContributionImporter.CQRS;

namespace ZUSContributionImporter.Contributions
{
    public class ContributionService
    {
        private readonly IQueryExecutor _queryExecutor;

        public ContributionService(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public async Task<Contribution> GetContributionAsync(DateTime validityDate)
        {
            return await _queryExecutor.ExecuteAsync(new ContributionQuery { ValidityDate = validityDate });
        }
    }
}