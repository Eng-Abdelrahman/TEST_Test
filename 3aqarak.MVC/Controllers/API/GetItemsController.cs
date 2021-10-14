using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Services;
using _3aqarak.DAL.Models;
using _3aqarak.DAL.Repositories;
using _3aqarak.MVC.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace _3aqarak.MVC.API
{
    public class GetItemsController : ApiController
    {
        private readonly IAvailableService _availableService;
        private readonly IAvailableLandsSevice _LAndAvailableService;
        private readonly IShopAvailableService _ShopAvailableService;
        private readonly IVillasAvailablesService _VillasAvailablesService;
     
        public GetItemsController()
        {
            _availableService = new AvailableService(new UnitOfWork("RealEstateDB"),new Confirmation());
            _LAndAvailableService= new LandsAvailableService(new UnitOfWork("RealEstateDB"), new Confirmation());
            _ShopAvailableService= new ShopAvailableService(new UnitOfWork("RealEstateDB"), new Confirmation());
            _VillasAvailablesService= new VillasAvailableService(new UnitOfWork("RealEstateDB"), new Confirmation());
        }

        // GET api/GetItems/1
        public async Task<IHttpActionResult> GetApartment(int Id)
        {
            
            AvailableViewModel clientSale = Mapper.Map<AvailableDto, AvailableViewModel>(await _availableService.EditClientSale(Id));

            return Ok(clientSale);
        }
        [System.Web.Http.Route("api/GetItems/GetShop/{id}")]
        public async Task<IHttpActionResult> GetShop(int Id)
        {

            ShopAvailableViewModel clientSale = Mapper.Map<ShopAvailableDto, ShopAvailableViewModel>(await _ShopAvailableService.EditAvailableShop(Id));

            return Ok(clientSale);
        }
        [System.Web.Http.Route("api/GetItems/GetLand/{id}")]
        public async Task<IHttpActionResult> GetLand(int Id)
        {

            AvailableLandsViewModel clientSale = Mapper.Map<AvailableLandsDto, AvailableLandsViewModel>(await _LAndAvailableService.EditAvailableLand(Id));

            return Ok(clientSale);
        }
        [System.Web.Http.Route("api/GetItems/GetVilla/{id}")]
        public async Task<IHttpActionResult> GetVilla(int Id)
        {

            VillsAvailableViewModel clientSale = Mapper.Map<VillasAvailableDto, VillsAvailableViewModel>(await _VillasAvailablesService.EditClientSale(Id));
            
            return Ok(clientSale);
        }
    }
}
