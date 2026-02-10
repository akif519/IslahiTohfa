using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IslahiTohfa.WebUI.Helpers
{
    public static class LocalizationHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            ["en"] = new Dictionary<string, string>
            {
                // Common
                ["AppName"] = "Islahi Tohfa",
                ["Home"] = "Home",
                ["Books"] = "Books",
                ["About"] = "About Us",
                ["Contact"] = "Contact",
                ["Login"] = "Login",
                ["Register"] = "Register",
                ["Logout"] = "Logout",
                ["Profile"] = "Profile",
                ["Dashboard"] = "Dashboard",
                ["Search"] = "Search",
                ["Submit"] = "Submit",
                ["Cancel"] = "Cancel",
                ["Save"] = "Save",
                ["Delete"] = "Delete",
                ["Edit"] = "Edit",
                ["Details"] = "Details",
                ["Back"] = "Back",
                ["Next"] = "Next",
                ["Previous"] = "Previous",
                ["ViewAll"] = "View All",
                ["Language"] = "Language",
                
                // Homepage
                ["Tagline"] = "Islamic Books Online Library - Read and Download Free PDF Books",
                ["SearchPlaceholder"] = "Search for book or author...",
                ["FeaturedBooks"] = "Featured Books",
                ["RecentBooks"] = "Recent Books",
                ["PopularBooks"] = "Popular Books",
                ["TotalBooks"] = "Books",
                ["TotalUsers"] = "Users",
                ["TotalDownloads"] = "Downloads",
                ["TotalComments"] = "Comments",
                
                // Book
                ["BookCollection"] = "Book Collection",
                ["AllCategories"] = "All Categories",
                ["Islamic"] = "Islamic",
                ["Quran"] = "Quran",
                ["Hadith"] = "Hadith",
                ["Fiqh"] = "Fiqh",
                ["Aqeedah"] = "Aqeedah",
                ["Seerah"] = "Seerah",
                ["History"] = "History",
                ["Author"] = "Author",
                ["Publisher"] = "Publisher",
                ["PublicationYear"] = "Publication Year",
                ["Pages"] = "Pages",
                ["Category"] = "Category",
                ["Status"] = "Status",
                ["Views"] = "Views",
                ["Downloads"] = "Downloads",
                ["Likes"] = "Likes",
                ["Rating"] = "Rating",
                ["Read"] = "Read",
                ["Download"] = "Download",
                ["Like"] = "Like",
                ["Liked"] = "Liked",
                ["NoBooks"] = "No books found",
                ["TryDifferentSearch"] = "Please try a different search",
                
                // Book Details
                ["BookDetails"] = "Book Details",
                ["Description"] = "Description",
                ["BookInformation"] = "Book Information",
                ["Comments"] = "Comments",
                ["AddComment"] = "Add Your Comment",
                ["YourComment"] = "Your Comment",
                ["LoginToRead"] = "Login to read and download books",
                ["LoginToComment"] = "Login to add comments",
                ["CommentSubmitted"] = "Your comment has been submitted and is pending approval",
                
                // Admin
                ["AdminPanel"] = "Admin Panel",
                ["BookManagement"] = "Book Management",
                ["CommentManagement"] = "Comment Management",
                ["AddNewBook"] = "Add New Book",
                ["PendingComments"] = "Pending Comments",
                ["ApprovedComments"] = "Approved Comments",
                ["RejectedComments"] = "Rejected Comments",
                ["Approve"] = "Approve",
                ["Reject"] = "Reject",
                ["Reason"] = "Reason",
                
                // About
                ["OurMission"] = "Our Mission",
                ["MissionText"] = "Islahi Tohfa is a non-profit Islamic digital library aimed at making Islamic books easily accessible.",
                ["WideCollection"] = "Wide Collection",
                ["CollectionDesc"] = "Books on Quran, Hadith, Fiqh, Aqeedah, Seerah and other topics",
                ["FreeDownload"] = "Free Download",
                ["FreeDesc"] = "All books are completely free and without any restrictions",
                ["MobileFriendly"] = "Mobile Friendly",
                ["MobileDesc"] = "Can be easily used on all devices",
                ["EasySearch"] = "Easy Search",
                ["SearchDesc"] = "Quickly find the desired book",
                
                // Contact
                ["ContactUs"] = "Contact Us",
                ["ContactSubtitle"] = "We are here to help you",
                ["Email"] = "Email",
                ["Phone"] = "Phone",
                ["Address"] = "Address",
                ["SendMessage"] = "Send Message",
                ["Name"] = "Name",
                ["Subject"] = "Subject",
                ["Message"] = "Message",
                ["FollowUs"] = "Follow us on social media",
                
                // Footer
                ["FooterText"] = "Islamic books online library. Download free PDF books.",
                ["Links"] = "Links",
                ["AllRightsReserved"] = "All rights reserved.",
            },
            ["ur"] = new Dictionary<string, string>
            {
                // Common - Urdu
                ["AppName"] = "اصلاحی تحفہ",
                ["Home"] = "صفحہ اول",
                ["Books"] = "کتابیں",
                ["About"] = "ہمارے بارے میں",
                ["Contact"] = "رابطہ",
                ["Login"] = "لاگ ان",
                ["Register"] = "رجسٹر",
                ["Logout"] = "لاگ آؤٹ",
                ["Profile"] = "پروفائل",
                ["Dashboard"] = "ڈیش بورڈ",
                ["Search"] = "تلاش",
                ["Submit"] = "جمع کرائیں",
                ["Cancel"] = "منسوخ",
                ["Save"] = "محفوظ کریں",
                ["Delete"] = "حذف",
                ["Edit"] = "ترمیم",
                ["Details"] = "تفصیلات",
                ["Back"] = "واپس",
                ["Next"] = "اگلا",
                ["Previous"] = "پچھلا",
                ["ViewAll"] = "تمام دیکھیں",
                ["Language"] = "زبان",
                
                // Homepage - Urdu
                ["Tagline"] = "اسلامی کتابوں کی آن لائن لائبریری - مفت PDF کتابیں پڑھیں اور ڈاؤن لوڈ کریں",
                ["SearchPlaceholder"] = "کتاب یا مصنف کی تلاش کریں...",
                ["FeaturedBooks"] = "نمایاں کتابیں",
                ["RecentBooks"] = "حالیہ کتابیں",
                ["PopularBooks"] = "مقبول کتابیں",
                ["TotalBooks"] = "کتابیں",
                ["TotalUsers"] = "صارفین",
                ["TotalDownloads"] = "ڈاؤن لوڈز",
                ["TotalComments"] = "تبصرے",
                
                // Book - Urdu
                ["BookCollection"] = "کتابوں کا مجموعہ",
                ["AllCategories"] = "تمام زمرے",
                ["Islamic"] = "اسلامیات",
                ["Quran"] = "قرآن",
                ["Hadith"] = "حدیث",
                ["Fiqh"] = "فقہ",
                ["Aqeedah"] = "عقیدہ",
                ["Seerah"] = "سیرت",
                ["History"] = "تاریخ",
                ["Author"] = "مصنف",
                ["Publisher"] = "ناشر",
                ["PublicationYear"] = "سال اشاعت",
                ["Pages"] = "صفحات",
                ["Category"] = "زمرہ",
                ["Status"] = "حالت",
                ["Views"] = "ملاحظات",
                ["Downloads"] = "ڈاؤن لوڈز",
                ["Likes"] = "پسند",
                ["Rating"] = "درجہ بندی",
                ["Read"] = "پڑھیں",
                ["Download"] = "ڈاؤن لوڈ کریں",
                ["Like"] = "پسند کریں",
                ["Liked"] = "پسند ہے",
                ["NoBooks"] = "کوئی کتاب نہیں ملی",
                ["TryDifferentSearch"] = "براہ کرم مختلف تلاش کی کوشش کریں",
                
                // Book Details - Urdu
                ["BookDetails"] = "کتاب کی تفصیلات",
                ["Description"] = "تفصیل",
                ["BookInformation"] = "کتاب کی معلومات",
                ["Comments"] = "تبصرے",
                ["AddComment"] = "اپنا تبصرہ لکھیں",
                ["YourComment"] = "آپ کا تبصرہ",
                ["LoginToRead"] = "کتاب پڑھنے اور ڈاؤن لوڈ کرنے کے لیے لاگ ان کریں",
                ["LoginToComment"] = "تبصرہ کرنے کے لیے لاگ ان کریں",
                ["CommentSubmitted"] = "آپ کا تبصرہ جمع کرا دیا گیا ہے اور منظوری کے منتظر ہے",
                
                // Admin - Urdu
                ["AdminPanel"] = "ایڈمن پینل",
                ["BookManagement"] = "کتابوں کا انتظام",
                ["CommentManagement"] = "تبصروں کا انتظام",
                ["AddNewBook"] = "نئی کتاب شامل کریں",
                ["PendingComments"] = "زیر التواء تبصرے",
                ["ApprovedComments"] = "منظور شدہ تبصرے",
                ["RejectedComments"] = "مسترد تبصرے",
                ["Approve"] = "منظور کریں",
                ["Reject"] = "مسترد کریں",
                ["Reason"] = "وجہ",
                
                // About - Urdu
                ["OurMission"] = "ہمارا مقصد",
                ["MissionText"] = "اصلاحی تحفہ ایک غیر منافع بخش اسلامی ڈیجیٹل لائبریری ہے جس کا مقصد اسلامی کتابوں کو آسانی سے دستیاب کرنا ہے۔",
                ["WideCollection"] = "وسیع مجموعہ",
                ["CollectionDesc"] = "قرآن، حدیث، فقہ، عقیدہ، سیرت اور دیگر موضوعات پر کتابیں",
                ["FreeDownload"] = "مفت ڈاؤن لوڈ",
                ["FreeDesc"] = "تمام کتابیں مکمل طور پر مفت اور بغیر کسی پابندی کے دستیاب",
                ["MobileFriendly"] = "موبائل فرینڈلی",
                ["MobileDesc"] = "تمام آلات پر آسانی سے استعمال کی جا سکتی ہے",
                ["EasySearch"] = "آسان تلاش",
                ["SearchDesc"] = "مطلوبہ کتاب تیزی سے تلاش کریں",
                
                // Contact - Urdu
                ["ContactUs"] = "ہم سے رابطہ کریں",
                ["ContactSubtitle"] = "ہم آپ کی مدد کے لیے موجود ہیں",
                ["Email"] = "ای میل",
                ["Phone"] = "فون",
                ["Address"] = "پتہ",
                ["SendMessage"] = "پیغام بھیجیں",
                ["Name"] = "نام",
                ["Subject"] = "موضوع",
                ["Message"] = "پیغام",
                ["FollowUs"] = "سوشل میڈیا پر ہمارے ساتھ جڑیں",
                
                // Footer - Urdu
                ["FooterText"] = "اسلامی کتابوں کی آن لائن لائبریری۔ مفت PDF کتابیں ڈاؤن لوڈ کریں۔",
                ["Links"] = "روابط",
                ["AllRightsReserved"] = "تمام حقوق محفوظ ہیں۔",
            }
        };

        public static string GetCurrentLanguage(HttpContext context)
        {
            return context.Session.GetString("Language") ?? "en";
        }

        public static void SetLanguage(HttpContext context, string language)
        {
            context.Session.SetString("Language", language);
        }

        public static string GetText(string key, HttpContext context)
        {
            var language = GetCurrentLanguage(context);
            
            if (Translations.ContainsKey(language) && Translations[language].ContainsKey(key))
            {
                return Translations[language][key];
            }
            
            // Fallback to English
            if (Translations["en"].ContainsKey(key))
            {
                return Translations["en"][key];
            }
            
            return key; // Return key if translation not found
        }
        
        public static string GetDirection(HttpContext context)
        {
            var language = GetCurrentLanguage(context);
            return language == "ur" ? "rtl" : "ltr";
        }
        
        public static string GetTextDirection(HttpContext context)
        {
            return GetDirection(context);
        }
    }
}
