using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models;
using System.Net.Http.Headers;
using DataAccess.DTOs.Publishers;
using System.Text.Json;
using DataAccess.DTOs.User;

namespace eBookStoreWebClient.Pages.Users
{
    public class EditModel : PageModel
    {

        private AppDbContext _context = new AppDbContext();
        private readonly HttpClient client = null;
        private string UserApiUrl = "";

        [BindProperty]
        public User User { get; set; } = default!;

        public EditModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "http://localhost:3000/api/users";
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userid = HttpContext.Session.GetString("user");
            if (userid != null)
            {
                id = int.Parse(userid);
            }
            ViewData["PubId"] = new SelectList(_context.Publishers, "PubId", "PublisherName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleDesc");
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<User> list = JsonSerializer.Deserialize<List<User>>(strData, options);

            User = list.AsQueryable().SingleOrDefault(x => x.PubId == id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(UpdateUserRequest updateUserRequest)
        {
            var uid = HttpContext.Session.GetString("user");
            var newUserRequest = new UpdateUserRequest
            {
               Email = User.Email,
               Source = User.Source,
               RoleId = User.RoleId,
               Password = User.Password,
               HireDate = User.HireDate,
               PubId = User.PubId,
               MiddleName = User.MiddleName,
               FirstName = User.FirstName,
               LastName = User.LastName
            };

            using (var respone = await client.PutAsJsonAsync(UserApiUrl + $"/update-user/{User.PubId}", newUserRequest))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            if (!string.IsNullOrEmpty(uid))
            {
                return RedirectToPage("/Homes/UserHomePage");
            }
            return RedirectToPage("./Index");
        }
    }
}
