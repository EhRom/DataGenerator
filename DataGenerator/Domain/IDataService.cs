using DataGenerator.Domain.Models;

namespace DataGenerator.Domain;

public interface IDataService
{
    void GenerateData(DataContainer dataContainer);
}
