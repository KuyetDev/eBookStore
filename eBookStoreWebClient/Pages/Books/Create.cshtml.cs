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
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess;
using DataAccess.Models;
using DataAccess.DTOs.Books;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.eBookStore.Pages.Books
{
    public class CreateModel : PageModel
    {
        private  AppDbContext _context = new AppDbContext();
        private readonly HttpClient client = null;
        private string BooksApiUrl = "";
        [BindProperty]
        public Book Book { get; set; }
        public CreateModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BooksApiUrl = "http://localhost:3000/api/books/add-book";
        }

        public IActionResult OnGet()
        {
            ViewData["PubId"] = new SelectList(_context.Publishers, "PubId", "PublisherName");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "LastName");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(CreateBookRequest createBookRequest)
        {
            var authorId = int.Parse(Request.Form["AuthortId"]);

            var newBookRequest = new CreateBookRequest
            {
                Title = Book.Title,
                Type = Book.Type,
                PubId = Book.PubId,
                Price = (double)Book.Price,
                Royalty = (float)Book.Royalty,
                YtdSales = (int)Book.YtdSales,
                Notes = Book.Notes,
                PublishedDate = Book.PublishedDate,
                Advance = Book.Advance,
                AuthorId = authorId,
            };
            using (var respone = await client.PostAsJsonAsync(BooksApiUrl, newBookRequest))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return RedirectToPage("./Index");

        }
    }
}
