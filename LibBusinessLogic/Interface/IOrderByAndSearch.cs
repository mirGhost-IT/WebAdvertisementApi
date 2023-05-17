using LibAdvertisementDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBusinessLogic.Interface
{
    public interface IOrderByAndSearch
    {
        Task<List<Advertisement>> Search(string str);
        Task<List<Advertisement>> MultiSort(string? search, string orderByQueryString);
        Task<List<Advertisement>> DateFiltering(DateTime startDate, DateTime endDate);
    }
}
