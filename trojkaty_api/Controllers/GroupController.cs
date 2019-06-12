using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.GroupService;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.UserService;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService _groupService;
        private IUserService _userService;
        private IMapper _mapper;
        private IQuestionService _questionService;

        public GroupController(IMapper mapper, IGroupService groupService, IUserService userService, IQuestionService questionService)
        {
            _mapper = mapper;
            _groupService = groupService;
            _userService = userService;
            _questionService = questionService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO groupDto)
        {
           User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
           
            try
            {
                var group = await _groupService.CreateGroupAsync(groupDto, user);
                return Ok(new {id = group.Id, name = group.Name});
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> AllGroup()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var v = await _groupService.GetGroupsAsync(user);
            return Ok(_mapper.Map<List<Group>,List<GroupResponseDTO>>(v));
        }

        [Authorize]
        [HttpPost("insert/question/{id}")]
        public async Task<IActionResult> InsertQuestion(int id, QuestionDTO questionDto)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var group = await _groupService.GetGroupAsync(id, user);

            try
            {
                var result = await _groupService.InsertQuestionAsync(group, questionDto);

                return Ok(_mapper.Map<Group, GroupResponseDTO>(result));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [Authorize]
        [HttpPost("insert/User")]
        public async Task<IActionResult> InsertUser(int id, string email)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var userInserted = _userService.GetByEmail(email);
            var group = await _groupService.GetGroupAsync(id, user);

            try
            {
                var result = await _groupService.InsertUserAsync(group, userInserted);

                return Ok(_mapper.Map<Group, GroupResponseDTO>(result));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("delete/User")]
        public async Task<IActionResult> DeleteUser(int id, string email)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var userDeleted = _userService.GetByEmail(email);
            var group = await _groupService.GetGroupAsync(id, user);

            try
            {
                var result = await _groupService.RemoveUserAsync(group, userDeleted);

                return Ok(_mapper.Map<Group, GroupResponseDTO>(result));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("delete/question")]
        public async Task<IActionResult> DeleteUser(int idGroup, int idQuestion)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);
            var group = await _groupService.GetGroupAsync(idGroup, user);
            var question = await _questionService.GetQuestionAsync(idQuestion);

            try
            {
                var result = await _groupService.RemoveQuestionAsync(group, question);

                return Ok(_mapper.Map<Group, GroupResponseDTO>(result));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("publish/{id}")]
        public async Task<IActionResult> PublishGroup(int id)
        {
            try
            {
                User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

                var group = await _groupService.GetGroupAsync(id, user);

                var retult = await _groupService.PublishAsync(group);

                return Ok(_mapper.Map<Group, GroupResponseDTO>(retult));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}