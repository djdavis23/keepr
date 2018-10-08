using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace keepr.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VaultKeepController : Controller
  {
    private VaultKeepRepository _repo;

    public VaultKeepController(VaultKeepRepository repo)
    {
      _repo = repo;
    }

    //GET ALL VAULTKEEPS BY USER ID
    [HttpGet]
    public IEnumerable<VaultKeep> Get()
    {
      string userId = HttpContext.User.Identity.Name;
      return _repo.GetByUserId(userId);
    }

    //GET VAULTKEEPS BY VAULTID
    [HttpGet("{id}")]
    public IEnumerable<VaultKeep> GetById([FromRoute] int id)
    {
      return _repo.GetByVaultId(id);
    }

    //CREATE NEW VAULTKEEP
    [HttpPost]
    public VaultKeep Post([FromBody] VaultKeep vk)
    {
      if (ModelState.IsValid)
      {
        return _repo.Create(vk);
      }
      throw new Exception("Invalid VaultKeep Entry");
    }

    //DELETE A VAULTKEEP    
    [HttpDelete]
    public void Delete([FromBody] VaultKeep vk)
    {
      if (vk.UserId == HttpContext.User.Identity.Name)
      {
        _repo.Delete(vk.VaultId, vk.KeepId);
      }
      else throw new Exception("Not authorized to edit another user's vault");
    }
  }
}