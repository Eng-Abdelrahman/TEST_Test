using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface ICatService
    {
        Task<List<CatDto>> GetCats();
        Task<CatDto> FindByID(int id);
        Task<bool> UpdateCat(CatDto Cat, int userId);
        Task<bool> SaveCat(CatDto Cat, int userId);
        Task<bool> DeleteCat(int id,int userId);
    }
}
