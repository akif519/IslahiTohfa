using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IslahiTohfa.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    // GET: Admin/Comment
    public async Task<IActionResult> Index(CommentStatus status = CommentStatus.Pending, int pageNumber = 1)
    {
        var comments = await _commentService.GetCommentsByStatusAsync(status, pageNumber, 20);
        ViewBag.Status = status;
        return View(comments);
    }

    // POST: Admin/Comment/Approve/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, string? notes)
    {
        try
        {
            var moderatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _commentService.ApproveCommentAsync(id, moderatorId, notes);
            
            TempData["Success"] = "Comment approved successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error approving comment: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/Comment/Reject/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id, string notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            TempData["Error"] = "Please provide a reason for rejection.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var moderatorId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _commentService.RejectCommentAsync(id, moderatorId, notes);
            
            TempData["Success"] = "Comment rejected successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error rejecting comment: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
