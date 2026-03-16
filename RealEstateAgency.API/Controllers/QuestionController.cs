using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgency.Application.Dto;
using RealEstateAgency.Application.Interfaces.Services;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController(IQuestionsService questionsService,
        IAnswersService answersService,
        UserManager<User> userManager) : ControllerBase
    {
        [HttpGet("get-all-by-announcement-id/{chatId:guid}")]
        public async Task<IActionResult> GetAllByAnnouncementId(Guid chatId)
        {
            var res = await questionsService.GetQuestionsAnswersByAnnouncementId(chatId);
            return Ok(res);
        }
        
        [HttpPost("add-question")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto questionDto)
        {
            var questionId = await questionsService.InsertQuestionAsync(questionDto);
            return questionId == Guid.Empty
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpPost("add-answer")]
        public async Task<IActionResult> AddAnswer([FromBody] AnswerDto answerDto)
        {
            var answerId = await answersService.InsertAnswerAsync(answerDto);
            return answerId == Guid.Empty
                ? StatusCode(StatusCodes.Status500InternalServerError)
                : StatusCode(StatusCodes.Status201Created);
        }
        
        [HttpPost("delete-question-by-id")]
        public async Task<IActionResult> DeleteQuestionById([FromBody]Guid questionId)
        {
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
