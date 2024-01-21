using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace eBookStoreWebClient.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient client = null;
        private string UsersApiUrl = "";
        [BindProperty]
        public List<User> Users { get; set; }

        public LoginModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            UsersApiUrl = "http://localhost:3000/api/users";
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var email = Request.Form["email"];
                var pass = Request.Form["pass"];
                if (email == "" || pass == "")
                {
                    ViewData["msg"] = "fill in the blank";
                    return Page();
                }
                HttpResponseMessage response = await client.GetAsync(UsersApiUrl);
                var strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                Users = JsonSerializer.Deserialize<List<User>>(strData, options);
                foreach (var item in Users)
                {
                    if (item.Email.Equals(email) && item.Password.Equals(pass))
                    {
                        if (item.RoleId == 1)
                        {
                            HttpContext.Session.SetString("admin", "ok");
                            return Redirect("./Homes/AdminHomePage");
                        }
                        else if (item.RoleId == 2)
                        {
                            HttpContext.Session.SetString("user", item.Id.ToString());
                            return Redirect("./Homes/UserHomePage");
                        }
                    }
                }
                ViewData["msg"] = "Wrong Email or Password!";
                return Page();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
                throw;
            }
          
        }
    }
}
