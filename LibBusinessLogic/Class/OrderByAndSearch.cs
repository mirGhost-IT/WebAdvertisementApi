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
        public async Task<List<Advertisement>> MultiSort(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate)
        {
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
                    .PaginationAdv()
                    .ToListAsync();
            }                   

            if (orderByQueryString!=null)
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

                adv = await _db.Advertisements
                .Include(i => i.User)
                .PaginationAdv()
                .OrderBy(orderQuery).ToListAsync();
            }
            
            if (startDate!=null && endDate!= null)
            {
                try
                {

                    adv = await _db.Advertisements
                    .Include(i => i.User)
                    .PaginationAdv()
                    .Where(i => i.Created <= DateTime.Parse(endDate.ToString()).ToUniversalTime() && i.Created >= DateTime.Parse(startDate.ToString()).ToUniversalTime())
                    .ToListAsync();
                }
                catch (Exception)
                {
                }               
                       
            }
            return adv;
        }




    }
}
