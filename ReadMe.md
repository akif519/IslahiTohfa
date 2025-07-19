# الإصلاحي تحفة - Islahi Tohfa

<div align="center">
  <img src="wwwroot/images/islahi-tohfa-logo.png" alt="Islahi Tohfa Logo" width="200" height="200">
  
  ## منصة الكتب التعليمية الإسلامية
  ### Islamic Educational Books Platform

  [![Build Status](https://github.com/islahitohfa/islahi-tohfa-platform/workflows/CI/badge.svg)](https://github.com/islahitohfa/islahi-tohfa-platform/actions)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
  [![Arabic](https://img.shields.io/badge/Language-Arabic-green.svg)](README_AR.md)
</div>

## 📖 About | حول المشروع

**Islahi Tohfa** is a comprehensive Islamic educational books platform built with modern technologies. It provides free access to authentic Islamic literature, featuring a clean architecture and user-friendly interface that supports both Arabic and English languages.

**الإصلاحي تحفة** منصة شاملة للكتب التعليمية الإسلامية مبنية بأحدث التقنيات. توفر وصولاً مجانياً للأدب الإسلامي الأصيل، مع معمارية نظيفة وواجهة سهلة الاستخدام تدعم اللغتين العربية والإنجليزية.

## ✨ Features | المميزات

### 🎯 Core Features | المميزات الأساسية
- **📚 Comprehensive Library** - مكتبة شاملة من الكتب الإسلامية
- **🔍 Advanced Search** - بحث متقدم وفلترة ذكية
- **📱 Responsive Design** - تصميم متجاوب لجميع الأجهزة
- **🌙 Dark/Light Mode** - وضع ليلي ونهاري
- **🌐 RTL Support** - دعم كامل للغة العربية واتجاه RTL
- **📖 PDF Reader** - قارئ PDF متطور مع ميزات تفاعلية
- **🔖 Bookmarks** - علامات مرجعية وملاحظات شخصية
- **💬 Comments & Ratings** - تعليقات وتقييمات
- **👤 User Profiles** - ملفات شخصية للمستخدمين
- **📊 Analytics Dashboard** - لوحة تحكم إحصائية للإدارة

### 🔐 Security Features | مميزات الأمان
- **JWT Authentication** - مصادقة آمنة باستخدام JWT
- **Role-based Access** - تحكم بالصلاحيات حسب الأدوار
- **Input Validation** - التحقق من صحة المدخلات
- **XSS Protection** - حماية من هجمات XSS
- **CSRF Protection** - حماية من هجمات CSRF

### 🚀 Technical Features | المميزات التقنية
- **Clean Architecture** - معمارية نظيفة
- **Repository Pattern** - نمط المستودع
- **AutoMapper** - تحويل الكائنات تلقائياً
- **FluentValidation** - التحقق من صحة البيانات
- **SignalR** - تحديثات فورية
- **Caching** - نظام تخزين مؤقت
- **Background Services** - خدمات خلفية
- **Health Checks** - فحص صحة النظام

## 🛠️ Technology Stack | التقنيات المستخدمة

### Backend
- **.NET 8** - إطار العمل الأساسي
- **ASP.NET Core** - تطوير API
- **Entity Framework Core** - ORM للبيانات
- **SQL Server** - قاعدة البيانات
- **AutoMapper** - تحويل الكائنات
- **FluentValidation** - التحقق من البيانات

### Frontend
- **Blazor Server** - واجهة المستخدم التفاعلية
- **MudBlazor** - مكتبة مكونات حديثة
- **SignalR** - التحديثات الفورية
- **JavaScript** - تحسينات إضافية
- **CSS3** - تصميم متجاوب

### Additional Tools
- **PDF.js** - عرض ملفات PDF
- **Docker** - تخزين الحاويات
- **GitHub Actions** - CI/CD
- **Application Insights** - مراقبة الأداء

## 📋 Prerequisites | المتطلبات المسبقة

### Development Environment | بيئة التطوير
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/) (for additional tooling)
- [Git](https://git-scm.com/)

### Optional
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for containerization)
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/) (for database management)

## 🚀 Getting Started | البدء السريع

### 1. Clone the Repository | استنساخ المستودع

```bash
git clone https://github.com/islahitohfa/islahi-tohfa-platform.git
cd islahi-tohfa-platform
```

### 2. Setup Database | إعداد قاعدة البيانات

#### Using SQL Server LocalDB (Recommended for Development)
```bash
# Install Entity Framework CLI tools
dotnet tool install --global dotnet-ef

# Create and apply migrations
dotnet ef database update --project src/IsladebhiTohfa.Infrastructure --startup-project src/IsladebhiTohfa.Web
```

#### Using Docker (Alternative)
```bash
# Start SQL Server in Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

# Update connection string in appsettings.Development.json
```

### 3. Configuration | الإعدادات

Create `appsettings.Development.json` in the Web project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IsladebhiTohfaDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "your-development-jwt-key-for-local-testing-minimum-256-bits",
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5001"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Islahi Tohfa Dev",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "EnableSsl": true
  }
}
```

### 4. Install Dependencies | تثبيت التبعيات

```bash
# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build
```

### 5. Run the Application | تشغيل التطبيق

```bash
# Navigate to the Web project
cd src/IsladebhiTohfa.Web

