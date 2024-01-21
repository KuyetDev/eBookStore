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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Text.Json;

namespace eBookStoreWebClient.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string PublisherApiUrl = "";
        [BindProperty]
        public List<Publisher> Publishers { get; set; }

        public IndexModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PublisherApiUrl = "http://localhost:3000/api/publishers";
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
                string filter = $"?$filter=contains(PublisherName, '{keyword}') ";
                PublisherApiUrl += filter;
            }
            HttpResponseMessage response = await client.GetAsync(PublisherApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Publishers = JsonSerializer.Deserialize<List<Publisher>>(strData, options);
            return Page();
        }
    }
}
