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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace eBookStoreWebClient.Pages.Authors
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string AuthorApiUrl = "";
        public int count { get; set; }
        [BindProperty]
        public List<Author> Authors { get; set; }

        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AuthorApiUrl = "http://localhost:3000/api/authors";
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
                string filter = $"?$filter=contains(FirstName, '{keyword}') " +
            $"or contains(LastName, '{keyword}') " +
            $"or contains(Phone, '{keyword}') " +
            $"or contains(Address, '{keyword}') " +
            $"or contains(City, '{keyword}') " +
            $"or contains(State, '{keyword}') " +
            $"or contains(Email, '{keyword}')";
                AuthorApiUrl += filter;
            }
            HttpResponseMessage response = await client.GetAsync(AuthorApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Authors = JsonSerializer.Deserialize<List<Author>>(strData, options);
            return Page();
        }
    }
}
