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
        public static int Page { get; set; } = 1;
        public static int Count { get; set; } = 10;

        public static void EditCount(int count)
        {
            Count = count;
        }

        public static void EditPage(int page)
        {
            Page = page;
        }
        public static IQueryable<Advertisement> PaginationAdv(this IQueryable<Advertisement> query)
        {
            return query.Skip(Count * (Page - 1))
                .Take(Count);          
        }

    }
}
