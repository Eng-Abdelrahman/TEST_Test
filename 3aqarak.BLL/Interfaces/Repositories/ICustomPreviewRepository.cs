using _3aqarak.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces.Repositories
{
    public interface ICustomPreviewRepository
    {
        Task<List<tbl_PreviewHeaders>> GetTodayPreviews();
        List<tbl_PreviewHeaders> GetTodayPreviewsWithoutAsync();
    }
}
