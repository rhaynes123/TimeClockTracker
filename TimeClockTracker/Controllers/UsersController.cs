using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeClockTracker.Data;
using TimeClockTracker.Models;

namespace TimeClockTracker.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var LastPunch = _applicationDbContext.TimePunches.OrderByDescending(t => t.Id).FirstOrDefault(t => t.UserId == UserId).LastPunch;
                ApplicationUser employee = await _userManager.FindByIdAsync(UserId);
                return StatusCode(200, new { userId = UserId, lastPunch = LastPunch, userName = employee.UserName });
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        

    }
}