# Run the application
dotnet run
```

The application will be available at:
- **HTTPS:** `https://localhost:5001`
- **HTTP:** `http://localhost:5000`

### 6. Default Admin Account | الحساب الإداري الافتراضي

```
Email: admin@islahitohfa.com
Username: admin
Password: Admin@123
```

## 🐳 Docker Setup | إعداد Docker

### Using Docker Compose (Recommended)

```bash
# Build and run with Docker Compose
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Manual Docker Build

```bash
# Build the image
docker build -t islahi-tohfa:latest .

# Run the container
docker run -d -p 8080:80 --name islahi-tohfa-app islahi-tohfa:latest
```

## 📁 Project Structure | هيكل المشروع

```
IsladebhiTohfa.Solution/
├── src/
│   ├── IsladebhiTohfa.Web/              # Blazor Server App
│   ├── IsladebhiTohfa.API/              # Web API
│   ├── IsladebhiTohfa.Application/      # Application Layer
│   ├── IsladebhiTohfa.Domain/           # Domain Layer
│   └── IsladebhiTohfa.Infrastructure/   # Infrastructure Layer
├── tests/
│   ├── IsladebhiTohfa.UnitTests/
│   ├── IsladebhiTohfa.IntegrationTests/
│   └── IsladebhiTohfa.PerformanceTests/
├── docs/                                # Documentation
├── docker-compose.yml                   # Docker configuration
├── Dockerfile                           # Docker image definition
└── README.md                            # This file
```

## 🧪 Testing | الاختبارات

### Run Unit Tests
```bash
dotnet test tests/IsladebhiTohfa.UnitTests/
```

### Run Integration Tests
```bash
dotnet test tests/IsladebhiTohfa.IntegrationTests/
```

### Run All Tests
```bash
dotnet test
```

### Generate Test Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## 📚 API Documentation | توثيق API

The API documentation is available via Swagger UI when running in development mode:
- **Swagger UI:** `https://localhost:5001/api/docs`
- **OpenAPI Spec:** `https://localhost:5001/swagger/v1/swagger.json`

## 🔧 Development Guidelines | إرشادات التطوير

### Code Style
- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use meaningful variable and method names
- Write XML documentation for public APIs
- Follow SOLID principles

### Git Workflow
1. Create feature branches from `main`
2. Use conventional commit messages
3. Submit pull requests for code review
4. Ensure all tests pass before merging

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/IsladebhiTohfa.Infrastructure --startup-project src/IsladebhiTohfa.Web

