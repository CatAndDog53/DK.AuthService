using System.ComponentModel.DataAnnotations;

namespace DK.AuthService.Model.Dtos
{
    public class UpdateUserInfoDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
    }
}
