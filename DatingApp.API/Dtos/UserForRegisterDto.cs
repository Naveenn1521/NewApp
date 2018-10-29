using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 5,ErrorMessage="you mustspecify password between 5 and 8 Charecters")]
        public string password { get; set; }
    }
}