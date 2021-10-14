using _3aqarak.BLL.Interfaces.Repositories;
using _3aqarak.BLL.Models;
using _3aqarak.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.DAL.Repositories.CustomRepositories
{
  

    public class CustomPreviewRepository : ICustomPreviewRepository
    {
        private readonly RealEstateDB _context;

        public CustomPreviewRepository(RealEstateDB context)
        {
            _context = context;
        }
        public async Task<List<tbl_PreviewHeaders>> GetTodayPreviews()
        {
            return await (from h in _context.tbl_PreviewHeaders
                          join d in _context.tbl_PreviewDetails
                          on h.PK_PreviewHeaders_Id equals d.Fk_PreviewDetails_PreviewHeaders_Id
                          where (!h.IsDeleted && DbFunctions.TruncateTime(h.ReviewDate) == DbFunctions.TruncateTime(DateTime.Today)
                               && !d.IsDeleted && !d.IsCancelled && !d.IsNoDecision && !d.IsPostponed && !d.IsRejected && !d.IsSucceded)
                               || !h.IsDeleted && DbFunctions.TruncateTime(h.ReviewDate) == DbFunctions.TruncateTime(DateTime.Today) && h.IsSuspended
                          group h by h.PK_PreviewHeaders_Id into grp
                          select grp.FirstOrDefault()).ToListAsync();
        }

        public  List<tbl_PreviewHeaders> GetTodayPreviewsWithoutAsync()
        {
            return  (from h in _context.tbl_PreviewHeaders
                          join d in _context.tbl_PreviewDetails
                          on h.PK_PreviewHeaders_Id equals d.Fk_PreviewDetails_PreviewHeaders_Id
                          where (!h.IsDeleted && DbFunctions.TruncateTime(h.ReviewDate) == DbFunctions.TruncateTime(DateTime.Today)
                               && !d.IsDeleted && !d.IsCancelled && !d.IsNoDecision && !d.IsPostponed && !d.IsRejected && !d.IsSucceded)
                               || !h.IsDeleted && DbFunctions.TruncateTime(h.ReviewDate) == DbFunctions.TruncateTime(DateTime.Today) && h.IsSuspended
                          group h by h.PK_PreviewHeaders_Id into grp
                          select grp.FirstOrDefault()).ToList();
        }
    }
}
