namespace SportsStore.Models
{
    public class EFStoreRepository(StoreDbContext ctx) : IStoreRepository
    {
        private readonly StoreDbContext context = ctx;

        public IQueryable<Product> Products => context.Products;
        public void CreateProduct(Product P)
        {
            context.Add(P);
            context.SaveChanges();
        }
        public void DeleteProduct(Product P)
        {
            context.Remove(P);
            context.SaveChanges();
        }
        public void SaveProduct(Product P)
        {
            context.SaveChanges();
        }
    }
}