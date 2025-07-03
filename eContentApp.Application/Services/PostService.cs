using AutoMapper;
using eContentApp.Application.DTOs.Post;
using eContentApp.Application.Interfaces;
using eContentApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace eContentApp.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository; 
        private readonly IMapper _mapper;


        public PostService(
            IPostRepository postRepository, 
            ICategoryRepository categoryRepository, 
            IMapper mapper)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostListDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsSimpleAsync();
            //var sortedPosts = posts.OrderByDescending(p => p.PublishedAt);
            return _mapper.Map<IEnumerable<PostListDto>>(posts);
        }
        public async Task<PostDetailDto?> GetPostDetailAsync(Guid id)
        {
            var post = await _postRepository.GetPostByIdWithCategoriesAsync(id);
            if (post == null) return null;

            return _mapper.Map<PostDetailDto>(post);
        }
        public async Task<Guid> CreatePostAsync(CreatePostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = Guid.NewGuid();
            post.PublishedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(postDto.Status) && Enum.TryParse(postDto.Status, true, out Domain.Entities.PostStatus parsedStatus))
            {
                post.Status = parsedStatus;
            }
            else
            {
                post.Status = Domain.Entities.PostStatus.Draft;
            }

            if (postDto.CategoryIds != null && postDto.CategoryIds.Count != 0)
            {
                var categories = new List<Category>();
                foreach (var categoryId in postDto.CategoryIds)
                {
                    
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        categories.Add(category);
                    }
                    else
                    {
                        throw new ApplicationException($"Category with ID {categoryId} not found.");
                    }
                }
                post.Categories = categories;
            }

            await _postRepository.AddAsync(post);
            return post.Id;
        }

        public async Task UpdatePostAsync(UpdatePostDto postDto)
        {
            var existingPost = await _postRepository.GetPostByIdWithCategoriesAsync(postDto.Id);
            if (existingPost == null)
            {
                throw new ApplicationException($"Post with ID {postDto.Id} not found.");
            }
            _mapper.Map(postDto, existingPost);

            if (string.IsNullOrEmpty(postDto.ThumbnailUrl))
            {

                existingPost.ThumbnailUrl = existingPost.ThumbnailUrl;
            }
            else
            {
                existingPost.ThumbnailUrl = postDto.ThumbnailUrl;
            }

            if (!string.IsNullOrEmpty(postDto.Status) && Enum.TryParse(postDto.Status, true, out Domain.Entities.PostStatus parsedStatus))
            {
                existingPost.Status = parsedStatus;
            }
            else
            {
                existingPost.Status = Domain.Entities.PostStatus.Draft; // Default to Draft if status is not provided or invalid
            }

            if (postDto.CategoryIds != null)
            {
                var categories = new List<Category>();
                foreach (var categoryId in postDto.CategoryIds)
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        categories.Add(category);
                    }
                    else
                    {
                        throw new ApplicationException($"Category with ID {categoryId} not found.");
                    }
                }
                existingPost.Categories = categories;
            }
            else
            {
                existingPost.Categories.Clear();
            }

            await _postRepository.UpdateAsync(existingPost);
        }

        public async Task DeletePostAsync(Guid id)
        {
            await _postRepository.DeleteAsync(id);
        }
    }
}
