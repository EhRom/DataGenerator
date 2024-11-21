using DataGenerator.Domain.Models;
using DataGenerator.Domain.Products.Models;
using Microsoft.Extensions.Configuration;

namespace DataGenerator.Domain.Products;

public class ProductService(IConfiguration configuration) : IProductService
{
    private readonly Lazy<IEnumerable<Product>> productListLazy = new(() =>
    {
        return configuration.GetSection(nameof(productList)).Get<IEnumerable<Product>>()!;
    });

    private IEnumerable<Product> productList => productListLazy.Value;

    public void GenerateData(DataContainer dataContainer)
    {
        // TODO generic loop
        for (DateOnly currentDate = dataContainer.StartDate; currentDate <= dataContainer.EndDate; currentDate = currentDate.AddDays(1))
        {
            bool isHoliday = dataContainer.Holidays.Where(h => h.Date == currentDate).Any();
            string holidayName = dataContainer.Holidays.Where(h => h.Date == currentDate).Select(h => h.Name).FirstOrDefault(string.Empty);

            foreach (Product product in productList)
            {
                double volume = dataContainer.GetRandomDoubleValue(product.DefaultVolume, product.DefaultVolumeVariation, product.VolumeVariationDivisor);
                double peopleTime = dataContainer.GetRandomDoubleValue(product.DefaultPeopleTime, product.DefaultPeopleTimeVariation, product.PeopleTimeVariationDivisor);

                IData generatedData = ProductivityData.CreateNew(currentDate, isHoliday, holidayName, product.Name, volume, peopleTime);
                dataContainer.AddData(generatedData);
            }
        }
    }
}
