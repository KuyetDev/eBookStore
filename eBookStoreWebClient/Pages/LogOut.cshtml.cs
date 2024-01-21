using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment2.eBookStore.Pages
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
