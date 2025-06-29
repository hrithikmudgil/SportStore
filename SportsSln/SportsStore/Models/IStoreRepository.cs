namespace SportsStore.Models
{
    public interface IStoreRepository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product P);
        void CreateProduct(Product P);
        void DeleteProduct(Product P);
    }
}