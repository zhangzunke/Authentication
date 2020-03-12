namespace IdentityServer.Controllers
{
    public class LoginViewModel
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}