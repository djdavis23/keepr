using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultKeepRepository
  {
    private IDbConnection _db;

    public VaultKeepRepository(IDbConnection db)
    {
      _db = db;
    }

    //GET VAULTKEEP BY USERID
    public IEnumerable<VaultKeep> GetByUserId(string userId)
    {
      return _db.Query<VaultKeep>(@"
      SELECT *
      FROM vaultkeeps
      WHERE userId=@userId;", new { userId });
    }

    //GET VAULTKEEP BY VAULT ID
    public IEnumerable<VaultKeep> GetByVaultId(int id)
    {
      return _db.Query<VaultKeep>(@"
        SELECT * FROM vaultkeeps
        WHERE vaultId = @id;", new { id });
    }

    //POST A NEW VAULTKEEP
    public VaultKeep Create(VaultKeep vk)
    {
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO vaultkeeps (vaultId, keepId, userId)
        VALUES(@VaultId, @KeepId, @UserId);
        SELECT LAST_INSERT_ID();", vk);
      if (id == 0) return null;
      vk.Id = id;
      return vk;
    }

    //DELETE A VAULTKEEP
    public int Delete(int vaultId, int keepId)
    {
      return _db.Execute(@"
      DELETE FROM vaultkeeps 
      WHERE vaultId = @vaultId AND keepId = @keepId;", new { vaultId, keepId });
    }
  }
}