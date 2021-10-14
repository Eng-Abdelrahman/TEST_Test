using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Services
{

    public class AvailableService : IAvailableService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        private readonly HttpContextBase _context;
        private readonly HttpSessionStateBase _session;
        public AvailableService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server, HttpContextBase context, HttpSessionStateBase session)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
            _session = session;
            _context = context;
        }
        public AvailableService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }
        // add function to get all available (Mostafa)
        public async Task<List<AvailableDto>> GetAllAveilableByDateAndRegion(DateTime from, DateTime to, int cat, int regionIdFrom, int regionIdTo, int ElevatorFrom, int ElevatorTo, int SpaceFrom, int SpaceTo, int PriceFrom, int PriceTo,
            int Payments, int BasisOfInstallmentDropdown, int OverFrom, int OverTo, int RemainingFrom, int RemainingTo,
            int YearOfInstallmentFrom, int YearOfInstallmentTo, string FlatNumber, string ApartmentNumber, string GroupNumber,int Available_Id)
        {

            List<AvailableDto> AvailableDtos = new List<AvailableDto>();
            if (regionIdTo == 0)
            {
                regionIdTo = (await _uow.RegionRepo.GetAllAsync()).Max(m => m.PK_Regions_ID);
            }
            if (SpaceTo == 0)
            {
                try
                {
                    SpaceTo = (int)_uow.UnitCustRepo.GetMaxIntValue(u => u.Space);
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
                    decimal price = (decimal)_uow.AvailableCustRepo.GetMaxDecimalValue(m => m.Price);
                    PriceTo = decimal.ToInt32(price);
                }
                catch (Exception)
                {

                    PriceTo = 1;
                }
            }
            if (ElevatorTo == 0)
            {
                try
                {
                    ElevatorTo = (int)_uow.AvailableCustRepo.GetMaxIntValue(m => m.NoOfElevators);
                }
                catch (Exception)
                {

                    ElevatorTo=1;
                }
            }
            if (RemainingTo == 0)
            {
                try
                {
                    decimal Remaining = (decimal)_uow.AvailableCustRepo.GetMaxDecimalValue(m => m.Remaining);
                    RemainingTo = decimal.ToInt32(Remaining);
                }
                catch (Exception)
                {

                    RemainingTo=1;
                }
            }

            if (OverTo == 0)
            {
                try
                {
                    decimal Over = (decimal)_uow.AvailableCustRepo.GetMaxDecimalValue(m => m.Over);
                    OverTo = decimal.ToInt32(Over);
                }
                catch (Exception)
                {

                    OverTo=1;
                }
            }
            if (YearOfInstallmentTo == 0)
            {
                try
                {
                    decimal YearOfInstallment = (decimal)_uow.AvailableCustRepo.GetMaxDecimalValue(m => m.YearOfInstallment);
                    YearOfInstallmentTo = decimal.ToInt32(YearOfInstallment);
                }
                catch (Exception)
                {

                    YearOfInstallmentTo=1;
                }
            }

            IEnumerable<tbl_AvailableUnits> availalbles;
            // if user selcet all
            if (regionIdFrom > 0 && ElevatorFrom > 0 && SpaceFrom > 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.tbl_units.FK_Units_Categories_Id == cat &&
                   (a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo) && (a.Price >= PriceFrom && a.Price <= PriceTo) &&
                   a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            // if user select just category
            else if (regionIdFrom > 0 && ElevatorFrom > 0 && SpaceFrom > 0 && PriceFrom > 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo) &&
                   a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            // if user select cat and price 
            else if (regionIdFrom > 0 && ElevatorFrom > 0 && SpaceFrom > 0 && PriceFrom == 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price <= PriceTo) &&
                   a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //if user select select cat , price and space
            else if (regionIdFrom > 0 && ElevatorFrom > 0 && SpaceFrom == 0 && PriceFrom == 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price <= PriceTo)
                   && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //if user select select cat , price , space and elevator
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && SpaceFrom == 0 && PriceFrom == 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators <= ElevatorTo && (a.Price <= PriceTo)
                   && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //if user select select cat , price , space , elevator and region
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom == 0 && SpaceTo > 0 && PriceFrom == 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators <= ElevatorTo && (a.Price <= PriceTo)
                   && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //if user set value in cat only
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom == 0 && PriceFrom == 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators <= ElevatorTo && (a.Price <= PriceTo)
                   && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //user set value in cat and price only
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom == 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                   && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            //if user set val in cat , space and price
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            // if user set all data except region
            else if (regionIdFrom == 0 && ElevatorFrom > 0 && SpaceFrom > 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            // if user set just space 
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom == 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            else if (regionIdFrom == 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom == 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Regions_Id >= regionIdFrom && a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom > 0 && cat == 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Regions_Id >= regionIdFrom &&
                   !a.IsDeleted && !a.IsClosed && a.NoOfElevators >= ElevatorFrom && a.NoOfElevators <= ElevatorTo && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            else if (regionIdFrom > 0 && ElevatorFrom == 0 && SpaceFrom > 0 && PriceFrom > 0 && cat > 0)
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id <= regionIdTo && a.tbl_units.FK_Units_Regions_Id >= regionIdFrom &&
                a.tbl_units.FK_Units_Categories_Id == cat &&
                   !a.IsDeleted && !a.IsClosed && (a.Price >= PriceFrom && a.Price <= PriceTo)
                  && a.tbl_units.Space >= SpaceFrom && a.tbl_units.Space <= SpaceTo && (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }
            else
            {
                availalbles = (await _uow.AvailableRepo.FindAsync(a => !a.IsDeleted && !a.IsClosed &&
                   (a.Date >= from && a.Date <= to), a => a.tbl_units));
            }

            if(Payments==4)
            {
                if(BasisOfInstallmentDropdown==0)
                {
                    availalbles=availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && !a.IsDeleted && !a.IsClosed);
                }
                else if (BasisOfInstallmentDropdown == 1)
                {
                    availalbles = availalbles.Where(a => a.Remaining >= RemainingFrom && a.Remaining <= RemainingTo && a.Over >= OverFrom && a.Over <= OverTo &&
                a.YearOfInstallment >= YearOfInstallmentFrom && a.YearOfInstallment <= YearOfInstallmentTo && a.BasisOfInstallment==1 && !a.IsDeleted && !a.IsClosed);
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
            if(FlatNumber !=null)
            {
                availalbles = availalbles.Where(a => a.tbl_units.FlatNumber==FlatNumber && !a.IsDeleted && !a.IsClosed);
            }
            if (ApartmentNumber != null)
            {
                availalbles = availalbles.Where(a => a.tbl_units.ApartmentNumber == ApartmentNumber && !a.IsDeleted && !a.IsClosed);
            }
            if (GroupNumber != null)
            {
                availalbles = availalbles.Where(a => a.tbl_units.GroupNumber == GroupNumber && !a.IsDeleted && !a.IsClosed);
            }
            if (Available_Id != 0)
            {
                availalbles = availalbles.Where(a => a.PK_AvailableUnits_Id == Available_Id && !a.IsDeleted && !a.IsClosed);
            }
            AvailableDtos = Mapper.Map<IEnumerable<tbl_AvailableUnits>, IEnumerable<AvailableDto>>(availalbles).ToList();
            return AvailableDtos;
        }

        public async Task<List<AvailableDto>> ClientSales(int id)
        {
            List<AvailableDto> clientSales = Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>((await _uow.AvailableRepo
                .FindAsync(a => a.FK_AvaliableUnits_Clients_ClientId == id && !a.IsDeleted && !a.IsClosed)).ToList());


            if (clientSales.Any() && clientSales != null)
            {
                foreach (AvailableDto item in clientSales)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_AvaliableUnits_Transaction_TransactionId))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.UnitRepo.FindAsync(t => t.PK_Units_Id == item.FK_AvaliableUnits_Units_UnitId))
                        .FirstOrDefault().tbl_Categories.CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientSales;
            }
            return new List<AvailableDto>();
        }

        public async Task<List<AvailableDto>> GetClosedSalesByClient(int id)
        {
            List<AvailableDto> clientSales = Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>((await _uow.AvailableRepo
                .FindAsync(a => a.FK_AvaliableUnits_Clients_ClientId == id && !a.IsDeleted))
                .ToList());

            if (clientSales.Any() && clientSales != null)
            {
                foreach (AvailableDto item in clientSales)
                {
                    item.ShortDescription = ((await _uow.TransRepo.FindAsync(t => t.PK_Transactions_Id == item.FK_AvaliableUnits_Transaction_TransactionId))
                        .FirstOrDefault().TransType)
                        + " " + ((await _uow.UnitRepo.FindAsync(t => t.PK_Units_Id == item.FK_AvaliableUnits_Units_UnitId))
                        .FirstOrDefault().tbl_Categories.CategoryName);
                    item.DateString = item.CreatedAt.ToString("dd/MM/yyyy");
                }
                return clientSales;
            }
            return new List<AvailableDto>();
        }

        public async Task<SelectList> GetRegionsAsync()
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Regions_Id", "Region");
        }

        public async Task<SelectList> GetRegionById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Regions>, List<RegionDto>>((await _uow.RegionRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Regions_Id", "Region", await _uow.RegionRepo.FindAsync(reg => reg.PK_Regions_ID == id));
        }

        public async Task<SelectList> GetFinishingsAsync()
        {
            return new SelectList(Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false && !v.IsMaster).ToList()), "PK_Finishings_Id", "Type");
        }

        public async Task<SelectList> GetFinishingsByIdAsync(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Finishings>, List<FinishingDto>>((await _uow.FinishRepo.GetAllAsync()).Where(v => v.IsDeleted == false && !v.IsMaster).ToList()), "PK_Finishings_Id", "Type", await _uow.FinishRepo.FindAsync(f => f.PK_Finishings_Id == id));
        }

        public async Task<SelectList> GetCatsAsync()
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Categories_Id", "CategoryName");
        }

        public async Task<SelectList> GetCatsById(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Categories>, List<CatDto>>((await _uow.CatRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Categories_Id", "CategoryName", await _uow.CatRepo.FindAsync(cat => cat.PK_Categories_Id == id));
        }

        public async Task<SelectList> GetTransAsync(string name)
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

        public async Task<SelectList> GetViewsAsync()
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name");

        }

        public async Task<SelectList> GetViewsByIdAsync(int id)
        {
            return new SelectList(Mapper.Map<List<tbl_Views>, List<ViewDto>>((await _uow.ViewsRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Views_Id", "Name", id);
        }

        public async Task<MultiSelectList> GetAccessAsync()
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name");
        }

        public async Task<MultiSelectList> GetAccessByIdAsync(int[] ids)
        {
            return new MultiSelectList(Mapper.Map<List<tbl_Accessories>, List<AccessDto>>((await _uow.AcssRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList()), "PK_Accessories_Id", "Name", ids);

        }

        public async Task<SelectList> GetPaymentsAsync()
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod");

        }

        public async Task<SelectList> GetPaymentsIdAsync(int id)
        {
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.GetAllAsync()).Where(v => v.IsDeleted == false).ToList();
            //_context.Session["payments"] = payments;
            return new SelectList(Mapper.Map<List<tbl_PaymentMethods>, List<PaymentDto>>(payments), "PK_PaymentMethods_Id", "PaymentMethod", id);

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
        //**********************************************************************************
        /// <summary>
        /// To Check on Duplicate Client Sales by make compare between each item 
        /// </summary>
        /// <param name="ClientSales">we just take data from user and comapre it</param>
        /// <returns>we return true or false with Messagebox</returns>
        public async Task<IConfirmation> CheckDuplicateClientSales(AvailableDto ClientSales)
        {

            bool exists = (await _uow.AvailableRepo.FindAsync(b => b.DateOfBuild == ClientSales.DateOfBuild && b.FK_AvailableUnits_Usage_Id == ClientSales.FK_AvailableUnits_Usage_Id &&
             b.NoOfElevators == ClientSales.NoOfElevators &&
             b.FK_AvaliableUnits_Transaction_TransactionId == ClientSales.FK_AvaliableUnits_Transaction_TransactionId &&
             b.Price == ClientSales.Price && b.FK_AvailableUnits_PaymentMethod_Id == ClientSales.FK_AvailableUnits_PaymentMethod_Id &&
             b.tbl_units.ApartmentNumber == ClientSales.tbl_units.ApartmentNumber&&b.tbl_units.FlatNumber==ClientSales.tbl_units.FlatNumber
             && b.tbl_units.GroupNumber==ClientSales.tbl_units.GroupNumber&& b.tbl_units.FK_Units_Regions_Id == ClientSales.tbl_units.FK_Units_Regions_Id &&
             b.tbl_units.FK_Units_Categories_Id == ClientSales.tbl_units.FK_Units_Categories_Id && b.tbl_units.Space == ClientSales.tbl_units.Space &&
             b.tbl_units.Rooms == ClientSales.tbl_units.Rooms && b.tbl_units.Bathrooms == ClientSales.tbl_units.Bathrooms &&
             b.tbl_units.Floor == ClientSales.tbl_units.Floor && b.tbl_units.FK_Units_Finishing_Id == ClientSales.tbl_units.FK_Units_Finishing_Id &&
             b.tbl_units.FK_Units_Views_Id == ClientSales.tbl_units.FK_Units_Views_Id && b.FK_AvaliableUnits_Clients_ClientId == ClientSales.FK_AvaliableUnits_Clients_ClientId)).Any();
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

        public async Task<IConfirmation> SaveImportedClientSale(AvailableDto sale, int userId, int branchId)
        {
            tbl_units newUnit = Mapper.Map<AvailableDto, tbl_units>(sale);
            newUnit.FK_Units_Users_CreatedBy = userId;
            newUnit.FK_Units_Users_ModidfiedBy = userId;
            newUnit.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            sale.AdvancePayment = sale.Price + sale.Over;
            _uow.UnitRepo.Add(newUnit);
            try
            {
                _conf.Valid = await _uow.SaveAsync() > 0;
             
            }
            catch (Exception ex)
            {
                throw ex;
            }

            int i = 0;

            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ بيانات الوحدة بنجاح!";
                return _conf;
            }
           
            tbl_AvailableUnits newSale = Mapper.Map<AvailableDto, tbl_AvailableUnits>(sale);
            newSale.FK_AvaliableUnits_Users_CreatedBy = userId;
            newSale.FK_AvaliableUnits_Users_ModifiedBy = userId;
            newSale.FK_AvaliableUnits_Units_UnitId = newUnit.PK_Units_Id;
            newSale.FK_AvailableUnits_PaymentMethod_Id = sale.FK_AvailableUnits_PaymentMethod_Id;
            newSale.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            newSale.FK_AvaliableUnits_Branches_BranchId = branchId;
                _uow.AvailableRepo.Add(newSale);
            try
            {
            _conf.Valid = await _uow.SaveAsync() > 0;
            }
            catch (Exception ex )
            {

                throw ex ;
            }
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            _conf.Message = "تم الحفظ بنجاح!";
          
            return _conf;

        }

        public async Task<IConfirmation> SaveClientSale(AvailableDto sale, int userId, List<string> images, int branchId)
        {
            tbl_units newUnit = Mapper.Map<UnitDto, tbl_units>(sale.tbl_units);
            newUnit.FK_Units_Users_CreatedBy = userId;
            newUnit.FK_Units_Users_ModidfiedBy = userId;
            newUnit.CreatedAt = DateTime.UtcNow.AddMinutes(120);
            sale.AdvancePayment = sale.Price + sale.Over;
            _uow.UnitRepo.Add(newUnit);
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
                    tbl_UnitImages unitImage = new tbl_UnitImages
                    {
                        CreatedAt = DateTime.UtcNow.AddMinutes(120),
                        ModifiedAt = DateTime.UtcNow.AddMinutes(120),
                        FK_UnitImages_Users_CreatedBy = userId,
                        FK_UnitImages_Users_ModidfiedBy = userId,
                        FK_UnitImages_Units_UnitId = newUnit.PK_Units_Id,
                        ImageUrl = path,
                    };
                    if (i == 0)
                    {
                        unitImage.IsMainImage = true;
                    }
                    _uow.UnitImagesRepo.Add(unitImage);
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
                    tbl_UnitAccessories unitAccess = new tbl_UnitAccessories
                    {
                        CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                        FK_UnitAccessories_Users_CreatedBy = userId,
                        FK_UnitAccessories_Users_ModidfiedBy = userId,
                        FK_UnitAccessories_Units_Id = newUnit.PK_Units_Id,
                        FK_UnitAccessories_Accessories_Id = int.Parse(access),
                    };
                    _uow.UnitAccessRepo.Add(unitAccess);
                }
                _conf.Valid = await _uow.SaveAsync() == (sale.AccessoriesArr.Length);
                if (!_conf.Valid)
                {
                    _conf.Message = "لم يتم حفظ الكماليات بنجاح!";
                    return _conf;
                }
            }
            tbl_AvailableUnits newSale = Mapper.Map<AvailableDto, tbl_AvailableUnits>(sale);
            newSale.FK_AvaliableUnits_Users_CreatedBy = userId;
            newSale.FK_AvaliableUnits_Users_ModifiedBy = userId;
            newSale.FK_AvaliableUnits_Units_UnitId = newUnit.PK_Units_Id;
            newSale.FK_AvailableUnits_PaymentMethod_Id = sale.FK_AvailableUnits_PaymentMethod_Id;
            newSale.Date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
            newSale.FK_AvaliableUnits_Branches_BranchId = branchId;
            _uow.AvailableRepo.Add(newSale);
            _conf.Valid = await _uow.SaveAsync() > 0;
            if (!_conf.Valid)
            {
                _conf.Message = "لم يتم حفظ العرض بنجاح!";
                return _conf;
            }
            _conf.Message = "تم الحفظ بنجاح!";
            sale.PK_AvailableUnits_Id = newSale.PK_AvailableUnits_Id;
            _conf.DemandsAndExcluded = await FilterAvailables(sale, userId);
            _conf.availables.Clear();

            _conf.availables.Add(Mapper.Map<tbl_AvailableUnits, AvailableDto>(newSale));
            return _conf;

        }
        //******************************************************************************
        public async Task<IConfirmation> UpdateClientSale(AvailableDto sale, int userId, List<string> images, int branchId)
        {
            if (sale.PK_AvailableUnits_Id > 0)
            {
                tbl_AvailableUnits DBclientSale = (await _uow.AvailableRepo.FindAsync(a => a.PK_AvailableUnits_Id == sale.PK_AvailableUnits_Id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientSale != null)
                {
                    DBclientSale.FK_AvaliableUnits_Transaction_TransactionId = sale.FK_AvaliableUnits_Transaction_TransactionId;
                    DBclientSale.FK_AvailableUnits_PaymentMethod_Id = sale.FK_AvailableUnits_PaymentMethod_Id;
                    DBclientSale.IsNegotiable = sale.IsNegotiable;
                    DBclientSale.Notes = sale.Notes;
                    DBclientSale.Price = sale.Price;
                    //DBclientSale.Title = sale.Title;
                    DBclientSale.BasisOfInstallment = sale.BasisOfInstallment;
                    DBclientSale.YearOfInstallment = sale.YearOfInstallment;
                    DBclientSale.Remaining = sale.Remaining;
                    DBclientSale.Over = sale.Over;
                    DBclientSale.FK_AvaliableUnits_Users_ModifiedBy = userId;
                    DBclientSale.FK_AvaliableUnits_Branches_BranchId = branchId;
                    DBclientSale.FK_AvaliableUnits_Users_SalesId = sale.FK_AvaliableUnits_Users_SalesId;
                    DBclientSale.FK_AvailableUnits_Usage_Id = sale.FK_AvailableUnits_Usage_Id;
                    DBclientSale.NoOfElevators = sale.NoOfElevators;
                    DBclientSale.DateOfBuild = sale.DateOfBuild;
                    DBclientSale.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                    DBclientSale.AdvancePayment = sale.Price + sale.Over;
                }
                _uow.AvailableRepo.Update(DBclientSale);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حفظ العرض بنجاح";
                    return _conf;
                }

                if (sale.FK_AvaliableUnits_Units_UnitId > 0)
                {
                    tbl_units DBUnit = (await _uow.UnitRepo.FindAsync(a => a.PK_Units_Id == sale.FK_AvaliableUnits_Units_UnitId && !a.IsDeleted)).FirstOrDefault();
                    if (DBUnit != null)
                    {
                        DBUnit.ApartmentNumber = sale.tbl_units.ApartmentNumber;
                        DBUnit.FlatNumber = sale.tbl_units.FlatNumber;
                        DBUnit.GroupNumber = sale.tbl_units.GroupNumber;
                        //DBUnit.Address = sale.tbl_units.Address;
                        DBUnit.Bathrooms = sale.tbl_units.Bathrooms;
                        DBUnit.Descreption = sale.tbl_units.Descreption;
                        DBUnit.FK_Units_Categories_Id = sale.tbl_units.FK_Units_Categories_Id;
                        DBUnit.FK_Units_Finishing_Id = sale.tbl_units.FK_Units_Finishing_Id;
                        DBUnit.FK_Units_Regions_Id = sale.tbl_units.FK_Units_Regions_Id;
                        DBUnit.FK_Units_Views_Id = sale.tbl_units.FK_Units_Views_Id;
                        DBUnit.Rooms = sale.tbl_units.Rooms;
                        DBUnit.Space = sale.tbl_units.Space;
                        DBUnit.FK_Units_Users_ModidfiedBy = userId;
                        DBUnit.Floor = sale.tbl_units.Floor;
                        DBUnit.IsFurniture = sale.tbl_units.IsFurniture;
                        DBUnit.IsMarketResearch = sale.tbl_units.IsMarketResearch;
                        _uow.UnitRepo.Update(DBUnit);
                        _conf.Valid = await _uow.SaveAsync() > 0;
                        if (_conf.Valid == false)
                        {
                            _conf.Message = "لم يتم حفظ العرض بنجاح";
                            return _conf;
                        }
                    }
                }
                if (images != null && images.Any())
                {
                    List<tbl_UnitImages> unitImages = (await _uow.UnitImagesRepo.FindAsync(ui => ui.FK_UnitImages_Units_UnitId == sale.FK_AvaliableUnits_Units_UnitId && !ui.IsDeleted)).ToList();
                    foreach (tbl_UnitImages item in unitImages)
                    {
                        _uow.UnitImagesRepo.Remove(item);
                    }

                    int i = 0;
                    foreach (string path in images)
                    {
                        tbl_UnitImages unitImage = new tbl_UnitImages
                        {
                            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                            ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                            FK_UnitImages_Users_CreatedBy = userId,
                            FK_UnitImages_Users_ModidfiedBy = userId,
                            FK_UnitImages_Units_UnitId = sale.FK_AvaliableUnits_Units_UnitId,
                            ImageUrl = path,
                        };
                        if (i == 0)
                        {
                            unitImage.IsMainImage = true;
                        }
                        _uow.UnitImagesRepo.Add(unitImage);
                        i++;
                    }

                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حفظ الصور بنجاح!";
                        return _conf;
                    }
                    foreach (tbl_UnitImages path in unitImages)
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

                List<tbl_UnitAccessories> unitAccessList = (await _uow.UnitAccessRepo.FindAsync(ui => ui.FK_UnitAccessories_Units_Id == sale.FK_AvaliableUnits_Units_UnitId && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_UnitAccessories item in unitAccessList)
                    {
                        _uow.UnitAccessRepo.Remove(item);
                    }
                    if (sale.AccessoriesArr != null && sale.AccessoriesArr.Any())
                    {
                        foreach (string access in sale.AccessoriesArr)
                        {
                            tbl_UnitAccessories unitAccess = new tbl_UnitAccessories
                            {
                                CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time"),
                                FK_UnitAccessories_Users_CreatedBy = userId,
                                FK_UnitAccessories_Users_ModidfiedBy = userId,
                                FK_UnitAccessories_Units_Id = sale.FK_AvaliableUnits_Units_UnitId,
                                FK_UnitAccessories_Accessories_Id = int.Parse(access),
                            };
                            _uow.UnitAccessRepo.Add(unitAccess);
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
                _conf.DemandsAndExcluded = await FilterAvailables(sale, userId);
                _conf.availables.Clear();
                var available = Mapper.Map<tbl_AvailableUnits, AvailableDto>(DBclientSale);
                available.tbl_units = Mapper.Map<tbl_units, UnitDto>(DBclientSale.tbl_units);
                _conf.availables.Add(available);
                return _conf;
            }
            _conf.Valid = false;
            _conf.Message = "لم يتم الحفظ حدث خطأ ما!";
            return _conf;

        }

        public async Task<List<DemandDto>> GetDemandsBuyerName(List<DemandDto> demandDtos)
        {
            foreach (DemandDto item in demandDtos)
            {
                item.BuyerName = (await _uow.ClientRepo.FindAsync(c => c.PK_Client_Id == item.FK_DemandUnits_Clients_ClientId)).FirstOrDefault().Name;
            }
            return demandDtos;
        }
        public async Task<AvailableDto> EditClientSale(int id)
        {
            List<AvailableDto> clientSales = Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>((await _uow.AvailableRepo
                .FindAsync(a => a.PK_AvailableUnits_Id == id && !a.IsDeleted, a => a.tbl_units)).ToList());

            if (clientSales.Any() && clientSales != null)
            {
                AvailableDto clientSale = clientSales.FirstOrDefault();
                clientSale.tbl_units = Mapper.Map<tbl_units, UnitDto>((await _uow.UnitRepo.FindAsync(u => u.PK_Units_Id == clientSale.FK_AvaliableUnits_Units_UnitId)).FirstOrDefault());
                clientSale.AccessoriesIds = (await _uow.UnitAccessRepo.FindAsync(ua => ua.FK_UnitAccessories_Units_Id == clientSale.tbl_units.PK_Units_Id)).Select(x => x.FK_UnitAccessories_Accessories_Id).ToArray();
                clientSale.Images = (await _uow.UnitImagesRepo.FindAsync(ui => ui.FK_UnitImages_Units_UnitId == clientSale.tbl_units.PK_Units_Id)).Select(x => x.ImageUrl).ToArray();
                return clientSale;
            }
            return new AvailableDto();
        }

        public async Task<IConfirmation> DeleteClientSale(int id, int userId)
        {
            if (id > 0)
            {
                tbl_AvailableUnits DBclientSale = (await _uow.AvailableRepo.FindAsync(a => a.PK_AvailableUnits_Id == id && !a.IsDeleted)).FirstOrDefault();
                if (DBclientSale != null)
                {
                    DBclientSale.IsDeleted = true;
                    DBclientSale.FK_AvaliableUnits_Users_ModifiedBy = userId;
                }
                _uow.AvailableRepo.Update(DBclientSale);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم حذف العرض بنجاح";
                    return _conf;
                }
                tbl_units DBUnit = (await _uow.UnitRepo.FindAsync(a => a.PK_Units_Id == DBclientSale.FK_AvaliableUnits_Units_UnitId && !a.IsDeleted)).FirstOrDefault();
                if (DBUnit != null)
                {

                    DBUnit.FK_Units_Users_ModidfiedBy = userId;
                    DBUnit.IsDeleted = true;
                }
                _uow.UnitRepo.Update(DBUnit);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid == false)
                {
                    _conf.Message = "لم يتم  حذف الوحدة بنجاح";
                    return _conf;
                }
                List<tbl_UnitImages> unitImages = (await _uow.UnitImagesRepo.FindAsync(ui => ui.FK_UnitImages_Units_UnitId == DBclientSale.FK_AvaliableUnits_Units_UnitId && !ui.IsDeleted)).ToList();
                if (unitImages != null && unitImages.Count > 0)
                {
                    foreach (tbl_UnitImages path in unitImages)
                    {
                        string photo = Directory
                                             .GetFiles(_server.MapPath("/Assets/Img/ClientSalesImages"), path.ImageUrl.Split('/')[3], SearchOption.AllDirectories)
                                             .FirstOrDefault();
                        if (photo != null)
                        {
                            System.IO.File.Delete(photo);
                        }
                    }
                    foreach (tbl_UnitImages item in unitImages)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_UnitImages_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.UnitImagesRepo.Update(item);
                    }
                    _conf.Valid = await _uow.SaveAsync() > 0;
                    if (!_conf.Valid)
                    {
                        _conf.Message = "لم يتم حذف الصور بنجاح!";
                        return _conf;
                    }

                }

                List<tbl_UnitAccessories> unitAccessList = (await _uow.UnitAccessRepo.FindAsync(ui => ui.FK_UnitAccessories_Units_Id == DBclientSale.FK_AvaliableUnits_Units_UnitId && !ui.IsDeleted)).ToList();
                if (unitAccessList != null && unitAccessList.Any())
                {
                    foreach (tbl_UnitAccessories item in unitAccessList)
                    {
                        item.ModifiedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Egypt Standard Time");
                        item.FK_UnitAccessories_Users_ModidfiedBy = userId;
                        item.IsDeleted = true;
                        _uow.UnitAccessRepo.Update(item);
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
        public async Task<IConfirmation> GetInstantMatches(int saleId, int userId)
        {
            tbl_AvailableUnits DBsale = (await _uow.AppartmentCustomrepository.LoadRelatedUnitAsync(saleId));
            AvailableDto sale = Mapper.Map<tbl_AvailableUnits, AvailableDto>(DBsale);

            if (sale != null)
            {
                sale.tbl_units = Mapper.Map<tbl_units, UnitDto>(DBsale.tbl_units);
                _conf.DemandsAndExcluded = await FilterAvailables(sale, userId);
                _conf.availables.Clear();
                _conf.availables.Add(sale);
                return _conf;
            }
            return _conf;
        }

        public async Task<(List<DemandDto>, List<DemandDto>)> GetAvailableMatches(AvailableDto available, int userId)
        {
            var DALAvailable = Mapper.Map<AvailableDto, tbl_AvailableUnits>(available);
            int RegCode = (await _uow.RegionRepo.FindAsync(r => r.PK_Regions_ID == available.tbl_units.FK_Units_Regions_Id)).FirstOrDefault().RegCode;
            List<tbl_DemandUnits> demands = new List<tbl_DemandUnits>();
            List<tbl_PaymentMethods> payments = (await _uow.PaymentRepo.FindAsync(p => !p.IsDeleted)).ToList();
            if (payments != null && payments.Any())
            {
                tbl_PaymentMethods payment = payments.Find(p => p.PK_PaymentMethods_Id == available.FK_AvailableUnits_PaymentMethod_Id);
                if (payment != null && !payment.IsMaster)
                {
                    if (available.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple)
                    {
                        demands = await _uow.AppartmentCustomrepository.DemandsForMultiUsageAndPayment(DALAvailable);
                    }
                    else
                    {
                        demands = await _uow.AppartmentCustomrepository.DemandsForUsageAndPayment(DALAvailable);
                    }
                }
                //else
                //{
                //    if (available.FK_AvailableUnits_Usage_Id == UnitUsages.Multiple)
                //    {
                //        demands = await _uow.AppartmentCustomrepository.DemandsForMultiUsageAnyPayment(DALAvailable);
                //    }
                //    else
                //    {
                //        demands = await _uow.AppartmentCustomrepository.DemandsForUsageAnyPayment(DALAvailable);
                //    }
                //}
            }
            var collegueDemands = demands.Any() ? demands.Where(d => d.FK_DemandUnits_Users_CreatedBy != userId).ToList() : new List<tbl_DemandUnits>();

            var empDemands = demands.Any() ? demands.Where(d => d.FK_DemandUnits_Users_CreatedBy == userId).ToList() : new List<tbl_DemandUnits>();

            return (Mapper.Map<List<tbl_DemandUnits>, List<DemandDto>>(empDemands), Mapper.Map<List<tbl_DemandUnits>, List<DemandDto>>(collegueDemands));
        }


        //******************************************************************************************
        public async Task<bool> CloseAvailable(int id, int userId)
        {
            tbl_AvailableUnits available = (await _uow.AvailableRepo.FindAsync(u => u.PK_AvailableUnits_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = true;
            available.FK_AvaliableUnits_Users_ModifiedBy = userId;
            _uow.AvailableRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> ReleaseAvailable(int id, int userId)
        {
            tbl_AvailableUnits available = (await _uow.AvailableRepo.FindAsync(u => u.PK_AvailableUnits_Id == id && !u.IsDeleted)).FirstOrDefault();
            available.IsClosed = false;
            available.FK_AvaliableUnits_Users_ModifiedBy = userId;
            _uow.AvailableRepo.Update(available);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<List<AvailableDto>> ClientBranchSalesByDateAndRegion(DateTime date, int regionId, int branchId)
        {
            IEnumerable<tbl_AvailableUnits> availalbles = (await _uow.AvailableRepo.FindAsync(a => a.tbl_units.FK_Units_Regions_Id == regionId && a.FK_AvaliableUnits_Branches_BranchId == branchId 
            && !a.IsDeleted && !a.IsClosed && (a.Date.Day == date.Day && a.Date.Month == date.Month && a.Date.Year == date.Year), a => a.tbl_units));
            List<AvailableDto> AvailableDtos = new List<AvailableDto>();
            foreach (tbl_AvailableUnits available in availalbles)
            {
                AvailableDto availableDto = new AvailableDto
                {
                    PK_AvailableUnits_Id = available.PK_AvailableUnits_Id,
                    tbl_units = new UnitDto
                    {
                        Bathrooms = available.tbl_units.Bathrooms,
                        Floor = available.tbl_units.Floor,
                        Rooms = available.tbl_units.Rooms,
                        Space = available.tbl_units.Space,
                    },
                    Price = available.Price
                };
                availableDto.tbl_units.FK_Units_Regions_Id = available.tbl_units.FK_Units_Regions_Id;
                availableDto.tbl_units.FK_Units_Finishing_Id = available.tbl_units.FK_Units_Finishing_Id;
                AvailableDtos.Add(availableDto);
            }

            return AvailableDtos;
            //return Mapper.Map<List<tbl_AvailableUnits>, List<AvailableDto>>(availalbles.ToList());
        }

        public async Task<bool> UpdateAvailableTransaction(int availableId, int transCode)
        {
            tbl_AvailableUnits available = (await _uow.AvailableRepo.FindAsync(u => u.PK_AvailableUnits_Id == availableId && !u.IsDeleted)).FirstOrDefault();
            if (available != null)
            {
                int? transactionId = (await _uow.TransRepo.FindAsync(t => t.TransCode == transCode && !t.IsDeleted)).FirstOrDefault()?.PK_Transactions_Id;
                available.FK_AvaliableUnits_Transaction_TransactionId = transactionId.Value;
            }
            return await _uow.SaveAsync() > 0;
        }

        public async Task<(List<DemandDto>, DemandDto, List<DemandDto>, List<DemandDto>)> FilterAvailables(AvailableDto available, int userId)
        {
            (List<DemandDto>, List<DemandDto>) demands = await GetAvailableMatches(available, userId);
            demands.Item1 = await GetDemandsBuyerName(demands.Item1);
            List<DemandDto> demandsWithPreviews = new List<DemandDto>();
            DemandDto sameClientDemand = demands.Item1.Find(d => d.FK_DemandUnits_Clients_ClientId == available.FK_AvaliableUnits_Clients_ClientId);
            if (sameClientDemand != null)
            {
                demands.Item1.Remove(sameClientDemand);
            }
            demands.Item2.RemoveAll(d => d.FK_DemandUnits_Clients_ClientId == available.FK_AvaliableUnits_Clients_ClientId);
            
            if (demands.Item1.Any() || demands.Item2.Any())
            {
               var DALEmpDemands= Mapper.Map<List<DemandDto>, List<tbl_DemandUnits>>(demands.Item1);
                var DALCollegesDemands =Mapper.Map<List<DemandDto>,List<tbl_DemandUnits>>(demands.Item2);
                var availablePreviews = await _uow.AppartmentCustomrepository.FilterDemandsHasPreviews(DALEmpDemands, DALCollegesDemands, available);


                if (availablePreviews.Any() && availablePreviews != null)
                {
                    foreach (DemandDto demand in demands.Item1.ToList())
                    {
                        if (availablePreviews.Exists(ap => ap.DemandId == demand.PK_DemandUnits_Id))
                        {
                            demandsWithPreviews.Add(demand);
                            demands.Item1.Remove(demand);
                        }
                    }
                    foreach (DemandDto demand in demands.Item2.ToList())
                    {
                        if (availablePreviews.Exists(ap => ap.DemandId == demand.PK_DemandUnits_Id))
                        {
                            demands.Item2.Remove(demand);
                        }
                    }
                }
            }
            return (demands.Item1, sameClientDemand, demandsWithPreviews, demands.Item2);

        }
    }

}

//if (demands.Item2.Any())
//{


//}
//var availablePreviews = (from header in _uow.PreviewRepo.GetAll()
//                         join detail
//                         in _uow.PreviewDetailRepo.GetAll()
//                         on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
//                         where (detail.AvailableUnits_Id == available.PK_AvailableUnits_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_AvaliableUnits_Clients_ClientId)
//                              && (header.ReviewDate >= DateTime.Today)
//                              && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision //remove the rejected 
//                              && (demands.Item1.Exists(d => d.PK_DemandUnits_Id == header.DemandUnit_Id && d.FK_DemandUnits_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId) ||
//                              demands.Item2.Exists(d => d.PK_DemandUnits_Id == header.DemandUnit_Id && d.FK_DemandUnits_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId))
//                         select new DemandsWithPreviews
//                         {
//                             HeaderId = header.PK_PreviewHeaders_Id,
//                             PreviewDate = header.ReviewDate,
//                             DemandId = header.DemandUnit_Id,
//                             BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
//                             AvailableId = detail.AvailableUnits_Id,
//                             Seller = detail.Fk_PreviewDetails_Clients_SellerId,
//                             DetailId = detail.PK_PreviewDetails_Id
//                         }
//                         ).ToList();

//var availablePreviews = (from header in _uow.PreviewRepo.GetAll()
//                         join detail
//                         in _uow.PreviewDetailRepo.GetAll()
//                         on header.PK_PreviewHeaders_Id equals detail.Fk_PreviewDetails_PreviewHeaders_Id
//                         where (detail.AvailableUnits_Id == available.PK_AvailableUnits_Id && detail.Fk_PreviewDetails_Clients_SellerId == available.FK_AvaliableUnits_Clients_ClientId)
//                              && (header.ReviewDate >= DateTime.Today)
//                              && !header.IsDeleted && !detail.IsDeleted && detail.IsNoDecision //remove the rejected 
//                              && demands.Item2.Exists(d => d.PK_DemandUnits_Id == header.DemandUnit_Id && d.FK_DemandUnits_Clients_ClientId == header.FK_PreviewHeaders_Clients_BuyerId)
//                         select new
//                         {
//                             HeaderId = header.PK_PreviewHeaders_Id,
//                             PreviewDate = header.ReviewDate,
//                             DemandId = header.DemandUnit_Id,
//                             BuyerId = header.FK_PreviewHeaders_Clients_BuyerId,
//                             AvailableId = detail.AvailableUnits_Id,
//                             Seller = detail.Fk_PreviewDetails_Clients_SellerId,
//                             DetailId = detail.PK_PreviewDetails_Id
//                         }
//                         ).ToList();