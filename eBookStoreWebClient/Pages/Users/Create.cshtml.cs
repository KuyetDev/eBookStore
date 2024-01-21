using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess;
using DataAccess.Models;
using System.Net.Http.Headers;
using DataAccess.DTOs.Books;
using DataAccess.DTOs.User;

namespace eBookStoreWebClient.Pages.Users
{
    public class CreateModel : PageModel
    {
        private AppDbContext _context = new AppDbContext();
        private readonly HttpClient client = null;
        private string UsersApiUrl = "";

        [BindProperty]
        public User User { get; set; } = default!;
        public CreateModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UsersApiUrl = "http://localhost:3000/api/users/add-user";
        }

        public IActionResult OnGet()
        {
        ViewData["PubId"] = new SelectList(_context.Publishers, "PubId", "PublisherName");
        ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleDesc");
            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(CreateUserRequest createUserRequest)
        {
            var pubId = int.Parse(Request.Form["PubId"]);

            var newUserRequest = new CreateUserRequest
            {
                LastName = User.LastName,
                FirstName = User.FirstName,
                MiddleName = User.MiddleName,
                Email = User.Email,
                PubId = pubId,
                HireDate = DateTime.Now,
                Password = User.Password,
                RoleId = User.RoleId,
                Source = User.Source
            };
            using (var respone = await client.PostAsJsonAsync(UsersApiUrl, newUserRequest))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
