using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IslahiTohfa.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: Admin/Book
    public async Task<IActionResult> Index(BookFilterDto filter)
    {
        var books = await _bookService.GetBooksAsync(filter);
        ViewBag.Filter = filter;
        return View(books);
    }

    // GET: Admin/Book/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // GET: Admin/Book/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookDto model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _bookService.CreateBookAsync(model, userId);
            
            TempData["Success"] = "Book created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error creating book: {ex.Message}");
            return View(model);
        }
    }

    // GET: Admin/Book/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        var model = new UpdateBookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear,
            Publisher = book.Publisher,
            Category = book.Category,
            Language = book.Language,
            PageCount = book.PageCount,
            Status = book.Status,
            IsFeatured = book.IsFeatured,
            ExistingPdfPath = book.FileSizeFormatted,
            ExistingCoverPath = book.CoverImagePath
        };

        return View(model);
    }

    // POST: Admin/Book/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateBookDto model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _bookService.UpdateBookAsync(model, userId);
            
            TempData["Success"] = "Book updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error updating book: {ex.Message}");
            return View(model);
        }
    }

    // GET: Admin/Book/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // POST: Admin/Book/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result)
                return NotFound();

            TempData["Success"] = "Book deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting book: {ex.Message}";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
