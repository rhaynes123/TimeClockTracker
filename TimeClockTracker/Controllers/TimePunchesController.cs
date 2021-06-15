using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimeClockTracker.Models;
using Microsoft.AspNetCore.Identity;
using TimeClockTracker.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TimeClockTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TimePunchesController : Controller
    {
        private readonly ILogger<TimePunchesController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        public TimePunchesController(ILogger<TimePunchesController> logger
            , ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Post([FromBody] TimePunch input)
        {
            TimePunch newpunch = input;
            TimePunch openpunch = _applicationDbContext.TimePunches.OrderByDescending(t=>t.Id).FirstOrDefault(t=>t.UserId == newpunch.UserId && t.ClockOut == null);
            if(openpunch == null)
            {
                newpunch.UserName = newpunch.UserName;
                newpunch.ClockIn = newpunch.LastPunch;
                _applicationDbContext.Add(newpunch);
            }
            else
            {
                newpunch = openpunch;
                newpunch.ClockOut = input.LastPunch;
                newpunch.LastPunch = input.LastPunch;
                _applicationDbContext.Update(newpunch);
            }
            _applicationDbContext.SaveChanges();
            return Ok(newpunch);
        }

        [HttpGet]
        public IEnumerable<TimePunch> Get()
        {
            try
            {
                return _applicationDbContext.TimePunches.AsNoTracking().Where(t => t.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(t => t.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return new List<TimePunch>();
            }

        }
        [HttpGet("GetPunchById/{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                return Ok(_applicationDbContext.TimePunches.FirstOrDefault(t => t.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return NotFound();
            }

        }
        [HttpGet("LastPunch/{userid}")]
        public IActionResult LastPunch(string userid)
        {
            try
            {
                return Ok(_applicationDbContext.TimePunches.OrderByDescending(t => t.Id).FirstOrDefault(t => t.UserId == userid).LastPunch);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"{ex}");
                return NotFound();
            }
        }

        [HttpPut("[action]/{id}")]
        public IActionResult AdjustPunch(int id, [FromBody]TimePunch timePunch)
        {
            try
            {
                var existingPunch = _applicationDbContext.TimePunches.AsNoTracking().FirstOrDefault(e => e.Id == id);
                if (existingPunch == null)
                {
                    return NotFound();
                }
                else
                {
                    existingPunch = timePunch;

                    if (existingPunch.ClockIn > existingPunch.LastPunch)
                    {
                        existingPunch.LastPunch = existingPunch.ClockIn;
                    }
                    if (existingPunch.ClockOut != null && existingPunch.ClockOut > existingPunch.LastPunch)
                    {
                        existingPunch.LastPunch = (DateTime)existingPunch.ClockOut;
                    }
                    
                    _applicationDbContext.Attach(existingPunch);
                    _applicationDbContext.Entry(existingPunch).State = EntityState.Modified;
                    _applicationDbContext.SaveChanges();
                    return Ok(existingPunch);
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"{ex}");
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}