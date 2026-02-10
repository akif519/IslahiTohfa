using AutoMapper;
using IslahiTohfa.Application.DTOs;
using IslahiTohfa.Application.Interfaces;
using IslahiTohfa.Domain.Entities;

namespace IslahiTohfa.Application.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public BookService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<PaginatedList<BookDto>> GetBooksAsync(BookFilterDto filter)
    {
        var books = await _unitOfWork.Books.GetFilteredBooksAsync(filter);
        
        var bookDtos = new PaginatedList<BookDto>
        {
            Items = _mapper.Map<List<BookDto>>(books.Items),
            PageNumber = books.PageNumber,
            PageSize = books.PageSize,
            TotalCount = books.TotalCount
        };
        
        return bookDtos;
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        var book = await _unitOfWork.Books.GetBookWithDetailsAsync(id);
        return book == null ? null : _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto createDto, string userId)
    {
        var book = _mapper.Map<Book>(createDto);
        
        // Save PDF file
        if (createDto.PdfFile != null)
        {
            book.PdfFilePath = await _fileService.SaveBookPdfAsync(createDto.PdfFile);
            book.FileSize = createDto.PdfFile.Length;
        }
        
        // Save cover image
        if (createDto.CoverImage != null)
        {
            book.CoverImagePath = await _fileService.SaveBookCoverAsync(createDto.CoverImage);
        }
        
        book.CreatedBy = userId;
        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> UpdateBookAsync(UpdateBookDto updateDto, string userId)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(updateDto.Id);
        if (book == null)
            throw new KeyNotFoundException($"Book with ID {updateDto.Id} not found");
        
        // Map updates
        _mapper.Map(updateDto, book);
        
        // Update PDF if provided
        if (updateDto.PdfFile != null)
        {
            // Delete old PDF
            if (!string.IsNullOrEmpty(book.PdfFilePath))
                await _fileService.DeleteFileAsync(book.PdfFilePath);
                
            book.PdfFilePath = await _fileService.SaveBookPdfAsync(updateDto.PdfFile);
            book.FileSize = updateDto.PdfFile.Length;
        }
        
        // Update cover if provided
        if (updateDto.CoverImage != null)
        {
            // Delete old cover
            if (!string.IsNullOrEmpty(book.CoverImagePath))
                await _fileService.DeleteFileAsync(book.CoverImagePath);
                
            book.CoverImagePath = await _fileService.SaveBookCoverAsync(updateDto.CoverImage);
        }
        
        book.UpdatedBy = userId;
        book.UpdatedAt = DateTime.UtcNow;
        
        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<BookDto>(book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(id);
        if (book == null)
            return false;
        
        // Soft delete
        book.IsDeleted = true;
        book.UpdatedAt = DateTime.UtcNow;
        
        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<BookDto>> GetFeaturedBooksAsync(int count = 6)
    {
        var books = await _unitOfWork.Books.GetFeaturedBooksAsync(count);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<BookDto>> GetRecentBooksAsync(int count = 10)
    {
        var books = await _unitOfWork.Books.GetRecentBooksAsync(count);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<IEnumerable<BookDto>> GetPopularBooksAsync(int count = 10)
    {
        var books = await _unitOfWork.Books.GetPopularBooksAsync(count);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task IncrementViewCountAsync(int bookId)
    {
        await _unitOfWork.Books.IncrementViewCountAsync(bookId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<byte[]> GetBookPdfAsync(int bookId)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(bookId);
        if (book == null || string.IsNullOrEmpty(book.PdfFilePath))
            throw new FileNotFoundException("PDF file not found");
        
        await _unitOfWork.Books.IncrementDownloadCountAsync(bookId);
        await _unitOfWork.SaveChangesAsync();
        
        return await _fileService.ReadFileAsync(book.PdfFilePath);
    }
}
