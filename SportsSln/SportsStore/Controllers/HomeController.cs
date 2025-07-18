using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController(IStoreRepository repo) : Controller
    {
        private readonly IStoreRepository repository = repo;

        // public IActionResult Index() => View(repository.Products);
        public int PageSize = 4;
        // public ViewResult Index(int productPage = 1) =>
        //     View(repository.Products.OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize));

        // public ViewResult Index(int productPage = 1) =>
        //     View(new ProductsListViewModel
        //     {
        //         Products = repository.Products.OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
        //         PagingInfo = new PagingInfo{CurrentPage = productPage, ItemsPerPage = PageSize, TotalItems = repository.Products.Count()}
        //     });
        public ViewResult Index(string? category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = repository.Products.Where(p => category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((productPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(p => p.Category == category).Count()
                },
                CurrentCategory = category
            });
    }
}