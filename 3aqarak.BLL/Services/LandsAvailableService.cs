using _3aqarak.BLL.Dto;
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

    public class LandsAvailableService : IAvailableLandsSevice
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;

        public LandsAvailableService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _context = context;
        }
        public LandsAvailableService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }
        //****************** Mostafa add fiter here **********************
        public async Task<List<AvailableLandsDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int regionIdFrom, int regionIdTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,int Available_Id)
        {
            List<AvailableLandsDto> AvailableDtos = new List<AvailableLandsDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (from == new DateTime(2017, 1, 18))
            {
                try
                {
                    from = (DateTime)_uow.AvailableLandsCustRepo.GetMinlValue(m => m.CreatedAt);
                }
                catch (Exception)
                {

                    from=DateTime.Now;
                }
            }
            if (SpaceTo == 0)
            {
                try
                {
                    decimal Space = (decimal)_uow.AvailableLandsCustRepo.GetMaxDecimalValue(m => m.Space);
                    SpaceTo = decimal.ToInt32(Space);
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
                    decimal price = (decimal)_uow.VillasAvailablesCustRepo.GetMaxDecimalValue(m => m.Price);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo=1;
                }
            }
            IEnumerable<tbl_AvailableLands> availalbles;
            // if user selcet all
            if (regionIdFrom > 0 && SpaceFrom > 0 && PriceFrom > 0)
            {
                availalbles = (await _uow.AvailableLandsRepo.FindAsync(a => a.FK_AvailabeLands_Regions_RegionId >= regionIdFrom && a.FK_AvailabeLands_Regions_RegionId <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && (a.Price >= PriceFrom && a.Price <= PriceTo) &&
                   a.Space >= SpaceFrom && a.Space <= SpaceTo && (a.CreatedAt >= from && a.CreatedAt <= to)));
            }
            else
            {
                availalbles = (await _uow.AvailableLandsRepo.FindAsync(a => !a.IsDeleted && !a.IsClosed &&
                  (a.CreatedAt >= from && a.CreatedAt <= to)));
            }
            if (Available_Id != 0)
            {
                availalbles = availalbles.Where(a => a.PK_AvailableLands_Id == Available_Id && !a.IsDeleted && !a.IsClosed);
            }
            AvailableDtos = Mapper.Map<IEnumerable<tbl_AvailableLands>, IEnumerable<AvailableLandsDto>>(availalbles).ToList();
            return AvailableDtos;
        }


        public async Task<IConfirmation> CheckDuplicateLandSales(AvailableLandsDto availableLandsDto)
        {
            bool exists = (await _uow.AvailableLandsRepo.
              FindAsync(a => a.Price == availableLandsDto.Price
              && a.Space == availableLandsDto.Space
              && a.Description == availableLandsDto.Description
              && a.Type == a.Type
              && a.FK_AvaliableLands_Branches_BranchId == availableLandsDto.FK_AvaliableLands_Branches_BranchId
              && a.FK_AvailableLands_Categories_CategoryId == availableLandsDto.FK_AvailableLands_Categories_CategoryId
              && a.FK_AvailableLands_Views_ViewId == availableLandsDto.FK_AvailableLands_Views_ViewId
              && a.FK_AvailabeLands_Regions_RegionId == availableLandsDto.FK_AvailabeLands_Regions_RegionId
              && a.FK_AvaliableLands_Transactions_TransactionId == availableLandsDto.FK_AvaliableLands_Transactions_TransactionId
              && a.FK_AvailableLands_PaymentMethod_Id == availableLandsDto.FK_AvailableLands_PaymentMethod_Id
              && a.FK_AvaliableLands_Clients_ClientId == availableLandsDto.FK_AvaliableLands_Clients_ClientId)).Any();
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

        public async Task<(List<LandsDemandsDto>, LandsDemandsDto, List<LandsDemandsDto>, List<LandsDemandsDto>)> FilterLandAvailable(AvailableLandsDto availableLandsDto, int userId)
        {
            (List<LandsDemandsDto>, List<LandsDemandsDto>) landsDemands = await GetAvailableLandMatches(availableLandsDto, userId);
            landsDemands.Item1 = await GetBuyerName(landsDemands.Item1);
            List<LandsDemandsDto> landsDemandsWithPreviews = new List<LandsDemandsDto>();
            LandsDemandsDto landsDemandsWithSameClient = landsDemands.Item1.Find(ld => ld.FK_LandsDemands_Clients_ClientId == availableLandsDto.FK_AvaliableLands_Clients_ClientId);
            if (landsDemandsWithSameClient != null)
            {
                landsDemands.Item1.Remove(landsDemandsWithSameClient);
            }
            landsDemands.Item2.RemoveAll(d => d.FK_LandsDemands_Clients_ClientId == availableLandsDto.FK_AvaliableLands_Clients_ClientId);

            if (landsDemands.Item1.Any() || landsDemands.Item2.Any())
            {
                var availableLandPreview = await _uow.LandCustomRepository.FilterDemandsHasPreviews(landsDemands.Item1, landsDemands.Item2, availableLandsDto);
                if (availableLandPreview.Any() && availableLandPreview != null)
                {
                    foreach (var item in landsDemands.Item1.ToList())
                    {
                        if (availableLandPreview.Exists(al => al.DemandId == item.PK_LandsDemands_Id))
                        {
                            landsDemands.Item1.Remove(item);
                            landsDemandsWithPreviews.Add(item);
                        }
                    }
                }
                if (availableLandPreview.Any() && availableLandPreview != null)
                {
                    foreach (var item in landsDemands.Item2.ToList())
                    {
                        if (availableLandPreview.Exists(al => al.DemandId == item.PK_LandsDemands_Id))
                        {
                            landsDemands.Item2.Remove(item);
                        }
                    }
                }
            }



            return (landsDemands.Item1, landsDemandsWithSameClient, landsDemandsWithPreviews, landsDemands.Item2);
        }

        public async Task<(List<LandsDemandsDto>, List<LandsDemandsDto>)> GetAvailableLandMatches(AvailableLandsDto availableLandsDto, int userId)
        {
            var DALAvailable = Mapper.Map<AvailableLandsDto, tbl_AvailableLands>(availableLandsDto);
            int RegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == availableLandsDto.FK_AvailabeLands_Regions_RegionId)).FirstOrDefault().RegCode;
            List<tbl_LandsDemands> LandsDemands = new List<tbl_LandsDemands>();
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            if (payments.Any() && payments != null)
            {
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == availableLandsDto.FK_AvailableLands_PaymentMethod_Id);
                if (payment != null && payment.IsMaster)
                {
                    LandsDemands = await _uow.LandCustomRepository.AvailablesForMasters(DALAvailable);
                }
                else
                {
                    LandsDemands = await _uow.LandCustomRepository.AvailablesForPayment(DALAvailable);
                }
            }

            var collegueDemands = LandsDemands.Any() ? LandsDemands.Where(d => d.FK_LandsDemands_Users_CreatedBy != userId).ToList() : new List<tbl_LandsDemands>();

            var empDemands = LandsDemands.Any() ? LandsDemands.Where(d => d.FK_LandsDemands_Users_CreatedBy == userId).ToList() : new List<tbl_LandsDemands>();

            return (Mapper.Map<List<tbl_LandsDemands>, List<LandsDemandsDto>>(empDemands), Mapper.Map<List<tbl_LandsDemands>, List<LandsDemandsDto>>(collegueDemands));
        }


        public async Task<List<LandsDemandsDto>> GetBuyerName(List<LandsDemandsDto> landsDemandsDtos)
        {
            foreach (var item in landsDemandsDtos)
            {
                item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_LandsDemands_Clients_ClientId)).FirstOrDefault().Name;
            }
            return landsDemandsDtos;
        }

        public async Task<SelectList> GetPayments()
        {
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>((await _uow.PaymentRepo.FindAsync(c => c.IsDeleted == false)).ToList()), "PK_PaymentMethods_Id", "PaymentMethod");
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



        public async Task<IConfirmation> SaveLandsSale(AvailableLandsDto availableLandsDto, int userId, List<string> images, int branchId)
        {
            tbl_AvailableLands newAvailableLands = Mapper.Map<AvailableLandsDto, tbl_AvailableLands>(availableLandsDto);
            newAvailableLands.CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            newAvailableLands.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            newAvailableLands.FK_AvaliableLands_Users_CreatedBy = userId;
            newAvailableLands.FK_AvaliableLands_Users_ModifiedBy = userId;
            newAvailableLands.FK_AvaliableLands_Branches_BranchId = branchId;
            _uow.AvailableLandsRepo.Add(newAvailableLands);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            int i = 0;
            if (images != null && images.Any())
            {
                foreach (string path in images)
                {
                    tbl_LandImages LandImages = new tbl_LandImages
                    {
                        FK_LandImages_AvailableLands_AvailableLandId = newAvailableLands.PK_AvailableLands_Id,
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_LandImages_Users_CreatedBy = userId,
                        FK_LandImages_Users_ModidfiedBy = userId,
                        ImageUrl = path
                    };
                    if (i == 0)
                    {
                        LandImages.IsMainImage = true;
                    }
                    _uow.LandImagesRepo.Add(LandImages);
                    i++;
                }
                _conf.Valid = await _uow.SaveAsync() == (images.Count);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الصور بنجاح!";
                    return _conf;
                }
            }

            _conf.Message = "تم الحفظ بنجاح!";
            availableLandsDto.PK_AvailableLands_Id = newAvailableLands.PK_AvailableLands_Id;
            _conf.LandsDemandsAndExcluded = await FilterLandAvailable(availableLandsDto, userId);
            _conf.LandAvailables.Clear();
            _conf.LandAvailables.Add(Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(newAvailableLands));
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
                        string fname = Path.Combine("Assets/Img/AvailableLandsImages/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
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

        public async Task<AvailableLandsDto> EditAvailableLand(int id)
        {
            List<AvailableLandsDto> availableLandsDtos = Mapper.Map<List<tbl_AvailableLands>, List<AvailableLandsDto>>((await _uow.AvailableLandsRepo.FindAsync(al => al.PK_AvailableLands_Id == id && !al.IsDeleted)).ToList());
            if (availableLandsDtos.Any() && availableLandsDtos != null)
            {
                AvailableLandsDto availableLandsDto = availableLandsDtos.FirstOrDefault();
                availableLandsDto.Images = ((await _uow.LandImagesRepo.FindAsync(image => image.FK_LandImages_AvailableLands_AvailableLandId == availableLandsDto.PK_AvailableLands_Id)).Select(i => i.ImageUrl)).ToArray();
                return availableLandsDto;
            }
            return new AvailableLandsDto();
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

            return new SelectList(Mapper.Map<List<tbl_Transactions>, List<TransDto>>((await _uow.TransRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Transactions_Id", "TransType");
        }

        public async Task<SelectList> GetPaymentsId(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(v => v.IsDeleted == false)).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);
        }

        public async Task<SelectList> GetViewsById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.FindAsync(v => v.IsDeleted == false)).ToList()), "PK_Views_Id", "Name", id);
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

        public async Task<IConfirmation> DeleteLandAvailable(int id, int userId)
        {
            if (id > 0)
            {
                tbl_AvailableLands tbl_AvailableLand = (await _uow.AvailableLandsRepo.FindAsync(al => al.PK_AvailableLands_Id == id && !al.IsDeleted)).FirstOrDefault();
                if (tbl_AvailableLand != null)
                {
                    tbl_AvailableLand.IsDeleted = true;
                    tbl_AvailableLand.FK_AvaliableLands_Users_ModifiedBy = userId;
                }
                _uow.AvailableLandsRepo.Update(tbl_AvailableLand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف العرض بنجاح";
                    return _conf;
                }
                List<tbl_LandImages> tbl_LandImage = (await _uow.LandImagesRepo.FindAsync(li => li.FK_LandImages_AvailableLands_AvailableLandId == tbl_AvailableLand.PK_AvailableLands_Id && !li.IsDeleted)).ToList();
                if (tbl_LandImage != null)
                {
                    foreach (var path in tbl_LandImage)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/AvailableLandsImages"), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    foreach (var item in tbl_LandImage)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_LandImages_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.LandImagesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {

                        _conf.Message = "لم يتم حذف الصور بنجاح!";
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

        public async Task<IConfirmation> UpdateAvailableLand(AvailableLandsDto sale, int userId, List<string> images, int branchId)
        {
            if (sale.FK_AvaliableLands_Clients_ClientId > 0)
            {
                tbl_AvailableLands tbl_AvailableLand = (await _uow.AvailableLandsRepo.FindAsync(al => al.PK_AvailableLands_Id == sale.PK_AvailableLands_Id && !al.IsDeleted)).FirstOrDefault();
                if (tbl_AvailableLand != null)
                {
                    tbl_AvailableLand.FK_AvaliableLands_Transactions_TransactionId = sale.FK_AvaliableLands_Transactions_TransactionId;
                    tbl_AvailableLand.FK_AvailableLands_PaymentMethod_Id = sale.FK_AvailableLands_PaymentMethod_Id;
                    tbl_AvailableLand.Notes = sale.Notes;
                    tbl_AvailableLand.Space = sale.Space;
                    tbl_AvailableLand.Price = sale.Price;
                    tbl_AvailableLand.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                    tbl_AvailableLand.FK_AvaliableLands_Users_ModifiedBy = userId;
                    tbl_AvailableLand.Type = sale.Type;
                }
                _uow.AvailableLandsRepo.Update(tbl_AvailableLand);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ العرض بنجاح";
                    return _conf;
                }
                if (images != null && images.Any())
                {
                    List<tbl_LandImages> unitImages = (await _uow.LandImagesRepo.FindAsync(ui => ui.FK_LandImages_AvailableLands_AvailableLandId == sale.PK_AvailableLands_Id && !ui.IsDeleted)).ToList();
                    foreach (tbl_LandImages item in unitImages)
                    {
                        _uow.LandImagesRepo.Remove(item);
                    }

                    int i = 0;
                    foreach (string path in images)
                    {
                        tbl_LandImages unitImage = new tbl_LandImages
                        {
                            ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                            FK_LandImages_Users_ModidfiedBy = userId,
                            ImageUrl = path,
                        };
                        if (i == 0)
                        {
                            unitImage.IsMainImage = true;
                        }
                        _uow.LandImagesRepo.Add(unitImage);
                        i++;
                    }

                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ الصور بنجاح!";
                        return _conf;
                    }
                    foreach (tbl_LandImages path in unitImages)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/AvailableLandsImages" +
                                             ""), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                }

                _conf.Message = "تم الحفظ بنجاح!";
                _conf.LandsDemandsAndExcluded = await FilterLandAvailable(sale, userId);
                _conf.LandAvailables.Clear();
                _conf.LandAvailables.Add(Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(tbl_AvailableLand));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;
        }

        // contract functions

        public async Task<bool> CloseLandAvailable(int id, int userId)
        {
            tbl_AvailableLands available = (await _uow.AvailableLandsRepo.FindAsync(u => u.PK_AvailableLands_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = true;
            available.FK_AvaliableLands_Users_ModifiedBy = userId;
            _uow.AvailableLandsRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> UpdateLandAvailableTransaction(int availableId, int transCode)
        {
            tbl_AvailableLands available = (await _uow.AvailableLandsRepo.FindAsync(u => u.PK_AvailableLands_Id == availableId && !u.IsDeleted)).FirstOrDefault();
            if (available != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                available.FK_AvaliableLands_Transactions_TransactionId = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }
        public async Task<bool> ReleaseLandAvailable(int id, int userId)
        {
            tbl_AvailableLands available = (await _uow.AvailableLandsRepo.FindAsync(u => u.PK_AvailableLands_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = false;
            available.FK_AvaliableLands_Users_ModifiedBy = userId;
            _uow.AvailableLandsRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }
        public async Task<List<AvailableLandsDto>> GetClosedLandAvailable(int id)
        {
            List<AvailableLandsDto> clientSales = Mapper.Map<List<tbl_AvailableLands>, List<AvailableLandsDto>>((await _uow.AvailableLandsRepo
                .FindAsync(a => a.FK_AvaliableLands_Clients_ClientId == id && !a.IsDeleted))
                .ToList());

            if (clientSales.Any() && clientSales != null)
            {
                foreach (AvailableLandsDto item in clientSales)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_AvaliableLands_Transactions_TransactionId))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.UnitRepo.FindAsync(t => t.PK_Units_Id == item.PK_AvailableLands_Id))
                        .FirstOrDefault().tbl_Categories.CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientSales;
            }
            return new List<AvailableLandsDto>();
        }


        public async Task<IConfirmation> GetInstantMatches(int saleId, int userId)
        {
            tbl_AvailableLands DBsale = (await _uow.AvailableLandsRepo.FindAsync(d => d.PK_AvailableLands_Id == saleId)).FirstOrDefault();
            AvailableLandsDto sale = Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(DBsale);

            if (sale != null)
            {
                sale = Mapper.Map<tbl_AvailableLands, AvailableLandsDto>(DBsale);
                _conf.LandsDemandsAndExcluded = await FilterLandAvailable(sale, userId);
                foreach (LandsDemandsDto item in _conf.landsDemands)
                {
                    item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_LandsDemands_Clients_ClientId)).FirstOrDefault().Name;
                }
                _conf.LandAvailables.Clear();
                _conf.LandAvailables.Add(sale);
                return _conf;
            }
            return _conf;
        }

        public async Task<List<AvailableLandsDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId)
        {
            IEnumerable<tbl_AvailableLands> availalbles = (await _uow.AvailableLandsRepo.FindAsync(a => a.FK_AvailabeLands_Regions_RegionId == regionId && a.FK_AvaliableLands_Branches_BranchId == branchId && !a.IsDeleted && !a.IsClosed && (a.CreatedAt.Day == date.Day && a.CreatedAt.Month == date.Month && a.CreatedAt.Year == date.Year)));
            List<AvailableLandsDto> AvailableDtos = new List<AvailableLandsDto>();
            foreach (tbl_AvailableLands available in availalbles)
            {
                AvailableLandsDto availableDto = new AvailableLandsDto
                {
                    PK_AvailableLands_Id = available.PK_AvailableLands_Id,
                    Space = available.Space,
                    Price = available.Price
                };
                availableDto.FK_AvailabeLands_Regions_RegionId = available.FK_AvailabeLands_Regions_RegionId;
                AvailableDtos.Add(availableDto);
            }

            return AvailableDtos;
        }



    }
}
