using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IViewService
    {
        Task<List<ViewDto>> GetViews();
        Task<ViewDto> FindByID(int id);
        Task<bool> UpdateView(ViewDto viewn, int userId);
        Task<bool> SaveView(ViewDto view, int userId);
        Task<bool> DeleteView(int id, int userId);     
    }
}
