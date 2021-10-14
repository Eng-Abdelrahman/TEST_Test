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
    

    public class ShopAvailableService : IShopAvailableService
    { 
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        private readonly HttpServerUtilityBase _server;
        public ShopAvailableService(IUnitOfWork uow, IConfirmation conf, HttpContextBase context, HttpSessionStateBase session, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _session = session;
            _context = context;
            _server = server;
        }
        public ShopAvailableService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }
        public async Task<List<ShopAvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo , int Available_Id)
        {
            List<ShopAvailableDto> AvailableDtos = new List<ShopAvailableDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (from == new DateTime(2017, 1, 18))
            {
                try
                {
                    from = (DateTime)_uow.ShopAvailableCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    from = DateTime.UtcNow.AddMinutes(120);
                }
            }
            if (SpaceTo == 0)
            {
                try
                {
                    decimal Space = (decimal)_uow.ShopAvailableCustRepo.GetMaxDecimalValue(m => m.Space);
                    SpaceTo = decimal.ToInt32(Space);
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
                    decimal price = (decimal)_uow.ShopAvailableCustRepo.GetMaxDecimalValue(m => m.Price);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo = 0;
                }

            }
            IEnumerable<tbl_ShopAvailable> availalbles;
            // if user selcet all
            if (regionIdFrom > 0 && SpaceFrom > 0 && PriceFrom > 0)
            {
                availalbles = await _uow.ShopAvailableRepo.FindAsync(a => a.FK_ShopAvailable_Regions_Id >= regionIdFrom && a.FK_ShopAvailable_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && (a.Price >= PriceFrom && a.Price <= PriceTo) &&
                   a.Space >= SpaceFrom && a.Space <= SpaceTo && (a.CreatedAt >= from && a.CreatedAt <= to));
            }
            else
            {
                availalbles = await _uow.ShopAvailableRepo.FindAsync(a => !a.IsDeleted && !a.IsClosed &&
                  (a.CreatedAt >= from && a.CreatedAt <= to));
            }
            if (Available_Id != 0)
            {
                availalbles = availalbles.Where(a => a.PK_ShopAvailable_Id == Available_Id && !a.IsDeleted && !a.IsClosed);
            }
            AvailableDtos = Mapper.Map<IEnumerable<tbl_ShopAvailable>, IEnumerable<ShopAvailableDto>>(availalbles).ToList();
            return AvailableDtos;
        }


        public async Task<MultiSelectList> GetAccess()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name");

        }

        public async Task<MultiSelectList> GetAccessById(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Accessories_Id", "Name", ids);

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
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Transactions_Id", "TransType", await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == id));
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

        public async Task<SelectList> GetTransactions()
        {
            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(t => t.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<IConfirmation> SaveShopAvailable(ShopAvailableDto shopAvailableDto, int userId, List<string> images, int branchId)
        {
            tbl_ShopAvailable newShopAvailable = Mapper.Map<ShopAvailableDto, tbl_ShopAvailable>(shopAvailableDto);
            int i = 0;
            newShopAvailable.FK_ShopAvailable_Users_CreatedBy = userId;
            newShopAvailable.FK_ShopAvailable_Users_ModidfiedBy = userId;
            newShopAvailable.CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            newShopAvailable.FK_ShopAvailable_Branches_BranchId = branchId;
            newShopAvailable.FK_ShopAvailable_PaymentMethod_Id = shopAvailableDto.FK_ShopAvailable_PaymentMethod_Id;
            newShopAvailable.FK_ShopAvialable_Views_Id = shopAvailableDto.FK_ShopAvialable_Views_Id;
            _uow.ShopAvailableRepo.Add(newShopAvailable);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ الطلب بنجاح!";
                return _conf;
            }
            if (images != null && images.Any())
            {
                foreach (string path in images)
                {
                    tbl_ShopAvailableImages ShopAvailableImages = new tbl_ShopAvailableImages
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_ShopAvailableImages_Users_CreatedBy = userId,
                        FK_ShopAvailableImages_Users_ModidfiedBy = userId,
                        FK_ShopAvailableImages_ShopAvailable_Id = newShopAvailable.PK_ShopAvailable_Id,
                        ImageUrl = path,
                    };
                    if (i == 0)
                    {
                        ShopAvailableImages.IsMainImage = true;
                    }
                    _uow.ShopAvailableImagesRepo.Add(ShopAvailableImages);
                    i++;
                }

                _conf.Valid = await _uow.SaveAsync() == (images.Count);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الصور بنجاح!";
                    return _conf;
                }
            }

            if (shopAvailableDto.AccessoriesArr != null && shopAvailableDto.AccessoriesArr.Any())
            {
                foreach (string access in shopAvailableDto.AccessoriesArr)
                {
                    tbl_ShopAvailabeAccessories unitAccess = new tbl_ShopAvailabeAccessories
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_ShopAccessories_Users_CreatedBy = userId,
                        FK_ShopAccessories_Users_ModidfiedBy = userId,
                        FK_ShopAccessories_ShopAvailable_Id = newShopAvailable.PK_ShopAvailable_Id,
                        FK_ShopAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.ShopAvailableAccessoriesRepo.Add(unitAccess);
                }
                _conf.Valid = await _uow.SaveAsync() == (shopAvailableDto.AccessoriesArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                    return _conf;
                }
            }


            _conf.Message = "تم الحفظ بنجاح!";
            shopAvailableDto.PK_ShopAvailable_Id = newShopAvailable.PK_ShopAvailable_Id;
            _conf.ShopDemandsAndExcluded = await FilterShopAvailables(shopAvailableDto, userId);
            _conf.ShopAvailables.Clear();
            _conf.ShopAvailables.Add(Mapper.Map<tbl_ShopAvailable, ShopAvailableDto>(newShopAvailable));
            return _conf;
        }

        public async Task<IConfirmation> CheckDuplicateClientShopAvailable(ShopAvailableDto ShopAvailableDto)
        {
            bool exists = (await _uow.ShopAvailableRepo.FindAsync(shop => shop.DateOfBuild == ShopAvailableDto.DateOfBuild
               && shop.FK_ShopAvailable_PaymentMethod_Id == ShopAvailableDto.FK_ShopAvailable_PaymentMethod_Id
               && shop.Price == ShopAvailableDto.Price
               && shop.Space == ShopAvailableDto.Space
               && shop.Address == ShopAvailableDto.Address
               && shop.Islicense == ShopAvailableDto.Islicense
               && shop.FK_ShopAvailable_Transactions_Id == ShopAvailableDto.FK_ShopAvailable_Transactions_Id
               && shop.FK_ShopAvailable_Clients_ClientId == ShopAvailableDto.FK_ShopAvailable_Clients_ClientId)).Any();
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

        public async Task<(List<ShopDemandDto>, List<ShopDemandDto>)> GetShopAvailableMatches(ShopAvailableDto ShopAvailableDto, int userId)
        {
            var DALAvailable = Mapper.Map<ShopAvailableDto, tbl_ShopAvailable>(ShopAvailableDto);
            int RegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == ShopAvailableDto.FK_ShopAvailable_Regions_Id)).FirstOrDefault().RegCode;
            List<tbl_ShopDemands> shopDemands = new List<tbl_ShopDemands>();
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            if (payments.Any() && payments != null)
            {
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == ShopAvailableDto.FK_ShopAvailable_PaymentMethod_Id);
                if (payment != null && !payment.IsMaster)
                {
                    if (ShopAvailableDto.FK_ShopAvailable_Usage_Id == UnitUsages.Multiple)
                    {
                        shopDemands = await _uow.ShopCustomRepository.DemandsByPayment(DALAvailable);
                    }
                    else
                    {
                        shopDemands = await _uow.ShopCustomRepository.DemnadsByPaymentUsage(DALAvailable);
                    }
                }
                else
                {
                    if (ShopAvailableDto.FK_ShopAvailable_Usage_Id == UnitUsages.Multiple)
                    {
                        shopDemands = await _uow.ShopCustomRepository.DemandsByMasters(DALAvailable);
                    }
                    else
                    {
                        shopDemands = await _uow.ShopCustomRepository.DemandsByUsage(DALAvailable);
                    }
                }
            }
            var collegueDemands = shopDemands.Any() ? shopDemands.Where(d => d.FK_ShopDemands_Users_CreatedBy != userId).ToList() : new List<tbl_ShopDemands>();

            var empDemands = shopDemands.Any() ? shopDemands.Where(d => d.FK_ShopDemands_Users_CreatedBy == userId).ToList() : new List<tbl_ShopDemands>();

            return (Mapper.Map<List<tbl_ShopDemands>, List<ShopDemandDto>>(empDemands), Mapper.Map<List<tbl_ShopDemands>, List<ShopDemandDto>>(collegueDemands));
        }


        public async Task<(List<ShopDemandDto>, ShopDemandDto, List<ShopDemandDto>, List<ShopDemandDto>)> FilterShopAvailables(ShopAvailableDto shopAvailableDto, int userId)
        {
            (List<ShopDemandDto>, List<ShopDemandDto>) shopDemandDtos = await GetShopAvailableMatches(shopAvailableDto, userId);
            shopDemandDtos.Item1 = await GetShopDemandsBuyerName(shopDemandDtos.Item1);
            List<ShopDemandDto> shopDemandsWithPreviews = new List<ShopDemandDto>();
            ShopDemandDto shopDemandsWithSameClient = shopDemandDtos.Item1.Find(sc => sc.FK_ShopDemands_Clients_ClientId == shopAvailableDto.FK_ShopAvailable_Clients_ClientId);
            if (shopDemandsWithSameClient != null)
            {
                shopDemandDtos.Item1.Remove(shopDemandsWithSameClient);
            }
            shopDemandDtos.Item2.RemoveAll(d => d.FK_ShopDemands_Clients_ClientId == shopAvailableDto.FK_ShopAvailable_Clients_ClientId);
            if (shopDemandDtos.Item1.Any() || shopDemandDtos.Item2.Any())
            {
                var shopAvailablePreviews = await _uow.ShopCustomRepository.FilterDemandsHasPreviews(shopDemandDtos.Item1, shopDemandDtos.Item2, shopAvailableDto);
                if (shopAvailablePreviews.Any() && shopAvailableDto != null)
                {
                    foreach (ShopDemandDto item in shopDemandDtos.Item1.ToList())
                    {
                        if (shopAvailablePreviews.Exists(sd => sd.DemandId == item.PK_ShopDemands_Id))
                        {
                            shopDemandsWithPreviews.Add(item);
                            shopDemandDtos.Item1.Remove(item);
                        }
                    }
                    foreach (ShopDemandDto item in shopDemandDtos.Item2.ToList())
                    {
                        if (shopAvailablePreviews.Exists(sd => sd.DemandId == item.PK_ShopDemands_Id))
                        {
                            shopDemandDtos.Item2.Remove(item);
                        }
                    }
                }
            }

            return (shopDemandDtos.Item1, shopDemandsWithSameClient, shopDemandsWithPreviews, shopDemandDtos.Item2);
        }

        public async Task<List<ShopDemandDto>> GetShopDemandsBuyerName(List<ShopDemandDto> shopDemandDtos)
        {
            foreach (var item in shopDemandDtos)
            {
                item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_ShopDemands_Clients_ClientId)).FirstOrDefault().Name;
            }
            return shopDemandDtos;
        }

        public async Task<ShopAvailableDto> EditAvailableShop(int id)
        {
            List<ShopAvailableDto> shopAvailableDtos = Mapper.Map<List<tbl_ShopAvailable>, List<ShopAvailableDto>>((await _uow.ShopAvailableRepo.FindAsync(al => al.PK_ShopAvailable_Id == id && !al.IsDeleted)).ToList());
            if (shopAvailableDtos.Any() && shopAvailableDtos != null)
            {
                ShopAvailableDto shopAvailableDto = shopAvailableDtos.FirstOrDefault();
                shopAvailableDto.Images = (await _uow.ShopAvailableImagesRepo.FindAsync(image => image.FK_ShopAvailableImages_ShopAvailable_Id == shopAvailableDto.PK_ShopAvailable_Id)).Select(i => i.ImageUrl).ToArray();
                shopAvailableDto.AccessoriesIds = (await _uow.ShopAvailableAccessoriesRepo.FindAsync(acces => acces.FK_ShopAccessories_Accessories_Id == shopAvailableDto.PK_ShopAvailable_Id)).Select(acc => acc.FK_ShopAccessories_Accessories_Id).ToArray();
                return shopAvailableDto;
            }
            return new ShopAvailableDto();
        }

        public async Task<IConfirmation> DeleteShopAvailable(int id, int userId)
        {
            if (id > 0)
            {
                tbl_ShopAvailable tbl_ShopAvailable = (await _uow.ShopAvailableRepo.FindAsync(al => al.PK_ShopAvailable_Id == id && !al.IsDeleted)).FirstOrDefault();
                if (tbl_ShopAvailable != null)
                {
                    tbl_ShopAvailable.IsDeleted = true;
                    tbl_ShopAvailable.FK_ShopAvailable_Users_ModidfiedBy = userId;
                }
                _uow.ShopAvailableRepo.Update(tbl_ShopAvailable);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف العرض بنجاح";
                    return _conf;
                }
                List<tbl_ShopAvailableImages> tbl_ShopAvailableImages = (await _uow.ShopAvailableImagesRepo.FindAsync(li => li.FK_ShopAvailableImages_ShopAvailable_Id == tbl_ShopAvailable.PK_ShopAvailable_Id && !li.IsDeleted)).ToList();
                if (tbl_ShopAvailableImages != null)
                {
                    foreach (var path in tbl_ShopAvailableImages)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/AvailableShopImages"), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    foreach (var item in tbl_ShopAvailableImages)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_ShopAvailableImages_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.ShopAvailableImagesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    {
                        _conf.Message = "لم يتم حذف الصور بنجاح!";
                        return _conf;
                    }
                }
                List<tbl_ShopAvailabeAccessories> tbl_ShopAvailabeAccessories = (await _uow.ShopAvailableAccessoriesRepo.FindAsync(acc => acc.FK_ShopAccessories_ShopAvailable_Id == tbl_ShopAvailable.PK_ShopAvailable_Id && !acc.IsDeleted)).ToList();
                if (tbl_ShopAvailabeAccessories != null)
                {
                    foreach (var item in tbl_ShopAvailabeAccessories)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_ShopAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.ShopAvailableAccessoriesRepo.Update(item);
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
            _conf.Message = "لم يتم الحذف حدث خطأ ما!";
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
                        string fname = Path.Combine("Assets/Img/AvailableShopImages/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
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
                if (!ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase) && !ext.Equals("png", StringComparison.OrdinalIgnoreCase) && !ext.Equals("jpg", StringComparison.OrdinalIgnoreCase))
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
        // contract functions

        public async Task<bool> CloseShopAvailable(int id, int userId)
        {
            tbl_ShopAvailable available = (await _uow.ShopAvailableRepo.FindAsync(u => u.PK_ShopAvailable_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = true;
            available.FK_ShopAvailable_Users_ModidfiedBy = userId;
            _uow.ShopAvailableRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateShopAvailableTransaction(int availableId, int transId)
        {
            tbl_ShopAvailable available = (await _uow.ShopAvailableRepo.FindAsync(u => u.PK_ShopAvailable_Id == availableId && !u.IsDeleted)).FirstOrDefault();
            if (available != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transId && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                available.FK_ShopAvailable_Transactions_Id = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> ReleaseShopAvailable(int id, int userId)
        {
            tbl_ShopAvailable available = (await _uow.ShopAvailableRepo.FindAsync(u => u.PK_ShopAvailable_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = false;
            available.FK_ShopAvailable_Users_ModidfiedBy = userId;
            _uow.ShopAvailableRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<List<ShopAvailableDto>> GetClosedShopAvailable(int id)
        {
            List<ShopAvailableDto> shopAvailable = Mapper.Map<List<tbl_ShopAvailable>, List<ShopAvailableDto>>((await _uow.ShopAvailableRepo.
                FindAsync(a => a.FK_ShopAvailable_Clients_ClientId == id && !a.IsDeleted))
                .ToList());

            if (shopAvailable.Any() && shopAvailable != null)
            {
                foreach (ShopAvailableDto item in shopAvailable)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_ShopAvailable_Transactions_Id))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.CatRepo.FindAsync(t => t.PK_Categories_Id == item.FK_ShopAvailable_Categories_Id))
                        .FirstOrDefault().CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return shopAvailable;
            }
            return new List<ShopAvailableDto>();
        }

        public async Task<IConfirmation> UpdateAvailableLand(ShopAvailableDto sale, int userId, List<string> images, int branchId)
        {
            tbl_ShopAvailable tbl_ShopAvailable = (await _uow.ShopAvailableRepo.FindAsync(al => al.PK_ShopAvailable_Id == sale.PK_ShopAvailable_Id && !al.IsDeleted)).FirstOrDefault();

            if (sale.FK_ShopAvailable_Clients_ClientId > 0)
            {
                tbl_ShopAvailable.Price = sale.Price;
                tbl_ShopAvailable.Space = sale.Space;
                tbl_ShopAvailable.Notes = sale.Notes;
                tbl_ShopAvailable.FK_ShopAvailable_Users_ModidfiedBy = userId;
                tbl_ShopAvailable.FK_ShopAvailable_PaymentMethod_Id = sale.FK_ShopAvailable_PaymentMethod_Id;
                tbl_ShopAvailable.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            }
            _uow.ShopAvailableRepo.Update(tbl_ShopAvailable);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح";
                return _conf;
            }
            if (images != null && images.Any())
            {
                List<tbl_ShopAvailableImages> unitImages = (await _uow.ShopAvailableImagesRepo.FindAsync(ui => ui.FK_ShopAvailableImages_ShopAvailable_Id == sale.PK_ShopAvailable_Id && !ui.IsDeleted)).ToList();
                foreach (tbl_ShopAvailableImages item in unitImages)
                {
                    _uow.ShopAvailableImagesRepo.Remove(item);
                }

                int i = 0;
                foreach (string path in images)
                {
                    tbl_ShopAvailableImages unitImage = new tbl_ShopAvailableImages
                    {
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_ShopAvailableImages_Users_ModidfiedBy = userId,
                        ImageUrl = path,
                    };
                    if (i == 0)
                    {
                        unitImage.IsMainImage = true;
                    }
                    _uow.ShopAvailableImagesRepo.Add(unitImage);
                    i++;
                }

                _conf.Valid = await _uow.SaveAsync() > 0;
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الصور بنجاح!";
                    return _conf;
                }
                foreach (tbl_ShopAvailableImages path in unitImages)
                {
                    string photo = Directory
                                         .GetFiles(_server.MapPath("/Assets/Img/AvailableShopImages" +
                                         ""), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                         .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }
            }
            if (sale.AccessoriesArr != null && sale.AccessoriesArr.Any())
            {
                foreach (string access in sale.AccessoriesArr)
                {
                    tbl_ShopAvailabeAccessories unitAccess = new tbl_ShopAvailabeAccessories
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_ShopAccessories_Users_CreatedBy = userId,
                        FK_ShopAccessories_Users_ModidfiedBy = userId,
                        FK_ShopAccessories_ShopAvailable_Id = sale.PK_ShopAvailable_Id,
                        FK_ShopAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.ShopAvailableAccessoriesRepo.Add(unitAccess);
                }
                _conf.Valid = await _uow.SaveAsync() == (sale.AccessoriesArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                    return _conf;
                }
            }
            _conf.Message = "تم الحفظ بنجاح!";
            _conf.ShopDemandsAndExcluded = await FilterShopAvailables(sale, userId);
            _conf.ShopAvailables.Clear();
            _conf.ShopAvailables.Add(Mapper.Map<tbl_ShopAvailable, ShopAvailableDto>(tbl_ShopAvailable));
            return _conf;
        }


        public async Task<IConfirmation> GetInstantMatches(int saleId, int userId)
        {
            tbl_ShopAvailable DBsale = (await _uow.ShopAvailableRepo.FindAsync(d => d.PK_ShopAvailable_Id == saleId)).FirstOrDefault();
            ShopAvailableDto sale = Mapper.Map<tbl_ShopAvailable, ShopAvailableDto>(DBsale);

            if (sale != null)
            {
                _conf.ShopDemandsAndExcluded = await FilterShopAvailables(sale, userId);
                _conf.ShopAvailables.Clear();
                _conf.ShopAvailables.Add(sale);
                return _conf;
            }
            return _conf;
        }

        public async Task<List<ShopAvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId)
        {
            IEnumerable<tbl_ShopAvailable> availalbles = (await _uow.ShopAvailableRepo.FindAsync(a => a.FK_ShopAvailable_Regions_Id == regionId && a.FK_ShopAvailable_Branches_BranchId == branchId && !a.IsDeleted && !a.IsClosed && (a.CreatedAt.Day == date.Day && a.CreatedAt.Month == date.Month && a.CreatedAt.Year == date.Year)));
            List<ShopAvailableDto> AvailableDtos = new List<ShopAvailableDto>();
            foreach (tbl_ShopAvailable available in availalbles)
            {
                ShopAvailableDto availableDto = new ShopAvailableDto
                {
                    BathRooms = available.BathRooms,
                    Space = available.Space,
                    Price = available.Price
                };
                availableDto.FK_ShopAvailable_Regions_Id = available.FK_ShopAvailable_Regions_Id;
                AvailableDtos.Add(availableDto);
            }

            return AvailableDtos;
        }

    }
}
