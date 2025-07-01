using eContentApp.Application.DTOs.Post; 
using eContentApp.Application.Interfaces; 
using eContentApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eContentApp.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostListDto>>> GetAllPosts()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PostDetailDto>> GetPostDetail(Guid id)
        {
            var post = await _postService.GetPostDetailAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newPostId = await _postService.CreatePostAsync(createPostDto);

                var createdPost = await _postService.GetPostDetailAsync(newPostId);

                if (createdPost == null)
                {
                    return StatusCode(500, "Failed to retrieve the created post.");
                }

                return CreatedAtAction(nameof(GetPostDetail), new { id = newPostId }, createdPost);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message }); // Handle application-specific errors
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostDto updatePostDto)
        {
            if (id != updatePostDto.Id)
            {
                return BadRequest("ID in URL and body mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _postService.UpdatePostAsync(updatePostDto);
                return NoContent(); 
            }
            catch (ApplicationException ex) 
            {
                return NotFound(ex.Message); 
            }
            catch (Exception) 
            {
                return StatusCode(500, "An error occurred while updating the post."); // 500 Internal Server Error
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return NoContent(); 
            }
            catch (ApplicationException ex) 
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }
    }
}
