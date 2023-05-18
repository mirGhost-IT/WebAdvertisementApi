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
        Task<List<Advertisement>> MultiSort(string? search, string? orderByQueryString, DateTime? startDate, DateTime? endDate);
    }
}
