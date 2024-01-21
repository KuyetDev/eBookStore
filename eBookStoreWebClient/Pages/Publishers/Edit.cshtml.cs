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
using DataAccess;
using DataAccess.Models;
using DataAccess.DTOs.Publishers;

namespace Assignment2.eBookStore.Pages.Publishers
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string PublisherApiUrl = "";


        public EditModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PublisherApiUrl = "http://localhost:3000/api/publishers";
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
            List<Publisher> list = JsonSerializer.Deserialize<List<Publisher>>(strData, options);

            Publisher = list.AsQueryable().SingleOrDefault(x => x.PubId == id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(UpdatePublisherRequest updatePublisherRequest)
        {
            var newPubRequest = new UpdatePublisherRequest
            {
                City = Publisher.City,
                Country = Publisher.Country,
                PublisherName = Publisher.PublisherName,
                State = Publisher.State,
            };

            using (var respone = await client.PutAsJsonAsync(PublisherApiUrl + $"/update-publisher/{Publisher.PubId}", newPubRequest))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
