using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Assignment2.eBookStore.Pages.Publishers
{
	public class CreateModel : PageModel
    {
		private readonly HttpClient client = null;
		private string PublisherApiUrl = "";
	

		public CreateModel()
        {
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			PublisherApiUrl = "http://localhost:3000/api/publishers/add-publisher";
		}

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Publisher Publisher { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid)
			{
				return Page();
			}

			string data = JsonSerializer.Serialize(Publisher);
			var response = await client.PostAsync(PublisherApiUrl, new StringContent(data, Encoding.UTF8, "application/json"));
			return RedirectToPage("./Index");
		}
    }
}
