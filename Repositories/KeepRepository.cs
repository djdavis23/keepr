using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class KeepRepository
  {
    private IDbConnection _db;

    public KeepRepository(IDbConnection db)
    {
      _db = db;
    }

    //GET ALL KEEPS
    public IEnumerable<Keep> GetKeeps()
    {
      return _db.Query<Keep>("SELECT * from keeps;");
    }

    //GET KEEP BY ID
    public Keep GetKeepById(int id)
    {
      return _db.Query<Keep>(@"
        SELECT * FROM keeps
        WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //POST A NEW KEEP
    public Keep CreateKeep(object keep)
    {
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO keeps (name, description, userId, img, isPrivate)
        VALUES(@keep.name, @keep.description, @keep.userId, @keep.img, @keep.isPrivate);
        SELECT LAST_INSERT_ID", keep);
      if (id == 0) return null;
      return GetKeepById(id);
    }
    //EDIT A KEEP
    public int UpdateKeep(Keep keep)
    {
      return _db.Execute(@"
        UPDATE keeps
        SET name=@Name, description@description, userId=@UserId, img=@Img, isPrivate=@IsPrivate,
        views=@Views, shares=@Shares, keeps=@Keeps
        WHERE id=@Id;", keep);
    }

    //DELETE A KEEP
    public int DeleteKeep(int id)
    {
      return _db.Execute("DELETE FROM keeps WHERE id = @id", new { id });
    }
  }
}