# Islahi Tohfa - E-Book Management System

## 📚 Complete .NET 8 Clean Architecture Solution

A comprehensive E-Book Management System built with **Clean Architecture (Onion Architecture)** using .NET 8, featuring a public-facing website for readers and an AdminLTE-powered admin panel for content management.

---

## 🏗️ Architecture Overview

### **4-Layer Clean Architecture**

```
┌─────────────────────────────────────────────────────────┐
│                      WebUI Layer                         │
│  (Controllers, Views, Areas/Admin, wwwroot)             │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│                  Application Layer                       │
│  (DTOs, Interfaces, Services, Mappings)                 │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│                Infrastructure Layer                      │
│  (DbContext, Repositories, FileService)                 │
└──────────────────┬──────────────────────────────────────┘
                   │
┌──────────────────▼──────────────────────────────────────┐
│                    Domain Layer                          │
│  (Entities, Enums, Value Objects)                       │
└─────────────────────────────────────────────────────────┘
```

---

## 📦 Project Structure

```
IslahiTohfa/
├── IslahiTohfa.sln
└── src/
    ├── IslahiTohfa.Domain/
    │   ├── Entities/
    │   │   ├── ApplicationUser.cs
    │   │   ├── Book.cs
    │   │   ├── Comment.cs
    │   │   └── Like.cs
    │   ├── Enums/
    │   │   ├── BookCategory.cs
    │   │   ├── BookStatus.cs
    │   │   └── CommentStatus.cs
    │   └── Common/
    │       └── BaseEntity.cs
    │
    ├── IslahiTohfa.Application/
    │   ├── DTOs/
    │   │   ├── BookDto.cs
    │   │   ├── CreateBookDto.cs
    │   │   ├── UpdateBookDto.cs
    │   │   ├── CommentDto.cs
    │   │   ├── UserDto.cs
    │   │   └── PaginatedList.cs
    │   ├── Interfaces/
    │   │   ├── IRepository.cs
    │   │   ├── IBookRepository.cs
    │   │   ├── ICommentRepository.cs
    │   │   ├── ILikeRepository.cs
    │   │   ├── IUnitOfWork.cs
    │   │   ├── IFileService.cs
    │   │   ├── IBookService.cs
    │   │   ├── ICommentService.cs
    │   │   └── ILikeService.cs
    │   ├── Services/
    │   │   ├── BookService.cs
    │   │   ├── CommentService.cs
    │   │   └── LikeService.cs
    │   └── Mappings/
    │       └── MappingProfile.cs
    │
    ├── IslahiTohfa.Infrastructure/
    │   ├── Data/
    │   │   └── ApplicationDbContext.cs
    │   ├── Repositories/
    │   │   ├── Repository.cs
    │   │   ├── BookRepository.cs
    │   │   ├── CommentRepository.cs
    │   │   ├── LikeRepository.cs
    │   │   └── UnitOfWork.cs
    │   └── Services/
    │       └── FileService.cs
    │
    └── IslahiTohfa.WebUI/
        ├── Controllers/
        │   ├── HomeController.cs
        │   └── BookController.cs
        ├── Areas/Admin/
        │   └── Controllers/
        │       ├── DashboardController.cs
        │       ├── BookController.cs
        │       └── CommentController.cs
        ├── Views/
        ├── wwwroot/
        ├── Program.cs
        └── appsettings.json
```

---

## 🚀 Getting Started

### **Prerequisites**
- .NET 8 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or VS Code
- Git

### **Installation Steps**

1. **Clone/Copy the Solution**
   ```bash
   # The solution is already created in /home/claude/IslahiTohfa
   # Copy it to your D:\ClaudeAI\IslahiTohfa directory
   ```

