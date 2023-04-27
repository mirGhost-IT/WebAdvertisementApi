using LibAdvertisementDB;
using LibBusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace LibBusinessLogic.Class
{
    public class OrderByAndSearch : IOrderByAndSearch
    {
        private readonly AdvertisementContext _db;
        public OrderByAndSearch(AdvertisementContext db)
        {
            _db = db;
        }
        public async Task<List<Advertisement>> MultiSort(string? search, string orderByQueryString)
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
                .Include(i => i.User)
                .OrderBy(orderQuery).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByCreated()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Created).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByDescCreated()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Created).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByDescNumber()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Number).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByDescRating()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Rating).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByDescText()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.Text).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByDescUser()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderByDescending(i => i.User).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByNumber()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Number).ToListAsync();

            return adv;
        }
        
        public async Task<List<Advertisement>> OrderByRating()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Rating).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByText()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.Text).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> OrderByUser()
        {
            var adv = await _db.Advertisements
             .Include(i => i.User).OrderBy(i => i.User.Name).ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> Search(string str)
        {
            var adv = await _db.Advertisements
            .Include(i => i.User)
            .Where(i => i.Text.ToLower().Contains(str.ToLower())
                || i.Rating.ToString().ToLower().Contains(str.ToLower())
                || i.User.Name.ToLower().Contains(str.ToLower())
                || i.Created.Date.ToString().ToLower().Contains(str.ToLower())
                || i.Number.ToString().ToLower().Contains(str.ToLower()))
            .ToListAsync();

            return adv;
        }

        public async Task<List<Advertisement>> DateFiltering(DateTime startDate, DateTime endDate)
        {
            var adv = await _db.Advertisements
                .Include(i => i.User)
                .Where(i => i.Created <= endDate.ToUniversalTime() && i.Created >= startDate.ToUniversalTime())
                .ToListAsync();

            return adv;
        }

    }
}
