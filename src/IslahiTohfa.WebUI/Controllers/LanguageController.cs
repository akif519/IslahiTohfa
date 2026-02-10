using IslahiTohfa.WebUI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace IslahiTohfa.WebUI.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        public IActionResult ChangeLanguage(string language, string returnUrl)
        {
            if (language == "en" || language == "ur")
            {
                LocalizationHelper.SetLanguage(HttpContext, language);
            }
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
