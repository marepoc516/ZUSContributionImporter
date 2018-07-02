using System;
using ZUSContributionImporter.CQRS;

namespace ZUSContributionImporter.Contributions
{
    public class ContributionQuery : IQuery<Contribution>
    {
        public DateTime ValidityDate { get; set; }
    }
}