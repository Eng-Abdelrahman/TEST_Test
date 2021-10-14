using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using _3aqarak.BLL.Interfaces;
using AutoMapper;
using _3aqarak.BLL.Models;
using _3aqarak.BLL.Dto;
using System.Web;
using System.IO;
using System.Data.Entity;
using _3aqarak.BLL.Helpers;

namespace _3aqarak.BLL.Services
{
   
    public class LandsDemandsService : ILandsDemandsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;

        public LandsDemandsService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _context = context;
        }
        //add function for filter  (Welcome in my Filter function this function use for filter Land demands ) 
        public async Task<List<LandsDemandsDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand_Id)
        {
            List<LandsDemandsDto> LandsDemandDtos = new List<LandsDemandsDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (SpaceTo == 0)
            {
                try
                {
                    SpaceTo = decimal.ToInt32((decimal)_uow.LandsDemandsCustRepo.GetMaxDecimalValue(m => m.MaxSpace));
                }
                catch (Exception)
                {

                    SpaceTo = 0;
                }
            }
            if (PriceTo == 0)
            {
                try
                {
                    decimal price = (decimal)_uow.LandsDemandsCustRepo.GetMaxDecimalValue(m => m.MaxPrice);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo = 0;
                }

            }
            if (fromDate == new DateTime(2017, 1, 18))
            {
                try
                {
                    fromDate = (DateTime)_uow.LandsDemandsCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    fromDate = new DateTime(2017, 1, 18);
                }

            }
            IEnumerable<tbl_LandsDemands> demands;
            if (regionIdFrom > 0 && PriceFrom > 0 && SpaceFrom > 0)
            {
                demands = await _uow.LandsDemandsRepo.FindAsync(d => (d.FK_LandsDemands_Regions_ToId <= regionIdTo && d.FK_LandsDemands_Regions_FromId >= regionIdFrom) &&
                 (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo)
                && !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate));
            }

            else
            {
                demands = await _uow.LandsDemandsRepo.FindAsync(d => !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate));
            }
            if (Demand_Id != 0)
            {
                demands = demands.Where(a => a.PK_LandsDemands_Id == Demand_Id && !a.IsDeleted && !a.IsClosed);
            }
            LandsDemandDtos = Mapper.Map<IEnumerable<tbl_LandsDemands>, IEnumerable<LandsDemandsDto>>(demands).ToList();
            return LandsDemandDtos;
        }
        //***************************************************************************************************************

        public async Task<SelectList> GetPayments()
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>((await _uow.PaymentRepo.GetAllAsync()).Where(c => c.IsDeleted == false).ToList()), "PK_PaymentMethods_Id", "PaymentMethod");
        }

        public async Task<SelectList> GetRegions()
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region");
        }

        public async Task<SelectList> GetTransactions()
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(t => t.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<SelectList> GetViews()
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name");
        }

        public async Task<SelectList> GetRegionById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region", await _uow.RegionRepo.FindAsync(reg => reg.PK_Regions_ID == id));
        }

        public async Task<SelectList> GetTransById(int regId, string name)
        {
            if (name != null)
            {
                List<TransDto> list = Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false && v.TransType == name)).ToList());
                return new SelectList(list, "PK_Transactions_Id", "TransType");
            }

            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == regId));
        }

        public async Task<SelectList> GetPaymentsId(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);
        }

        public async Task<MultiSelectList> GetViewsById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name", ids);
        }

        public async Task<SelectList> GetTrans(string name)
        {
            if (name != null)
            {
                List<TransDto> list = Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false && v.TransType == name)).ToList());
                return new SelectList(list, "PK_Transactions_Id", "TransType");
            }

            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<IConfirmation> SaveLandsDemand(LandsDemandsDto landsDemandsDto, int userId, int branchId)
        {
            tbl_LandsDemands newLandDemand = Mapper.Map<LandsDemandsDto, tbl_LandsDemands>(landsDemandsDto);
            newLandDemand.FK_LandsDemands_Branches_BranchId = branchId;
            newLandDemand.FK_LandsDemands_Users_CreatedBy = userId;
            newLandDemand.FK_LandsDemands_Users_ModidfiedBy = userId;
            newLandDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newLandDemand.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
            _uow.LandsDemandsRepo.Add(newLandDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            if (landsDemandsDto.ViewsArr != null && landsDemandsDto.ViewsArr.Any())
            {
                foreach (string view in landsDemandsDto.ViewsArr)
                {
                    tbl_LandDemandViews unitViews = new tbl_LandDemandViews
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_LandDemandView_Users_CreatedBy = userId,
                        FK_LandDemandView_Users_ModidfiedBy = userId,
                        FK_LandDemandView_LandDemand_DemandId = newLandDemand.PK_LandsDemands_Id,
                        FK_LandDemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.LandsDemandViewsRepo.Add(unitViews);
                }
                _conf.Valid = await _uow.SaveAsync() == landsDemandsDto.ViewsArr.Length;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            landsDemandsDto.PK_LandsDemands_Id = newLandDemand.PK_LandsDemands_Id;
            _conf.LandsAvailableAndExcluded = await FilterLandAvailable(landsDemandsDto);
            _conf.LandAvailables = await GetSellerName(_conf.LandAvailables);
            _conf.landsDemands.Clear();
            _conf.landsDemands.Add(Mapper.Map<tbl_LandsDemands, LandsDemandsDto>(newLandDemand));
            return _conf;
        }

        public async Task<(List<AvailableLandsDto>, AvailableLandsDto, List<AvailableLandsDto>)> FilterLandAvailable(LandsDemandsDto landsDemandsDto)
        {
            List<tbl_AvailableLands> availableLands = await GetLandAvailableMatch(landsDemandsDto);
            var availableLandsToDto = await GetSellerName(Mapper.Map<List<tbl_AvailableLands>, List<AvailableLandsDto>>(availableLands));
            availableLands = Mapper.Map<List<AvailableLandsDto>, List<tbl_AvailableLands>>(availableLandsToDto);
            List<AvailableLandsDto> availableLandsWithPreviews = new List<AvailableLandsDto>();
            tbl_AvailableLands availableLandsWithsameClient = availableLands.Find(c => c.FK_AvaliableLands_Clients_ClientId == landsDemandsDto.FK_LandsDemands_Clients_ClientId);
            if (availableLandsWithsameClient != null)
            {
                availableLands.Remove(availableLandsWithsameClient);
            }
            if (availableLands.Any())
            {
                List<tbl_PreviewHeaders> demandPreviewHeaders = (await _uow.PreviewRepo.FindAsync(p => p.FK_PreviewHeaders_Clients_BuyerId == landsDemandsDto.FK_LandsDemands_Clients_ClientId
                   && p.DemandUnit_Id == landsDemandsDto.PK_LandsDemands_Id
                   && !p.IsDeleted && p.IsNoDecision && p.ReviewDate >= DbFunctions.TruncateTime(DateTime.Today))).ToList();
                if (demandPreviewHeaders != null && demandPreviewHeaders.Any())
                {
                    foreach (var header in demandPreviewHeaders)
                    {
                        List<tbl_PreviewDetails> demandPreviewDetails = (await _uow.PreviewDetailRepo.FindAsync(detail => detail.Fk_PreviewDetails_PreviewHeaders_Id == header.PK_PreviewHeaders_Id
                         && detail.IsNoDecision && !detail.IsDeleted)).ToList();
                        if (demandPreviewDetails.Any() && demandPreviewDetails != null)
                        {
                            foreach (var detail in demandPreviewDetails)
                            {
                                if (availableLands.Exists(a => a.PK_AvailableLands_Id == detail.AvailableUnits_Id))
                                {
                                    tbl_AvailableLands availableLand = availableLands.Find(a => a.PK_AvailableLands_Id == detail.AvailableUnits_Id
                                    && a.FK_AvailableLands_Categories_CategoryId == detail.Category_Id);
                                    availableLandsWithPreviews.Add(Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(availableLand));
                                    availableLands.Remove(availableLand);
                                }
                            }
                        }
                    }
                }
            }
            return (Mapper.Map<List<tbl_AvailableLands>, List<AvailableLandsDto>>(availableLands), Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(availableLandsWithsameClient), availableLandsWithPreviews);
        }

        public async Task<List<tbl_AvailableLands>> GetLandAvailableMatch(LandsDemandsDto landsDemandsDto)
        {
            List<tbl_AvailableLands> availableLands = new List<tbl_AvailableLands>();
            int RegionCodeFrom = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == landsDemandsDto.FK_LandsDemands_Regions_FromId)).FirstOrDefault().RegCode;
            int RegionCodeTo = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == landsDemandsDto.FK_LandsDemands_Regions_ToId)).FirstOrDefault().RegCode;
            int[] codes = RegionCodeFrom < RegionCodeTo ? new int[] { RegionCodeFrom, RegionCodeTo } : new int[] { RegionCodeTo, RegionCodeFrom };
            var lastCode = codes.LastOrDefault();
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            if (payments != null && payments.Any(p => !p.IsMaster))
            {
                availableLands = (await _uow.AvailableLandsRepo.FindAsync(al => !al.IsDeleted && !al.IsClosed
                && al.Type == landsDemandsDto.Type
                && al.FK_AvailableLands_PaymentMethod_Id == landsDemandsDto.FK_LandsDemands_PaymentMethod_Id
                && al.Price <= landsDemandsDto.MaxPrice
                && al.Space <= landsDemandsDto.MaxSpace && al.Price >= landsDemandsDto.MinSpace
                && al.tbl_Regions.RegCode >= codes.FirstOrDefault() && al.tbl_Regions.RegCode <= lastCode
                && al.FK_AvailableLands_Categories_CategoryId == landsDemandsDto.FK_LandsDemands_Categories_Id
                )).ToList();
            }
            else
            {
                availableLands = (await _uow.AvailableLandsRepo.FindAsync(al => !al.IsDeleted && !al.IsClosed
                && al.Type == landsDemandsDto.Type
                && al.Price <= landsDemandsDto.MaxPrice
                && al.Space <= landsDemandsDto.MaxSpace && al.Price >= landsDemandsDto.MinSpace
                && al.tbl_Regions.RegCode >= codes.FirstOrDefault() && al.tbl_Regions.RegCode <= lastCode
                && al.FK_AvailableLands_Categories_CategoryId == landsDemandsDto.FK_LandsDemands_Categories_Id
                )).ToList();
            }
            return availableLands;
        }

        public async Task<List<AvailableLandsDto>> GetSellerName(List<AvailableLandsDto> availableLandsDtos)
        {
            foreach (var item in availableLandsDtos)
            {
                item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_AvaliableLands_Clients_ClientId)).FirstOrDefault().Name;
            }
            return availableLandsDtos;
        }

        public async Task<List<AvailableLandsDto>> GetLandAvailableMatchOnFly(LandsDemandsDto landsDemandsDto)
        {
            List<tbl_AvailableLands> availableLands = await GetLandAvailableMatch(landsDemandsDto);
            return Mapper.Map<List<tbl_AvailableLands>, List<AvailableLandsDto>>(availableLands);
        }

        public async Task<LandsDemandsDto> EditLandDemand(int id)
        {
            List<tbl_LandsDemands> landsDemands = (await _uow.LandsDemandsRepo.FindAsync(ld => ld.PK_LandsDemands_Id == id && !ld.IsDeleted)).ToList();
            if (landsDemands.Any() && landsDemands != null)
            {
                tbl_LandsDemands tbl_Lands = landsDemands.FirstOrDefault();
                LandsDemandsDto landsDemandsDto = Mapper.Map<tbl_LandsDemands, LandsDemandsDto>(tbl_Lands);
                landsDemandsDto.MaxPrice = tbl_Lands.MaxPrice;
                //landsDemandsDto.FK_LandsDemands_Views_ViewId = tbl_Lands.FK_LandsDemands_Views_ViewId;
                landsDemandsDto.ViewsIds = (await _uow.LandsDemandViewsRepo.FindAsync(ua => ua.FK_LandDemandView_LandDemand_DemandId == landsDemandsDto.PK_LandsDemands_Id)).Select(x => x.FK_LandDemandView_View_ViewId).ToArray();


                return landsDemandsDto;
            }
            return new LandsDemandsDto();
        }

        public async Task<IConfirmation> DeleteLandDemand(int id, int userId)
        {
            if (id > 0)
            {
                tbl_LandsDemands tbl_LandsDemands = (await _uow.LandsDemandsRepo.FindAsync(ld => ld.PK_LandsDemands_Id == id)).FirstOrDefault();
                if (tbl_LandsDemands != null)
                {
                    tbl_LandsDemands.IsDeleted = true;
                    tbl_LandsDemands.FK_LandsDemands_Users_ModidfiedBy = userId;
                }
                _uow.LandsDemandsRepo.Remove(tbl_LandsDemands);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف الطلب بنجاح";
                    return _conf;
                }

                List<tbl_LandDemandViews> unitViewsList = (await _uow.LandsDemandViewsRepo.FindAsync(ui => ui.FK_LandDemandView_LandDemand_DemandId == tbl_LandsDemands.PK_LandsDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_LandDemandViews item in unitViewsList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_LandDemandView_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.LandsDemandViewsRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                        return _conf;
                    }

                }

                _conf.Message = "تم الحذف بنجاح!";
                return _conf;
            }

            _conf.Valid = false;
            _conf.Message = "لم يتم الحذف حدث خطأ ما!";
            return _conf;
        }

        public async Task<IConfirmation> UpdateClientDemand(LandsDemandsDto landsDemandsDto, int userId, int branchId)
        {
            if (landsDemandsDto.PK_LandsDemands_Id > 0)
            {
                tbl_LandsDemands tbl_LandsDemand = (await _uow.LandsDemandsRepo.FindAsync(ld => ld.PK_LandsDemands_Id == landsDemandsDto.PK_LandsDemands_Id)).FirstOrDefault();
                if (tbl_LandsDemand != null)
                {
                    tbl_LandsDemand.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                    tbl_LandsDemand.FK_LandsDemands_Categories_Id = landsDemandsDto.FK_LandsDemands_Categories_Id;
                    tbl_LandsDemand.FK_LandsDemands_PaymentMethod_Id = landsDemandsDto.FK_LandsDemands_PaymentMethod_Id;
                    tbl_LandsDemand.MinPrice = landsDemandsDto.MinPrice;
                    tbl_LandsDemand.MaxPrice = landsDemandsDto.MaxPrice;
                    tbl_LandsDemand.MinSpace = landsDemandsDto.MinSpace;
                    tbl_LandsDemand.MaxSpace = landsDemandsDto.MaxSpace;
                    tbl_LandsDemand.FK_LandsDemands_Branches_BranchId = branchId;
                    tbl_LandsDemand.FK_LandsDemands_Regions_FromId = landsDemandsDto.FK_LandsDemands_Regions_FromId;
                    tbl_LandsDemand.FK_LandsDemands_Regions_ToId = landsDemandsDto.FK_LandsDemands_Regions_ToId;
                    tbl_LandsDemand.FK_LandsDemands_Users_ModidfiedBy = userId;
                    tbl_LandsDemand.FK_LandsDemands_Users_SalesId = landsDemandsDto.FK_LandsDemands_Users_SalesId;
                }
                _uow.LandsDemandsRepo.Update(tbl_LandsDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;

                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح";
                    return _conf;
                }

                List<tbl_LandDemandViews> unitViewsList = (await _uow.LandsDemandViewsRepo.FindAsync(ui => ui.FK_LandDemandView_LandDemand_DemandId == landsDemandsDto.PK_LandsDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_LandDemandViews item in unitViewsList)
                    {
                        _uow.LandsDemandViewsRepo.Remove(item);
                    }
                    if (landsDemandsDto.ViewsArr != null && landsDemandsDto.ViewsArr.Any())
                    {
                        foreach (string view in landsDemandsDto.ViewsArr)
                        {
                            tbl_LandDemandViews unitView = new tbl_LandDemandViews
                            {
                                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                                FK_LandDemandView_Users_CreatedBy = userId,
                                FK_LandDemandView_Users_ModidfiedBy = userId,
                                FK_LandDemandView_LandDemand_DemandId = tbl_LandsDemand.PK_LandsDemands_Id,
                                FK_LandDemandView_View_ViewId = int.Parse(view),
                            };
                            _uow.LandsDemandViewsRepo.Add(unitView);
                        }
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (!_conf.Valid)
                        {
                            _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                            return _conf;
                        }
                    }

                }



                _conf.Message = "تم الحفظ بنجاح!";
                _conf.LandsAvailableAndExcluded = await FilterLandAvailable(landsDemandsDto);
                _conf.landsDemands.Clear();
                _conf.landsDemands.Add(Mapper.Map<tbl_LandsDemands, LandsDemandsDto>(tbl_LandsDemand));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;
        }

        // add here function LandDemands
        public async Task<List<LandsDemandsDto>> LandDemands(int id)
        {
            List<LandsDemandsDto> LandDemands = Mapper.Map<List<tbl_LandsDemands>, List<LandsDemandsDto>>((await _uow.LandsDemandsRepo.
                FindAsync(a => a.FK_LandsDemands_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed)).ToList());

            if (LandDemands.Any() && LandDemands != null)
            {
                foreach (LandsDemandsDto item in LandDemands)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_LandsDemands_Transactions_Id))
                        .FirstOrDefault().TransType) + " " + ((await _uow.CatRepo.FindAsync(c => c.PK_Categories_Id == item.FK_LandsDemands_Categories_Id)).FirstOrDefault().CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
            }
            return new List<LandsDemandsDto>();
        }

        //contract functions

        public async Task<bool> CloseLandDemand(int id, int userId)
        {
            tbl_LandsDemands demand = (await _uow.LandsDemandsRepo.FindAsync(u => u.PK_LandsDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = true;
            demand.FK_LandsDemands_Users_ModidfiedBy = userId;
            _uow.LandsDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> ReleaseLandDemand(int id, int userId)
        {
            tbl_LandsDemands demand = (await _uow.LandsDemandsRepo.FindAsync(u => u.PK_LandsDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = false;
            demand.FK_LandsDemands_Users_ModidfiedBy = userId;
            _uow.LandsDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> UpdateLandDemandTransaction(int demandId, int transCode)
        {
            tbl_LandsDemands demand = (await _uow.LandsDemandsRepo.FindAsync(u => u.PK_LandsDemands_Id == demandId && !u.IsDeleted)).FirstOrDefault();
            if (demand != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                demand.FK_LandsDemands_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> GetInstantMatches(int demandId)
        {
            LandsDemandsDto demand = Mapper.Map<tbl_LandsDemands, LandsDemandsDto>((await _uow.LandsDemandsRepo.FindAsync(d => d.PK_LandsDemands_Id == demandId)).FirstOrDefault());
            if (demand != null)
            {
                _conf.LandsAvailableAndExcluded = await FilterLandAvailable(demand);

                foreach (AvailableLandsDto item in _conf.LandsAvailableAndExcluded.Item1)
                {
                    item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_AvaliableLands_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.landsDemands.Clear();
                _conf.landsDemands.Add(demand);

                return _conf;
            }
            return _conf;
        }

        public async Task<IConfirmation> CreateDemandForAvailable(AvailableLandsDto available, int userId, int branchId, int clientId, string notes)
        {
            tbl_LandsDemands newDemand = new tbl_LandsDemands();

            newDemand.FK_LandsDemands_Users_CreatedBy = userId;
            newDemand.FK_LandsDemands_Users_ModidfiedBy = userId;
            newDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newDemand.FK_LandsDemands_Branches_BranchId = branchId;
            newDemand.FK_LandsDemands_Categories_Id = Categories.Lands;
            newDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newDemand.FK_LandsDemands_Clients_ClientId = clientId;
            newDemand.FK_LandsDemands_PaymentMethod_Id = available.FK_AvailableLands_PaymentMethod_Id;
            newDemand.FK_LandsDemands_Regions_FromId = available.FK_AvailabeLands_Regions_RegionId;
            newDemand.FK_LandsDemands_Regions_ToId = available.FK_AvailabeLands_Regions_RegionId;
            newDemand.FK_LandsDemands_Transactions_Id = available.FK_AvaliableLands_Transactions_TransactionId;
            newDemand.Type = available.Type;
            newDemand.FK_LandsDemands_Users_SalesId = available.FK_AvaliableLands_Users_SalesId;
            newDemand.MaxPrice = available.Price;
            newDemand.MaxSpace = available.Space;
            newDemand.MinPrice = available.Price;
            newDemand.MinSpace = available.Space;
            newDemand.Notes = notes;
            newDemand.IsClosed = false;
            newDemand.IsDeleted = false;
            newDemand.IsMatched = false;
            _uow.LandsDemandsRepo.Add(newDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }

            _conf.Message = "تم الحفظ بنجاح!";
            _conf.LandAvailables.Add(available);
            _conf.landsDemands.Add(Mapper.Map<tbl_LandsDemands, LandsDemandsDto>(newDemand));
            return _conf;
        }


    }
}
