using LibAdvertisementDB;
using LibBusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Class
{
    public static class Pagination
    {
        public static IQueryable<T> PaginationAdv<T>(this IQueryable<T> query, int page, int count)
        {
            return query.Skip(count * (page - 1))
                .Take(count);          
        }

    }
}
