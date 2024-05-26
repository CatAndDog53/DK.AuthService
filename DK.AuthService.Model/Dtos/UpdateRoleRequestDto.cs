using System.ComponentModel.DataAnnotations;

namespace DK.AuthService.Model.Dtos
{
    public class UpdateRoleRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requested Role is required")]
        public string RequestedRole { get; set; }
    }
}