2. **Update Connection String**
   
   Edit `appsettings.json` in WebUI project:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IslahiTohfaDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
   }
   ```

3. **Restore NuGet Packages**
   ```bash
   cd D:\ClaudeAI\IslahiTohfa
   dotnet restore
   ```

4. **Create Database Migration**
   ```bash
   cd src\IslahiTohfa.WebUI
   dotnet ef migrations add InitialCreate --project ..\IslahiTohfa.Infrastructure
   ```

5. **Update Database**
   ```bash
   dotnet ef database update --project ..\IslahiTohfa.Infrastructure
   ```

6. **Run the Application**
   ```bash
   dotnet run
   ```

---

## 👤 Default Admin Credentials

After first run, use these credentials:

- **Email**: admin@islahitohfa.com
- **Username**: admin
- **Password**: Admin@123

**⚠️ Change this password immediately in production!**

---

## 📋 Key Features

### **Public Website**
✅ Modern, responsive book catalog  
✅ Advanced search and filter (by category, language, year)  
✅ Book detail page with PDF viewer  
✅ User authentication (Register/Login)  
✅ Like/Unlike books  
✅ Comment and rate books (1-5 stars)  
✅ Download books (PDF)  

### **Admin Panel (AdminLTE 3)**
✅ Dashboard with statistics  
✅ Complete Book CRUD operations  
✅ PDF and cover image upload  
✅ User management  
✅ Comment moderation (Approve/Reject/Flag)  
✅ Book status management (Draft/Published/Archived)  

---

## 🔧 Technologies Used

| Layer | Technologies |
|-------|-------------|
| **Domain** | .NET 8, Entity Classes |
| **Application** | DTOs, AutoMapper, Interfaces |
| **Infrastructure** | EF Core 8, SQL Server, Repository Pattern |
| **WebUI** | ASP.NET Core MVC, Identity, Razor |

---

## 📊 Database Schema

### **Main Entities**

**Books**
- Id, Title, Author, Description, ISBN
- Category, Language, PageCount, Publisher
- PdfFilePath, CoverImagePath, FileSize
- ViewCount, DownloadCount, LikeCount, CommentCount
- Status (Draft/Published/Archived), IsFeatured

**Comments**
- Id, Content, Rating (1-5), BookId, UserId
- Status (Pending/Approved/Rejected), ModeratorNotes

**Likes**
- Id, BookId, UserId (Unique constraint)

**ApplicationUser** (extends IdentityUser)
- FirstName, LastName, ProfilePicture, Bio

---

## 🎯 Design Patterns Implemented

✅ **Repository Pattern** - Data access abstraction  
✅ **Unit of Work Pattern** - Transaction management  
✅ **Dependency Injection** - Loose coupling  
✅ **DTO Pattern** - Data transfer objects  
✅ **Service Layer Pattern** - Business logic separation  

---

## 📝 API Endpoints

### **Public Routes**
- `GET /` - Home page
- `GET /Book/Index` - Browse books
- `GET /Book/Details/{id}` - Book details
- `GET /Book/Read/{id}` - PDF viewer
- `GET /Book/Download/{id}` - Download PDF
- `POST /Book/AddComment` - Add comment
- `POST /Book/ToggleLike/{id}` - Like/Unlike

### **Admin Routes**
- `GET /Admin/Dashboard` - Admin dashboard
- `GET /Admin/Book` - Manage books
- `GET /Admin/Book/Create` - Create book form
- `POST /Admin/Book/Create` - Save new book
- `GET /Admin/Book/Edit/{id}` - Edit book form
- `POST /Admin/Book/Edit/{id}` - Update book
- `POST /Admin/Book/Delete/{id}` - Delete book
- `GET /Admin/Comment` - Comment moderation
- `POST /Admin/Comment/Approve/{id}` - Approve comment
- `POST /Admin/Comment/Reject/{id}` - Reject comment

---

## 🛡️ Security Features

✅ ASP.NET Core Identity for authentication  
✅ Role-based authorization (Admin, User)  
✅ Password policy enforcement  
✅ Account lockout after failed attempts  
✅ Anti-forgery tokens on all forms  
✅ Secure file upload validation  

---

## 📂 File Upload Configuration

**Supported Formats:**
- **PDFs**: .pdf only
- **Cover Images**: .jpg, .jpeg, .png, .gif, .webp

**Storage Location:**
- PDFs: `wwwroot/uploads/pdfs/`
- Covers: `wwwroot/uploads/covers/`
- Profiles: `wwwroot/uploads/profiles/`

---

## 🔄 Database Migrations

### **Create New Migration**
```bash
cd src\IslahiTohfa.WebUI
dotnet ef migrations add MigrationName --project ..\IslahiTohfa.Infrastructure
```

### **Update Database**
```bash
dotnet ef database update --project ..\IslahiTohfa.Infrastructure
```

### **Remove Last Migration**
```bash
dotnet ef migrations remove --project ..\IslahiTohfa.Infrastructure
```

---

## 🎨 UI Customization

### **Public Website**
- Modify views in `Views/` folder
- Update styles in `wwwroot/css/`
- Add JavaScript in `wwwroot/js/`

### **Admin Panel**
- AdminLTE 3 theme ready
- Download from: https://adminlte.io/
- Place in `wwwroot/lib/adminlte/`

---

## 📖 Usage Guide

### **For Admins**

1. **Login to Admin Panel**
   - Navigate to `/Admin/Dashboard`
   - Use admin credentials

2. **Add a New Book**
   - Click "Books" → "Create New"
   - Fill in book details
   - Upload PDF and cover image
   - Set category and status
   - Click "Save"

3. **Moderate Comments**
   - Go to "Comments" section
   - Review pending comments
   - Approve or reject with notes

### **For Users**

1. **Browse Books**
   - Visit homepage
   - Use search/filter options
   - Click on book for details

2. **Read or Download**
   - Login to your account
   - Click "Read Online" for PDF viewer
   - Click "Download" to save PDF

3. **Engage with Books**
   - Like books you enjoy
   - Leave comments and ratings
   - Comments pending admin approval

---

## 🐛 Troubleshooting

### **Database Connection Issues**
```bash
# Check SQL Server is running
# Update connection string in appsettings.json
# Run: dotnet ef database update
```

### **Migration Errors**
```bash
# Delete Migrations folder
# Drop database
# Recreate migrations: dotnet ef migrations add InitialCreate
# Update database: dotnet ef database update
```

### **File Upload Errors**
```bash
# Ensure wwwroot/uploads folders exist
# Check file permissions
# Verify file size limits in IIS/Kestrel
```

---

## 📧 Support & Contact

For issues or questions:
- Create an issue in the repository
- Contact: admin@islahitohfa.com

---

## 📄 License

This project is licensed under the MIT License.

---

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- AdminLTE for admin template
- ASP.NET Core Team
- Entity Framework Core Team

---

**Built with ❤️ using .NET 8 and Clean Architecture principles**
