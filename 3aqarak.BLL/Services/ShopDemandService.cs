
using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    

    public class ShopDemandService : IShopDemandService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public ShopDemandService(IUnitOfWork uow, IConfirmation conf, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _session = session;
            _context = context;
        }

        public async Task<bool> CloseDemand(int id, int userId)
        {
            tbl_ShopDemands demand = (await _uow.ShopDemandsRepo.FindAsync(u => u.PK_ShopDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = true;
            demand.FK_ShopDemands_Users_ModidfiedBy = userId;
            _uow.ShopDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> DeleteShopDemand(int id, int userId)
        {
            if (id > 0)
            {
                tbl_ShopDemands DBclientDemand = (await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.IsDeleted = true;
                    DBclientDemand.FK_ShopDemands_Users_ModidfiedBy = userId;
                }
                _uow.ShopDemandsRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف الطلب بنجاح";
                    return _conf;
                }



                List<tbl_ShopDemandAccessories> unitAccessList = (await _uow.ShopDemandAccessoriesRepo.FindAsync(ui => ui.FK_ShopDemandAccessories_ShopDemand_Id == DBclientDemand.PK_ShopDemands_Id)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_ShopDemandAccessories item in unitAccessList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_ShopDemandAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.ShopDemandAccessoriesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الكماليات بنجاح!";
                        return _conf;
                    }
                }
                List<tbl_ShopDemandViews> unitViewsList = (await _uow.ShopDemandViewsRepo.FindAsync(ui => ui.FK_ShopDemandView_ShopDemand_DemandId == DBclientDemand.PK_ShopDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_ShopDemandViews item in unitViewsList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_ShopDemandView_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.ShopDemandViewsRepo.Update(item);
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
        /// <summary>
        /// for Shop Demand Quic Search
        /// </summary>
        /// <param name="fromDate"> to get date from</param>
        /// <param name="toDate"> to get end date </param>
        /// <param name="regionIdFrom">to get region from</param>
        /// <param name="regionIdTo">to get region to</param>
        /// <param name="SpaceFrom"></param>
        /// <param name="SpaceTo"></param>
        /// <param name="PriceFrom"></param>
        /// <param name="PriceTo"></param>
        /// <returns></returns>
        public async Task<List<ShopDemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo, int Demand_Id)
        {
            List<ShopDemandDto> ShopDemandDtos = new List<ShopDemandDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (SpaceTo == 0)
            {
                try
                {
                    SpaceTo = decimal.ToInt32((decimal)_uow.ShopDemandsCustRepo.GetMaxDecimalValue(m => m.MaxSpace));
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
                    decimal price = (decimal)_uow.ShopDemandsCustRepo.GetMaxDecimalValue(m => m.MaxPrice);
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
                    fromDate = (DateTime)_uow.ShopDemandsCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    fromDate = DateTime.Now;
                }

            }
            IEnumerable<tbl_ShopDemands> demands;
            if (regionIdFrom > 0 && PriceFrom > 0 && SpaceFrom > 0)
            {
                demands = (await _uow.ShopDemandsRepo.FindAsync(d => (d.FK_ShopDemands_Regions_ToId <= regionIdTo && d.FK_ShopDemands_Regions_FromId >= regionIdFrom) &&
                 (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo)
                && !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate)));
            }

            else
            {
                demands = (await _uow.ShopDemandsRepo.FindAsync(d => !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate)));
            }
            if (Demand_Id != 0)
            {
                demands = demands.Where(a => a.PK_ShopDemands_Id == Demand_Id && !a.IsDeleted && !a.IsClosed);
            }
            ShopDemandDtos = Mapper.Map<IEnumerable<tbl_ShopDemands>, IEnumerable<ShopDemandDto>>(demands).ToList();
            return ShopDemandDtos;
        }

        public async Task<ShopDemandDto> EditShopDemand(int id)
        {
            List<ShopDemandDto> ShopDemands = Mapper.Map<List<tbl_ShopDemands>, List<ShopDemandDto>>((await _uow.ShopDemandsRepo.
                FindAsync(a => a.PK_ShopDemands_Id == id && !a.IsDeleted)).ToList());

            if (ShopDemands.Any() && ShopDemands != null)
            {
                ShopDemandDto demand = ShopDemands.FirstOrDefault();
                demand.AccessoriesIds = (await _uow.ShopDemandAccessoriesRepo.FindAsync(ua => ua.FK_ShopDemandAccessories_ShopDemand_Id == demand.PK_ShopDemands_Id)).Select(x => x.FK_ShopDemandAccessories_Accessories_Id).ToArray();
                demand.ViewsIds = (await _uow.ShopDemandViewsRepo.FindAsync(ua => ua.FK_ShopDemandView_ShopDemand_DemandId == demand.PK_ShopDemands_Id)).Select(x => x.FK_ShopDemandView_View_ViewId).ToArray();

                return demand;
            }
            return new ShopDemandDto();
        }

        public async Task<(List<ShopAvailableDto>, ShopAvailableDto, List<ShopAvailableDto>)> FilterAvailableMatches(ShopDemandDto demand)
        {
            List<tbl_ShopAvailable> availables = await GetAvailableMatches(demand);
            List<ShopAvailableDto> availablesWithPreviews = new List<ShopAvailableDto>();
            tbl_ShopAvailable sameClientAvailable = availables.Find(a => a.FK_ShopAvailable_Clients_ClientId == demand.FK_ShopDemands_Clients_ClientId);
            if (sameClientAvailable != null)
            {
                availables.Remove(sameClientAvailable);
            }
            if (availables.Any())
            {
                List<tbl_PreviewHeaders> demandClientPreviews = (await _uow.PreviewRepo.FindAsync(p => p.FK_PreviewHeaders_Clients_BuyerId == demand.FK_ShopDemands_Clients_ClientId &&
              (p.DemandUnit_Id == demand.PK_ShopDemands_Id && p.Category_Id == demand.FK_ShopDemands_Clients_ClientId) && !p.IsDeleted && p.IsSuspended &&
              (p.ReviewDate >= DbFunctions.TruncateTime(DateTime.Today)
               ))).ToList();
                if (demandClientPreviews != null && demandClientPreviews.Any())

                    foreach (tbl_PreviewHeaders item in demandClientPreviews)
                    {
                        List<tbl_PreviewDetails> previewDetails = (await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == item.PK_PreviewHeaders_Id && d.IsNoDecision)).ToList();
                        if (previewDetails != null && previewDetails.Any())
                        {
                            foreach (tbl_PreviewDetails detail in previewDetails)
                            {
                                if (availables.Exists(a => a.PK_ShopAvailable_Id == detail.AvailableUnits_Id && a.FK_ShopAvailable_Clients_ClientId == detail.Category_Id))
                                {
                                    tbl_ShopAvailable available = availables.Find(a => a.PK_ShopAvailable_Id == detail.AvailableUnits_Id);
                                    availablesWithPreviews.Add(Mapper.Map<tbl_ShopAvailable, ShopAvailableDto>(available));
                                    availables.Remove(available);
                                }
                            }
                        }
                    }
            }
            return (Mapper.Map<List<tbl_ShopAvailable>, List<ShopAvailableDto>>(availables), Mapper.Map<tbl_ShopAvailable, ShopAvailableDto>(sameClientAvailable), availablesWithPreviews);

        }

        public async Task<MultiSelectList> GetAccess()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name");

        }

        public async Task<MultiSelectList> GetAccessById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name", ids);

        }

        public async Task<List<tbl_ShopAvailable>> GetAvailableMatches(ShopDemandDto demand)
        {
            var DALDemand = Mapper.Map<ShopDemandDto, tbl_ShopDemands>(demand);
            List<tbl_ShopAvailable> availables = new List<tbl_ShopAvailable>();
            int fromRegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_ShopDemands_Regions_FromId)).FirstOrDefault().RegCode;
            int toRegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_ShopDemands_Regions_ToId)).FirstOrDefault().RegCode;
            int[] codes = fromRegCode < toRegCode ? new int[] { fromRegCode, toRegCode } : new int[] { toRegCode, fromRegCode };
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            List<tbl_Finishings> finishings = (await _uow.FinishRepo.FindAsync(f => !f.IsDeleted)).ToList();


            if (payments != null && payments.Any() && finishings != null && finishings.Any())
            {

                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == demand.FK_ShopDemands_PaymentMethod_Id);


                if (payment != null)
                {
                    if (!payment.IsMaster)
                    {
                        if (demand.FK_ShopDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.ShopCustomRepository.AvailablesForPayment(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.ShopCustomRepository.AvailablesForPaymentAndUsage(DALDemand, codes);
                        }
                    }
                    else if (payment.IsMaster)
                    {
                        if (demand.FK_ShopDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.ShopCustomRepository.AvailablesForMasters(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.ShopCustomRepository.AvailablesForUsage(DALDemand, codes);
                        }
                    }
                }
            }
            return availables;
        }


        public async Task<List<ShopAvailableDto>> GetAvailableMatchesOnTheFly(ShopDemandDto demand)
        {
            List<tbl_ShopAvailable> availables = await GetAvailableMatches(demand);
            return Mapper.Map<List<tbl_ShopAvailable>, List<ShopAvailableDto>>(availables);
        }

        public async Task<IConfirmation> GetInstantMatches(int demandId)
        {
            ShopDemandDto demand = Mapper.Map<tbl_ShopDemands, ShopDemandDto>((await _uow.ShopDemandsRepo.FindAsync(d => d.PK_ShopDemands_Id == demandId)).FirstOrDefault());
            if (demand != null)
            {
                _conf.ShopAvailableAndExcluded = await FilterAvailableMatches(demand);
                foreach (ShopAvailableDto item in _conf.ShopAvailables)
                {
                    item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_ShopAvailable_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.ShopDemands.Clear();
                _conf.ShopDemands.Add(demand);
                return _conf;
            }
            return _conf;
        }

        public async Task<SelectList> GetPayments()
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod");
        }

        public async Task<SelectList> GetPaymentsId(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);
        }

        public async Task<SelectList> GetRegionById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region", await _uow.RegionRepo.FindAsync(reg => reg.PK_Regions_ID == id));
        }

        public async Task<SelectList> GetRegions()
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region");
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

        public async Task<SelectList> GetTransById(int id, string name)
        {
            if (name != null)
            {
                List<TransDto> list = Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false && v.TransType == name)).ToList());
                return new SelectList(list, "PK_Transactions_Id", "TransType");
            }
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));
        }

        public async Task<SelectList> GetUsages()
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_UnitUsage_Id", "Name");
        }

        public async Task<SelectList> GetUsagesId(int usageId)
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_UnitUsage_Id", "Name", await _uow.UnitUsageRepo.FindAsync(u => u.PK_UnitUsage_Id == usageId));
        }

        public async Task<MultiSelectList> GetViews()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name");

        }

        public async Task<MultiSelectList> GetViewsById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name", ids);
        }

        public async Task<bool> ReleaseDemand(int id, int userId)
        {
            tbl_ShopDemands demand = (await _uow.ShopDemandsRepo.FindAsync(u => u.PK_ShopDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = false;
            demand.FK_ShopDemands_Users_ModidfiedBy = userId;
            _uow.ShopDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> SaveShopDemand(ShopDemandDto demand, int userId, int branchId)
        {
            tbl_ShopDemands newDemand = Mapper.Map<ShopDemandDto, tbl_ShopDemands>(demand);

            newDemand.FK_ShopDemands_Users_CreatedBy = userId;
            newDemand.FK_ShopDemands_Users_ModidfiedBy = userId;
            newDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newDemand.FK_ShopDemands_Branches_BranchId = branchId;
            _uow.ShopDemandsRepo.Add(newDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }

            if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
            {
                foreach (string access in demand.AccessoriesArr)
                {
                    tbl_ShopDemandAccessories demandAccess = new tbl_ShopDemandAccessories
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_ShopDemandAccessories_Users_CreatedBy = userId,
                        FK_ShopDemandAccessories_Users_ModidfiedBy = userId,
                        FK_ShopDemandAccessories_ShopDemand_Id = newDemand.PK_ShopDemands_Id,
                        FK_ShopDemandAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.ShopDemandAccessoriesRepo.Add(demandAccess);
                }
                _conf.Valid = await _uow.SaveAsync() == (demand.AccessoriesArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                    return _conf;
                }
            }

            if (demand.ViewsArr != null && demand.ViewsArr.Any())
            {
                foreach (string view in demand.ViewsArr)
                {
                    tbl_ShopDemandViews unitViews = new tbl_ShopDemandViews
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_ShopDemandView_Users_CreatedBy = userId,
                        FK_ShopDemandView_Users_ModidfiedBy = userId,
                        FK_ShopDemandView_ShopDemand_DemandId = newDemand.PK_ShopDemands_Id,
                        FK_ShopDemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.ShopDemandViewsRepo.Add(unitViews);
                }
                _conf.Valid = await _uow.SaveAsync() == demand.ViewsArr.Length;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            demand.PK_ShopDemands_Id = newDemand.PK_ShopDemands_Id;
            _conf.ShopAvailableAndExcluded = await FilterAvailableMatches(demand);
            foreach (ShopAvailableDto item in _conf.ShopAvailables)
            {
                item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_ShopAvailable_Clients_ClientId)).FirstOrDefault().Name;
            }
            _conf.ShopDemands.Clear();
            _conf.ShopDemands.Add(Mapper.Map<tbl_ShopDemands, ShopDemandDto>(newDemand));
            return _conf;

        }

        public async Task<bool> UpdateDemandTransaction(int demandId, int transCode)
        {
            tbl_ShopDemands demand = (await _uow.ShopDemandsRepo.FindAsync(u => u.PK_ShopDemands_Id == demandId && !u.IsDeleted)).FirstOrDefault();
            if (demand != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                demand.FK_ShopDemands_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> UpdateShopDemand(ShopDemandDto demand, int userId, int branchId)
        {
            if (demand.PK_ShopDemands_Id > 0)
            {
                tbl_ShopDemands DBclientDemand = (await _uow.ShopDemandsRepo.FindAsync(a => a.PK_ShopDemands_Id == demand.PK_ShopDemands_Id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.CreatedAt = demand.CreatedAt;
                    DBclientDemand.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                    DBclientDemand.FK_ShopDemands_Categories_Id = demand.FK_ShopDemands_Categories_Id;
                    DBclientDemand.FK_ShopDemands_PaymentMethod_Id = demand.FK_ShopDemands_PaymentMethod_Id;
                    DBclientDemand.FK_ShopDemands_Branches_BranchId = branchId;
                    DBclientDemand.Notes = demand.Notes;
                    DBclientDemand.MinPrice = demand.MinPrice;
                    DBclientDemand.MaxPrice = demand.MaxPrice;
                    DBclientDemand.MinSpace = demand.MinSpace;
                    DBclientDemand.MaxSpace = demand.MaxSpace;
                    DBclientDemand.MinBathRooms = demand.MinBathRooms;
                    DBclientDemand.MaxBathRooms = demand.MaxBathRooms;
                    DBclientDemand.ScaleNumber = demand.ScaleNumber;
                    DBclientDemand.FK_ShopDemands_Regions_FromId = demand.FK_ShopDemands_Regions_FromId;
                    DBclientDemand.FK_ShopDemands_Regions_ToId = demand.FK_ShopDemands_Regions_ToId;
                    DBclientDemand.FK_ShopDemands_Transactions_Id = demand.FK_ShopDemands_Transactions_Id;
                    //DBclientDemand.Title = demand.Title;
                    DBclientDemand.FK_ShopDemands_Users_ModidfiedBy = userId;
                    DBclientDemand.IsDivider = demand.IsDivider;
                    DBclientDemand.Islicense = demand.Islicense;
                    DBclientDemand.IsFurnisher = demand.IsFurnisher;
                    DBclientDemand.FK_ShopDemands_Users_SalesId = demand.FK_ShopDemands_Users_SalesId;

                }
                _uow.ShopDemandsRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح";
                    return _conf;
                }


                List<tbl_ShopDemandAccessories> unitAccessList = (await _uow.ShopDemandAccessoriesRepo.FindAsync(ui => ui.FK_ShopDemandAccessories_ShopDemand_Id == demand.PK_ShopDemands_Id && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_ShopDemandAccessories item in unitAccessList)
                    {
                        _uow.ShopDemandAccessoriesRepo.Remove(item);
                    }
                    if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
                    {
                        foreach (string access in demand.AccessoriesArr)
                        {
                            tbl_ShopDemandAccessories unitAccess = new tbl_ShopDemandAccessories
                            {
                                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                                FK_ShopDemandAccessories_Users_CreatedBy = userId,
                                FK_ShopDemandAccessories_Users_ModidfiedBy = userId,
                                FK_ShopDemandAccessories_ShopDemand_Id = DBclientDemand.PK_ShopDemands_Id,
                                FK_ShopDemandAccessories_Accessories_Id = int.Parse(access),
                            };
                            _uow.ShopDemandAccessoriesRepo.Add(unitAccess);
                        }
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (!_conf.Valid)
                        {
                            _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                            return _conf;
                        }
                    }

                }

                List<tbl_ShopDemandViews> unitViewsList = (await _uow.ShopDemandViewsRepo.FindAsync(ui => ui.FK_ShopDemandView_ShopDemand_DemandId == demand.PK_ShopDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_ShopDemandViews item in unitViewsList)
                    {
                        _uow.ShopDemandViewsRepo.Remove(item);
                    }
                    if (demand.ViewsArr != null && demand.ViewsArr.Any())
                    {
                        foreach (string view in demand.ViewsArr)
                        {
                            tbl_ShopDemandViews unitView = new tbl_ShopDemandViews
                            {
                                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                                FK_ShopDemandView_Users_CreatedBy = userId,
                                FK_ShopDemandView_Users_ModidfiedBy = userId,
                                FK_ShopDemandView_ShopDemand_DemandId = DBclientDemand.PK_ShopDemands_Id,
                                FK_ShopDemandView_View_ViewId = int.Parse(view),
                            };
                            _uow.ShopDemandViewsRepo.Add(unitView);
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

                _conf.ShopAvailableAndExcluded = await FilterAvailableMatches(demand);
                foreach (ShopAvailableDto item in _conf.ShopAvailables)
                {
                    item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_ShopAvailable_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.ShopDemands.Clear();
                _conf.ShopDemands.Add(Mapper.Map<tbl_ShopDemands, ShopDemandDto>(DBclientDemand));

                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }

        public async Task<List<ShopDemandDto>> ShopDemands(int id)
        {
            List<ShopDemandDto> ShopDemands = Mapper.Map<List<tbl_ShopDemands>, List<ShopDemandDto>>((await _uow.ShopDemandsRepo.FindAsync(a => a.FK_ShopDemands_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed))
                .ToList());

            if (ShopDemands.Any() && ShopDemands != null)
            {
                foreach (ShopDemandDto item in ShopDemands)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_ShopDemands_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.CatRepo.FindAsync(t => t.PK_Categories_Id == item.FK_ShopDemands_Categories_Id))
                        .FirstOrDefault().CategoryName);
                    item.CreatedAtString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return ShopDemands;
            }
            return new List<ShopDemandDto>();
        }

        public List<ShopDemandDto> ShopClientDemands(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetShortDescAndDate(ShopDemandDto shopDemandDto)
        {
            var shortdesc = (await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == shopDemandDto.FK_ShopDemands_Transactions_Id)).FirstOrDefault().TransType + " " +
               (await _uow.CatRepo.FindAsync(c => c.PK_Categories_Id == shopDemandDto.FK_ShopDemands_Categories_Id)).FirstOrDefault().CategoryName;
            return shortdesc;
        }

        public async Task<IConfirmation> CreateDemandForAvailable(ShopAvailableDto available, int userId, int branchId, int clientId, string notes)
        {

            tbl_ShopDemands newDemand = new tbl_ShopDemands();
            newDemand.FK_ShopDemands_Users_CreatedBy = userId;
            newDemand.FK_ShopDemands_Users_ModidfiedBy = userId;
            newDemand.CreatedAt = DateTime.UtcNow.AddHours(2);
            newDemand.FK_ShopDemands_Branches_BranchId = branchId;
            newDemand.FK_ShopDemands_Categories_Id = Categories.Shops;
            newDemand.ModifiedAt = DateTime.UtcNow.AddHours(2);
            newDemand.FK_ShopDemands_Clients_ClientId = clientId;
            newDemand.DateOfBuildFrom = available.DateOfBuild;
            newDemand.DateOfBuildTo = available.DateOfBuild;
            newDemand.FK_ShopDemands_PaymentMethod_Id = available.FK_ShopAvailable_PaymentMethod_Id;
            newDemand.FK_ShopDemands_Regions_FromId = available.FK_ShopAvailable_Regions_Id;
            newDemand.FK_ShopDemands_Regions_ToId = available.FK_ShopAvailable_Regions_Id;
            newDemand.FK_ShopDemands_Transactions_Id = available.FK_ShopAvailable_Transactions_Id;
            newDemand.FK_ShopDemands_Usage_Id = available.FK_ShopAvailable_Usage_Id;
            newDemand.FK_ShopDemands_Users_SalesId = available.FK_ShopAvailable_Users_SalesId;
            newDemand.IsClosed = false;
            newDemand.IsDeleted = false;
            newDemand.IsDivider = available.IsDivider;
            newDemand.IsFurnisher = available.IsFurnished;
            newDemand.Islicense = available.Islicense;
            newDemand.MaxBathRooms = available.BathRooms;
            newDemand.MaxPrice = available.Price;
            newDemand.MaxSpace = available.Space;
            newDemand.MinBathRooms = available.BathRooms;
            newDemand.MinPrice = available.Price;
            newDemand.MinSpace = available.Space;
            newDemand.Notes = notes;
            newDemand.ScaleNumber = newDemand.ScaleNumber;
            _uow.ShopDemandsRepo.Add(newDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }

            _conf.Message = "تم الحفظ بنجاح!";
            _conf.ShopAvailables.Add(available);
            _conf.ShopDemands.Add(Mapper.Map<tbl_ShopDemands, ShopDemandDto>(newDemand));
            return _conf;
        }

    }
}
