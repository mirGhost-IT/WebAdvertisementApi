using LibAdvertisementDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Linq;
using System.Reflection;
using System.Text;
using WebAdvertisementApi.Models;
using static System.Net.Mime.MediaTypeNames;
using LibBusinessLogic.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace WebAdvertisementApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        IInfo _info;
        IOrderByAndSearch _orderByAndSearch;
        IAdvertisementInteraction _advertisementInteraction;
        IPagination _pagination;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;
        AdvertisementContext _db;

        public HomeController(ILogger<HomeController> logger,
            IPagination pagination,
            IAdvertisementInteraction advertisementInteraction,
            IOrderByAndSearch orderByAndSearch, IInfo info, 
            AdvertisementContext db, 
            IWebHostEnvironment environment)
        {
            _db = db;
            _pagination = pagination;
            _advertisementInteraction = advertisementInteraction;
            _orderByAndSearch = orderByAndSearch;
            _info = info;
            _logger = logger;
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
            var adv = await _info.AllAdvertisements();
            adv = _pagination.SelectionPage(adv);
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
            var adv = await _orderByAndSearch.MultiSort(search, orderByQueryString);
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByNumber();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByDescNumber();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByRating();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByDescRating();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByText();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByDescText();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByCreated();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByDescCreated();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByUser();
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.OrderByDescUser();
            adv = _pagination.SelectionPage(adv);

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
            var advertisement = await _info.InfoAdvertisement(id);
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

            var ms = _info.ImageResize(imagePhysicalPath, width, height);
            return File(ms.ToArray(), "image/jpeg");
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
            var adv = await _orderByAndSearch.Search(str);
            adv = _pagination.SelectionPage(adv);

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
            var adv = await _orderByAndSearch.DateFiltering(myDate.startDate, myDate.endDate);
            adv = _pagination.SelectionPage(adv);

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

            var delete = await _info.InfoAdvertisement(id);
            if (delete == null)
            {
                return NotFound();
            }
            var imagePath = delete.ImageUrl;
            var imagePhysicalPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
            if (!System.IO.File.Exists(imagePhysicalPath))
            {
                return NotFound();
            }

            await _advertisementInteraction.Remove(delete, imagePhysicalPath);

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

            Advertisement advertisement = await _info.InfoAdvertisement(editAdvertisement.Id);
            if (advertisement == null)
            {
                return NotFound();
            }

            advertisement.Text = editAdvertisement.Text;
            advertisement.ImageUrl = imageUrl;
            advertisement.Rating = editAdvertisement.Rating;
            advertisement.ExpirationDate = editAdvertisement.ExpirationDate;

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

            var user = await _info.GetUser(addAdvertisement.UserId);
            if (user == null)
            {
                return NotFound();
            }

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

            await _advertisementInteraction.Add(advertisement);

            return Ok(new { Message = "Add successfully" });
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpGet("GetInfo")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Info(Guid id)
        {
            var res = await _info.InfoAdvertisement(id);
            return Ok(res);
        }

        /// <summary>
        /// Получает 1 объявление по id из бд
        /// </summary>
        /// <param name="id">Id объявления в формате JSON</param>
        /// <returns>Возвращает 1 объявление по id из бд</returns>
        [HttpPost("GetInfoJSON")]
        [ProducesResponseType(typeof(Advertisement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InfoJSON([FromBody] Guid id) => await Info(id);

        /// <summary>
        /// Поставить новое значение страницы 
        /// </summary>
        /// <param name="number">Номер страницы</param>
        /// <returns>Возвращает строку подтверждения успеха</returns>
        [HttpGet("NewPage")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult NewPage(int number)
        {
            _pagination.EditPage(number);
            return Ok(new { Message = "All good" });
        }

        /// <summary>
        /// Поставить новое значение страницы 
        /// </summary>
        /// <param name="number">Номер страницы в формате JSON</param>
        /// <returns>Возвращает строку подтверждения успеха</returns>
        [HttpPost("NewPageJSON")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult NewPageJSON([FromBody] int number) => NewPage(number);

        /// <summary>
        /// Поставить новое значение количество объявлений на странице 
        /// </summary>
        /// <param name="number">Количество объявлений на странице</param>
        /// <returns>Возвращает строку подтверждения успеха</returns>
        [HttpGet("NewCount")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult NewCount(int number)
        {
            _pagination.EditCount(number);
            return Ok(new { Message = "All good" });
        }

        /// <summary>
        /// Поставить новое значение количество объявлений на странице 
        /// </summary>
        /// <param name="number">Количество объявлений на странице в формате JSON</param>
        /// <returns>Возвращает строку подтверждения успеха</returns>
        [HttpPost("NewCountJSON")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult NewCountJSON([FromBody] int number) => NewCount(number);
    }
}


