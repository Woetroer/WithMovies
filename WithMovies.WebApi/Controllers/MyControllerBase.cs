using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WithMovies.Business;

namespace WithMovies.WebApi.Controllers
{
    public class MyControllerBase : ControllerBase
    {
        private readonly DataContext _dataContext;
        protected string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