# Update database
dotnet ef database update --project src/IsladebhiTohfa.Infrastructure --startup-project src/IsladebhiTohfa.Web

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/IsladebhiTohfa.Infrastructure --startup-project src/IsladebhiTohfa.Web
```

## 🚀 Deployment | النشر

### Development Deployment
```bash
# Publish the application
dotnet publish src/IsladebhiTohfa.Web -c Release -o publish/

# Run published application
cd publish && dotnet IsladebhiTohfa.Web.dll
```

### Production Deployment with Docker
```bash
# Build production image
docker build -t islahi-tohfa:production -f Dockerfile.production .

# Deploy with Docker Compose
docker-compose -f docker-compose.production.yml up -d
```

### Azure Deployment
- Configure Azure App Service
- Set up Azure SQL Database
- Configure Application Insights
- Set environment variables for production

## 📊 Monitoring | المراقبة

### Application Insights
Configure Application Insights for production monitoring:
```json
{
  "ApplicationInsights": {
    "ConnectionString": "your-app-insights-connection-string"
  }
}
```

### Health Checks
Health checks are available at:
- `https://localhost:5001/health`

### Logging
Logs are configured for:
- Console output (Development)
- Application Insights (Production)
- File logging (configurable)

## 🤝 Contributing | المساهمة

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### How to Contribute
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

### Code of Conduct
Please read our [Code of Conduct](CODE_OF_CONDUCT.md) before contributing.

## 📄 License | الترخيص

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments | شكر وتقدير

- **MudBlazor Team** for the excellent UI components
- **Microsoft** for .NET and Azure platform
- **PDF.js Team** for PDF rendering capabilities
- **FontAwesome** for icons
- **All Contributors** who helped build this platform

## 📞 Support | الدعم الفني

### Get Help
- 📧 **Email:** support@islahitohfa.com
- 📖 **Documentation:** [docs.islahitohfa.com](https://docs.islahitohfa.com)
- 🐛 **Issues:** [GitHub Issues](https://github.com/islahitohfa/islahi-tohfa-platform/issues)
- 💬 **Discussions:** [GitHub Discussions](https://github.com/islahitohfa/islahi-tohfa-platform/discussions)

### Community
- 🌐 **Website:** [islahitohfa.com](https://islahitohfa.com)
- 📱 **Telegram:** [@islahitohfa](https://t.me/islahitohfa)
- 🐦 **Twitter:** [@islahitohfa](https://twitter.com/islahitohfa)
- 📺 **YouTube:** [Islahi Tohfa Channel](https://youtube.com/islahitohfa)

## 🗺️ Roadmap | خارطة الطريق

### Version 1.0 (Current)
- ✅ Basic book management
- ✅ User authentication
- ✅ PDF reader
- ✅ Search functionality
- ✅ Admin dashboard

### Version 1.1 (Q2 2024)
- 🔄 Advanced search filters
- 🔄 Book recommendations
- 🔄 Social features enhancement
- 🔄 Mobile app (React Native)
- 🔄 Offline reading support

### Version 2.0 (Q4 2024)
- 📋 AI-powered recommendations
- 📋 Multi-language support expansion
- 📋 Advanced analytics
- 📋 API for third-party integrations
- 📋 Enhanced accessibility features

## 📈 Statistics | الإحصائيات

- **Books:** 500+ Islamic educational books
- **Users:** 1000+ registered users
- **Categories:** 15+ book categories
- **Languages:** Arabic, English, Urdu support
- **Downloads:** 10,000+ book downloads

---

<div align="center">
  <strong>الإصلاحي تحفة - نشر المعرفة الإسلامية النافعة</strong><br>
  <em>Islahi Tohfa - Spreading Beneficial Islamic Knowledge</em><br><br>
  
  Made with ❤️ for the Islamic Ummah
  
  ![Islamic](https://img.shields.io/badge/Made%20for-Islamic%20Ummah-green.svg)
  ![Education](https://img.shields.io/badge/Purpose-Education-blue.svg)
  ![Free](https://img.shields.io/badge/Access-Free-brightgreen.svg)
</div>