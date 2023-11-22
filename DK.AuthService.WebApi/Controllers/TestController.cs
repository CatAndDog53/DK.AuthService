using DK.AuthService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DK.AuthService.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string DataToAccess = "Data accessed successfully";
        private readonly IHttpContextAccessor _contextAccessor;
        public TestController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        [Route("GetData")]
        public IActionResult Get()
        {
            var user = _contextAccessor.HttpContext?.User;
            return Ok(DataToAccess);
        }

        [HttpGet]
        [Route("GetDataAllowedOnlyForUser")]
        [Authorize(Roles = PredefinedUserRoles.USER)]
        public IActionResult GetUserRole()
        {
            var user = _contextAccessor.HttpContext?.User;
            return Ok(DataToAccess);
        }

        [HttpGet]
        [Route("GetDataAllowedOnlyForAdmin")]
        [Authorize(Roles = PredefinedUserRoles.ADMIN)]
        public IActionResult GetAdminRole()
        {
            return Ok(DataToAccess);
        }
    }
}
