using DataGenerator.Domain;
using DataGenerator.Domain.Products;
using DataGenerator.Infra;
using Microsoft.Extensions.Configuration;
using Puffix.ConsoleLogMagnifier;
using Puffix.IoC.Configuration;

ConsoleHelper.Write("Welcome to the data generator console App.");
ConsoleHelper.Write("This help will generate sample data for your sample dashboards.");

IIoCContainerWithConfiguration container;
try
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
           .Build();

    container = IoCContainer.BuildContainer(configuration);
}
catch (Exception error)
{
    ConsoleHelper.Write("Error while initializong the console App");
    ConsoleHelper.WriteError(error);

    return;
}

GeneratorService service = container.Resolve<GeneratorService>();
ConsoleKey key;
do
{
    ConsoleHelper.WriteInfo("Press Y to specify start and end year for data generation, D to specify start and end date for data generation, to Q to quit.");
    key = Console.ReadKey().Key;
    ConsoleHelper.ClearLastCharacters(1);
    try
    {
        ConsoleHelper.WriteVerbose("Load configuration.");
        int maxTryCount = container.ConfigurationRoot.GetValue<int>("maxRetryCount");
        IEnumerable<Product> productList = container.ConfigurationRoot.GetSection("productList").Get<IEnumerable<Product>>();

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

            using DataContainer dataContainer = await service.GenerateData(startDate, endDate, productList);
            // TODO generate data
            // TODO save data

            ConsoleHelper.WriteWarning("Under construction");
        }
        else if (key == ConsoleKey.D)
        {
            service.SetStartAndEndDate(maxTryCount, out DateOnly startDate, out DateOnly endDate);
            ConsoleHelper.WriteInfo($"Generate data for the period between {startDate} and {endDate}");

            using DataContainer dataContainer = await service.GenerateData(startDate, endDate, productList);
            // TODO generate data
            // TODO save data

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

