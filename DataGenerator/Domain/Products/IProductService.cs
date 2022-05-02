namespace DataGenerator.Domain.Products;

public interface IProductService : IItemService
{
    void GenerateData(DataContainer dataContainer, IEnumerable<Product> productList);
}
