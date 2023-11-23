using System.ComponentModel.DataAnnotations;

namespace DK.AuthService.Model.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } 
    }
}
