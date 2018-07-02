using System.Threading.Tasks;

namespace ZUSContributionImporter.CQRS
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}