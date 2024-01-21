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

namespace Assignment2.eBookStore.Pages.Publishers
{
    public class DeleteModel : PageModel
    {

        private readonly HttpClient client = null;
        private string PublisherApiUrl = "";

        public DeleteModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PublisherApiUrl = "http://localhost:3000/api/publishers/";
        }

        [BindProperty]
      public Publisher Publisher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(PublisherApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Publisher> list;
            list = JsonSerializer.Deserialize<List<Publisher>>(strData, options);

            Publisher = list.AsQueryable().SingleOrDefault(x => x.PubId == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            string data = JsonSerializer.Serialize(Publisher);
            var response = await client.DeleteAsync(PublisherApiUrl + "delete-publisher/" + $"{Publisher.PubId}");

            return RedirectToPage("./Index");
        }
    }
}
