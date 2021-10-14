using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using System.IO;
using _3aqarak.MVC.ViewModels;
using _3aqarak.DAL.Models;
using _3aqarak.BLL.Interfaces;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using AutoMapper;
using _3aqarak.BLL.Helpers;

namespace _3aqarak.MVC.Controllers
{
    public class AvailableImportExportController : Controller
    {
        private readonly IDemandService _demandService;
        private readonly IVillasDemandService _VillademandService;
        private readonly IVillasAvailablesService _VillaAvailableService;
        private readonly IAvailableService _availableService;
        private readonly IUSerService _userService;
        private readonly ISpecialService _specialService;
        private readonly ITransService _transService;
        private readonly IRegionService _regionService;
        private readonly IFinishService _finishService;
        private readonly IViewService _viewService;
        private readonly IClientService _clientService;
        private IConfirmation _conf;
        public AvailableImportExportController(ITransService transService, IVillasDemandService VillademandService, ISpecialService specialService, IDemandService demandService, IConfirmation conf, IAvailableService availableService, IUSerService userService, IRegionService regionService, IFinishService finishService, IViewService viewService, IClientService clientService, IVillasAvailablesService villasAvailablesService)
        {
            _VillademandService = VillademandService;
            _VillaAvailableService = villasAvailablesService;
            _demandService = demandService;
            _conf = conf;
            _availableService = availableService;
            _userService = userService;
            _specialService = specialService;
            _transService = transService;
            _regionService = regionService;
            _finishService = finishService;
            _viewService = viewService;
            _clientService = clientService;
        }
        // GET: AvailableImportExport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(/*HttpPostedFileBase postedFile*/)
        {
            HttpPostedFileBase postedFile = Request.Files[0];
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {
                        ViewBag.Message = "Please select the excel file with .xls or .xlsx extension";
                        return View();
                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    Session["ExcelFilePathList"] = filePath;
                    Session["ExcelFileExtension"] = fileExtension;


                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            //return View();
            return Json(new { valid = true, message = "Ok" }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetApartmentUploadFile()
        {
            //Get file extension

            string excelConString = "";

            //Get connection string using extension 
            switch (Session["ExcelFileExtension"])
            {
                //If uploaded file is Excel 1997-2007.
                case ".xls":
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                //If uploaded file is Excel 2007 and above
                case ".xlsx":
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            //Read data from first sheet of excel into datatable
            DataTable dt = new DataTable();
            excelConString = string.Format(excelConString, Session["ExcelFilePathList"]);

            using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
            {
                using (OleDbCommand excelDbCommand = new OleDbCommand())
                {
                    using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                    {
                        excelDbCommand.Connection = excelOledbConnection;

                        excelOledbConnection.Open();
                        //Get schema from excel sheet
                        DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                        //Get sheet name
                        string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                        excelOledbConnection.Close();

                        //Read Data from First Sheet.
                        excelOledbConnection.Open();
                        excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                        excelDataAdapter.SelectCommand = excelDbCommand;
                        //Fill datatable from adapter
                        excelDataAdapter.Fill(dt);
                        excelOledbConnection.Close();
                    }
                }
            }

            var Availables = new List<ImportAvailableViewModel>();
            //Insert records to Employee table.
            using (var context = new RealEstateDB())
            {

                //Loop through datatable and add employee data to employee table. 
                foreach (DataRow row in dt.Rows)
                {
                    Availables.Add(GetDemandFromExcelRow(row));

                }

            }
            ViewBag.Lists = new SelectedListsViewModel()
            {
                Views = await _demandService.GetViews(),//col[15]

                RegionsFrom = await _demandService.GetRegions(),//col[15]

                Finishings = await _demandService.GetFinishings(),//col[17]
                Usages = await _demandService.GetUsages(),//col[18]
                Transactions = await _availableService.GetTransAsync(null),//col[19]
                Payments = await _demandService.GetPayments(),//col[20]
            };
            return View("ApartmentImportView", Availables);
        }

        private static DataTable GetSchemaFromExcel(OleDbConnection excelOledbConnection)
        {
            return excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        }

        private ImportAvailableViewModel GetDemandFromExcelRow(DataRow row)
        {
            return new ImportAvailableViewModel
            {
                ClientName = row[0].ToString(),//col[0]
                ClientMobile = row[1].ToString(),//col[1]
                Price = int.Parse(row[2].ToString()),//col[2]     
                Space = int.Parse(row[3].ToString()),//col[3] 
                BathRooms = int.Parse(row[4].ToString()),//col[4]
                Rooms = int.Parse(row[5].ToString()),//col[5]
                Floor = int.Parse(row[6].ToString()),//col[6]     
                NoOfElevators = int.Parse(row[7].ToString()),//col[7]
                DateOfBuild = int.Parse(row[8].ToString()),//col[8]
                FlatNumber = row[9].ToString(),//col[9]
                ApartmentNumber = row[10].ToString(),//col[10]
                GroupNumber = row[11].ToString(),//col[11]

            };
        }

        [HttpPost]
        public async Task<JsonResult> SaveImportApartmentAvailable(ImportAvailableViewModel[] itemlist)
        {
            foreach (ImportAvailableViewModel Available in itemlist)
            {
                int catId = Categories.Apartements;
                int userId = ((UserDto)Session["User"]).PK_Users_Id;
                int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
                Available.CreatedAt = DateTime.UtcNow.AddHours(120);
                Available.FK_AvailableUnits_Categories_Id = catId;
                var ClientId = 0;
                var clientDto = Mapper.Map<ImportAvailableViewModel, ClientDto>(Available);
                clientDto.Name = Available.ClientName.Trim();
                clientDto.Mobile = Available.ClientMobile.Trim();
                _conf = await _clientService.SaveClient(clientDto, userId);
                if (_conf.Valid && _conf.Id > 0)
                {
                    ClientId = _conf.Id;
                    var availableDto = Mapper.Map<ImportAvailableViewModel, AvailableDto>(Available);
                    availableDto.FK_AvaliableUnits_Clients_ClientId = _conf.Id;
                    _conf = await _availableService.SaveImportedClientSale(availableDto, userId, branchId);
                }
            }
            return Json("Ok");
        }

        public async Task<ActionResult> GetVillaUploadFile()
        {
            //Get file extension

            string excelConString = "";

            //Get connection string using extension 
            switch (Session["ExcelFileExtension"])
            {
                //If uploaded file is Excel 1997-2007.
                case ".xls":
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                //If uploaded file is Excel 2007 and above
                case ".xlsx":
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            //Read data from first sheet of excel into datatable
            DataTable dt = new DataTable();
            excelConString = string.Format(excelConString, Session["ExcelFilePathList"]);

            using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
            {
                using (OleDbCommand excelDbCommand = new OleDbCommand())
                {
                    using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                    {
                        excelDbCommand.Connection = excelOledbConnection;

                        excelOledbConnection.Open();
                        //Get schema from excel sheet
                        DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                        //Get sheet name
                        string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                        excelOledbConnection.Close();

                        //Read Data from First Sheet.
                        excelOledbConnection.Open();
                        excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                        excelDataAdapter.SelectCommand = excelDbCommand;
                        //Fill datatable from adapter
                        excelDataAdapter.Fill(dt);
                        excelOledbConnection.Close();
                    }
                }
            }

            var Availables = new List<ImportVillaAvailableViewModel>();
            //Insert records to Employee table.
            using (var context = new RealEstateDB())
            {

                //Loop through datatable and add employee data to employee table. 
                foreach (DataRow row in dt.Rows)
                {
                    Availables.Add(GetAvailableVillaFromExcelRow(row));

                }

            }
            ViewBag.Lists = new SelectedListsViewModel()
            {
                Views = await _demandService.GetViews(),

                RegionsFrom = await _demandService.GetRegions(),//col[12]

                Finishings = await _demandService.GetFinishings(),//col[13]
                Usages = await _demandService.GetUsages(),//col[14]
                Transactions = await _availableService.GetTransAsync(null),//col[15]
                Payments = await _demandService.GetPayments(),//col[16]
            };
            return View("VillaImportView", Availables);
        }

        private ImportVillaAvailableViewModel GetAvailableVillaFromExcelRow(DataRow row)
        {
            return new ImportVillaAvailableViewModel
            {
                ClientName = row[0].ToString(),//col[0]
                ClientMobile = row[1].ToString(),//col[1]
                Price = int.Parse(row[2].ToString()),//col[2]     
                Space = int.Parse(row[3].ToString()),//col[3] 
                BathRooms = int.Parse(row[4].ToString()),//col[4]
                Rooms = int.Parse(row[5].ToString()),//col[5]
                AreaSpace = int.Parse(row[6].ToString()),//col[6]
                NoOfElevators = int.Parse(row[7].ToString()),//col[7]
                VillaNumber = row[8].ToString(),//col[8]
                GroupNumber = row[9].ToString(),//col[9]

            };
        }


        [HttpPost]
        public async Task<JsonResult> SaveImportVillaAvailable(ImportVillaAvailableViewModel[] itemlist)
        {
            foreach (ImportVillaAvailableViewModel Available in itemlist)
            {
                int catId = Categories.Villas;
                int userId = ((UserDto)Session["User"]).PK_Users_Id;
                int branchId = ((UserDto)Session["User"]).FK_Users_Branches_Id;
                Available.CreatedAt = DateTime.UtcNow.AddHours(120);
                Available.FK_VillasAvailables_Categories_Id = catId;
                var ClientId = 0;
                var clientDto = Mapper.Map<ImportVillaAvailableViewModel, ClientDto>(Available);
                clientDto.Name = Available.ClientName.Trim();
                clientDto.Mobile = Available.ClientMobile.Trim();
                _conf = await _clientService.SaveClient(clientDto, userId);
                if (_conf.Valid && _conf.Id > 0)
                {
                    ClientId = _conf.Id;
                    var availableDto = Mapper.Map<ImportVillaAvailableViewModel, ImportVillasAvailableDto>(Available);
                    availableDto.FK_VillasAvailables_Clients_ClientId = _conf.Id;

                    _conf = await _VillaAvailableService.SaveImportedVillasAvailable(availableDto, userId, branchId);

                }
            }
            return Json("Ok");
        }



        //********save client import
        public ActionResult GetClientUploadFile()
        {
            //Get file extension

            string excelConString = "";

            //Get connection string using extension 
            switch (Session["ExcelFileExtension"])
            {
                //If uploaded file is Excel 1997-2007.
                case ".xls":
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                //If uploaded file is Excel 2007 and above
                case ".xlsx":
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            //Read data from first sheet of excel into datatable
            DataTable dt = new DataTable();
            excelConString = string.Format(excelConString, Session["ExcelFilePathList"]);

            using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
            {
                using (OleDbCommand excelDbCommand = new OleDbCommand())
                {
                    using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                    {
                        excelDbCommand.Connection = excelOledbConnection;

                        excelOledbConnection.Open();
                        //Get schema from excel sheet
                        DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                        //Get sheet name
                        string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                        excelOledbConnection.Close();

                        //Read Data from First Sheet.
                        excelOledbConnection.Open();
                        excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                        excelDataAdapter.SelectCommand = excelDbCommand;
                        //Fill datatable from adapter
                        excelDataAdapter.Fill(dt);
                        excelOledbConnection.Close();
                    }
                }
            }

            var Demands = new List<ImportCientViewModel>();
            //Insert records to Employee table.
            using (var context = new RealEstateDB())
            {

                //Loop through datatable and add employee data to employee table. 
                foreach (DataRow row in dt.Rows)
                {
                    Demands.Add(GetClientFromExcelRow(row));

                }

            }

            return View("ImportClient", Demands);
        }

        private ImportCientViewModel GetClientFromExcelRow(DataRow row)
        {
            return new ImportCientViewModel
            {
                Name = row[0].ToString(),//col[1]
                Mobile = row[1].ToString(),//col[2]
                IdNumber = row[2].ToString(),//col[3]
                Address = row[3].ToString(),//col[4]
            };
        }


        [HttpPost]
        public async Task<JsonResult> SaveImportClient(ImportCientViewModel[] itemlist)
        {
            foreach (ImportCientViewModel Demand in itemlist)
            {
                Demand.Address = Demand.Address.Trim();
                Demand.Name = Demand.Name.Trim();
                Demand.IdNumber = Demand.IdNumber.Trim();
                Demand.Mobile = Demand.Mobile.Trim();
                var userId = ((UserDto)Session["User"]).PK_Users_Id;
                //  _conf = await _clientService.SaveClient(Mapper.Map<ImportCientViewModel, ClientDto>(Demand), userId); // ????????????????????????????????????????????????????????????????????
            }
            return Json("Ok");
        }
    }

}
