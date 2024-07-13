using DataAPIs.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NavigationController : Controller
    {
        private readonly INavigationService _navigationService;

        public NavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet]
        [Route("GetNavigationItems")]
        public IActionResult GetNavigationItems()
        {
            var result = _navigationService.GetNavigationItems();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetChildrenNavigationItems")]
        public IActionResult GetChildrenNavigationItems([FromQuery]int parentId)
        {
            var result = _navigationService.GetChildrentNavigationItems(parentId);
            return Ok(result);
        }
    }
}
