using DataGenerator.Domain.Holidays;
using Puffix.ConsoleLogMagnifier;

namespace DataGenerator.Domain;

public class GeneratorService
{
    private readonly IHolidayService holidayService;

    public GeneratorService(IHolidayService holidayService)
    {
        this.holidayService = holidayService;
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

    public async Task<DataContainer> GenerateData(DateOnly startDate, DateOnly endDate)
    {
        ConsoleHelper.WriteVerbose("Get holidays.");
        IEnumerable<Holiday> holidays = await holidayService.GetHolidays(startDate, endDate);

        ConsoleHelper.WriteVerbose("Initialize data container.");
        DataContainer dataContainer = DataContainer.CreateNew(holidays, startDate, endDate);

        ConsoleHelper.WriteVerbose($"Generate date from {startDate} to {endDate}.");
        GenerateData(dataContainer);

        return dataContainer;
    }

    private static void GenerateData(DataContainer dataContainer)
    {
        for (DateOnly currentDate = dataContainer.StartDate; currentDate <= dataContainer.EndDate; currentDate = currentDate.AddDays(1))
        {
            bool isHoliday = dataContainer.Holidays.Where(h => h.Date == currentDate).Any();
            string holidayName = dataContainer.Holidays.Where(h => h.Date == currentDate).Select(h => h.Name).FirstOrDefault(string.Empty);

            IData generatedData = ProductivityData.CreateNew(currentDate, isHoliday, holidayName);
            dataContainer.AddData(generatedData);
        }
    }
}
