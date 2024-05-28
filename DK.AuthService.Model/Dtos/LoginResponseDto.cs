namespace DK.AuthService.Model.Dtos
{
    public class LoginResponseDto
    {
        public string AuthenticationToken {  get; set; }
        public string Username { get; set; }
        public bool IsAdmin {  get; set; }
    }
}
