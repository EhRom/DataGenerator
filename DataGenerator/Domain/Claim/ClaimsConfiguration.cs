namespace DataGenerator.Domain.Claim;

public class ClaimsConfiguration
{
    public long AverageClaimPerDay { get; set; }

    public long DefaultAverageClaimPerDayVariation { get; set; }

    public double PercentageOfNewCustomers { get; set; }

    public double PercentageOfNewFiles { get; set; }

    public int AverageResolutionDelay { get; set; }

    public int DefaultAverageResolutionDelayVariation { get; set; }
}
