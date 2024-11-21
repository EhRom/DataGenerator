using DataGenerator.Domain.Models;

namespace DataGenerator.Domain;

public interface ISetupService
{
    IPeriod SetStartAndEndPeriod(bool isWholeYear);
}
