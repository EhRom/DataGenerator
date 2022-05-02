using DataGenerator.Domain.Claim;
using DataGenerator.Domain.Holidays;
using DataGenerator.Domain.Products;
using Puffix.ConsoleLogMagnifier;

namespace DataGenerator.Domain;

public class GeneratorService
{
    private readonly IHolidayService holidayService;
    private readonly IProductService productService;
    private readonly IClaimService claimService;


    public GeneratorService(IHolidayService holidayService, IProductService productService, IClaimService claimService)
    {
        this.holidayService = holidayService;
        this.productService = productService;
        this.claimService = claimService;
    }

    public void SetStartAndEndDate(int maxTryCount, out DateOnly startDate, out DateOnly endDate)
    {
        int currentTryCount;
        currentTryCount = 0;
        while (!EnterDate(maxTryCount, ++currentTryCount, "start", out startDate)) { }

        currentTryCount = 0;
        while (!EnterDate(maxTryCount, ++currentTryCount, "end", out endDate)) { }

        if (startDate > endDate)
        {
            DateOnly tempDate = startDate;
            startDate = endDate;
            endDate = tempDate;
        }
    }

    public bool EnterDate(int maxTryCount, int currentTryCount, string dateName, out DateOnly date)
    {
        ConsoleHelper.WriteInfo($"Enter the {dateName} date (dd/MM/yyyy or yyyy-MM-dd formats, years between 2000 and 2100):");
        if (currentTryCount > 1)
            ConsoleHelper.WriteVerbose($"Try {currentTryCount} / {maxTryCount}");
        string? enteredDate = Console.ReadLine();

        ConsoleHelper.ClearLastLines();

        bool valid = !string.IsNullOrEmpty(enteredDate) && ValidateDate(enteredDate, out date);

        if (!valid)
        {
            ConsoleHelper.WriteWarning($"The entered date ('{enteredDate}') is not valid.");
            if (currentTryCount >= maxTryCount)
                throw new ArgumentException("The entered date is not in the right format. Format accepted: dd/MM/yyyy or yyyy-MM-dd, years between 2000 and 2100.");
        }
        else
        {
            ConsoleHelper.WriteSuccess($"The {dateName} date is '{enteredDate}'.");
        }

        return valid;
    }

    public bool ValidateDate(string dateValue, out DateOnly date)
    {
        const string isoDateFormat = "yyyy-MM-dd";
        const string frenchDateFormat = "dd/MM/yyyy";

        bool isValid = DateOnly.TryParseExact(dateValue, isoDateFormat, out date) ||
                       DateOnly.TryParseExact(dateValue, frenchDateFormat, out date);

        isValid = isValid && date.Year > 2000 && date.Year < 2100;

        return isValid;
    }

    public void SetStartAndEndYear(int maxTryCount, out int startYear, out int endYear)
    {
        int currentTryCount;
        currentTryCount = 0;
        while (!EnterDate(maxTryCount, ++currentTryCount, "start", out startYear)) { }

        currentTryCount = 0;
        while (!EnterDate(maxTryCount, ++currentTryCount, "end", out endYear)) { }

        if (startYear > endYear)
            (endYear, startYear) = (startYear, endYear);
    }

    public bool EnterDate(int maxTryCount, int currentTryCount, string yearName, out int year)
    {
        year = 0;
        ConsoleHelper.WriteInfo($"Enter the {yearName} year (between 2000 and 2100):");
        if (currentTryCount > 1)
            ConsoleHelper.WriteVerbose($"Try {currentTryCount} / {maxTryCount}");
        string? enteredYear = Console.ReadLine();

        ConsoleHelper.ClearLastLines();

        bool valid = !string.IsNullOrEmpty(enteredYear) && ValidateYear(enteredYear, out year);

        if (!valid)
        {
            ConsoleHelper.WriteWarning($"The entered year ('{enteredYear}') is not valid.");
            if (currentTryCount >= maxTryCount)
                throw new ArgumentException("The entered year is not in the right format. Integer between 2000 and 2100.");
        }
        else
        {
            ConsoleHelper.WriteSuccess($"The {yearName} year is '{enteredYear}'.");
        }

        return valid;
    }

    public bool ValidateYear(string yearValue, out int year)
    {
        bool isValid = int.TryParse(yearValue, out year);

        isValid = isValid && year >= 2000 && year <= 2100;

        return isValid;
    }
    public async Task<DataContainer> GenerateData(DateOnly startDate, DateOnly endDate, ClaimsConfiguration claimsConfiguration)
    {
        ConsoleHelper.WriteVerbose("Get holidays.");
        IEnumerable<Holiday> holidays = await holidayService.GetHolidays(startDate, endDate);

        ConsoleHelper.WriteVerbose("Initialize data container.");
        DataContainer dataContainer = DataContainer.CreateNew(holidays, startDate, endDate);

        ConsoleHelper.WriteVerbose($"Generate claims data from {startDate} to {endDate}.");
        claimService.GenerateData(dataContainer, claimsConfiguration);

        return dataContainer;
    }

    public async Task<DataContainer> GenerateData(DateOnly startDate, DateOnly endDate, IEnumerable<Product> productList)
    {
        ConsoleHelper.WriteVerbose("Get holidays.");
        IEnumerable<Holiday> holidays = await holidayService.GetHolidays(startDate, endDate);

        ConsoleHelper.WriteVerbose("Initialize data container.");
        DataContainer dataContainer = DataContainer.CreateNew(holidays, startDate, endDate);

        ConsoleHelper.WriteVerbose($"Generate productivity data from {startDate} to {endDate}.");
        productService.GenerateData(dataContainer, productList);

        return dataContainer;
    }

    public async Task<string> SaveDataToFile(DataContainer dataContainer, string outputDirectory, string fileNamePrefix)
    {
        string generatedFileName = BuildFilePath(outputDirectory, fileNamePrefix, dataContainer.StartDate, dataContainer.EndDate);

        string fileContent = dataContainer.GetCsvContent();

        await File.WriteAllTextAsync(generatedFileName, fileContent);

        return generatedFileName;
    }

    private string BuildFilePath(string outputDirectory, string fileNamePrefix, DateOnly startDate, DateOnly endDate)
    {
        if (!Path.IsPathFullyQualified(outputDirectory))
            outputDirectory = Path.GetFullPath(outputDirectory);

        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        return Path.Combine(outputDirectory, BuildFileName(fileNamePrefix, startDate, endDate));
    }

    private string BuildFileName(string fileNamePrefix, DateOnly startDate, DateOnly endDate)
    {
        const string DATA_FORMAT = "yyyyMMdd";
        string guid = Guid.NewGuid().ToString("N");

        return $"{fileNamePrefix}-{startDate.ToString(DATA_FORMAT)}-{endDate.ToString(DATA_FORMAT)}-{guid}.csv";
    }
}
