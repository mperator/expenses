using AutoMapper;
using Expenses.Api.Data.Dtos;
using Expenses.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Api.Controllers
{
    //public class AttendeesFilter
    //{
    //    public string Name { get; set; }
    //}

    //[Route("api/[controller]")]
    //[ApiController]
    //public class AttendeesController : ControllerBase
    //{
    //    private readonly UserManager<User> _userManager;
    //    private readonly IMapper _mapper;

    //    public AttendeesController(UserManager<User> userManager, IMapper mapper)
    //    {
    //        _userManager = userManager;
    //        _mapper = mapper;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<AttendeeReadModel>>> GetAttendeesAsync([FromQuery] AttendeesFilter filter)
    //    {
    //        var query = _userManager.Users;
    //        if (filter?.Name != null)
    //            query = query.Where(u =>
    //            u.FirstName.Contains(filter.Name) ||
    //            u.LastName.Contains(filter.Name) ||
    //            u.UserName.Contains(filter.Name));

    //        var users = await query.ToListAsync();
            
    //        return Ok(_mapper.Map<IEnumerable<AttendeeReadModel>>(users));
    //    }
    //}
}
