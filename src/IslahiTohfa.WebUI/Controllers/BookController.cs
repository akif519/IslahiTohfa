using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IslahiTohfa.WebUI.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICommentService _commentService;
    private readonly ILikeService _likeService;

    public BookController(
        IBookService bookService,
        ICommentService commentService,
        ILikeService likeService)
    {
        _bookService = bookService;
        _commentService = commentService;
        _likeService = likeService;
    }

    // GET: Book/Index
    public async Task<IActionResult> Index(BookFilterDto filter)
    {
        var books = await _bookService.GetBooksAsync(filter);
        ViewBag.Filter = filter;
        return View(books);
    }

    // GET: Book/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        await _bookService.IncrementViewCountAsync(id);

        var comments = await _commentService.GetCommentsByBookIdAsync(id);
        var averageRating = await _commentService.GetAverageRatingForBookAsync(id);

        bool isLiked = false;
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            isLiked = await _likeService.IsBookLikedByUserAsync(id, userId);
        }

        ViewBag.Comments = comments;
        ViewBag.AverageRating = averageRating;
        ViewBag.IsLiked = isLiked;

        return View(book);
    }

    // GET: Book/Read/5
    [Authorize]
    public async Task<IActionResult> Read(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // GET: Book/Download/5
    [Authorize]
    public async Task<IActionResult> Download(int id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            var pdfBytes = await _bookService.GetBookPdfAsync(id);
            var fileName = $"{book.Title.Replace(" ", "_")}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }

    // POST: Book/AddComment
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(CreateCommentDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide valid comment details.";
            return RedirectToAction(nameof(Details), new { id = model.BookId });
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _commentService.CreateCommentAsync(model, userId);
            
            TempData["Success"] = "Your comment has been submitted and is pending approval.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Failed to add comment: {ex.Message}";
        }

        return RedirectToAction(nameof(Details), new { id = model.BookId });
    }

    // POST: Book/ToggleLike/5
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ToggleLike(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isLiked = await _likeService.ToggleLikeAsync(id, userId);
            
            var likeCount = await _likeService.GetLikeCountForBookAsync(id);
            
            return Json(new { success = true, isLiked, likeCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}
