using System.ComponentModel.DataAnnotations;

namespace DK.AuthService.Model.Dtos
{
    public class ChangeUserPasswordDto
    {
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
