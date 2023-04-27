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
        Task<List<Advertisement>> OrderByDescUser();
        Task<List<Advertisement>> OrderByUser();
        Task<List<Advertisement>> OrderByDescCreated();
        Task<List<Advertisement>> OrderByCreated();
        Task<List<Advertisement>> OrderByDescText();
        Task<List<Advertisement>> OrderByText();
        Task<List<Advertisement>> OrderByDescRating();
        Task<List<Advertisement>> OrderByRating();
        Task<List<Advertisement>> OrderByDescNumber();
        Task<List<Advertisement>> OrderByNumber();
        Task<List<Advertisement>> MultiSort(string? search, string orderByQueryString);
        Task<List<Advertisement>> DateFiltering(DateTime startDate, DateTime endDate);
    }
}
