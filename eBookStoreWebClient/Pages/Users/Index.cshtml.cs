using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eBookStoreWebClient.Pages.Users
{
    public class IndexModel : PageModel
    {
        private AppDbContext _context = new AppDbContext();
        private readonly HttpClient client = null;
        private string UserApiUrl = "";
        [BindProperty]
        public List<User> Users { get; set; }

        public IndexModel(AppDbContext context)
        {
          
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "http://localhost:3000/api/users";
        }


        public async Task<IActionResult> OnGetAsync(string keyword)
        {
            var username = HttpContext.Session.GetString("admin");
            if (username == null)
            {
                return RedirectToPage("/Login");
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                string filter = $"?$filter=contains(Email, '{keyword}') ";
                UserApiUrl += filter;
            }
         
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Users = JsonSerializer.Deserialize<List<User>>(strData, options);
            return Page();
        }
    }
}
