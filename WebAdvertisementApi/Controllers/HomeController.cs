using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAdvertisementApi.Models;

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

        /// <summary>
        /// Получить список объявлений
        /// </summary>
        /// <returns>Возвращет все объявления из бд</returns>
        [HttpGet("GetAdvertisements")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AllAdvertisements()
        {

            var adv = await _db.Advertisements
            .Include(i => i.User)
            .ToListAsync();

            return Ok(adv);
        }
        /// <summary>
        /// Получить отсартированный список объявлений по номеру по возрастанию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по номеру из бд</returns>
        [HttpGet("GetOrderByNumber")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByNumber()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Number).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по номеру по убыванию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по номеру из бд</returns>
        [HttpGet("GetOrderByDescNumber")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByDescNumber()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Number).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по рейтингу по возрастанию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по рейтингу из бд</returns>
        [HttpGet("GetOrderByRating")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByRating()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Rating).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по рейтингу по убыванию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по рейтингу из бд</returns>
        [HttpGet("GetOrderByDescRating")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByDescRating()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Rating).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по тексту по возрастанию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по тексту из бд</returns>
        [HttpGet("GetOrderByText")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByText()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Text).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по тексту по убыванию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по номеру из бд</returns>
        [HttpGet("GetOrderByDescText")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByDescText()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Text).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по дате создания по возрастанию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по дате создания из бд</returns>
        [HttpGet("GetOrderByCreated")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByCreated()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Created).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по дате создания по убыванию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по дате создания из бд</returns>
        [HttpGet("GetOrderByDescCreated")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByDescCreated()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Created).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по пользователю по возрастанию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по пользователю из бд</returns>
        [HttpGet("GetOrderByUser")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByUser()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.User.Name).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить отсартированный список объявлений по пользователю по убыванию
        /// </summary>
        /// <returns>Возврашает отсартированный список объявлений по пользователю из бд</returns>
        [HttpGet("GetOrderByDescUser")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderByDescUser()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.User).ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить изображение
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Возвращает изображение определенного объявления из бд</returns>
        [HttpGet("GetImage/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Image), StatusCodes.Status200OK)]
        [Produces("image/jpeg")]
        public async Task<IActionResult> GetImage(Guid id)
        {

            var image = await _db.Images.FirstOrDefaultAsync(x => x.AdvertisementId == id);
            if (image != null)
            {
                return File(image.Img, "image/jpeg");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Получить фильтрацию по поиску
        /// </summary>
        /// <param name="str">Строка поиска</param>
        /// <returns>Возврашает список объявлений отфильтрованных по поиску из бд</returns>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search(string str)
        {
            var adv = await _db.Advertisements
            .Include(i => i.User)
            .Include(i => i.Image)
            .Where(i => i.Text.ToLower().Contains(str.ToLower())
                || i.Rating.ToString().ToLower().Contains(str.ToLower())
                || i.User.Name.ToLower().Contains(str.ToLower())
                || i.Created.Date.ToString().ToLower().Contains(str.ToLower())
                || i.Number.ToString().ToLower().Contains(str.ToLower()))
            .ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Получить фильтрацию по поиску
        /// </summary>
        /// <param name="str">Строка поиска в формате JSON</param>
        /// <returns>Возврашает список объявлений отфильтрованных по поиску из бд</returns>
        [HttpPost("SearchJSON")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchJSON([FromBody] string str) => await Search(str);

        /// <summary>
        /// Получить фильтрацию по дате
        /// </summary>
        /// <param name="myDate">Даты с какого по какое в формате JSON</param>
        /// <returns>Возврашает список объявлений отфильтрованных по дате из бд</returns>
        [HttpGet("DateFiltering")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DateFiltering(MyDate myDate)
        {
            var adv = await _db.Advertisements
                .Include(i => i.User)
                .Include(i => i.Image)
                .Where(i => i.Created <= myDate.endDate.ToUniversalTime() && i.Created >= myDate.startDate.ToUniversalTime())
                .ToListAsync();

            return Ok(adv);
        }

        /// <summary>
        /// Удаляет объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Строку подтверждения успеха</returns>
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var delete = await _db.Advertisements
                .Include(i => i.User)
                .Include(i => i.Image)
                .Where(i => i.Id == id).ToListAsync();
            _db.RemoveRange(delete);
            await _db.SaveChangesAsync();

            return Ok(new {Message = "Delete successfully"});
        }

        /// <summary>
        /// Удаляет объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления в формате JSON</param>
        /// <returns>Строку подтверждения успеха</returns>
        [HttpDelete("DeleteJSON")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJSON([FromBody] Guid id) => await Delete(id);
    }
}


