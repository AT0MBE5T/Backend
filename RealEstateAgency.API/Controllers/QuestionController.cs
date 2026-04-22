using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController(IQuestionsService questionsService,
        IAnswersService answersService,
        UserManager<User> userManager) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("get-all-by-announcement-id/{chatId:guid}")]
        public async Task<IActionResult> GetAllByAnnouncementId(Guid chatId)
        {
            var res = await questionsService.GetQuestionsAnswersByAnnouncementId(chatId);
            return Ok(res);
        }
        
        [HttpGet("get-questions-grid")]
        public async Task<IActionResult> GetQuestionsGrid()
        {
            if (!User.IsInRole(Roles.ADMIN))
                return BadRequest();
            
            var result = await questionsService.GetQuestionsAnswersGrid();
            return Ok(result);
        }

        [HttpPost("delete-question-by-id")]
        public async Task<IActionResult> DeleteQuestionById([FromBody]Guid questionId)
        {
            if (!User.IsInRole(Roles.ADMIN))
                return BadRequest();
            
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await questionsService.DeleteByQuestionIdAsync(questionId, user.Id)
                ?  Ok()
                : BadRequest();
        }
        
        [HttpPost("delete-answer-by-id")]
        public async Task<IActionResult> DeleteAnswerById([FromBody]Guid answerId)
        {
            if (!User.IsInRole(Roles.ADMIN))
                return BadRequest();
            
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await answersService.DeleteByAnswerIdAsync(answerId, user.Id)
                ?  Ok()
                : BadRequest();
        }
    }
}
