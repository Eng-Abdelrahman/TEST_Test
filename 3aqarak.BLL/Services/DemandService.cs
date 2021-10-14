using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;

using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{

    public class DemandService : IDemandService
    { 
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public DemandService(IUnitOfWork uow, IConfirmation conf, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _session = session;
            _context = context;

        }
        public async Task<List<DemandDto>> ClientDemands(int id)
        {
            List<DemandDto> clientDemands = Mapper.Map<List<tbl_DemandUnits>, List<DemandDto>>((await _uow.DemandRepo.FindAsync
                (a => a.FK_DemandUnits_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed))
                .ToList());

            if (clientDemands.Any() && clientDemands != null)
            {
                foreach (DemandDto item in clientDemands)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_DemandUnits_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.CatRepo.FindAsync(t => t.PK_Categories_Id == item.FK_DemandUnits_Categories_Id))
                        .FirstOrDefault().CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientDemands;
            }
            return new List<DemandDto>();
        }

        public async Task<SelectList> GetUsages()
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_UnitUsage_Id", "Name");
        }

        public async Task<SelectList> GetUsagesId(int usageId)
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_UnitUsage_Id", "Name", await _uow.UnitUsageRepo.FindAsync(u => u.PK_UnitUsage_Id == usageId));
        }

        public async Task<SelectList> GetRegions()
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region");
        }

        public async Task<SelectList> GetRegionById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region", await _uow.RegionRepo.FindAsync(reg => reg.PK_Regions_ID == id));
        }

        public async Task<MultiSelectList> GetFinishings()
        {
            List<FinishingDto> finishings = Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.FindAsync(v => v.IsDeleted == false)).ToList());
            //_context.Session["finishings"] = finishings;
            return new MultiSelectList(finishings, "PK_Finishings_Id", "Type");
        }

        public async Task<MultiSelectList> GetFinishingsById(int[] ids)
        {
            List<FinishingDto> finishings = Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.FindAsync(v => v.IsDeleted == false)).ToList());
            //_context.Session["finishings"] = finishings;
            return new MultiSelectList(finishings, "PK_Finishings_Id", "Type", ids);
        }

        public async Task<SelectList> GetCats()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Categories_Id", "CategoryName");
        }

        public async Task<SelectList> GetCatsById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Categories_Id", "CategoryName", await _uow.CatRepo.FindAsync(cat => cat.PK_Categories_Id == id));
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

        public async Task<SelectList> GetPayments()
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod");
        }

        public async Task<SelectList> GetPaymentsId(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);
        }

        public async Task<MultiSelectList> GetViews()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name");

        }

        public async Task<MultiSelectList> GetViewsById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name", ids);
        }

        public async Task<MultiSelectList> GetAccess()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name");
        }

        public async Task<MultiSelectList> GetAccessById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name", ids);

        }
        /// <summary>
        /// for save import demand
        /// </summary>
        /// <param name="demand"></param>
        /// <param name="userId"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<IConfirmation> SaveImportDemand(DemandDto demand, int userId, int branchId)
        {
            tbl_DemandUnits newDemand = Mapper.Map<DemandDto, tbl_DemandUnits>(demand);
            try
            {
                newDemand.FK_DemandUnits_Users_CreatedBy = userId;
                newDemand.FK_DemandUnits_Users_ModidfiedBy = userId;
                newDemand.IsMatched = false;
                newDemand.Date = DateTime.UtcNow.AddMinutes(120);
                newDemand.FK_DemandUnits_Branches_BranchId = branchId;
                _uow.DemandRepo.Add(newDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                    return _conf;
                }
            }
            catch (Exception ex)
            {

                string x = ex.Message;
            }

            if (demand.FinishArr != null && demand.FinishArr.Any())
            {
                foreach (string finish in demand.FinishArr)
                {
                    tbl_Demand_Finishings demandFinish = new tbl_Demand_Finishings
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_DemandFinishing_Users_CreatedBy = userId,
                        FK_DemandFinishing_Users_ModidfiedBy = userId,
                        FK_DemandFinishing_Demand_Id = newDemand.PK_DemandUnits_Id,
                        FK_DemandFinishing_Finish_Id = int.Parse(finish),
                    };
                    _uow.DemandFinishRepo.Add(demandFinish);
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
                    tbl_DemandAccessories demandAccess = new tbl_DemandAccessories
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        FK_DemandAccessories_Users_CreatedBy = userId,
                        FK_DemandAccessories_Users_ModidfiedBy = userId,
                        FK_DemandAccessories_Demand_Id = newDemand.PK_DemandUnits_Id,
                        FK_DemandAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.DemandAccessRepo.Add(demandAccess);
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
                    tbl_DemandViews unitViews = new tbl_DemandViews
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_DemandView_Users_CreatedBy = userId,
                        FK_DemandView_Users_ModidfiedBy = userId,
                        FK_DemandView_Demand_DemandId = newDemand.PK_DemandUnits_Id,
                        FK_DemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.DemandViewRepo.Add(unitViews);
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
        public async Task<IConfirmation> SaveClientDemand(DemandDto demand, int userId, int branchId)
        {
            tbl_DemandUnits newDemand = Mapper.Map<DemandDto, tbl_DemandUnits>(demand);
            try
            {
                newDemand.FK_DemandUnits_Users_CreatedBy = userId;
                newDemand.FK_DemandUnits_Users_ModidfiedBy = userId;
                newDemand.IsMatched = false;
                newDemand.Date = DateTime.UtcNow.AddMinutes(120);
                newDemand.FK_DemandUnits_Branches_BranchId = branchId;
                _uow.DemandRepo.Add(newDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                    return _conf;
                }
            }
            catch (Exception ex)
            {

                string x = ex.Message;
            }

            if (demand.FinishArr != null && demand.FinishArr.Any())
            {
                foreach (string finish in demand.FinishArr)
                {
                    tbl_Demand_Finishings demandFinish = new tbl_Demand_Finishings
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_DemandFinishing_Users_CreatedBy = userId,
                        FK_DemandFinishing_Users_ModidfiedBy = userId,
                        FK_DemandFinishing_Demand_Id = newDemand.PK_DemandUnits_Id,
                        FK_DemandFinishing_Finish_Id = int.Parse(finish),
                    };
                    _uow.DemandFinishRepo.Add(demandFinish);
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
                    tbl_DemandAccessories demandAccess = new tbl_DemandAccessories
                    {
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                        FK_DemandAccessories_Users_CreatedBy = userId,
                        FK_DemandAccessories_Users_ModidfiedBy = userId,
                        FK_DemandAccessories_Demand_Id = newDemand.PK_DemandUnits_Id,
                        FK_DemandAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.DemandAccessRepo.Add(demandAccess);
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
                    tbl_DemandViews unitViews = new tbl_DemandViews
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_DemandView_Users_CreatedBy = userId,
                        FK_DemandView_Users_ModidfiedBy = userId,
                        FK_DemandView_Demand_DemandId = newDemand.PK_DemandUnits_Id,
                        FK_DemandView_View_ViewId = int.Parse(view),
                    };
                    _uow.DemandViewRepo.Add(unitViews);
                }
                _conf.Valid = await _uow.SaveAsync() == demand.ViewsArr.Length;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الاطلالات بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            demand.PK_DemandUnits_Id = newDemand.PK_DemandUnits_Id;
            _conf.AvailablesAndExcluded = await FilterAvailableMatches(demand);
            _conf.availables = await GetSellerName(_conf.availables);
            _conf.Demands.Clear();
            _conf.Demands.Add(Mapper.Map<tbl_DemandUnits, DemandDto>(newDemand));
            return _conf;

        }

        public async Task<IConfirmation> UpdateClientDemand(DemandDto demand, int userId, int branchId)
        {
            if (demand.PK_DemandUnits_Id > 0)
            {
                tbl_DemandUnits DBclientDemand = (await _uow.DemandRepo.FindAsync(a => a.PK_DemandUnits_Id == demand.PK_DemandUnits_Id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                    DBclientDemand.FK_DemandUnits_Categories_Id = demand.FK_DemandUnits_Categories_Id;
                    DBclientDemand.FK_DemandUnits_PaymentMethod_Id = demand.FK_DemandUnits_PaymentMethod_Id;
                    DBclientDemand.FK_DemandUnits_Branches_BranchId = branchId;
                    DBclientDemand.FK_DemandUnits_Usage_Id = demand.FK_DemandUnits_Usage_Id;
                    DBclientDemand.DateOfBuildFrom = demand.DateOfBuildFrom;
                    DBclientDemand.DateOfBuildTo = demand.DateOfBuildTo;
                    DBclientDemand.NoOfElevatorsFrom = demand.NoElevatorsFrom;
                    DBclientDemand.NoOfElevatorsTo = demand.NoElevatorsTo;
                    DBclientDemand.Notes = demand.Notes;
                    DBclientDemand.MinPrice = demand.MinPrice;
                    DBclientDemand.MaxPrice = demand.MaxPrice;
                    DBclientDemand.MaxRooms = demand.MaxRooms;
                    DBclientDemand.MinRooms = demand.MinRooms;
                    DBclientDemand.MinSpace = demand.MinSpace;
                    DBclientDemand.MaxSpace = demand.MaxSpace;
                    DBclientDemand.MinBathRooms = demand.MinBathRooms;
                    DBclientDemand.MaxBathRooms = demand.MaxBathRooms;
                    DBclientDemand.MaxFloor = demand.MaxFloor;
                    DBclientDemand.MinFloor = demand.MinFloor;
                    DBclientDemand.FK_DemandUnits_Regions_FromId = demand.FK_DemandUnits_Regions_FromId;
                    DBclientDemand.FK_DemandUnits_Regions_ToId = demand.FK_DemandUnits_Regions_ToId;
                    DBclientDemand.FK_DemandUnits_Transactions_Id = demand.FK_DemandUnits_Transactions_Id;
                    //DBclientDemand.Title = demand.Title;
                    DBclientDemand.FK_DemandUnits_Users_ModidfiedBy = userId;
                    DBclientDemand.IsFurnished = demand.IsFurnished;
                    DBclientDemand.FK_DemandUnits_Users_Sales = demand.FK_DemandUnits_Users_Sales;
                }
                _uow.DemandRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ الطلب بنجاح";
                    return _conf;
                }
                List<tbl_Demand_Finishings> unitFinishList = (await _uow.DemandFinishRepo.FindAsync(ui => ui.FK_DemandFinishing_Demand_Id == demand.PK_DemandUnits_Id && !ui.IsDeleted)).ToList();

                if (unitFinishList != null && unitFinishList.Any())
                {
                    foreach (tbl_Demand_Finishings item in unitFinishList)
                    {
                        _uow.DemandFinishRepo.Remove(item);
                    }
                    if (demand.FinishArr != null && demand.FinishArr.Any())
                    {
                        foreach (string finish in demand.FinishArr)
                        {
                            tbl_Demand_Finishings demandFinish = new tbl_Demand_Finishings
                            {
                                CreatedAt = DateTime.UtcNow.AddHours(2),
                                ModifiedAt = DateTime.UtcNow.AddHours(2),
                                FK_DemandFinishing_Users_CreatedBy = userId,
                                FK_DemandFinishing_Users_ModidfiedBy = userId,
                                FK_DemandFinishing_Demand_Id = DBclientDemand.PK_DemandUnits_Id,
                                FK_DemandFinishing_Finish_Id = int.Parse(finish),
                            };
                            _uow.DemandFinishRepo.Add(demandFinish);
                        }
                    }

                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ التشطيبات بنجاح!";
                        return _conf;
                    }
                }

                List<tbl_DemandAccessories> unitAccessList = (await _uow.DemandAccessRepo.FindAsync(ui => ui.FK_DemandAccessories_Demand_Id == demand.PK_DemandUnits_Id && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_DemandAccessories item in unitAccessList)
                    {
                        _uow.DemandAccessRepo.Remove(item);
                    }
                    if (demand.AccessoriesArr != null && demand.AccessoriesArr.Any())
                    {
                        foreach (string access in demand.AccessoriesArr)
                        {
                            tbl_DemandAccessories unitAccess = new tbl_DemandAccessories
                            {
                                CreatedAt = DateTime.UtcNow.AddHours(2),
                                ModifiedAt = DateTime.UtcNow.AddHours(2),
                                FK_DemandAccessories_Users_CreatedBy = userId,
                                FK_DemandAccessories_Users_ModidfiedBy = userId,
                                FK_DemandAccessories_Demand_Id = DBclientDemand.PK_DemandUnits_Id,
                                FK_DemandAccessories_Accessories_Id = int.Parse(access),
                            };
                            _uow.DemandAccessRepo.Add(unitAccess);
                        }
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (!_conf.Valid)
                        {
                            _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                            return _conf;
                        }
                    }

                }

                List<tbl_DemandViews> unitViewsList = (await _uow.DemandViewRepo.FindAsync(ui => ui.FK_DemandView_Demand_DemandId == demand.PK_DemandUnits_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_DemandViews item in unitViewsList)
                    {
                        _uow.DemandViewRepo.Remove(item);
                    }
                    if (demand.ViewsArr != null && demand.ViewsArr.Any())
                    {
                        foreach (string view in demand.ViewsArr)
                        {
                            tbl_DemandViews unitView = new tbl_DemandViews
                            {
                                CreatedAt = DateTime.UtcNow.AddHours(2),
                                ModifiedAt = DateTime.UtcNow.AddHours(2),
                                FK_DemandView_Users_CreatedBy = userId,
                                FK_DemandView_Users_ModidfiedBy = userId,
                                FK_DemandView_Demand_DemandId = DBclientDemand.PK_DemandUnits_Id,
                                FK_DemandView_View_ViewId = int.Parse(view),
                            };
                            _uow.DemandViewRepo.Add(unitView);
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
                _conf.AvailablesAndExcluded = await FilterAvailableMatches(demand);
                _conf.Demands.Clear();
                _conf.Demands.Add(Mapper.Map<tbl_DemandUnits, DemandDto>(DBclientDemand));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }
        public async Task<List<AvailableDto>> GetSellerName(List<AvailableDto> availableDtos)
        {
            foreach (AvailableDto item in availableDtos)
            {
                item.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_AvaliableUnits_Clients_ClientId)).FirstOrDefault().Name;
            }
            return availableDtos;
        }
        public async Task<IConfirmation> GetInstantMatches(int demandId)
        {
            DemandDto demand = Mapper.Map<tbl_DemandUnits, DemandDto>((await _uow.DemandRepo.FindAsync(d => d.PK_DemandUnits_Id == demandId)).FirstOrDefault());
            if (demand != null)
            {
                _conf.AvailablesAndExcluded = await FilterAvailableMatches(demand);
                _conf.Demands.Clear();
                _conf.Demands.Add(demand);
                return _conf;
            }
            return _conf;
        }

        public async Task<DemandDto> EditClientDemand(int id)
        {
            List<tbl_DemandUnits> DBDemands = (await _uow.DemandRepo.FindAsync
                (a => a.PK_DemandUnits_Id == id && !a.IsDeleted)).ToList();
            //List<DemandDto> clientDemands = ;

            if (DBDemands.Any() && DBDemands != null)
            {
                tbl_DemandUnits Demand = DBDemands.FirstOrDefault();
                DemandDto clientDemand = Mapper.Map<tbl_DemandUnits, DemandDto>(Demand);
                clientDemand.NoElevatorsFrom = Demand.NoOfElevatorsFrom;
                clientDemand.NoElevatorsTo = Demand.NoOfElevatorsTo;
                clientDemand.AccessoriesIds = (await _uow.DemandAccessRepo.FindAsync(ua => ua.FK_DemandAccessories_Demand_Id == clientDemand.PK_DemandUnits_Id)).Select(x => x.FK_DemandAccessories_Accessories_Id).ToArray();
                clientDemand.ViewsIds = (await _uow.DemandViewRepo.FindAsync(ua => ua.FK_DemandView_Demand_DemandId == clientDemand.PK_DemandUnits_Id)).Select(x => x.FK_DemandView_View_ViewId).ToArray();
                clientDemand.FinishIds = (await _uow.DemandFinishRepo.FindAsync(df => df.FK_DemandFinishing_Demand_Id == clientDemand.PK_DemandUnits_Id)).Select(x => x.FK_DemandFinishing_Finish_Id).ToArray();
                return clientDemand;
            }
            return new DemandDto();
        }

        public async Task<IConfirmation> DeleteClientDemand(int id, int userId)
        {
            if (id > 0)
            {
                tbl_DemandUnits DBclientDemand = (await _uow.DemandRepo.FindAsync(a => a.PK_DemandUnits_Id == id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientDemand != null)
                {
                    DBclientDemand.IsDeleted = true;
                    DBclientDemand.FK_DemandUnits_Users_ModidfiedBy = userId;
                }
                _uow.DemandRepo.Update(DBclientDemand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف الطلب بنجاح";
                    return _conf;
                }

                List<tbl_Demand_Finishings> unitFinishList = (await _uow.DemandFinishRepo.FindAsync(ui => ui.FK_DemandFinishing_Demand_Id == DBclientDemand.PK_DemandUnits_Id && !ui.IsDeleted)).ToList();

                if (unitFinishList != null && unitFinishList.Any())
                {
                    foreach (tbl_Demand_Finishings item in unitFinishList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_DemandFinishing_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.DemandFinishRepo.Update(item);

                    }


                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف التشطيبات بنجاح!";
                        return _conf;
                    }
                }

                List<tbl_DemandAccessories> unitAccessList = (await _uow.DemandAccessRepo.FindAsync(ui => ui.FK_DemandAccessories_Demand_Id == DBclientDemand.PK_DemandUnits_Id)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_DemandAccessories item in unitAccessList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_DemandAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.DemandAccessRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الكماليات بنجاح!";
                        return _conf;
                    }
                }
                List<tbl_DemandViews> unitViewsList = (await _uow.DemandViewRepo.FindAsync(ui => ui.FK_DemandView_Demand_DemandId == DBclientDemand.PK_DemandUnits_Id && !ui.IsDeleted)).ToList();
                if (unitViewsList != null && unitViewsList.Any())
                {
                    foreach (tbl_DemandViews item in unitViewsList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_DemandView_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.DemandViewRepo.Update(item);
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

        public async Task<List<tbl_AvailableUnits>> GetAvailableMatches(DemandDto demand)
        {
            var DALDemand = Mapper.Map<DemandDto, tbl_DemandUnits>(demand);
            List<tbl_AvailableUnits> availables = new List<tbl_AvailableUnits>();
            int fromRegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_DemandUnits_Regions_FromId)).FirstOrDefault().RegCode;
            int toRegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == demand.FK_DemandUnits_Regions_ToId)).FirstOrDefault().RegCode;
            int[] codes = fromRegCode < toRegCode ? new int[] { fromRegCode, toRegCode } : new int[] { toRegCode, fromRegCode };
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            List<tbl_Finishings> finishings = (await _uow.FinishRepo.FindAsync(f => !f.IsDeleted)).ToList();
            int[] finishIds = (demand.FinishArr != null && demand.FinishArr.Any())
                ? demand.FinishArr.Select(f => int.Parse(f)).ToArray()
                : (await _uow.DemandFinishRepo.FindAsync(f => f.FK_DemandFinishing_Demand_Id == demand.PK_DemandUnits_Id))
                .Select(f => f.FK_DemandFinishing_Finish_Id).ToArray();

            if (payments != null && payments.Any() && finishings != null && finishings.Any())
            {
                List<tbl_Finishings> choosenFinishings = new List<tbl_Finishings>();
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == demand.FK_DemandUnits_PaymentMethod_Id);
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
                        if (demand.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailablesForPaymentFinish(DALDemand, codes, finishIds);
                        }
                        else
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailablesForPaymentFinishUsage(DALDemand, codes, finishIds);
                        }
                    }
                    else if (payment.IsMaster && choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForMaster(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForUsage(DALDemand, codes);
                        }
                    }
                    else if (payment.IsMaster && !choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForFinish(DALDemand, codes, finishIds);
                        }
                        else
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForFinishAndUsage(DALDemand, codes, finishIds);
                        }
                    }
                    else if (!payment.IsMaster && choosenFinishings.Exists(f => f.IsMaster))
                    {
                        if (demand.FK_DemandUnits_Usage_Id == UnitUsages.Multiple)
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForPayment(DALDemand, codes);
                        }
                        else
                        {
                            availables = await _uow.AppartmentCustomrepository.AvailableMatchesForPaymentAndUsage(DALDemand, codes);
                        }
                    }
                }
            }
            return availables;

        }



        public async Task<(List<AvailableDto>, AvailableDto, List<AvailableDto>)> FilterAvailableMatches(DemandDto demand)
        {
            List<tbl_AvailableUnits> availables = await GetAvailableMatches(demand);
            List<AvailableDto> VtblToVDto = await GetSellerName(Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>(availables));
            availables = Mapper.Map<List<AvailableDto>, List<tbl_AvailableUnits>>(VtblToVDto);
            List<AvailableDto> availablesWithPreviews = new List<AvailableDto>();
            tbl_AvailableUnits sameClientAvailable = availables.Find(a => a.FK_AvaliableUnits_Clients_ClientId == demand.FK_DemandUnits_Clients_ClientId);
            if (sameClientAvailable != null)
            {
                availables.Remove(sameClientAvailable);
            }
            if (availables.Any())
            {
                List<tbl_PreviewHeaders> demandClientPreviews = (await _uow.PreviewRepo.FindAsync(p => p.FK_PreviewHeaders_Clients_BuyerId == demand.FK_DemandUnits_Clients_ClientId &&
               p.DemandUnit_Id == demand.PK_DemandUnits_Id && !p.IsDeleted && (p.ReviewDate >= DbFunctions.TruncateTime(DateTime.Today)
               //||
               //(_uow.PreviewDetailRepo.Find(d => d.Fk_PreviewDetails_PreviewHeaders_Id == p.PK_PreviewHeaders_Id && d.PostPoneDate != null &&
               //d.PostPoneDate >=DbFunctions.TruncateTime(DateTime.Today)).Any()
               //))
               ))).ToList();
                if (demandClientPreviews != null && demandClientPreviews.Any())
                {
                    foreach (tbl_PreviewHeaders item in demandClientPreviews)
                    {
                        List<tbl_PreviewDetails> previewDetails = (await _uow.PreviewDetailRepo.FindAsync(d => d.Fk_PreviewDetails_PreviewHeaders_Id == item.PK_PreviewHeaders_Id && d.IsNoDecision)).ToList();
                        if (previewDetails != null && previewDetails.Any())
                        {
                            foreach (tbl_PreviewDetails detail in previewDetails)
                            {
                                if (availables.Exists(a => a.PK_AvailableUnits_Id == detail.AvailableUnits_Id))
                                {
                                    tbl_AvailableUnits available = availables.Find(a => a.PK_AvailableUnits_Id == detail.AvailableUnits_Id && a.tbl_units.FK_Units_Categories_Id == detail.Category_Id);
                                    availablesWithPreviews.Add(Mapper.Map<tbl_AvailableUnits, AvailableDto>(available));
                                    availables.Remove(available);
                                }
                            }
                        }
                    }
                }
            }

            return (Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>(availables), Mapper.Map<tbl_AvailableUnits, AvailableDto>(sameClientAvailable), availablesWithPreviews);
        }

        public async Task<List<AvailableDto>> GetAvailableMatchesOnTheFly(DemandDto demand)
        {
            List<tbl_AvailableUnits> availables = await GetAvailableMatches(demand);
            return Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>(availables);
        }

        public async Task<bool> CloseDemand(int id, int userId)
        {
            tbl_DemandUnits demand = (await _uow.DemandRepo.FindAsync(u => u.PK_DemandUnits_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = true;
            demand.FK_DemandUnits_Users_ModidfiedBy = userId;
            _uow.DemandRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> ReleaseDemand(int id, int userId)
        {
            tbl_DemandUnits demand = (await _uow.DemandRepo.FindAsync(u => u.PK_DemandUnits_Id == id && !u.IsDeleted)).FirstOrDefault();
            demand.IsClosed = false;
            demand.FK_DemandUnits_Users_ModidfiedBy = userId;
            _uow.DemandRepo.Update(demand);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateDemandTransaction(int demandId, int transCode)
        {
            tbl_DemandUnits demand = (await _uow.DemandRepo.FindAsync(u => u.PK_DemandUnits_Id == demandId && !u.IsDeleted)).FirstOrDefault();
            if (demand != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                demand.FK_DemandUnits_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<List<DemandDto>> DemandsByDateAndRegion(DateTime fromDate, DateTime toDate, int cat, int regionIdFrom, int regionIdTo, int ElevatorFrom, int ElevatorTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Demand_Id)
        {
            List<DemandDto> DemandDtos = new List<DemandDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (SpaceTo == 0)
            {
                try
                {
                    SpaceTo = decimal.ToInt32((decimal)_uow.DemandCustRepo.GetMaxDecimalValue(m => m.MaxSpace));
                }
                catch (Exception)
                {

                    SpaceTo=1;
                }
            }
            if (PriceTo == 0)
            {
                try
                {
                    decimal price = (decimal)_uow.DemandCustRepo.GetMaxDecimalValue(m => m.MaxPrice);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo=1;
                }
            }
            if (ElevatorTo == 0)
            {
                try
                {
                    ElevatorTo = (int)_uow.DemandCustRepo.GetMaxIntValue(m => m.NoOfElevatorsTo);
                }
                catch (Exception)
                {

                    ElevatorTo=1;
                }
            }
            IEnumerable<tbl_DemandUnits> demands;
            if (regionIdFrom > 0 && ElevatorFrom > 0 && PriceFrom > 0 && SpaceFrom > 0 && cat > 0)
            {
                demands = await _uow.DemandRepo.FindAsync(d => (d.FK_DemandUnits_Regions_ToId <= regionIdTo && d.FK_DemandUnits_Regions_FromId >= regionIdFrom) &&
                (d.NoOfElevatorsFrom >= ElevatorFrom && d.NoOfElevatorsTo <= ElevatorTo) && (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo) && d.FK_DemandUnits_Categories_Id == cat
                && !d.IsDeleted && !d.IsClosed && (d.Date >= fromDate && d.Date <= toDate));
            }
            //if user enter region , elevator , price and space
            else if (regionIdFrom > 0 && ElevatorFrom > 0 && PriceFrom > 0 && SpaceFrom > 0 && cat == 0)
            {
                demands = await _uow.DemandRepo.FindAsync(d => (d.FK_DemandUnits_Regions_ToId <= regionIdTo && d.FK_DemandUnits_Regions_FromId >= regionIdFrom) &&
                (d.NoOfElevatorsFrom >= ElevatorFrom && d.NoOfElevatorsTo <= ElevatorTo) && (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo)
                && !d.IsDeleted && !d.IsClosed && (d.Date >= fromDate && d.Date <= toDate));
            }
            // if user enter didn't set elevator
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && PriceFrom > 0 && SpaceFrom > 0 && cat > 0)
            {
                demands = await _uow.DemandRepo.FindAsync(d => (d.FK_DemandUnits_Regions_ToId <= regionIdTo && d.FK_DemandUnits_Regions_FromId >= regionIdFrom) &&
                 (d.NoOfElevatorsFrom >= ElevatorFrom && d.NoOfElevatorsTo <= ElevatorTo) && (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                 (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo) && d.FK_DemandUnits_Categories_Id == cat
                 && !d.IsDeleted && !d.IsClosed && (d.Date >= fromDate && d.Date <= toDate));
            }
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && PriceFrom > 0 && SpaceFrom > 0 && cat == 0)
            {
                demands = await _uow.DemandRepo.FindAsync(d => (d.NoOfElevatorsFrom >= ElevatorFrom && d.NoOfElevatorsTo <= ElevatorTo) &&
                (d.MinPrice >= PriceFrom && d.MaxPrice <= PriceTo) &&
                (d.MinSpace >= SpaceFrom && d.MaxSpace <= SpaceTo)
                && !d.IsDeleted && !d.IsClosed && (d.Date >= fromDate && d.Date <= toDate));
            }
            else
            {
                demands = await _uow.DemandRepo.FindAsync(d => !d.IsDeleted && !d.IsClosed && (d.Date >= fromDate && d.Date <= toDate));
            }
            if (Demand_Id != 0)
            {
                demands = demands.Where(a => a.PK_DemandUnits_Id == Demand_Id && !a.IsDeleted && !a.IsClosed);
            }
            DemandDtos = Mapper.Map<IEnumerable<tbl_DemandUnits>, IEnumerable<DemandDto>>(demands).ToList();
            return DemandDtos;
        }

        public async Task<IConfirmation> CreateDemandForAvailable(AvailableDto available, int userId, int branchId, int clientId, string notes)
        {
            tbl_DemandUnits newDemand = new tbl_DemandUnits
            {
                FK_DemandUnits_Users_CreatedBy = userId,
                FK_DemandUnits_Users_ModidfiedBy = userId,
                IsMatched = false,
                IsClosed = false,
                IsDeleted = false,
                IsResidential = false,
                Date = DateTime.UtcNow.AddMinutes(120),
                FK_DemandUnits_Branches_BranchId = branchId,
                DateOfBuildFrom = available.DateOfBuild,
                DateOfBuildTo = available.DateOfBuild,
                FK_DemandUnits_Categories_Id = Categories.Apartements,
                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                FK_DemandUnits_Clients_ClientId = clientId,
                FK_DemandUnits_PaymentMethod_Id = available.FK_AvailableUnits_PaymentMethod_Id,
                FK_DemandUnits_Regions_FromId = available.tbl_units.FK_Units_Regions_Id,
                FK_DemandUnits_Regions_ToId = available.tbl_units.FK_Units_Regions_Id,
                FK_DemandUnits_Transactions_Id = available.FK_AvaliableUnits_Transaction_TransactionId,
                FK_DemandUnits_Usage_Id = available.FK_AvailableUnits_Usage_Id,
                FK_DemandUnits_Users_Sales = available.FK_AvaliableUnits_Users_SalesId,
                IsFurnished = available.tbl_units.IsFurniture,
                MaxBathRooms = available.tbl_units.Bathrooms,
                MaxFloor = available.tbl_units.Floor,
                MaxPrice = available.Price,
                MaxRooms = available.tbl_units.Rooms,
                MaxSpace = available.tbl_units.Space,
                MinBathRooms = available.tbl_units.Bathrooms,
                MinFloor = available.tbl_units.Floor,
                MinPrice = available.Price,
                MinRooms = available.tbl_units.Rooms,
                MinSpace = available.tbl_units.Space
            };
            newDemand.MinBathRooms = available.tbl_units.Bathrooms;
            newDemand.Notes = notes;
            newDemand.NoOfElevatorsFrom = available.NoOfElevators;
            newDemand.NoOfElevatorsTo = available.NoOfElevators;
            _uow.DemandRepo.Add(newDemand);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }

            _conf.Message = "تم الحفظ بنجاح!";
            _conf.availables.Add(available);
            _conf.Demands.Add(Mapper.Map<tbl_DemandUnits, DemandDto>(newDemand));
            return _conf;

        }


    }
}
