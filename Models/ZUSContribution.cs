// https://zus.pox.pl/zus_skladki_historyczne.htm
using System;

public class ZUSContribution
{
    public int ContributionId { get; set; }
    public DateTime ValidityBeginDate { get; set; }
    public DateTime ValidityEndDate { get; set; }
    public decimal TotalContribution { get; set; }
    public SocialInsurance SocialInsuranceContribution { get; set; }
    public LaborFund LaborFund { get; set; }
    public HealthInsurance HealthInsuranceContribution { get; set; }
}