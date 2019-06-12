using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.StatisticService;
using trojakty_api.Core.UserService;
using trojkaty_api.DataAccess.Models;

namespace trojkaty_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private IStatisticService _statisticService;
        private IUserService _userService;

        public StatisticController(IStatisticService statisticService, IUserService userService)
        {
            _statisticService = statisticService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("categoriesCount")]
        public async Task<IActionResult> CategoryiesCount()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();

            return Ok( await _statisticService.CategoriesCountAsync());
        }

        [Authorize]
        [HttpGet("questionsCount")]
        public async Task<IActionResult> QuestionsCount()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();

            return Ok(await _statisticService.QuestionsCountAsync());
        }

        [Authorize]
        [HttpGet("groupsCount")]
        public async Task<IActionResult> GroupsCount()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();

            return Ok(await _statisticService.GroupsCountAsync());
        }

        [Authorize]
        [HttpGet("usersCount")]
        public async Task<IActionResult> UsersCount()
        {
            User user = _userService.GetByEmail(HttpContext.User.Identity.Name);

            if (user.Role != Role.Admin)
                return Unauthorized();

            return Ok(await _statisticService.UsersCountAsync());
        }

    }
}