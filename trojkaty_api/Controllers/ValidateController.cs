using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.UserService;
using trojakty_api.Core.ValidateService;
using trojakty_api.Core.ValidateService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidateController : ControllerBase
    {
        private IValidateService _validateService;
        private IUserService _userService;
        private IMapper _mapper;
        private IQuestionService _questionService;

        public ValidateController(IMapper mapper, IValidateService validateService, IUserService userService, IQuestionService questionService)
        {
            _questionService = questionService;
            _validateService = validateService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();
            try
            {
                var questions = await _validateService.GetAllAsync();
                return Ok(questions);
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("confirm")]
        public async Task<IActionResult> Confirm(int id, bool publish)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();
            try
            {
                var question = await _questionService.GetQuestionAsync(id);
                if(question == null)
                    throw new TrojkatyCoreException($"Cannot find question on id {id}");
                var v = await _validateService.ConfirmValidationAsync(question, publish);
                return Ok(_mapper.Map<ValidateResponseDTO>(v));
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

    //public class AdminRequiredAttribute : Attribute
    //{
    //    public AdminRequiredAttribute(HttpContext httpContext)
    //    {

    //    }
    //}
}