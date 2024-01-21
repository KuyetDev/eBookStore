using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.eBookStore.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string BookApiUrl = "";

        public DeleteModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BookApiUrl = "http://localhost:3000/api/books/";
        }

        [BindProperty]
      public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(BookApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Book> list;
            list = JsonSerializer.Deserialize<List<Book>>(strData, options);

            Book = list.AsQueryable().SingleOrDefault(x => x.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            string data = JsonSerializer.Serialize(Book);
            var response = await client.DeleteAsync(BookApiUrl + "delete-book/" + $"{Book.Id}");

            return RedirectToPage("./Index");
        }
    }
}
