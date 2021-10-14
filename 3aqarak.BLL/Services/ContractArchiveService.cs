using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using System.Threading.Tasks;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace _3aqarak.BLL.Services
{
    public class ContractArchiveService : IContractArchiveService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;
        public ContractArchiveService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _server = server;
        }
        /// <summary>
        /// to get all Contract in tatabase
        /// </summary>
        /// <returns>List of contractsDTO </returns>
        public async Task<List<ContractArchiveDto>> GetContractArchives()
        {
            var ContractArchive = await _uow.ContractArchiveRepo.FindAsync(u => u.IsDeleted == false);
            if (ContractArchive.Any() && ContractArchive != null)
            {
                return Mapper.Map<List<tbl_ContractArchives>, List<ContractArchiveDto>>(ContractArchive.ToList());
            }
            return new List<ContractArchiveDto>();
        }
        public async Task<bool> DeleteContractArchive(int id, int userId)
        {
            var DBContractArchive =(await _uow.ContractArchiveRepo.FindAsync(u => u.PK_ContractArchives_Id == id)).FirstOrDefault();
            var deleted = false;
            if (DBContractArchive != null)
            {
                DBContractArchive.IsDeleted = true;
                DBContractArchive.FK_ContractArchives_Users_ModidfiedBy = userId;
            }
            deleted = await _uow.SaveAsync() > 0;
            if (deleted)
            {
                if (DBContractArchive != null)
                {
                    var paths = DBContractArchive.ImageURL;

                    var photo = Directory.GetFiles(_server.MapPath("/Assets/ContractImage"), paths.Split('/')[2], SearchOption.AllDirectories)
                             .FirstOrDefault();
                    if (photo != null)
                    {
                        System.IO.File.Delete(photo);
                    }
                }
            }

            return deleted;
        }
        /// <summary>
        /// to finnd contract by contractId
        /// </summary>
        /// <param name="id">Set id to find Contract by id</param>
        /// <returns></returns>
        public async Task<ContractArchiveDto> FindContractArchiveByID(int id)
        {
            var ContractArchive =(await _uow.ContractArchiveRepo.FindAsync(u => u.PK_ContractArchives_Id == id)).FirstOrDefault();
            return (ContractArchive != null) ? Mapper.Map<tbl_ContractArchives, ContractArchiveDto>(ContractArchive) : new ContractArchiveDto();
        }
        /// <summary>
        /// To save Contract 
        /// </summary>
        /// <param name="ContractArchive"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> SaveContractArchive(ContractArchiveDto ContractArchive, int userId)
        {
            if (ContractArchive.PK_ContractArchives_Id == 0)
            {
                var newContractArchive = Mapper.Map<ContractArchiveDto, tbl_ContractArchives>(ContractArchive);
                newContractArchive.FK_ContractArchives_Users_CreatedBy = userId;
                newContractArchive.ImageURL = ContractArchive.ImageURL;
                newContractArchive.FK_ContractArchives_Users_ModidfiedBy = userId;
                _uow.ContractArchiveRepo.Add(newContractArchive);
                return await _uow.SaveAsync() > 0;
            }
            return false;
        }
        /// <summary>
        /// To chack Contract Id 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckContractId(string contractId, int id)
        {
            if (!string.IsNullOrEmpty(contractId) && id > 0)
            {
                var contract =(await _uow.ContractArchiveRepo.FindAsync(u => u.ContractID == contractId && u.IsDeleted == false)).FirstOrDefault();

                return (contract != null && contract.PK_ContractArchives_Id != id) ? true : false;
            }
            else if (!string.IsNullOrEmpty(contractId) && id == 0)
            {
                var contract =(await _uow.ContractArchiveRepo.FindAsync(u => u.ContractID == contractId && u.IsDeleted == false)).FirstOrDefault();

                return (contract != null) ? true : false;
            }
            return false;
        }
        /// <summary>
        /// to ubdate Contract
        /// </summary>
        /// <param name="ContractArchive"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateContractArchive(ContractArchiveDto ContractArchive, int userId)
        {
            var deleted = false;
            var DBContractArchive =(await _uow.ContractArchiveRepo.FindAsync(u => u.PK_ContractArchives_Id == ContractArchive.PK_ContractArchives_Id)).FirstOrDefault();

            if (DBContractArchive != null)
            {
                DBContractArchive.FK_ContractArchives_Users_ModidfiedBy = userId;
                DBContractArchive.ContractID = ContractArchive.ContractID;
                if (ContractArchive.ImageURL != null)
                {
                    if (ContractArchive.ImageURL != null)
                    {
                        var contractPDF = Directory.GetFiles(_server.MapPath("/Assets/ContractImage"), DBContractArchive.ImageURL.Split('/')[2], SearchOption.AllDirectories)
                                 .FirstOrDefault();
                        if (contractPDF != null)
                        {
                            System.IO.File.Delete(contractPDF);
                        }

                    }
                    DBContractArchive.ImageURL = ContractArchive.ImageURL;
                }

                _uow.ContractArchiveRepo.Update(DBContractArchive);
                deleted = await _uow.SaveAsync() > 0;
            }
            return deleted;

        }

        public IConfirmation SavePhotos(HttpFileCollectionBase files)
        {
            var conf = ValidatePhotos(files);
            if (conf.Valid)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var fname = Path.Combine("Assets/ContractImage/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                        file.SaveAs(_server.MapPath("/" + fname));
                        _conf.UrlList.Add(fname);
                        _conf.Message = "تم حفظ الملف بنجاح!";
                    }
                    return _conf;
                }
                catch (Exception)
                {
                    _conf.Valid = false;
                    _conf.Message = "حدث خطا ما عند حفظ الملف!";
                }
            }

            return _conf;
        }
        public IConfirmation DeletePhotos(HttpFileCollectionBase files)
        {
            var conf = ValidatePhotos(files);
            if (conf.Valid)
            {

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var fname = Path.Combine("Assets/ContractImage/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
                    // file.(_server.MapPath("/" + fname));
                    _conf.UrlList.Remove(fname);
                }
                return _conf;


            }

            return _conf;
        }


        public IConfirmation ValidatePhotos(HttpFileCollectionBase file)
        {
            for (int i = 0; i < file.Count; i++)
            {
                var files = file[i];
                if (files.ContentLength > (15 * (1024 * 1024)))
                {
                    _conf.Message = "Not allowed to upload files over 15 MB";
                    _conf.Valid = false;
                    break;
                }
                var ext = files.ContentType.Split('/')[1].ToLower();
                if (!ext.Equals("pdf", StringComparison.OrdinalIgnoreCase))
                {
                    _conf.Message = "only allowed file types:.pdf";
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
