using System.Threading.Tasks;

namespace ZUSContributionImporter.CQRS
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}