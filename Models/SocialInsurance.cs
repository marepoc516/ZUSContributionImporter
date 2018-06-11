using System;

public class SocialInsurance : Insurance
{
    public InsuranceDetails PensionInsurance { get; set; }
    public InsuranceDetails DisabilityInsurance { get; set; }
    public InsuranceDetails MedicalInsurance { get; set; }
    public InsuranceDetails AccidentInsurance { get; set; }
    public decimal TotalSocialWithoutDisease
    { 
        get
        {
            return PensionInsurance.BasisCost + DisabilityInsurance.BasisCost + AccidentInsurance.BasisCost;
        }
    }
    public decimal TotalSocialWithDisease
    {
        get
        {
            return PensionInsurance.BasisCost + DisabilityInsurance.BasisCost + MedicalInsurance.BasisCost + AccidentInsurance.BasisCost;
        } 
    }

}