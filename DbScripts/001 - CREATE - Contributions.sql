-- USE [ZusContribution]
-- GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contributions]
(
	[ContributionId] [int] IDENTITY(1,1) NOT NULL,
	[ValidityBeginDate] [datetime] NOT NULL,
	[ValidityEndDate] [datetime] NOT NULL,
	[SocialInsuranceBasisOfContributionDimension] [money] NOT NULL,
	[PensionInsuranceCost] [money] NOT NULL,
	[DisabilityInsuranceCost] [money] NOT NULL,
	[MedicalInsuranceCost] [money] NOT NULL,
	[AccidentInsuranceCost] [money] NOT NULL,
	[LaborFundBasisCost] [money] NOT NULL,
	[HealthInsuranceBasisOfContributionDimension] [money] NOT NULL,
	[HealthInsuranceCost] [money] NOT NULL,
	[HealthInsuranceTaxDeductible] [money] NOT NULL,
	[TotalContribution] [money] NOT NULL,
	CONSTRAINT [PK_Contribution] PRIMARY KEY CLUSTERED 
(
	[ContributionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO