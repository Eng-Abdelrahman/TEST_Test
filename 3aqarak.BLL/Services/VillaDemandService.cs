using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    public class VillaDemandService : IVillasDemandService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public VillaDemandService(IUnitOfWork uow, IConfirmation conf, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _session = session;
            _context = context;

        }

        public async Task<bool> CloseDemand(int id, int userId)
        {
            tbl_VillasDemands demand =(await _uow.VillasDemandsRepo.FindAsync(u => u.PK_VillasDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = true;
            demand.FK_VillasDemands_Users_ModidfiedBy = userId;
            _uow.VillasDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> CreateDemandForAvailable(VillasAvailableDto available, int userId, int branchId, int clientId, string notes)
        {
            tbl_VillasDemands newDemand = new tbl_VillasDemands();

            newDemand.FK_VillasDemands_Users_CreatedBy = userId;
            newDemand.IsClosed = false;
            newDemand.IsDeleted = false;
            newDemand.IsFurnished = available.IsFurnished;
            newDemand.FK_VillasDemands_Users_ModidfiedBy = userId;
            newDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newDemand.FK_VillasDemands_Branches_BranchId = branchId;
            newDemand.DateOfBuildFrom = available.DateOfBuild;
            newDemand.DateOfBuildTo = available.DateOfBuild;
            newDemand.FK_VillasDemands_Categories_Id = Categories.Villas;
            newDemand.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            newDemand.FK_VillasDemands_Clients_ClientId = clientId;
            newDemand.FK_VillasDemands_PaymentMethod_Id = available.FK_VillasAvailables_PaymentMethod_Id;
            newDemand.FK_VillasDemands_Regions_FromId = available.FK_VillasAvailables_Regions_Id;
            newDemand.FK_VillasDemands_Regions_ToId = available.FK_VillasAvailables_Regions_Id;
            newDemand.FK_VillasDemands_Transactions_Id = available.FK_VillasAvailables_Transactions_Id;
            newDemand.FK_VillasDemands_Usage_Id = available.FK_VillasAvailables_Usage_Id;
            newDemand.FK_VillasDemands_Users_SalesId = available.FK_VillasAvailables_Users_SalesId;
            newDemand.IsFurnished = available.IsFurnished;
            newDemand.MaxBathRooms = available.BathRooms;

            newDemand.MaxPrice = available.Price;
            newDemand.MaxRooms = available.Rooms;
            newDemand.MaxSpace = available.Space;
            newDemand.MinBathRooms = available.BathRooms;

            newDemand.MinPrice = available.Price;
            newDemand.MinRooms = available.Rooms;
            newDemand.MinSpace = available.Space;
            newDemand.MinBathRooms = available.BathRooms; 
            newDemand.Notes = notes;
            newDemand.MaxNoOfElevators = available.NoOfElevators;
            newDemand.MinNoOfElevators = available.NoOfElevators;
            newDemand.MaxAreaSpace = available.AreaSpace;
            newDemand.MinAreaSpace = available.AreaSpace;
            _uow.VillasDemandsRepo.Add(newDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }

            _conf.Message = "تم الحفظ بنجاح!";
            _conf.VillAvailables.Add(available);
            _conf.VillDemands.Add(Mapper.Map<tbl_VillasDemands, VillasDemandDto>(newDemand));
            return _conf;
        }

        public async Task<IConfirmation> DeleteVillaDemand(int id, int userId)
        {
            if (id > 0)
            {
     
                tbl_VillasDemands DBclientDemand = (await _uow.VillasDemandsRepo.FindAsync
                         (a => a.PK_VillasDemands_Id == id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.IsDeleted = true;
                    DBclientDemand.FK_VillasDemands_Users_ModidfiedBy = userId;
                    DBclientDemand.ModifiedAt = DateTime.UtcNow.AddHours(2);
                }
                _uow.VillasDemandsRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف الطلب بنجاح";
                    return _conf;
                }

                List<tbl_VillademandFinishings> unitFinishList =(await _uow.VillademandFinishingsRepo.FindAsync(ui => ui.FK_VillaDemandFinishing_VillaDemand_Id == DBclientDemand.PK_VillasDemands_Id && !ui.IsDeleted)).ToList();

                if (unitFinishList != null && unitFinishList.Any())
                {
                    foreach (tbl_VillademandFinishings item in unitFinishList)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_VillaDemandFinishing_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.VillademandFinishingsRepo.Update(item);

                    }


                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف التشطيبات بنجاح!";
                        return _conf;
                    }
                }

                List<tbl_VillaDemandAccessories> unitAccessList =(await _uow.VillaDemandAccessoriesRepo.FindAsync(ui => ui.FK_VillaDemandAccessories_VillaDemand_Id == DBclientDemand.PK_VillasDemands_Id)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_VillaDemandAccessories item in unitAccessList)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_VillaDemandAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.VillaDemandAccessoriesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الكماليات بنجاح!";
                        return _conf;
                    }
                }
                List<tbl_VillasDemandViews> unitViewsList =(await _uow.VillasDemandViewsRepo.FindAsync(ui => ui.FK_VillaDemandView_VillaDemand_DemandId == DBclientDemand.PK_VillasDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_VillasDemandViews item in unitViewsList)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_VillaDemandView_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.VillasDemandViewsRepo.Update(item);
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

        public async Task<List<VillasDemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int regionIdFrom, int regionIdTo ,int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand_Id)
        {
            List<VillasDemandDto> VillasDemandDtos = new List<VillasDemandDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (SpaceTo == 0)
            {
                try
                {
                    SpaceTo = decimal.ToInt32((decimal)_uow.VillasDemandsCustRepo.GetMaxDecimalValue(m => m.MaxSpace));
                }
                catch (Exception)
                {

                    SpaceTo=0;
                } 
            }
            if (PriceTo == 0)
            {
                try
                {
                    decimal price = (decimal)_uow.VillasDemandsCustRepo.GetMaxDecimalValue(m => m.MaxPrice);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo =0;
                }
                
            }
            if (fromDate == new DateTime(2017, 1, 18))
            {
                try
                {
                    fromDate =(DateTime) _uow.VillasDemandsCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    fromDate=new DateTime(2017,1,18);
                }
                
            }
            IEnumerable<tbl_VillasDemands> demands;
            if (regionIdFrom > 0 && PriceFrom > 0 && SpaceFrom > 0)
            {
                 demands =await _uow.VillasDemandsRepo.FindAsync(d => (d.FK_VillasDemands_Regions_ToId <= regionIdTo && d.FK_VillasDemands_Regions_FromId >= regionIdFrom) &&
                 (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo) 
                && !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate));
            }
           
            else
            {
                 demands =await _uow.VillasDemandsRepo.FindAsync(d => !d.IsDeleted && !d.IsClosed && (d.CreatedAt >= fromDate && d.CreatedAt <= toDate));
            }
            if (Demand_Id !=0)
            {
                demands = demands.Where(d => d.PK_VillasDemands_Id == Demand_Id && !d.IsDeleted && !d.IsClosed);
            }
            VillasDemandDtos = Mapper.Map<IEnumerable<tbl_VillasDemands>, IEnumerable<VillasDemandDto>>(demands).ToList();
            return VillasDemandDtos;
        }

        public async Task<VillasDemandDto> EditVillaDemand(int id)
        {
            List<VillasDemandDto> villaDemands = Mapper.Map<List<tbl_VillasDemands>, List<VillasDemandDto>>((await _uow.VillasDemandsRepo.GetAllAsync())
               .Where(a => a.PK_VillasDemands_Id == id && !a.IsDeleted).ToList());

            if (villaDemands.Any() && villaDemands != null)
            {
                VillasDemandDto demand = villaDemands.FirstOrDefault();
                demand.AccessoriesIds =(await _uow.VillaDemandAccessoriesRepo.FindAsync(ua => ua.FK_VillaDemandAccessories_VillaDemand_Id == demand.PK_VillasDemands_Id)).Select(x => x.FK_VillaDemandAccessories_Accessories_Id).ToArray();
                demand.ViewsIds =(await _uow.VillasDemandViewsRepo.FindAsync(ua => ua.FK_VillaDemandView_VillaDemand_DemandId == demand.PK_VillasDemands_Id)).Select(x => x.FK_VillaDemandView_View_ViewId).ToArray();
                demand.FinishIds =(await _uow.VillademandFinishingsRepo.FindAsync(df => df.FK_VillaDemandFinishing_VillaDemand_Id == demand.PK_VillasDemands_Id)).Select(x => x.FK_VillaDemandFinishing_Finish_Id).ToArray();
                return demand;
            }
            return new VillasDemandDto();
        }

        public async Task<(List<VillasAvailableDto>, VillasAvailableDto, List<VillasAvailableDto>)> FilterAvailableMatches(VillasDemandDto demand)
        {
            List<tbl_VillasAvailables> availables = await GetAvailableMatches(demand);
            List<VillasAvailableDto> availablesWithPreviews = new List<VillasAvailableDto>();
            tbl_VillasAvailables sameClientAvailable = availables.Find(a => a.FK_VillasAvailables_Clients_ClientId == demand.FK_VillasDemands_Clients_ClientId);
            if (sameClientAvailable != null)
            {
                availables.Remove(sameClientAvailable);
            }
            if (availables.Any())
            {
                List<tbl_PreviewHeaders> demandClientPreviews =(await _uow.PreviewRepo.FindAsync(p => p.FK_PreviewHeaders_Clients_BuyerId == demand.FK_VillasDemands_Clients_ClientId &&
             (p.DemandUnit_Id == demand.PK_VillasDemands_Id && p.Category_Id == demand.FK_VillasDemands_Categories_Id) && !p.IsDeleted && p.IsSuspended &&
             (p.ReviewDate >= DbFunctions.TruncateTime(DateTime.Today)
              ))).ToList();
                if (demandClientPreviews != null && demandClientPreviews.Any())

                    foreach (tbl_PreviewHeaders item in demandClientPreviews)
                    {
                        List<tbl_PreviewDetails> previewDetails =(await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == item.PK_PreviewHeaders_Id && d.IsNoDecision)).ToList();
                        if (previewDetails != null && previewDetails.Any())
                        {
                            foreach (tbl_PreviewDetails detail in previewDetails)
                            {
                                if (availables.Exists(a => a.PK_VillasAvailables_Id == detail.AvailableUnits_Id && a.FK_VillasAvailables_Categories_Id == detail.Category_Id))
                                {
                                    tbl_VillasAvailables available = availables.Find(a => a.PK_VillasAvailables_Id == detail.AvailableUnits_Id);
                                    availablesWithPreviews.Add(Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(available));
                                    availables.Remove(available);
                                }
                            }
                        }
                    }
            }
            return (Mapper.Map<List<tbl_VillasAvailables>, List<VillasAvailableDto>>(availables), Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(sameClientAvailable), availablesWithPreviews);

        }

        public async Task<MultiSelectList> GetAccess()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name");

        }

        public async Task<MultiSelectList> GetAccessById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name", ids);

        }

        public async Task<List<tbl_VillasAvailables>> GetAvailableMatches(VillasDemandDto demand)
        {
            var DALDemand = Mapper.Map<VillasDemandDto, tbl_VillasDemands>(demand);
            List<tbl_VillasAvailables> availables = new List<tbl_VillasAvailables>();
            int fromRegCode =(await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_VillasDemands_Regions_FromId)).FirstOrDefault().RegCode;
            int toRegCode =(await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_VillasDemands_Regions_ToId)).FirstOrDefault().RegCode;
            int[] codes = fromRegCode < toRegCode ? new int[] { fromRegCode, toRegCode } : new int[] { toRegCode, fromRegCode };
            //var viewsIds = demand.ViewsArr.Select(v => int.Parse(v)).ToArray();
            //var accessoriesIds = demand.AccessoriesArr.Select(a => int.Parse(a)).ToArray();          
            //var payments = (List<tbl_PaymentMethods>)_context.Session["payments"];
            //var finishings = (List<tbl_Finishings>)_context.Session["finishings"];
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(p => !p.IsDeleted).ToList();
            List<tbl_Finishings> finishings = (await _uow.FinishRepo.GetAllAsync()).Where(f => !f.IsDeleted).ToList();
            int[] finishIds = (demand.FinishArr != null && demand.FinishArr.Any())
                ? demand.FinishArr.Select(f => int.Parse(f)).ToArray()
                :(await _uow.VillademandFinishingsRepo.FindAsync(f => f.FK_VillaDemandFinishing_VillaDemand_Id == demand.PK_VillasDemands_Id))
                .Select(f => f.FK_VillaDemandFinishing_Finish_Id).ToArray();

            if (payments != null && payments.Any() && finishings != null && finishings.Any())
            {
                List<tbl_Finishings> choosenFinishings = new List<tbl_Finishings>();
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == demand.FK_VillasDemands_PaymentMethod_Id);
                for (int i = 0; i < finishIds.Length; i++)
                {
                    tbl_Finishings element = finishings.Find(e => e.PK_Finishings_Id == finishIds[i]);
                    if (element != null)
                    {
                        choosenFinishings.Add(element);
                    }
                }

                if (payment != null && choosenFinishings.Any())
                {
                    if (!payment.IsMaster && !choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_VillasDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables =await _uow.VillaCustomrepository.AvailableForPaymentAndFinish(DALDemand, codes, finishIds);
                        }
                        else
                        {
                            availables = await _uow.VillaCustomrepository.AvailableForPaymentFinishUsage(DALDemand, codes, finishIds);
                        }
                    }
                    else if (payment.IsMaster && choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_VillasDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatches(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatchesForUsage(DALDemand, codes);
                        }
                    }
                    else if (payment.IsMaster && !choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_VillasDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatchesForFinish(DALDemand, codes, finishIds);
                        }
                        else
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatchesForFinishAndUsage(DALDemand, codes, finishIds);
                        }
                    }
                    else if (!payment.IsMaster && choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_VillasDemands_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatchesForPayment(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.VillaCustomrepository.AvailableMatchesForPaymentAndUsage(DALDemand, codes);
                        }
                    }
                }
            }
            return availables;
        }

      
        public async Task<List<VillasAvailableDto>> GetAvailableMatchesOnTheFly(VillasDemandDto demand)
        {
            List<tbl_VillasAvailables> availables = await GetAvailableMatches(demand);
            return Mapper.Map<List<tbl_VillasAvailables>, List<VillasAvailableDto>>(availables);
        }

        public async Task<MultiSelectList> GetFinishings()
        {
            List<FinishingDto> finishings = Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList());
            //_context.Session["finishings"] = finishings;
            return new MultiSelectList(finishings, "PK_Finishings_Id", "Type");
        }

        public async Task<MultiSelectList> GetFinishingsById(int[] ids)
        {
            List<FinishingDto> finishings = Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList());
            //_context.Session["finishings"] = finishings;
            return new MultiSelectList(finishings, "PK_Finishings_Id", "Type", ids);
        }

        public async Task<IConfirmation> GetInstantMatches(int demandId)
        {
            VillasDemandDto demand = Mapper.Map<tbl_VillasDemands, VillasDemandDto>((await _uow.VillasDemandsRepo.FindAsync(d => d.PK_VillasDemands_Id == demandId)).FirstOrDefault());
            if (demand != null)
            {
                _conf.VillAvailablesAndExcluded = await FilterAvailableMatches(demand);
                foreach (VillasAvailableDto item in _conf.VillAvailables)
                {
                    item.SellerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasAvailables_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.VillDemands.Clear();
                _conf.VillDemands.Add(demand);
                return _conf;
            }
            return _conf;
        }

        public async Task<SelectList> GetPayments()
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList();
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod");
        }

        public async Task<SelectList> GetPaymentsId(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList();
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);
        }

        public async Task<SelectList> GetRegionById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region", await _uow.RegionRepo.FindAsync(reg => reg.PK_Regions_ID == id));

        }

        public async Task<SelectList> GetRegions()
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Regions_Id", "Region");
        }

        public async Task<SelectList> GetTrans(string name)
        {
            if (name != null)
            {
                List<TransDto> list = Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false && v.TransType == name).ToList());
                return new SelectList(list, "PK_Transactions_Id", "TransType");
            }

            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<SelectList> GetTransById(int id, string name)
        {
            if (name != null)
            {
                List<TransDto> list = Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false && v.TransType == name).ToList());
                return new SelectList(list, "PK_Transactions_Id", "TransType");
            }
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Transactions_Id", "TransType", _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));
        }

        public async Task<SelectList> GetUsages()
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_UnitUsage_Id", "Name");
        }

        public async Task<SelectList> GetUsagesId(int usageId)
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_UnitUsage_Id", "Name", _uow.UnitUsageRepo.FindAsync(u => u.PK_UnitUsage_Id == usageId));
        }


        public async Task<MultiSelectList> GetViews()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name");

        }

        public async Task<MultiSelectList> GetViewsById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name", ids);
        }

        public async Task<bool> ReleaseDemand(int id, int userId)
        {
            tbl_VillasDemands demand =(await _uow.VillasDemandsRepo.FindAsync(u => u.PK_VillasDemands_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = false;
            demand.FK_VillasDemands_Users_ModidfiedBy = userId;
            _uow.VillasDemandsRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }
        //*****
        public async Task<IConfirmation> SaveImportVillaDemand(VillasDemandDto demand, int userId, int branchId)
        {
            tbl_VillasDemands newDemand = Mapper.Map<VillasDemandDto, tbl_VillasDemands>(demand);
            try
            {
                newDemand.FK_VillasDemands_Users_CreatedBy = userId;
                newDemand.FK_VillasDemands_Users_ModidfiedBy = userId;
                newDemand.CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                newDemand.FK_VillasDemands_Branches_BranchId = branchId;
                _uow.VillasDemandsRepo.Add(newDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                    return _conf;
                }
            }
            catch (Exception ex)
            {

            }

            if (demand.FinishArr != null && demand.FinishArr.Any())
            {
                foreach (string finish in demand.FinishArr)
                {
                    tbl_VillademandFinishings demandFinish = new tbl_VillademandFinishings
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandFinishing_Users_CreatedBy = userId,
                        FK_VillaDemandFinishing_Users_ModidfiedBy = userId,
                        FK_VillaDemandFinishing_VillaDemand_Id = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandFinishing_Finish_Id = int.Parse(finish),
                    };
                    _uow.VillademandFinishingsRepo.Add(demandFinish);
                }
                _conf.Valid = await _uow.SaveAsync() == (demand.FinishArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ التشطيبات بنجاح!";
                    return _conf;
                }
            }

            if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
            {
                foreach (string access in demand.AccessoriesArr)
                {
                    tbl_VillaDemandAccessories demandAccess = new tbl_VillaDemandAccessories
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandAccessories_Users_CreatedBy = userId,
                        FK_VillaDemandAccessories_Users_ModidfiedBy = userId,
                        FK_VillaDemandAccessories_VillaDemand_Id = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.VillaDemandAccessoriesRepo.Add(demandAccess);
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
                    tbl_VillasDemandViews unitViews = new tbl_VillasDemandViews
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandView_Users_CreatedBy = userId,
                        FK_VillaDemandView_Users_ModidfiedBy = userId,
                        FK_VillaDemandView_VillaDemand_DemandId = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.VillasDemandViewsRepo.Add(unitViews);
                }
                _conf.Valid = await _uow.SaveAsync() == demand.ViewsArr.Length;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            return _conf;

        }
        //****
        public async Task<IConfirmation> SaveVillaDemand(VillasDemandDto demand, int userId, int branchId)
        {
            tbl_VillasDemands newDemand = Mapper.Map<VillasDemandDto, tbl_VillasDemands>(demand);
            try
            {
                newDemand.FK_VillasDemands_Users_CreatedBy = userId;
                newDemand.FK_VillasDemands_Users_ModidfiedBy = userId;
                newDemand.CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                newDemand.FK_VillasDemands_Branches_BranchId = branchId;
                _uow.VillasDemandsRepo.Add(newDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                    return _conf;
                }
            }
            catch (Exception ex)
            {

            }

            if (demand.FinishArr != null && demand.FinishArr.Any())
            {
                foreach (string finish in demand.FinishArr)
                {
                    tbl_VillademandFinishings demandFinish = new tbl_VillademandFinishings
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandFinishing_Users_CreatedBy = userId,
                        FK_VillaDemandFinishing_Users_ModidfiedBy = userId,
                        FK_VillaDemandFinishing_VillaDemand_Id = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandFinishing_Finish_Id = int.Parse(finish),
                    };
                    _uow.VillademandFinishingsRepo.Add(demandFinish);
                }
                _conf.Valid = await _uow.SaveAsync() == (demand.FinishArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ التشطيبات بنجاح!";
                    return _conf;
                }
            }

            if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
            {
                foreach (string access in demand.AccessoriesArr)
                {
                    tbl_VillaDemandAccessories demandAccess = new tbl_VillaDemandAccessories
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandAccessories_Users_CreatedBy = userId,
                        FK_VillaDemandAccessories_Users_ModidfiedBy = userId,
                        FK_VillaDemandAccessories_VillaDemand_Id = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.VillaDemandAccessoriesRepo.Add(demandAccess);
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
                    tbl_VillasDemandViews unitViews = new tbl_VillasDemandViews
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_VillaDemandView_Users_CreatedBy = userId,
                        FK_VillaDemandView_Users_ModidfiedBy = userId,
                        FK_VillaDemandView_VillaDemand_DemandId = newDemand.PK_VillasDemands_Id,
                        FK_VillaDemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.VillasDemandViewsRepo.Add(unitViews);
                }
                _conf.Valid = await _uow.SaveAsync() == demand.ViewsArr.Length;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            demand.PK_VillasDemands_Id = newDemand.PK_VillasDemands_Id;
            _conf.VillAvailablesAndExcluded = await FilterAvailableMatches(demand);
            foreach (VillasAvailableDto item in _conf.VillAvailables)
            {
                item.SellerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasAvailables_Clients_ClientId)).FirstOrDefault().Name;
            }
            _conf.VillDemands.Clear();
            _conf.VillDemands.Add(Mapper.Map<tbl_VillasDemands, VillasDemandDto>(newDemand));
            return _conf;

        }

        public async Task<bool> UpdateDemandTransaction(int demandId, int transCode)
        {
            tbl_VillasDemands demand =(await _uow.VillasDemandsRepo.FindAsync(u => u.PK_VillasDemands_Id == demandId && !u.IsDeleted)).FirstOrDefault();
            if (demand != null)
            {
                int? transactionId =(await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                demand.FK_VillasDemands_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> UpdateVillaDemand(VillasDemandDto demand, int userId, int branchId)
        {
            if (demand.PK_VillasDemands_Id > 0)
            {
                tbl_VillasDemands DBclientDemand = (await _uow.VillasDemandsRepo.FindAsync
                         (a => a.PK_VillasDemands_Id == demand.PK_VillasDemands_Id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                    DBclientDemand.FK_VillasDemands_Categories_Id = demand.FK_VillasDemands_Categories_Id;
                    DBclientDemand.FK_VillasDemands_PaymentMethod_Id = demand.FK_VillasDemands_PaymentMethod_Id;
                    DBclientDemand.FK_VillasDemands_Branches_BranchId = branchId;
                    DBclientDemand.Notes = demand.Notes;
                    DBclientDemand.MinPrice = demand.MinPrice;
                    DBclientDemand.MaxPrice = demand.MaxPrice;
                    DBclientDemand.MaxRooms = demand.MaxRooms;
                    DBclientDemand.MinRooms = demand.MinRooms;
                    DBclientDemand.MinSpace = demand.MinSpace;
                    DBclientDemand.MaxSpace = demand.MaxSpace;
                    DBclientDemand.MaxAreaSpace = demand.MaxAreaSpace;
                    DBclientDemand.MinAreaSpace = demand.MinAreaSpace;
                    DBclientDemand.MinBathRooms = demand.MinBathRooms;
                    DBclientDemand.MaxBathRooms = demand.MaxBathRooms;
                    DBclientDemand.FK_VillasDemands_Regions_FromId = demand.FK_VillasDemands_Regions_FromId;
                    DBclientDemand.FK_VillasDemands_Regions_ToId = demand.FK_VillasDemands_Regions_ToId;
                    DBclientDemand.FK_VillasDemands_Transactions_Id = demand.FK_VillasDemands_Transactions_Id;
                    DBclientDemand.FK_VillasDemands_Users_ModidfiedBy = userId;
                    DBclientDemand.IsFurnished = demand.IsFurnished;
                    DBclientDemand.FK_VillasDemands_Users_SalesId = demand.FK_VillasDemands_Users_SalesId;
                    DBclientDemand.FK_VillasDemands_Usage_Id = demand.FK_VillasDemands_Usage_Id;
                    DBclientDemand.DateOfBuildFrom = demand.DateOfBuildFrom;
                    DBclientDemand.DateOfBuildTo = demand.DateOfBuildTo;
                    DBclientDemand.MaxNoOfElevators = demand.MaxNoOfElevators;
                    DBclientDemand.MinNoOfElevators = demand.MinNoOfElevators;

                }
                _uow.VillasDemandsRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح";
                    return _conf;
                }
                List<tbl_VillademandFinishings> unitFinishList =(await _uow.VillademandFinishingsRepo.FindAsync(ui => ui.FK_VillaDemandFinishing_VillaDemand_Id == demand.PK_VillasDemands_Id && !ui.IsDeleted)).ToList();

                if (unitFinishList != null && unitFinishList.Any())
                {
                    foreach (tbl_VillademandFinishings item in unitFinishList)
                    {
                        _uow.VillademandFinishingsRepo.Remove(item);
                    }
                    if (demand.FinishArr != null && demand.FinishArr.Any())
                    {
                        foreach (string finish in demand.FinishArr)
                        {
                            tbl_VillademandFinishings demandFinish = new tbl_VillademandFinishings
                            {
                                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                FK_VillaDemandFinishing_Users_CreatedBy = userId,
                                FK_VillaDemandFinishing_Users_ModidfiedBy = userId,
                                FK_VillaDemandFinishing_VillaDemand_Id = DBclientDemand.PK_VillasDemands_Id,
                                FK_VillaDemandFinishing_Finish_Id = int.Parse(finish),
                            };
                            _uow.VillademandFinishingsRepo.Add(demandFinish);
                        }
                    }

                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ التشطيبات بنجاح!";
                        return _conf;
                    }
                }

                List<tbl_VillaDemandAccessories> unitAccessList =(await _uow.VillaDemandAccessoriesRepo.FindAsync(ui => ui.FK_VillaDemandAccessories_VillaDemand_Id == demand.PK_VillasDemands_Id && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_VillaDemandAccessories item in unitAccessList)
                    {
                        _uow.VillaDemandAccessoriesRepo.Remove(item);
                    }
                    if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
                    {
                        foreach (string access in demand.AccessoriesArr)
                        {
                            tbl_VillaDemandAccessories unitAccess = new tbl_VillaDemandAccessories
                            {
                                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                FK_VillaDemandAccessories_Users_CreatedBy = userId,
                                FK_VillaDemandAccessories_Users_ModidfiedBy = userId,
                                FK_VillaDemandAccessories_VillaDemand_Id = DBclientDemand.PK_VillasDemands_Id,
                                FK_VillaDemandAccessories_Accessories_Id = int.Parse(access),
                            };
                            _uow.VillaDemandAccessoriesRepo.Add(unitAccess);
                        }
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (!_conf.Valid)
                        {
                            _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                            return _conf;
                        }
                    }

                }

                List<tbl_VillasDemandViews> unitViewsList =(await _uow.VillasDemandViewsRepo.FindAsync(ui => ui.FK_VillaDemandView_VillaDemand_DemandId == demand.PK_VillasDemands_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_VillasDemandViews item in unitViewsList)
                    {
                        _uow.VillasDemandViewsRepo.Remove(item);
                    }
                    if (demand.ViewsArr != null && demand.ViewsArr.Any())
                    {
                        foreach (string view in demand.ViewsArr)
                        {
                            tbl_VillasDemandViews unitView = new tbl_VillasDemandViews
                            {
                                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                FK_VillaDemandView_Users_CreatedBy = userId,
                                FK_VillaDemandView_Users_ModidfiedBy = userId,
                                FK_VillaDemandView_VillaDemand_DemandId = DBclientDemand.PK_VillasDemands_Id,
                                FK_VillaDemandView_View_ViewId = int.Parse(view),
                            };
                            _uow.VillasDemandViewsRepo.Add(unitView);
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
                _conf.VillAvailablesAndExcluded =await FilterAvailableMatches(demand);
                foreach (VillasAvailableDto item in _conf.VillAvailables)
                {
                    item.SellerName =(await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasAvailables_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.VillDemands.Clear();
                _conf.VillDemands.Add(Mapper.Map<tbl_VillasDemands, VillasDemandDto>(DBclientDemand));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }

        public async Task<List<VillasDemandDto>> villaDemands(int id)
        {
            List<VillasDemandDto> villaDemands = Mapper.Map<List<tbl_VillasDemands>, List<VillasDemandDto>>((await _uow.VillasDemandsRepo.FindAsync
                (a => a.FK_VillasDemands_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed))
                .ToList());

            if (villaDemands.Any() && villaDemands != null)
            {
                foreach (VillasDemandDto item in villaDemands)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_VillasDemands_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.CatRepo.FindAsync(t => t.PK_Categories_Id == item.FK_VillasDemands_Categories_Id))
                        .FirstOrDefault().CategoryName);
                    item.CreatedAtString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return villaDemands;
            }
            return new List<VillasDemandDto>();
        }

    
    }
}
  

    
