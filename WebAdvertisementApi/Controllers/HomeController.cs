using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AdvertisementContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AdvertisementContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "GetAdvertisements")]
        public async Task<IActionResult> Get()
        {

            var adv = await _db.Advertisements
            .Include(i => i.User)
            .Include(i => i.Image).ToListAsync();

            return Ok(adv);
        }
    }
}

