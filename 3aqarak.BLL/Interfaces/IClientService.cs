using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _3aqarak.BLL.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetClients();      
        Task<ClientDto> FindClientByID(int id);     
        Task<bool> UpdateClient(ClientDto client, int userId);
        Task<IConfirmation> SaveClient(ClientDto client, int userId);
        Task<bool> DeleteClient(int id,int userId);
        IConfirmation SavePhotos(HttpFileCollectionBase files);
        IConfirmation ValidatePhotos(HttpFileCollectionBase file);
        Task<List<ClientDto>> ClientAutoComplete(string text);
    }
}
