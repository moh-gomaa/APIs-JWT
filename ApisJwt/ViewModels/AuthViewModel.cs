namespace ApisJwt.ViewModels
{
    public class AuthViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpireOn { get; set; }
    }
}
