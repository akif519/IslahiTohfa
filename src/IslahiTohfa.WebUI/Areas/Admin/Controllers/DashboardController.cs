using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IslahiTohfa.Domain.Entities;

namespace IslahiTohfa.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICommentService _commentService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        IBookService bookService,
        ICommentService commentService,
        UserManager<ApplicationUser> userManager)
    {
        _bookService = bookService;
        _commentService = commentService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var recentBooks = await _bookService.GetRecentBooksAsync(5);
        var popularBooks = await _bookService.GetPopularBooksAsync(5);
        var pendingComments = await _commentService.GetCommentsByStatusAsync(CommentStatus.Pending, 1, 10);
        var totalUsers = _userManager.Users.Count();

        ViewBag.RecentBooks = recentBooks;
        ViewBag.PopularBooks = popularBooks;
        ViewBag.PendingComments = pendingComments;
        ViewBag.TotalUsers = totalUsers;

        return View();
    }
}
