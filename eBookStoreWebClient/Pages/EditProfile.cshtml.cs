using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using DataAccess.Models;

namespace Assignment2.eBookStore.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "";

        public EditProfileModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UserApiUrl = "https://localhost:7087/api/user-management";
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<User> list;
            list = JsonSerializer.Deserialize<List<User>>(strData, options);
            String user_id = HttpContext.Session.GetString("user");
            if(user_id != null)
            {
                User = list.AsQueryable().SingleOrDefault(x => x.Id.ToString().Equals(user_id));
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string data = JsonSerializer.Serialize(User);
            var response = await client.PutAsync(UserApiUrl + $"/users/{User.Id}", new StringContent(data, Encoding.UTF8, "application/json"));

            return RedirectToPage("./Index");
        }
    }
}
