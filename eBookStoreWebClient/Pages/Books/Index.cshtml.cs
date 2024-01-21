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

namespace eBookStoreWebClient.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string BookApiUrl = "";
        [BindProperty]
        public List<Book> Books { get; set; }

        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BookApiUrl = "http://localhost:3000/api/books";
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
                string filter = $"?$filter=contains(Title, '{keyword}') " +
            $"or contains(Type, '{keyword}') ";
                BookApiUrl += filter;
            }
            HttpResponseMessage response = await client.GetAsync(BookApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Books = JsonSerializer.Deserialize<List<Book>>(strData, options);
            return Page();
        }
    }
}
