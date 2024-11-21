using DataGenerator.Domain.Calendar;
using DataGenerator.Domain.Calendar.Models;
using DataGenerator.Domain.Models;
using Microsoft.Extensions.Configuration;
using Puffix.ConsoleLogMagnifier;

namespace DataGenerator.Domain;

public class GeneratorService(IConfiguration configuration, IHolidayService holidayService, IDataService dataService) : IGeneratorService
{
    private readonly IHolidayService holidayService = holidayService;
    private readonly IDataService dataService = dataService;

    private readonly Lazy<string> outputDirectoryPathLazy = new(() =>
    {
        return configuration[nameof(outputDirectoryPath)]!;
    });
    private readonly Lazy<string> fileNamePrefixLazy = new(() =>
    {
        return configuration[nameof(fileNamePrefix)]!;
    });

    private string outputDirectoryPath => outputDirectoryPathLazy.Value;
    private string fileNamePrefix => fileNamePrefixLazy.Value;

    public async Task<string> GenerateAndPersistData(DateOnly startDate, DateOnly endDate)
    {
        using DataContainer dataContainer = await GenerateData(startDate, endDate);

        string generatedFilePath = await SaveDataToFile(dataContainer, outputDirectoryPath, fileNamePrefix);

        return generatedFilePath;
    }

    private async Task<DataContainer> GenerateData(DateOnly startDate, DateOnly endDate)
    {
        ConsoleHelper.WriteVerbose("Get holidays.");
        IEnumerable<Holiday> holidays = await holidayService.GetHolidays(startDate, endDate);

        // TODO add core generate data
        // TODO add loop on date
        // TODO add multiple append to file
        ConsoleHelper.WriteVerbose("Initialize data container.");
        DataContainer dataContainer = DataContainer.CreateNew(holidays, startDate, endDate);

        ConsoleHelper.WriteVerbose($"Generate productivity data from {startDate} to {endDate}.");
        dataService.GenerateData(dataContainer);

        return dataContainer;
    }

    private async Task<string> SaveDataToFile(DataContainer dataContainer, string outputDirectory, string fileNamePrefix)
    {
        // TODO make private
        // TODO split in two methods: Create file + Append to file
        string generatedFileName = BuildFilePath(outputDirectory, fileNamePrefix, dataContainer.StartDate, dataContainer.EndDate);

        string fileContent = dataContainer.GetCsvContent();

        await File.WriteAllTextAsync(generatedFileName, fileContent);

        return generatedFileName;
    }

    private static string BuildFilePath(string outputDirectory, string fileNamePrefix, DateOnly startDate, DateOnly endDate)
    {
        if (!Path.IsPathFullyQualified(outputDirectory))
            outputDirectory = Path.GetFullPath(outputDirectory);

        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        return Path.Combine(outputDirectory, BuildFileName(fileNamePrefix, startDate, endDate));
    }

    private static string BuildFileName(string fileNamePrefix, DateOnly startDate, DateOnly endDate)
    {
        const string DATA_FORMAT = "yyyyMMdd";
        string guid = Guid.NewGuid().ToString("N");

        return $"{fileNamePrefix}-{startDate.ToString(DATA_FORMAT)}-{endDate.ToString(DATA_FORMAT)}-{guid}.csv";
    }
}
