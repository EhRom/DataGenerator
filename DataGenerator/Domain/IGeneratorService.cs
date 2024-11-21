namespace DataGenerator.Domain;

public interface IGeneratorService
{
    Task<string> GenerateAndPersistData(DateOnly startDate, DateOnly endDate);
}