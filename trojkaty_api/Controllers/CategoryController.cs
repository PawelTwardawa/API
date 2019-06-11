using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.UserService;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        IQuestionService _questionService;
        private IUserService _userService;

        public CategoryController(IQuestionService questionService, IUserService userService)
        {
            _questionService = questionService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();
            try
            {
                var category = await _questionService.CreateCategory(categoryDTO);
                return Ok(category);
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var category = await _questionService.GetCategory();

            return Ok(category);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _questionService.GetCategories();

            return Ok(categories);
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _questionService.GetCategory(id);

            return Ok(category);
        }

        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> GetFromCategory(int id, CategoryDTO categoryDto)
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();
            try
            {
                var category = await _questionService.EditCategory(id, categoryDto);

                return Ok(category);
            }
            catch (TrojkatyCoreException ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
    }
}