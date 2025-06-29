using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class EFOrderRepository(StoreDbContext ctx) : IOrderRepository
    {
        private readonly StoreDbContext context = ctx;
        //This ensures that I receive all the data objects that I need without having to perform separate queries and then assemble the data myself
        public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);
        public void SaveOrder(Order order)
        {
            //This ensures that Entity Framework Core won’t try to write the de-serialized Product objects that are associated with the Order object
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}