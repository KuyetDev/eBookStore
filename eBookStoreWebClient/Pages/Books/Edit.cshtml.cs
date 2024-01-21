using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using DataAccess;
using DataAccess.Models;
using DataAccess.DTOs.Books;

namespace Assignment2.eBookStore.Pages.Books
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly HttpClient client = null;
        private string BookApiUrl = "";

        public EditModel(AppDbContext context)
        {
            _context = context;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BookApiUrl = "http://localhost:3000/api/books/";
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ViewData["PubId"] = new SelectList(_context.Publishers, "PubId", "City");
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName");

            HttpResponseMessage response = await client.GetAsync(BookApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Book> list = JsonSerializer.Deserialize<List<Book>>(strData, options);

            Book = list.AsQueryable().SingleOrDefault(x => x.Id == id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(UpdateBookRequest updateBookRequest)
        {
            var authorId = int.Parse(Request.Form["AuthortId"]);

            var newBookRequest = new UpdateBookRequest
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
                authorId = authorId,
            };

            using (var respone = await client.PutAsJsonAsync(BookApiUrl + $"update-book/{Book.Id}",newBookRequest))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
