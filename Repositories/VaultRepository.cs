using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultRepository
  {
    private IDbConnection _db;

    public VaultRepository(IDbConnection db)
    {
      _db = db;
    }

    //GET CURRENT USER'S VAULTS
    public IEnumerable<Vault> GetVaults(string userId)
    {
      return _db.Query<Vault>(@"
      SELECT *
      FROM vaults
      WHERE userId=@userId;", new { userId });
    }



    //GET VAULT BY ID
    public Vault GetById(int id)
    {
      return _db.Query<Vault>(@"
        SELECT * FROM vaults
        WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //POST A NEW VAULT
    public Vault Create(VaultForm vault)
    {
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO vaults (name, description, userId)
        VALUES(@Name, @Description, @UserId);
        SELECT LAST_INSERT_ID();", vault);
      if (id == 0) return null;
      return GetById(id);
    }

    //EDIT A VAULT
    public int Update(Vault vault)
    {
      return _db.Execute(@"
        UPDATE vaults
        SET name=@Name, description=@description        
        WHERE id=@Id;", vault);
    }

    //DELETE A KEEP
    public int Delete(int id)
    {
      return _db.Execute("DELETE FROM vaults WHERE id = @id", new { id });
    }
  }
}