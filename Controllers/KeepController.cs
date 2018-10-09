using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace keepr.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class KeepController : Controller
  {
    private KeepRepository _repo;

    public KeepController(KeepRepository repo)
    {
      _repo = repo;
    }

    //GET 40 MOST RECENT KEEPS
    [HttpGet]
    public IEnumerable<Keep> Get()
    {
      string userId = HttpContext.User.Identity.Name;
      if (userId == null) userId = "not-logged-in";
      return _repo.GetKeeps(userId);
    }

    //GET MORE KEEPS - returns next 40 keeps
    //param id is the id of the last post retrieved
    [HttpGet("more/{id}")]
    public IEnumerable<Keep> Get([FromRoute] int id)
    {
      string userId = HttpContext.User.Identity.Name;
      if (userId == null) userId = "not-logged-in";
      return _repo.GetMoreKeeps(id, userId);
    }

    //GET KEEP BY ID
    [HttpGet("{id}")]
    public Keep GetById([FromRoute] int id)
    {
      return _repo.GetById(id);
    }

    //CREATE NEW KEEP
    [HttpPost]
    [Authorize]
    public Keep Post([FromBody] KeepForm keep)
    {
      if (ModelState.IsValid)
      {
        return _repo.Create(keep);
      }
      throw new Exception("Invalid Keep Object");
    }

    //EDIT KEEP    
    [HttpPut]
    [Authorize]
    public void Put([FromBody] Keep keep)
    {
      string userId = HttpContext.User.Identity.Name;
      _repo.Update(keep, userId);
    }

    //DELETE A KEEP
    //Can only delete your own private posts
    [HttpDelete]
    [Authorize]
    public void Delete([FromBody] Keep keep)
    {
      if (keep.UserId == HttpContext.User.Identity.Name && keep.IsPrivate == 0)
      {
        _repo.Delete(keep.Id);
      }
      else
      {
        throw new Exception("Not authorized to delete public keeps");
      }
    }
  }
}