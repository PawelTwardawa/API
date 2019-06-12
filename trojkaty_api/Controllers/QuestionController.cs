using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.GroupService;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.UserService;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        IQuestionService _questionService;
        private IGroupService _groupService;
        private IUserService _userService;
        private IMapper _mapper;

        public QuestionController(IMapper mapper, IQuestionService questionService, IGroupService groupService, IUserService userService)
        {
            _questionService = questionService;
            _groupService = groupService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionDTO questionDto)
        {
            try
            {
                var question = await _questionService.CreateQuestionAsync(questionDto);
                return Ok(_mapper.Map<QuestionDTO>(question));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var question = await _questionService.GetQuestionAsync();

            return Ok(_mapper.Map<QuestionDTO>(question));
        }

        [Authorize]
        [HttpGet("random/{count}")]
        public async Task<IActionResult> RandomCount(int count)
        {
            var question = await _questionService.GetQuestionsAsync(count);

            //return Ok(question);
            return Ok(_mapper.Map<List<QuestionDTO>>(question));
        }

        [Authorize]
        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _questionService.GetQuestionAsync(id);

            return Ok(_mapper.Map<QuestionDTO>(question));
        }

        [Authorize]
        [HttpGet("fromGroup/{id}")]
        public async Task<IActionResult> GetByGroup(int id)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var group = await _groupService.GetGroupAsync(id, user);

            var question = await _questionService.GetQuestionsAsync(group);

            return Ok(_mapper.Map<List<QuestionDTO>>(question));
        }

        [Authorize]
        [HttpGet("fromCategory/id={id},count={count}")]
        public async Task<IActionResult> GetFromCategory(int id, int count)
        {
            var cat = await _questionService.GetCategoryAsync(id);
            if (cat == null)
                return BadRequest(new { message = "couldn't find category" });
            var question = await _questionService.GetQuestionsAsync(count, cat);

            return Ok(_mapper.Map<List<QuestionDTO>>(question));
        }

        [Authorize]
        [HttpGet("fromCategory/{id}")]
        public async Task<IActionResult> GetFromCategory(int id)
        {
            var cat = await _questionService.GetCategoryAsync(id);
            if (cat == null)
                return BadRequest(new { message = "couldn't find category" });
            var question = await _questionService.GetQuestionAsync( cat);

            return Ok(_mapper.Map<QuestionDTO>(question));
        }

        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int id, QuestionDTO questionDto)
        {
            if (questionDto == null)
                return BadRequest(new {Message = "questionDto cannot be null"});

            try
            {
                var question = await _questionService.EditQuestionAsync(id, questionDto);

                return Ok(_mapper.Map<QuestionDTO>(question));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}