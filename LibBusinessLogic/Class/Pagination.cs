using LibAdvertisementDB;
using LibBusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Class
{
    public class Pagination : IPagination
    {
        public static int Page { get; set; } = 1;
        public static int Count { get; set; } = 10;

        public void EditCount(int count)
        {
            Count = count;
        }

        public void EditPage(int page)
        {
            Page = page;
        }

        public List<Advertisement> SelectionPage(List<Advertisement> advertisements)
        {
            double temp = Convert.ToDouble(Page) / Count;
            int res = Convert.ToInt32(Math.Ceiling(temp));

            var adv = advertisements
                .Skip(Count * (Page - 1))
                .Take(Count).ToList();
            return adv;
        }
    }
}
