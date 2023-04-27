using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Linq;
using System.Reflection;
using System.Text;
using WebAdvertisementApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Linq.Dynamic.Core;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly AdvertisementContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, AdvertisementContext db, IWebHostEnvironment environment)
        {
            _logger = logger;
            _db = db;
            _environment = environment;
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
        /// Получить список объявлений мультисортировкой 
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="orderByQueryString">Список сортировок</param>
        /// <returns>Возврашает отсартированный список объявлений </returns>
        [HttpGet("GetMultiSort")]
        [ProducesResponseType(typeof(IEnumerable<Advertisement>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MultiSort(string? search, string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Advertisement).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            List<Advertisement> adv = new List<Advertisement>();
            if (search != null)
            {
                adv = await _db.Advertisements
                    .Include(i => i.User)
                    .Where(i => i.Text.ToLower().Contains(search.ToLower())
                        || i.Rating.ToString().ToLower().Contains(search.ToLower())
                        || i.User.Name.ToLower().Contains(search.ToLower())
                        || i.Created.Date.ToString().ToLower().Contains(search.ToLower())
                        || i.Number.ToString().ToLower().Contains(search.ToLower()))
                    .ToListAsync();
            }

            adv = await _db.Advertisements
                .Include(i=>i.User)
                .OrderBy(orderQuery).ToListAsync();

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
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        /// <returns>Возвращает изображение определенного объявления из бд заданных размеров</returns>
        [HttpGet("GetImageResize")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(SixLabors.ImageSharp.Image), StatusCodes.Status200OK)]
        [Produces("image/jpeg")]
        public async Task<IActionResult> GetImageResize(Guid id, int width, int height)
        {
            var advertisement = await _db.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }

            var imagePath = advertisement.ImageUrl;
            var imagePhysicalPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            if (!System.IO.File.Exists(imagePhysicalPath))
            {
                return NotFound();
            }

            using (var image = SixLabors.ImageSharp.Image.Load(imagePhysicalPath))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new JpegEncoder());
                    return File(ms.ToArray(), "image/jpeg");
                }
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
                .Where(i => i.Id == id).ToListAsync();
            _db.RemoveRange(delete);
            await _db.SaveChangesAsync();

            return Ok(new { Message = "Delete successfully" });
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

        /// <summary>
        /// Изменение объявления
        /// </summary>
        /// <param name="editAdvertisement">Модель объявления для изменения в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPut("Edit")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit([FromForm] EditAdvertisement editAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(editAdvertisement.ImageUrl.FileName);
            string filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await editAdvertisement.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;

            DateTime dateTimeUtc = editAdvertisement.Created.ToUniversalTime();
            editAdvertisement.Created = dateTimeUtc;

            dateTimeUtc = editAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            editAdvertisement.ExpirationDate = expirationDate;
            var adv = await _db.Advertisements
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.Id == editAdvertisement.Id);

            if (adv == null)
            {
                return Ok(new { Message = "Edit error" });
            }
            _db.Advertisements.Remove(adv);

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == editAdvertisement.UserId);

            Advertisement advertisement = new Advertisement
            {
                Text = editAdvertisement.Text,
                Number = editAdvertisement.Number,
                Rating = editAdvertisement.Rating,
                UserId = editAdvertisement.UserId,
                ImageUrl = imageUrl,
                User = user,
                Created = editAdvertisement.Created,
                ExpirationDate = editAdvertisement.ExpirationDate
            };

            await _db.Advertisements.AddAsync(advertisement);

            await _db.SaveChangesAsync();

            return Ok(new { Message = "Edit successfully" });
        }
        /// <summary>
        /// Добовляет новое объявление
        /// </summary>
        /// <param name="addAdvertisement">Модель объявления для добавления в формате FromForm</param>
        /// <returns>Возвращает строку подтверждения успеха или не успеха</returns>
        [HttpPost("Add")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Add([FromForm] AddAdvertisement addAdvertisement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(addAdvertisement.ImageUrl.FileName);
            string filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await addAdvertisement.ImageUrl.CopyToAsync(stream);
            }

            string imageUrl = "/images/" + fileName;

            var user = await _db.Users.FirstOrDefaultAsync(i => i.Id == addAdvertisement.UserId);

            Advertisement advertisement = new Advertisement
            {
                Text = addAdvertisement.Text,
                Number = addAdvertisement.Number,
                Rating = addAdvertisement.Rating,
                UserId = addAdvertisement.UserId,
                ImageUrl = imageUrl,
                User = user
            };
            DateTime dateTimeUnspecified = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            DateTime dateTimeUtc = dateTimeUnspecified.ToUniversalTime();
            advertisement.Created = dateTimeUtc;

            dateTimeUtc = addAdvertisement.ExpirationDate.ToUniversalTime();
            DateTime expirationDate = dateTimeUtc;
            advertisement.ExpirationDate = expirationDate;

            await _db.Advertisements.AddAsync(advertisement);
            await _db.SaveChangesAsync();

            return Ok(new { Message = "Add successfully" });
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


