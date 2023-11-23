using System.ComponentModel.DataAnnotations;

namespace DK.AuthService.Model.Dtos
{
    public class UserInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
