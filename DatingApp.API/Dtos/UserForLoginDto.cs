using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
       [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 5,ErrorMessage="you mustspecify password between 5 and 8 Charecters")]
        public string Password { get; set; }
    }
}