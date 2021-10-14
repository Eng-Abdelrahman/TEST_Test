using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IRegionService
    {
        Task<List<RegionDto>> GetRegions();
        Task<RegionDto> FindByID(int id);
        Task<bool> UpdateRegion(RegionDto Region, int userId);
        Task<bool> SaveRegion(RegionDto Region, int userId);
        Task<bool> DeleteRegion(int id,int userId);
        Task<bool> CheckRegCode(int code, int id = 0);
    }
}
