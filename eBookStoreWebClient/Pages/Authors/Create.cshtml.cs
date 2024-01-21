using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DataAccess.Models;

namespace Assignment2.eBookStore.Pages.Authors
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient client = null;
        private string AuthorsApiUrl = "";

        public CreateModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AuthorsApiUrl = "http://localhost:3000/api/authors/add-author";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Author Author { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string data = JsonSerializer.Serialize(Author);
            var response = await client.PostAsync(AuthorsApiUrl, new StringContent(data, Encoding.UTF8, "application/json"));
            return RedirectToPage("./Index");
        }
    }
}
