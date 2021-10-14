using _3aqarak.BLL.Dto;
using _3aqarak.BLL.Interfaces;
using _3aqarak.BLL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Services
{
    public class PostService : IpostsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfirmation _conf;

        public PostService(IUnitOfWork uow, IConfirmation conf)
        {
            _uow = uow;
            _conf = conf;
        }

        public async Task<bool> CheckExistedPost(PostsDto post)
        {
            var ExistedPost =(await _uow.PostsRepo.FindAsync(p => p.FK_Posts_Categories_Id == post.FK_Posts_Categories_Id && p.Mobile == post.Mobile && p.Unit_Id == post.Unit_Id && p.Name == post.Name)).FirstOrDefault();
            return ExistedPost != null ? true : false;
        }

        public Task<IConfirmation> DeletePosts(int id, int userId)
        {
            throw new NotImplementedException();

        }

        public async Task<PostsDto> GetPostDetails(int id)
        {
            tbl_Posts post =(await _uow.PostsRepo.FindAsync(p => p.PK_PostId==id)).FirstOrDefault();         
            return Mapper.Map<tbl_Posts, PostsDto>(post);
        }

        public async Task<List<PostsDto>> GetPosts(DateTime fromdate, DateTime todate)
        {
            List<tbl_Posts> posts = new List<tbl_Posts>();
            fromdate = fromdate.Date;
            todate = todate.Date;
            posts =(await _uow.PostsRepo.FindAsync(p => DbFunctions.TruncateTime(p.CreatedAt) >= fromdate && DbFunctions.TruncateTime(p.CreatedAt) <= todate)).ToList();
            return Mapper.Map<List<tbl_Posts>, List<PostsDto>>(posts);  
        }

        public async Task<IConfirmation> SavePosts(PostsDto post)
        {

            var newpost = Mapper.Map<PostsDto, tbl_Posts>(post);  
            
            _uow.PostsRepo.Add(newpost);
            _conf.Valid = await _uow.SaveAsync() > 0;
            _conf.Post = Mapper.Map<tbl_Posts, PostsDto>(newpost);
            return _conf;
        }
    }
}
