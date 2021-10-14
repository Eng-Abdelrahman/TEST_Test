using _3aqarak.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3aqarak.BLL.Interfaces
{
    public interface IpostsService
    {
        Task<PostsDto> GetPostDetails(int id);
        Task<List<PostsDto>> GetPosts(DateTime fromdate,DateTime todate);       
        Task<IConfirmation> SavePosts(PostsDto post);
        Task<IConfirmation> DeletePosts(int id, int userId);
        Task<bool> CheckExistedPost(PostsDto post);


    }
}
