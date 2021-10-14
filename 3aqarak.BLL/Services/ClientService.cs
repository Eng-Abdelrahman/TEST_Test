using _3aqarak.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3aqarak.BLL.Dto;
using System.Web.Mvc;
using AutoMapper;
using _3aqarak.BLL.Models;
using System.Web;
using System.IO;

namespace _3aqarak.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;
        private readonly HttpServerUtilityBase _server;



        public ClientService(IUnitOfWork uow, IConfirmation conf, HttpServerUtilityBase server)
        {
            _uow = uow;
            _conf = conf;
            _server = server;

        }

        public ClientService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;           
        }
        public async Task<List<ClientDto>> GetClients()
        {
            var clients = await _uow.ClientRepo.FindAsync(u => u.IsDeleted == false);
            if (clients.Any() && clients != null)
            {
                return Mapper.Map<List<tbl_Clients>, List<ClientDto>>(clients.ToList());
            }
            return new List<ClientDto>();
        }
        public async Task<List<ClientDto>> ClientAutoComplete(string text)
        {
            return Mapper.Map<List<tbl_Clients>, List<ClientDto>>((await _uow.ClientRepo.FindAsync(c => (c.Name.ToLower().Contains(text.ToLower()) || c.Mobile.Contains(text) ||(c.Phone!=null&&c.Phone.Contains(text))|| (c.Mobile2 != null && c.Mobile2.Contains(text))) && !c.IsDeleted)).Take(10).ToList());
        }

        public async Task<bool> DeleteClient(int id, int userId)
        {
            var DBClient =(await _uow.ClientRepo.FindAsync(u => u.PK_Client_Id == id)).FirstOrDefault();
            if (DBClient != null)
            {
                DBClient.IsDeleted = true;
                DBClient.FK_Clients_Users_ModidfiedBy = userId;
                _uow.ClientRepo.Update(DBClient);
                _conf.Valid = await _uow.SaveAsync() > 0;
                if (_conf.Valid)
                {
                    var clientSales =await _uow.AvailableRepo.FindAsync(a => a.FK_AvaliableUnits_Clients_ClientId == DBClient.PK_Client_Id);
                    var clientDemands =await _uow.DemandRepo.FindAsync(a => a.FK_DemandUnits_Clients_ClientId == DBClient.PK_Client_Id);
                    var clientUnits =await _uow.UnitRepo.FindAsync(a => a.FK_Units_Client_Id == DBClient.PK_Client_Id);
                    foreach (var item in clientSales)
                    {
                        item.IsDeleted = true;
                        item.FK_AvaliableUnits_Users_ModifiedBy = userId;
                    }
                    foreach (var item in clientDemands)
                    {
                        item.IsDeleted = true;
                        item.FK_DemandUnits_Users_ModidfiedBy = userId;
                    }

                    foreach (var item in clientUnits)
                    {
                        item.IsDeleted = true;
                        item.FK_Units_Users_ModidfiedBy = userId;
                    }

                    _conf.Valid = await _uow.SaveAsync() == (clientSales.Count() + clientUnits.Count() + clientDemands.Count());
                }
                return _conf.Valid;
            }
            return _conf.Valid = false;
        }

        public async Task<ClientDto> FindClientByID(int id)
        {
            var client =(await _uow.ClientRepo.FindAsync(u => u.PK_Client_Id == id)).FirstOrDefault();
            return (client != null) ? Mapper.Map<tbl_Clients, ClientDto>(client) : new ClientDto();
        }
           

        public async Task<IConfirmation> SaveClient(ClientDto client, int userId)
        {
           
            if (client.PK_Client_Id == 0)
            {
                var newClient = Mapper.Map<ClientDto, tbl_Clients>(client);                
                    newClient.FK_Clients_Users_CreatedBy = userId;
                    newClient.FK_Clients_Users_ModidfiedBy = userId;                            
                _uow.ClientRepo.Add(newClient);
                _conf.Valid=await _uow.SaveAsync() > 0;
                _conf.Id = newClient.PK_Client_Id;
                _conf.Clients.Add(Mapper.Map< tbl_Clients, ClientDto >(newClient));
                return _conf;
            }
            _conf.Valid = false;
            _conf.Id = 0;
            return _conf;
        }

        public async Task<bool> UpdateClient(ClientDto client, int userId)
        {
            var DBClient =(await _uow.ClientRepo.FindAsync(u => u.PK_Client_Id == client.PK_Client_Id)).FirstOrDefault();
            if (DBClient != null)
            {
                DBClient.FK_Clients_Users_ModidfiedBy = 11;
                DBClient.Name = client.Name;
                DBClient.Phone = client.Phone;
                if (!string.IsNullOrEmpty(client.IdUrlBack))
                {
                    DBClient.IdUrlBack = client.IdUrlBack;
                }
                if (!string.IsNullOrEmpty(client.IdUrlFront))
                {
                    DBClient.IdUrlFront = client.IdUrlFront;
                }             
                DBClient.Mobile = client.Mobile;
                DBClient.Mobile2 = client.Mobile2;
                DBClient.IdNumber = client.IdNumber;
                DBClient.Job = client.Job;
                DBClient.Address = client.Address;
                DBClient.Notes = client.Notes;
                DBClient.BestContactHour = client.BestContactHour;
                _uow.ClientRepo.Update(DBClient);
                return await _uow.SaveAsync() > 0;

            }
            return false;

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
                        var fname = Path.Combine("Assets/Img/Clients/", Guid.NewGuid().ToString() + new FileInfo(file.FileName).Extension);
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
                var file = files[i];
                if (file.ContentLength > (3 * (1024 * 1024)))
                {
                    _conf.Message = "Not allowed to upload files over 3 MB";
                    _conf.Valid = false;
                    break;
                }
                var ext = file.ContentType.Split('/')[1].ToLower();
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
