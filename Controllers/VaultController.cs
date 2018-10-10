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
  [Authorize]
  public class VaultController : Controller
  {
    private VaultRepository _repo;

    public VaultController(VaultRepository repo)
    {
      _repo = repo;
    }

    //GET ALL VAULTS BY USER ID
    [HttpGet]
    public IEnumerable<Vault> Get()
    {
      string userId = HttpContext.User.Identity.Name;
      return _repo.GetVaults(userId);
    }

    //GET VAULT BY ID
    [HttpGet("{id}")]
    public Vault GetById([FromRoute] int id)
    {
      return _repo.GetById(id);
    }

    //CREATE NEW VAULT
    [HttpPost]
    public Vault Post([FromBody] VaultForm vault)
    {
      if (ModelState.IsValid)
      {
        return _repo.Create(vault);
      }
      throw new Exception("Invalid Vault Object");
    }

    //EDIT VAULT    
    [HttpPut]
    public void Put([FromBody] Vault vault)
    {
      if (vault.UserId == HttpContext.User.Identity.Name)
      {
        _repo.Update(vault);
      }
      else throw new Exception("Not authorized to edit another user's vault");
    }

    //DELETE A VAULT    
    [HttpDelete]
    public void Delete([FromBody] VaultDiscard vault)
    {
      if (vault.UserId == HttpContext.User.Identity.Name)
      {
        _repo.Delete(vault.Id);
      }
      else throw new Exception("Not authorized to delete another user's vault");
    }
  }
}