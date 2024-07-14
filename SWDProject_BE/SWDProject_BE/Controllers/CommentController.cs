using BusinessLayer.RequestModels;
using BusinessLayer.Services;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;

        public CommentController(ICommentService commentService, IPostService postService)
        {
            _commentService = commentService;
            _postService = postService;
        }

        [HttpGet("GetCommentByPost/{postId}")]
        public async Task<IActionResult> GetAllCommentsByPost(int postId)
        {
            try
            {
                var existingPost = await _postService.GetPostByIdAsync(postId);
                if (existingPost == null)
                {
                    return NotFound(new { message = "Post ID not found." });
                }

                var comments = await _commentService.GetAllCommentsByPostAsync(postId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCommentById{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                if (comment == null)
                {
                    return NotFound(new { message = "Comment ID not found." });
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentRequestModel commentRequest, int postId)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                var comment = new Comment
                {
                    UserId = userId,
                    PostId = postId,
                    Content = commentRequest.Content,
                    Date = DateTime.Now,
                    Status = true
                };

                await _commentService.AddCommentAsync(comment);

                return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, CommentRequestModel comment)
        {
            try
            {
                var existingComment = await _commentService.GetCommentByIdAsync(id);
                if (existingComment == null)
                {
                    return NotFound(new { message = "Comment ID not found." });
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                if (existingComment.UserId != userId && !User.IsInRole("Moderator"))
                {
                    return BadRequest("Only the CommentOwner (or Moderator) can modify it");
                }

                existingComment.Content = comment.Content;
                existingComment.Date = DateTime.Now;

                await _commentService.UpdateCommentAsync(existingComment);
                return Ok(new { message = "Comment Updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var existingComment = await _commentService.GetCommentByIdAsync(id);
                if (existingComment == null)
                {
                    return NotFound(new { message = "Comment ID not found." });
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                if (existingComment.UserId != userId && !User.IsInRole("Moderator"))
                {
                    return BadRequest("Only the CommentOwner (or Moderator) can modify it");
                }

                await _commentService.DeleteCommentAsync(id);
                return Ok(new { message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
