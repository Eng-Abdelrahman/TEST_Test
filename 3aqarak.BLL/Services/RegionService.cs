using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class RegionService:IRegionService
    {
        private readonly IUnitOfWork _uow;

        public RegionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> CheckRegCode(int code, int id )
        {
            if( id > 0)
            {
                var region =(await _uow.RegionRepo.FindAsync(u => u.RegCode == code && u.IsDeleted==false)).FirstOrDefault();

                return (region != null && region.PK_Regions_ID != id) ? true : false;
            }
            else if (id == 0)
            {
                var region =(await _uow.RegionRepo.FindAsync(u => u.RegCode == code && u.IsDeleted == false)).FirstOrDefault();

                return (region != null) ? true : false;
            }
            return false;
        }

        public async Task<bool> DeleteRegion(int id,int userId)
        {
            var DBRegion =(await _uow.RegionRepo.FindAsync(u => u.PK_Regions_ID == id)).FirstOrDefault();
            if (DBRegion != null)
            {
                DBRegion.IsDeleted = true;
                DBRegion.FK_Regions_Users_ModidfiedBy = userId;
                _uow.RegionRepo.Update(DBRegion);

            }

            return await _uow.SaveAsync() > 0;
        }

        public async Task<RegionDto> FindByID(int id)
        {
            var Region =(await _uow.RegionRepo.FindAsync(u => u.PK_Regions_ID == id)).FirstOrDefault();
            return (Region != null) ? Mapper.Map<tbl_Regions, RegionDto>(Region) : new RegionDto();
        }

        public async Task<List<RegionDto>> GetRegions()
        {
            var Region = await _uow.RegionRepo.FindAsync(u => u.IsDeleted == false);
            if (Region.Any() && Region != null)
            {
                return Mapper.Map<List<tbl_Regions>, List<RegionDto>>(Region.ToList());
            }
            return new List<RegionDto>();
        }

        public async Task<bool> SaveRegion(RegionDto Region, int userId)
        {
            if (Region.PK_Regions_ID == 0)
            {
                var newRegion = Mapper.Map<RegionDto, tbl_Regions>(Region);
                newRegion.FK_Regions_Users_CreatedBy = userId;
                newRegion.FK_Regions_Users_ModidfiedBy = userId;               
                _uow.RegionRepo.Add(newRegion);
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateRegion(RegionDto Region, int userId)
        {
            var DBRegion =(await _uow.RegionRepo.FindAsync(u => u.PK_Regions_ID == Region.PK_Regions_ID)).FirstOrDefault();
            if (DBRegion != null)
            {
                DBRegion.Region= Region.Region;
                DBRegion.RegCode = Region.RegCode;
                DBRegion.FK_Regions_Users_ModidfiedBy = userId;
                _uow.RegionRepo.Update(DBRegion);

            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
