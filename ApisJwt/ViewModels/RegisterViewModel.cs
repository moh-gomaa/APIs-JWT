using System.ComponentModel.DataAnnotations;

namespace ApisJwt.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(250)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(250)]
        public string password { get; set; } = null!;
    }
}
