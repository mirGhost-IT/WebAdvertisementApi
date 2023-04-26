using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoAdvertisementController : ControllerBase
    {
        private readonly AdvertisementContext _db;
        private readonly ILogger<InfoAdvertisementController> _logger;

        public InfoAdvertisementController(ILogger<InfoAdvertisementController> logger, AdvertisementContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpGet("Info")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Info(Guid id)
        {
            var res = await _db.Advertisements
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == id);
            return Ok(res);
        }
        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления в формате JSON</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpGet("InfoJSON")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InfoJSON([FromBody] Guid id) => await Info(id);
    }
}
