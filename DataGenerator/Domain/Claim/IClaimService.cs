namespace DataGenerator.Domain.Claim;

public interface IClaimService : IItemService
{
    void GenerateData(DataContainer dataContainer, ClaimsConfiguration claimsConfiguration);
}
