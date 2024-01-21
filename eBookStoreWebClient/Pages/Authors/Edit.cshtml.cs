using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using DataAccess.Models;

namespace Assignment2.eBookStore.Pages.Authors
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string AuthorApiUrl = "";

        public EditModel()
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string data = JsonSerializer.Serialize(Author);
            var response = await client.PutAsync(AuthorApiUrl+ "update-author/" + $"{Author.Id}", new StringContent(data, Encoding.UTF8, "application/json"));

            return RedirectToPage("./Index");
        }


    }
}
