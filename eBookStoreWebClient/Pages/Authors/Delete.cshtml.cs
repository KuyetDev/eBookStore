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
using System.Text;

namespace eBookStoreWebClient.Pages.Authors
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string AuthorApiUrl = "";

        public DeleteModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AuthorApiUrl = "http://localhost:3000/api/authors/";
        }

        [BindProperty]
      public Author Author { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(AuthorApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Author> list;
            list = JsonSerializer.Deserialize<List<Author>>(strData, options);

            Author = list.AsQueryable().SingleOrDefault(x => x.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            string data = JsonSerializer.Serialize(Author);
            var response = await client.DeleteAsync(AuthorApiUrl + "delete-author/" + $"{Author.Id}");

            return RedirectToPage("./Index");
        }
    }
}
