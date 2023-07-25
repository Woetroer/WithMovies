using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WithMovies.WebApi.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
