using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Helpers;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Services;
using _3aqarak.DAL.Repositories;
using _3aqarak.MVC.Helpers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace _3aqarak.MVC.API
{
    public class ClientPostsController : ApiController
    {

        private readonly IpostsService _postService;
        private IConfirmation _conf;

        public ClientPostsController()
        {
            _conf = new Confirmation();
            _postService = new PostService(new UnitOfWork("RealEstateDB"), _conf);
        }
        //public HttpResponseMessage Options(PostsDto post)
        //{
        //    var response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.OK;
        //    return response;
        //}

        [HttpPost]
        public async Task<IHttpActionResult> SavePosts([FromBody] PostsDto post)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            bool existed = await _postService.CheckExistedPost(post);
            if (existed)
            {
                return  new System.Web.Http.Results.ResponseMessageResult(Request.CreateErrorResponse(HttpStatusCode.NotImplemented, "Post already created for the same offer"));
                 
            }
            post.CraetedAt = post.CraetedAt.ToUniversalTime().AddHours(2);
            _conf = await _postService.SavePosts(post);
            if (_conf.Valid)
            {
                return Created(new Uri(Request.RequestUri.ToString() + "/" + _conf.Post.PK_PostId), _conf.Post);
            }
            return BadRequest("Couldn't Create the resource!");
        }
    }
}
