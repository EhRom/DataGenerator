using DataGenerator.Domain;
using Puffix.ConsoleLogMagnifier;

const int maxTryCount = 5;

ConsoleHelper.Write("Welcome to the data generator console App.");
ConsoleHelper.Write("This help will generate sample data for your sample dashboards.");

GeneratorService service = GeneratorService.CreateNew();

ConsoleKey key;

do
{
    ConsoleHelper.WriteInfo("Press Y to specify start and end year for data generation, D to specify start and end date for data generation, to Q to quit.");
    key = Console.ReadKey().Key;
    ConsoleHelper.ClearLastCharacters(1);
    try
    {
        if (key == ConsoleKey.Q)
        {
            ConsoleHelper.WriteInfo("Thank you for using the data generator console App. See you soon!");
        }
        else if (key == ConsoleKey.Y)
        {
            service.SetStartAndEndYear(maxTryCount, out int startYear, out int endYear);
            ConsoleHelper.WriteInfo($"Generate data for the period between {startYear} and {endYear}");

            DateOnly startDate = new DateOnly(startYear, 1, 1);
            DateOnly endDate = new DateOnly(endYear, 12, 31);

            // TODO generate data
            // TODO save data
            // TODO manage holidays

            ConsoleHelper.WriteWarning("Under construction");
        }
        else if (key == ConsoleKey.D)
        {
            service.SetStartAndEndDate(maxTryCount, out DateOnly startDate, out DateOnly endDate);
            ConsoleHelper.WriteInfo($"Generate data for the period between {startDate} and {endDate}");

            // TODO generate data
            // TODO save data
            // TODO manage holidays

            ConsoleHelper.WriteWarning("Under construction");

        }
        else
        {
            ConsoleHelper.WriteWarning($"The key {key} is not a known command (for the moment :-) )");
        }
    }
    catch (Exception error)
    {
        ConsoleHelper.WriteError("Error while generating data");
        ConsoleHelper.WriteError(error);
        ConsoleHelper.WriteNewLine(2);
    }
} while (key != ConsoleKey.Q);

