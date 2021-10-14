using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{
    public class VillasAvailableService : IVillasAvailablesService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public VillasAvailableService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _context = context;
        }
        public VillasAvailableService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }
        public async Task<List<VillasAvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,
            int Payments, int BasisOfInstallmentDropdown, int OverFrom, int OverTo, int RemainingFrom, int RemainingTo,
            int YearOfInstallmentFrom, int YearOfInstallmentTo, string VillaNumber, string GroupNumber, int Available_Id)
        {
            List<VillasAvailableDto> AvailableDtos = new List<VillasAvailableDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (from == new DateTime(2017, 1, 18))
            {
                try
                {
                    from = (DateTime)_uow.VillasAvailablesCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    from = DateTime.Now;
                }
            }
            if (SpaceTo == 0)
            {
                try
                {
                    decimal Space = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.Space);
                    SpaceTo = decimal.ToInt32(Space);
                }
                catch (Exception)
                {

                    SpaceTo = 1;
                }
            }
            if (PriceTo == 0)
            {
                try
                {
                    decimal price = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.Price);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo = 1;
                }
            }
            if (RemainingTo == 0)
            {
                try
                {
                    decimal Remaining = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.Remaining);
                    RemainingTo = decimal.ToInt32(Remaining);
                }
                catch (Exception)
                {

                    RemainingTo = 1;
                }
            }

            if (OverTo == 0)
            {
                try
                {
                    decimal Over = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.Over);
                    OverTo = decimal.ToInt32(Over);
                }
                catch (Exception)
                {

                    OverTo = 1;
                }
            }
            if (YearOfInstallmentTo == 0)
            {
                try
                {
                    decimal YearOfInstallment = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.YearOfInstallment);
                    YearOfInstallmentTo = decimal.ToInt32(YearOfInstallment);
                }
                catch (Exception)
                {

                    YearOfInstallmentTo = 1;
                }
            }
            IEnumerable<tbl_VillasAvailables> availalbles;
            // if user selcet all
            if (regionIdFrom > 0 && SpaceFrom > 0 && PriceFrom > 0)
            {
                availalbles = (await _uow.VillasAvailablesRepo.FindAsync(a => a.FK_VillasAvailables_Regions_Id >= regionIdFrom && a.FK_VillasAvailables_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && (a.Price >= PriceFrom && a.Price <= PriceTo) &&
                   a.Space >= SpaceFrom && a.Space <= SpaceTo && (a.CreatedAt >= from && a.CreatedAt <= to)));
            }
            else
            {
                availalbles = (await _uow.VillasAvailablesRepo.FindAsync(a => !a.IsDeleted && !a.IsClosed &&
                  (a.CreatedAt >= from && a.CreatedAt <= to)));
            }
            if (Payments == 4)
            {
                if (BasisOfInstallmentDropdown == 0)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                  a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && !a.IsDeleted && !a.IsClosed);
                }
                else if (BasisOfInstallmentDropdown == 1)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && a.BasisOfInstallment == 1 && !a.IsDeleted && !a.IsClosed);
                }
                else if (BasisOfInstallmentDropdown == 2)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && a.BasisOfInstallment == 2 && !a.IsDeleted && !a.IsClosed);
                }
                else if (BasisOfInstallmentDropdown == 3)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && a.BasisOfInstallment == 3 && !a.IsDeleted && !a.IsClosed);
                }
                else if (BasisOfInstallmentDropdown == 4)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && a.BasisOfInstallment == 4 && !a.IsDeleted && !a.IsClosed);
                }
            }
            if (VillaNumber != null)
            {
                availalbles = availalbles.Where(a => a.VillaNumber == VillaNumber && !a.IsDeleted && !a.IsClosed);
            }
            if (GroupNumber != null)
            {
                availalbles = availalbles.Where(a => a.GroupNumber == GroupNumber && !a.IsDeleted && !a.IsClosed);
            }
            if (Available_Id != 0)
            {
                availalbles = availalbles.Where(a => a.PK_VillasAvailables_Id == Available_Id && !a.IsDeleted && !a.IsClosed);
            }
            AvailableDtos = Mapper.Map<IEnumerable<tbl_VillasAvailables>, IEnumerable<VillasAvailableDto>>(availalbles).ToList();
            return AvailableDtos;
        }

        public async Task<SelectList> GetPaymentsAsync()
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod");

        }
        public async Task<SelectList> GetUsages()
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_UnitUsage_Id", "Name");
        }

        public async Task<SelectList> GetUsagesId(int usageId)
        {
            return new SelectList(Mapper.Map<List<tbl_UnitUsage>, List<UnitUsageDto>>((await _uow.UnitUsageRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_UnitUsage_Id", "Name", _uow.UnitUsageRepo.FindAsync(u => u.PK_UnitUsage_Id == usageId));
        }
        public async Task<IConfirmation> CheckDuplicateClientSales(VillasAvailableDto ClientSales)
        {
            bool exists = (await _uow.VillasAvailablesRepo.FindAsync(b => b.FK_VillasAvailables_Transactions_Id == ClientSales.FK_VillasAvailables_Transactions_Id &&
            b.Price == ClientSales.Price && b.FK_VillasAvailables_PaymentMethod_Id == ClientSales.FK_VillasAvailables_PaymentMethod_Id &&
            b.GroupNumber == ClientSales.GroupNumber && b.VillaNumber == ClientSales.VillaNumber && b.FK_VillasAvailables_Regions_Id == ClientSales.FK_VillasAvailables_Regions_Id &&
            b.FK_VillasAvailables_Categories_Id == ClientSales.FK_VillasAvailables_Categories_Id && b.Space == ClientSales.Space &&
            b.Rooms == ClientSales.Rooms && b.BathRooms == ClientSales.BathRooms &&
            b.FK_VillasAvailables_Finishings_Id == ClientSales.FK_VillasAvailables_Finishings_Id &&
            b.FK_VillasAvailables_View_Id == ClientSales.FK_VillasAvailables_View_Id && b.FK_VillasAvailables_Clients_ClientId == ClientSales.FK_VillasAvailables_Clients_ClientId
            && b.AreaSpace == ClientSales.AreaSpace)).Any();
            if (exists)
            {
                _conf.Valid = false;
                _conf.Message = "هذا العميل لدية نفس العرض ";
            }
            else
            {
                _conf.Valid = true;

            }

            return _conf;
        }

        public async Task<List<VillasAvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId)
        {
            IEnumerable<tbl_VillasAvailables> availalbles = (await _uow.VillasAvailablesRepo.FindAsync(a => a.FK_VillasAvailables_Regions_Id == regionId && a.FK_VillasAvailables_Branches_BranchId == branchId && !a.IsDeleted && !a.IsClosed && (a.CreatedAt.Day == date.Day && a.CreatedAt.Month == date.Month && a.CreatedAt.Year == date.Year)));
            List<VillasAvailableDto> AvailableDtos = new List<VillasAvailableDto>();
            foreach (tbl_VillasAvailables available in availalbles)
            {
                VillasAvailableDto availableDto = new VillasAvailableDto
                {
                    PK_VillasAvailables_Id = available.PK_VillasAvailables_Id,
                    BathRooms = available.BathRooms,
                    Rooms = available.Rooms,
                    Space = available.Space,
                    Price = available.Price
                };
                availableDto.FK_VillasAvailables_Regions_Id = available.FK_VillasAvailables_Regions_Id;
                availableDto.FK_VillasAvailables_Finishings_Id = available.FK_VillasAvailables_Finishings_Id;
                AvailableDtos.Add(availableDto);
            }

            return AvailableDtos;
        }

        public async Task<List<VillasAvailableDto>> ClientSales(int id)
        {
            List<VillasAvailableDto> clientSales = Mapper.Map<List<tbl_VillasAvailables>, List<VillasAvailableDto>>((await _uow.VillasAvailablesRepo.FindAsync(
                a => a.FK_VillasAvailables_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed))
                .ToList());

            if (clientSales.Any() && clientSales != null)
            {
                foreach (VillasAvailableDto item in clientSales)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_VillasAvailables_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + "فيلات";
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientSales;
            }
            return new List<VillasAvailableDto>();
        }

        public async Task<bool> CloseAvailable(int id, int userId)
        {
            tbl_VillasAvailables available = (await _uow.VillasAvailablesRepo.FindAsync(u => u.PK_VillasAvailables_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = true;
            available.FK_VillasAvailables_Users_ModidfiedBy = userId;
            _uow.VillasAvailablesRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> DeleteClientSale(int id, int userId)
        {
            if (id > 0)
            {
                tbl_VillasAvailables DBclientSale = (await _uow.VillasAvailablesRepo.FindAsync
                         (a => a.PK_VillasAvailables_Id == id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientSale != null)
                {
                    DBclientSale.IsDeleted = true;
                    DBclientSale.FK_VillasAvailables_Users_ModidfiedBy = userId;
                }
                _uow.VillasAvailablesRepo.Update(DBclientSale);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف العرض بنجاح";
                    return _conf;
                }

                List<tbl_VillasImages> unitImages = (await _uow.VillasImagesRepo.FindAsync(ui => ui.FK_VillasImages_VillasAvailables_VillaId == DBclientSale.PK_VillasAvailables_Id && !ui.IsDeleted)).ToList();
                if (unitImages != null && unitImages.Count > 0)
                {
                    foreach (tbl_VillasImages path in unitImages)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/ClientSalesImages"), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    foreach (tbl_VillasImages item in unitImages)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_UnitImages_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.VillasImagesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الصور بنجاح!";
                        return _conf;
                    }

                }

                List<tbl_VillaAccessories> unitAccessList = (await _uow.VillaAccessoriesRepo.FindAsync(ui => ui.FK_VillaAccessories_Villas_Id == DBclientSale.PK_VillasAvailables_Id && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_VillaAccessories item in unitAccessList)
                    {
                        item.ModifiedAt = DateTime.UtcNow.AddMinutes(120);
                        item.FK_VillasAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.VillaAccessoriesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الكماليات بنجاح!";
                        return _conf;
                    }
                }
                _conf.Message = "تم الحذف بنجاح!";
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم اللحذف حدث خطأ ما!";
            return _conf;
        }

        public async Task<VillasAvailableDto> EditClientSale(int id)
        {
            List<VillasAvailableDto> clientSales = Mapper.Map<List<tbl_VillasAvailables>, List<VillasAvailableDto>>((await _uow.VillasAvailablesRepo.FindAsync
               (a => a.PK_VillasAvailables_Id == id && !a.IsDeleted)).ToList());

            if (clientSales.Any() && clientSales != null)
            {
                VillasAvailableDto clientSale = clientSales.FirstOrDefault();
                clientSale.AccessoriesIds = (await _uow.VillaAccessoriesRepo.FindAsync(ua => ua.FK_VillaAccessories_Villas_Id == clientSale.PK_VillasAvailables_Id)).Select(x => x.FK_VillasAccessories_Accessories_Id).ToArray();
                clientSale.Images = (await _uow.VillasImagesRepo.FindAsync(ui => ui.FK_VillasImages_VillasAvailables_VillaId == clientSale.PK_VillasAvailables_Id)).Select(x => x.ImageUrl).ToArray();
                clientSale.SellerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == clientSale.FK_VillasAvailables_Clients_ClientId)).FirstOrDefault().Name;

                return clientSale;
            }
            return new VillasAvailableDto();
        }

        public async Task<(List<VillasDemandDto>, VillasDemandDto, List<VillasDemandDto>, List<VillasDemandDto>)> FilterAvailables(VillasAvailableDto available, int userId)
        {
            (List<VillasDemandDto>, List<VillasDemandDto>) demands = await GetAvailableMatches(available, userId);
            List<VillasDemandDto> demandsWithPreviews = new List<VillasDemandDto>();
            VillasDemandDto sameClientDemand = demands.Item1.Find(d => d.FK_VillasDemands_Clients_ClientId == available.FK_VillasAvailables_Clients_ClientId);
            if (sameClientDemand != null)
            {
                demands.Item1.Remove(sameClientDemand);
            }
            demands.Item2.RemoveAll(d => d.FK_VillasDemands_Clients_ClientId == available.FK_VillasAvailables_Clients_ClientId);
            //if (demands.Item1.Any())
            //{
            //    var availablePreviews = (from header in await _uow.PreviewRepo.GetAllAsync()
            //                             join detail
            //                             in await _uow.PreviewDetailRepo.GetAllAsync()
            //                             on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
            //                             where (detail.AvailableUnits_Id == available.PK_VillasAvailables_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_VillasAvailables_Clients_ClientId)
            //                                  && (header.ReviewDate >= DateTime.Today)
            //                                  && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision //remove the rejected 
            //                                  && demands.Item1.Exists(d => d.PK_VillasDemands_Id == header.DemandUnit_Id && d.FK_VillasDemands_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)
            //                             select new
            //                             {
            //                                 HeaderId = header.PK_PreviewHeaders_Id,
            //                                 PreviewDate = header.ReviewDate,
            //                                 DemandId = header.DemandUnit_Id,
            //                                 BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
            //                                 AvailableId = detail.AvailableUnits_Id,
            //                                 Seller = detail.Fk_PreviewDetails_Clients_SellerId,
            //                                 DetailId = detail.PK_PreviewDetails_Id
            //                             }
            //                             ).ToList();
            //    if (availablePreviews.Any() && availablePreviews != null)
            //    {
            //        foreach (VillasDemandDto demand in demands.Item1.ToList())
            //        {
            //            if (availablePreviews.Exists(ap => ap.DemandId == demand.PK_VillasDemands_Id))
            //            {
            //                demandsWithPreviews.Add(demand);
            //                demands.Item1.Remove(demand);
            //            }
            //        }
            //    }
            //}
            //if (demands.Item2.Any())
            //{
            //    var availablePreviews = (from header in await _uow.PreviewRepo.GetAllAsync()
            //                             join detail
            //                             in await _uow.PreviewDetailRepo.GetAllAsync()
            //                             on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
            //                             where (detail.AvailableUnits_Id == available.PK_VillasAvailables_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_VillasAvailables_Clients_ClientId)
            //                                  && (header.ReviewDate >= DateTime.Today)
            //                                  && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision //remove the rejected 
            //                                  && demands.Item2.Exists(d => d.PK_VillasDemands_Id == header.DemandUnit_Id && d.FK_VillasDemands_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)
            //                             select new
            //                             {
            //                                 HeaderId = header.PK_PreviewHeaders_Id,
            //                                 PreviewDate = header.ReviewDate,
            //                                 DemandId = header.DemandUnit_Id,
            //                                 BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
            //                                 AvailableId = detail.AvailableUnits_Id,
            //                                 Seller = detail.Fk_PreviewDetails_Clients_SellerId,
            //                                 DetailId = detail.PK_PreviewDetails_Id
            //                             }
            //                             ).ToList();
            if (demands.Item1.Any() || demands.Item2.Any())
            {

                var availablePreviews = await _uow.VillaCustomrepository.FilterDemandsHasPreviews(demands.Item1, demands.Item2, available);

                if (availablePreviews.Any() && availablePreviews != null)
                {
                    foreach (VillasDemandDto demand in demands.Item2.ToList())
                    {
                        if (availablePreviews.Exists(ap => ap.DemandId == demand.PK_VillasDemands_Id))
                        {
                            demands.Item2.Remove(demand);
                        }
                    }
                }
            }


            return (demands.Item1, sameClientDemand, demandsWithPreviews, demands.Item2);
        }

        public async Task<MultiSelectList> GetAccess()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name");
        }

        public async Task<MultiSelectList> GetAccessById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name", ids);
        }

        public async Task<(List<VillasDemandDto>, List<VillasDemandDto>)> GetAvailableMatches(VillasAvailableDto available, int userId)
        {
            var DALAvailable = Mapper.Map<VillasAvailableDto, tbl_VillasAvailables>(available);
            int RegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == available.FK_VillasAvailables_Regions_Id)).FirstOrDefault().RegCode;
            List<tbl_VillasDemands> demands = new List<tbl_VillasDemands>();

            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(p => !p.IsDeleted).ToList();
            if (payments != null && payments.Any())
            {
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == available.FK_VillasAvailables_PaymentMethod_Id);
                if (payment != null && !payment.IsMaster)
                {
                    if (available.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple)
                    {
                        demands = await _uow.VillaCustomrepository.DemandsForpayment(DALAvailable);
                    }
                    else
                    {
                        demands = await _uow.VillaCustomrepository.DemandsForpaymentAndUsage(DALAvailable);
                    }
                }
                //else
                //{
                //    if (available.FK_VillasAvailables_Usage_Id == UnitUsages.Multiple)
                //    {
                //        demands = await _uow.VillaCustomrepository.DemandsForAnypaymentAnyUsage(DALAvailable);
                //    }
                //    else
                //    {
                //        demands = await _uow.VillaCustomrepository.DemandsForAnypaymentAndUsage(DALAvailable);
                //    }
                //}
            }
            var collegueDemands = demands.Any() ? demands.Where(d => d.FK_VillasDemands_Users_CreatedBy != userId).ToList() : new List<tbl_VillasDemands>();

            var empDemands = demands.Any() ? demands.Where(d => d.FK_VillasDemands_Users_CreatedBy == userId).ToList() : new List<tbl_VillasDemands>();

            return (Mapper.Map<List<tbl_VillasDemands>, List<VillasDemandDto>>(empDemands), Mapper.Map<List<tbl_VillasDemands>, List<VillasDemandDto>>(collegueDemands));
        }


        public async Task<SelectList> GetCats()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Categories_Id", "CategoryName");

        }

        public async Task<SelectList> GetCatsById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Categories_Id", "CategoryName", await _uow.CatRepo.FindAsync(cat => cat.PK_Categories_Id == id));
        }

        public async Task<List<VillasAvailableDto>> GetClosedSales(int id)
        {
            List<VillasAvailableDto> clientSales = Mapper.Map<List<tbl_VillasAvailables>, List<VillasAvailableDto>>((await _uow.VillasAvailablesRepo.FindAsync
                (a => a.FK_VillasAvailables_Clients_ClientId == id && !a.IsDeleted))
               .ToList());

            if (clientSales.Any() && clientSales != null)
            {
                foreach (VillasAvailableDto item in clientSales)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_VillasAvailables_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + "فيلات";
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientSales;
            }
            return new List<VillasAvailableDto>();
        }

        public async Task<SelectList> GetFinishings()
        {
            return new SelectList(Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false && !v.IsMaster).ToList()), "PK_Finishings_Id", "Type");

        }

        public async Task<SelectList> GetFinishingsById(int id) => new SelectList(Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false && !v.IsMaster).ToList()), "PK_Finishings_Id", "Type", await _uow.FinishRepo.FindAsync(f => f.PK_Finishings_Id == id));

        public async Task<IConfirmation> GetInstantMatches(int saleId, int userId)
        {
            tbl_VillasAvailables DBsale = (await _uow.VillasAvailablesRepo.FindAsync(d => d.PK_VillasAvailables_Id == saleId)).FirstOrDefault();
            VillasAvailableDto sale = Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(DBsale);

            if (sale != null)
            {
                sale = Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(DBsale);
                _conf.VillDemandsAndExcluded = await FilterAvailables(sale, userId);
                foreach (VillasDemandDto item in _conf.VillDemands)
                {
                    item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasDemands_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.VillAvailables.Clear();
                _conf.VillAvailables.Add(sale);
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
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Regions_Id", "Region");

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
                return new SelectList(list, "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));

            }
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));
        }

        public async Task<SelectList> GetViews()
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name");

        }

        public async Task<SelectList> GetViewsById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name", id);

        }

        public async Task<bool> ReleaseAvailable(int id, int userId)
        {
            tbl_VillasAvailables available = (await _uow.VillasAvailablesRepo.FindAsync(u => u.PK_VillasAvailables_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = false;
            available.FK_VillasAvailables_Users_ModidfiedBy = userId;
            _uow.VillasAvailablesRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> SaveImportedVillasAvailable(ImportVillasAvailableDto sale, int userId, int branchId)
        {
            tbl_VillasAvailables newUnit = Mapper.Map<ImportVillasAvailableDto, tbl_VillasAvailables>(sale);
            newUnit.FK_VillasAvailables_Users_CreatedBy = userId;
            newUnit.FK_VillasAvailables_Users_ModidfiedBy = userId;
            newUnit.FK_VillasAvailables_Branches_BranchId = branchId;
            newUnit.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            sale.AdvancePayment = sale.Price + sale.Over;
            _uow.VillasAvailablesRepo.Add(newUnit);

            _conf.Valid = await _uow.SaveAsync() > 0;


            int i = 0;

            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ بيانات الوحدة بنجاح!";
                return _conf;
            }

            tbl_VillasAvailables newSale = Mapper.Map<ImportVillasAvailableDto, tbl_VillasAvailables>(sale);
            newSale.FK_VillasAvailables_Users_CreatedBy = userId;
            newSale.FK_VillasAvailables_Users_ModidfiedBy = userId;

            newSale.PK_VillasAvailables_Id = newUnit.PK_VillasAvailables_Id;
            newSale.FK_VillasAvailables_PaymentMethod_Id = sale.FK_AvailableUnits_PaymentMethod_Id;
            newSale.FK_VillasAvailables_Branches_BranchId = branchId;
            _uow.VillasAvailablesRepo.Add(newSale);

           
                _conf.Valid = await _uow.SaveAsync() > 0;
         

            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            _conf.Message = "تم الحفظ بنجاح!";

            return _conf;
        }


        public async Task<IConfirmation> SaveClientSale(VillasAvailableDto sale, int userId, List<string> images, int branchId)
        {
            tbl_VillasAvailables newUnit = Mapper.Map<VillasAvailableDto, tbl_VillasAvailables>(sale);
            newUnit.FK_VillasAvailables_Users_CreatedBy = userId;
            newUnit.FK_VillasAvailables_Users_ModidfiedBy = userId;
            newUnit.FK_VillasAvailables_Branches_BranchId = branchId;
            newUnit.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            sale.AdvancePayment = sale.Price + sale.Over;
            _uow.VillasAvailablesRepo.Add(newUnit);
            _conf.Valid = await _uow.SaveAsync() > 0;
            int i = 0;

            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ بيانات الوحدة بنجاح!";
                return _conf;
            }
            if (images != null && images.Any())
            {
                foreach (string path in images)
                {
                    tbl_VillasImages unitImage = new tbl_VillasImages
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_UnitImages_Users_CreatedBy = userId,
                        FK_UnitImages_Users_ModidfiedBy = userId,
                        FK_VillasImages_VillasAvailables_VillaId = newUnit.PK_VillasAvailables_Id,
                        ImageUrl = path,
                    };
                    if (i == 0)
                    {
                        unitImage.IsMainImage = true;
                    }
                    _uow.VillasImagesRepo.Add(unitImage);
                    i++;
                }

                _conf.Valid = await _uow.SaveAsync() == (images.Count);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الصور بنجاح!";
                    return _conf;
                }
            }

            if (sale.AccessoriesArr != null && sale.AccessoriesArr.Any())
            {
                foreach (string access in sale.AccessoriesArr)
                {
                    tbl_VillaAccessories unitAccess = new tbl_VillaAccessories
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_VillasAccessories_Users_CreatedBy = userId,
                        FK_VillasAccessories_Users_ModidfiedBy = userId,
                        FK_VillaAccessories_Villas_Id = newUnit.PK_VillasAvailables_Id,
                        FK_VillasAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.VillaAccessoriesRepo.Add(unitAccess);
                }
                _conf.Valid = await _uow.SaveAsync() == (sale.AccessoriesArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                    return _conf;
                }
            }
            tbl_VillasAvailables newSale = Mapper.Map<VillasAvailableDto, tbl_VillasAvailables>(sale);
            newSale.FK_VillasAvailables_Users_CreatedBy = userId;
            newSale.FK_VillasAvailables_Users_ModidfiedBy = userId;

            newSale.PK_VillasAvailables_Id = newUnit.PK_VillasAvailables_Id;
            newSale.FK_VillasAvailables_PaymentMethod_Id = sale.FK_VillasAvailables_PaymentMethod_Id;
            newSale.FK_VillasAvailables_Branches_BranchId = branchId;
            _uow.VillasAvailablesRepo.Add(newSale);
            //_conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            _conf.Message = "تم الحفظ بنجاح!";
            sale.PK_VillasAvailables_Id = newSale.PK_VillasAvailables_Id;
            _conf.VillDemandsAndExcluded = await FilterAvailables(sale, userId);
            foreach (VillasDemandDto item in _conf.VillDemandsAndExcluded.Item1)
            {
                item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasDemands_Clients_ClientId)).FirstOrDefault().Name;
            }
            _conf.VillAvailables.Clear();
            _conf.VillAvailables.Add(Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(newSale));
            return _conf;

        }

        public IConfirmation SavePhotos(HttpFileCollectionBase files)
        {
            IConfirmation conf = ValidatePhotos(files);
            if (conf.Valid)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = Path.Combine("Assets/Img/ClientSalesImages/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                        file.SaveAs(_server.MapPath("/" + fname));
                        _conf.UrlList.Add(fname);
                        _conf.Message = "تم حفظ الصور بنجاح!";
                    }
                    return _conf;
                }
                catch (Exception)
                {

                    _conf.Valid = false;
                    _conf.Message = "حدث خطا ما عند حفظ الصور!";
                }
            }

            return _conf;

        }

        public async Task<bool> UpdateAvailableTransaction(int availableId, int transCode)
        {
            tbl_VillasAvailables available = (await _uow.VillasAvailablesRepo.FindAsync(u => u.PK_VillasAvailables_Id == availableId && !u.IsDeleted)).FirstOrDefault();
            if (available != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                available.FK_VillasAvailables_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<IConfirmation> UpdateClientSale(VillasAvailableDto sale, int userId, List<string> images, int branchId)
        {
            if (sale.PK_VillasAvailables_Id > 0)
            {
                tbl_VillasAvailables DBclientSale = (await _uow.VillasAvailablesRepo.FindAsync
                    (a => a.PK_VillasAvailables_Id == sale.PK_VillasAvailables_Id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientSale != null)
                {
                    DBclientSale.CreatedAt = sale.CreatedAt;
                    DBclientSale.FK_VillasAvailables_Transactions_Id = sale.FK_VillasAvailables_Transactions_Id;
                    DBclientSale.FK_VillasAvailables_PaymentMethod_Id = sale.FK_VillasAvailables_PaymentMethod_Id;
                    DBclientSale.IsNegotiable = sale.IsNegotiable;
                    DBclientSale.Notes = sale.Notes;
                    DBclientSale.Price = sale.Price;
                    DBclientSale.BasisOfInstallment = sale.BasisOfInstallment;
                    DBclientSale.YearOfInstallment = sale.YearOfInstallment;
                    DBclientSale.Remaining = sale.Remaining;
                    DBclientSale.Over = sale.Over;
                    DBclientSale.FK_VillasAvailables_Users_ModidfiedBy = userId;
                    DBclientSale.FK_VillasAvailables_Branches_BranchId = branchId;
                    DBclientSale.FK_VillasAvailables_Users_SalesId = sale.FK_VillasAvailables_Users_SalesId;
                    DBclientSale.BathRooms = sale.BathRooms;
                    DBclientSale.FK_VillasAvailables_Categories_Id = sale.FK_VillasAvailables_Categories_Id;
                    DBclientSale.FK_VillasAvailables_Finishings_Id = sale.FK_VillasAvailables_Finishings_Id;
                    DBclientSale.FK_VillasAvailables_Regions_Id = sale.FK_VillasAvailables_Regions_Id;
                    DBclientSale.FK_VillasAvailables_View_Id = sale.FK_VillasAvailables_View_Id;
                    DBclientSale.Rooms = sale.Rooms;
                    DBclientSale.Space = sale.Space;
                    DBclientSale.VillaNumber = sale.VillaNumber;
                    DBclientSale.GroupNumber = sale.GroupNumber;
                    DBclientSale.FK_VillasAvailables_Usage_Id = sale.FK_VillasAvailables_Usage_Id;
                    DBclientSale.FK_VillasAvailables_Users_ModidfiedBy = userId;
                    DBclientSale.IsFurnished = sale.IsFurnished;
                    DBclientSale.IsMarketResearch = sale.IsMarketResearch;
                    DBclientSale.AdvancePayment = sale.Price + sale.Over;
                }
                _uow.VillasAvailablesRepo.Update(DBclientSale);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ العرض بنجاح";
                    return _conf;
                }


                if (images != null && images.Any())
                {
                    List<tbl_VillasImages> unitImages = (await _uow.VillasImagesRepo.FindAsync(ui => ui.FK_VillasImages_VillasAvailables_VillaId == sale.PK_VillasAvailables_Id && !ui.IsDeleted)).ToList();
                    foreach (tbl_VillasImages item in unitImages)
                    {
                        _uow.VillasImagesRepo.Remove(item);
                    }

                    int i = 0;
                    foreach (string path in images)
                    {
                        tbl_VillasImages unitImage = new tbl_VillasImages
                        {
                            CreatedAt = DateTime.UtcNow.AddMinutes(120),
                            ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                            FK_UnitImages_Users_CreatedBy = userId,
                            FK_UnitImages_Users_ModidfiedBy = userId,
                            FK_VillasImages_VillasAvailables_VillaId = sale.PK_VillasAvailables_Id,
                            ImageUrl = path,
                        };
                        if (i == 0)
                        {
                            unitImage.IsMainImage = true;
                        }
                        _uow.VillasImagesRepo.Add(unitImage);
                        i++;
                    }

                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ الصور بنجاح!";
                        return _conf;
                    }
                    foreach (tbl_VillasImages path in unitImages)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/ClientSalesImages"), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                }

                List<tbl_VillaAccessories> unitAccessList = (await _uow.VillaAccessoriesRepo.FindAsync(ui => ui.FK_VillaAccessories_Villas_Id == sale.PK_VillasAvailables_Id && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_VillaAccessories item in unitAccessList)
                    {
                        _uow.VillaAccessoriesRepo.Remove(item);
                    }
                    if (sale.AccessoriesArr != null && sale.AccessoriesArr.Any())
                    {
                        foreach (string access in sale.AccessoriesArr)
                        {
                            tbl_VillaAccessories unitAccess = new tbl_VillaAccessories
                            {
                                CreatedAt = DateTime.UtcNow.AddMinutes(120),
                                ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                                FK_VillasAccessories_Users_CreatedBy = userId,
                                FK_VillasAccessories_Users_ModidfiedBy = userId,
                                FK_VillaAccessories_Villas_Id = sale.PK_VillasAvailables_Id,
                                FK_VillasAccessories_Accessories_Id = int.Parse(access),
                            };
                            _uow.VillaAccessoriesRepo.Add(unitAccess);
                        }
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (!_conf.Valid)
                        {
                            _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                            return _conf;
                        }
                    }

                }
                _conf.Message = "تم الحفظ بنجاح!";
                _conf.VillDemandsAndExcluded = await FilterAvailables(sale, userId);
                foreach (VillasDemandDto item in _conf.VillDemands)
                {
                    item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_VillasDemands_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.VillAvailables.Clear();
                _conf.VillAvailables.Add(Mapper.Map<tbl_VillasAvailables, VillasAvailableDto>(DBclientSale));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;
        }

        public IConfirmation ValidatePhotos(HttpFileCollectionBase files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                if (file.ContentLength > (3 * (1024 * 1024)))
                {
                    _conf.Message = "Not allowed to upload files over 3 MB";
                    _conf.Valid = false;
                    break;
                }
                string ext = file.ContentType.Split('/')[1].ToLower();
                if (!ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase) &&
                    !ext.Equals("png", StringComparison.OrdinalIgnoreCase) &&
                     !ext.Equals("jpg", StringComparison.OrdinalIgnoreCase))

                {
                    _conf.Message = "only allowed file types:.png,.jpg,.jpeg";
                    _conf.Valid = false;
                    break;
                }
                else
                {
                    _conf.Valid = true;
                }
            }
            return _conf;
        }
    }
}
