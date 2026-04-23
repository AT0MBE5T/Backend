using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Application.Utils;

namespace RealEstateAgency.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController(IQuestionsService questionsService,
        IAnswersService answersService) : ControllerBase
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

            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();
            
            return await questionsService.DeleteByQuestionIdAsync(questionId, userId)
                ? Ok()
                : BadRequest();
        }
        
        [HttpPost("delete-answer-by-id")]
        public async Task<IActionResult> DeleteAnswerById([FromBody]Guid answerId)
        {
            if (!User.IsInRole(Roles.ADMIN))
                return BadRequest();
            
            var userId = User.GetUserId();
            if (userId == Guid.Empty)
                return Unauthorized();
            
            return await answersService.DeleteByAnswerIdAsync(answerId, userId)
                ? Ok()
                : BadRequest();
        }
    }
}
