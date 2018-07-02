using System;

namespace ZUSContributionImporter.Contributions
{
    public class Contribution
    {
        public DateTime ValidityBeginDate { get; set; }
        public DateTime ValidityEndDate { get; set; }
        public decimal SocialInsuranceBasisOfContributionDimension { get; set; }
        public decimal PensionInsuranceCost { get; set; }
        public decimal DisabilityInsuranceCost { get; set; }
        public decimal MedicalInsuranceCost { get; set; }
        public decimal AccidentInsuranceCost { get; set; }
        public decimal LaborFundBasisCost { get; set; }
        public decimal HealthInsuranceBasisOfContributionDimension { get; set; }
        public decimal HealthInsuranceCost { get; set; }
        public decimal HealthInsuranceTaxDeductible { get; set; }
        public decimal TotalSocialWithoutDisease
        {
            get => PensionInsuranceCost + DisabilityInsuranceCost + AccidentInsuranceCost;
        }
        public decimal TotalSocialWithDisease
        {
            get => PensionInsuranceCost + DisabilityInsuranceCost + MedicalInsuranceCost + AccidentInsuranceCost;
        }
        public decimal TotalContribution { get; set; }
    }
}