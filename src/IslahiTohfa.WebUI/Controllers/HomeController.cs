using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IBookService _bookService;

    public HomeController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> Index()
    {
        var featuredBooks = await _bookService.GetFeaturedBooksAsync(6);
        var recentBooks = await _bookService.GetRecentBooksAsync(10);
        var popularBooks = await _bookService.GetPopularBooksAsync(10);
        
        ViewBag.FeaturedBooks = featuredBooks;
        ViewBag.RecentBooks = recentBooks;
        ViewBag.PopularBooks = popularBooks;
        
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
