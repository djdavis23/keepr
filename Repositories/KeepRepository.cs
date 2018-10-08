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

    //GET 40 MOST RECENT KEEPS
    //DO NOT RETURN PRIVATE KEEPS BY ANOTHER USER
    public IEnumerable<Keep> GetKeeps(string userId)
    {
      return _db.Query<Keep>(@"
      SELECT *
      FROM keeps
      WHERE userId=@userId OR isPrivate=0
      ORDER BY id DESC
      LIMIT 40;", new { userId });
    }

    //GET ADDITIONAL 40 KEEPS
    //DO NOT RETURN PRIVATE KEEPS BY ANOTHER USER
    public IEnumerable<Keep> GetMoreKeeps(int id, string userId)
    {
      return _db.Query<Keep>(@"
      SELECT *
      FROM keeps
      WHERE id < @id AND (userId=@userId OR isPrivate=0)      
      ORDER BY id DESC
      LIMIT 40;", new { id, userId });
    }

    //GET KEEP BY ID
    public Keep GetById(int id)
    {
      return _db.Query<Keep>(@"
        SELECT * FROM keeps
        WHERE id = @id;", new { id }).FirstOrDefault();
    }

    //POST A NEW KEEP
    public Keep Create(KeepForm keep)
    {
      int id = _db.ExecuteScalar<int>(@"
        INSERT INTO keeps (name, description, userId, img, isPrivate)
        VALUES(@Name, @Description, @UserId, @Img, @IsPrivate);
        SELECT LAST_INSERT_ID();", keep);
      if (id == 0) return null;

      return GetById(id);
    }

    //EDIT A KEEP
    public int Update(Keep keep, string userId)
    {
      string sqlStmt;
      //only the post author can edit all fields
      if (keep.UserId == userId)
      {
        sqlStmt = @"
        UPDATE keeps
        SET name=@Name, description=@description, userId=@UserId, img=@Img, isPrivate=@IsPrivate,
        views=@Views, shares=@Shares, keeps=@Keeps
        WHERE id=@Id;";
      }
      else
      {
        sqlStmt = @"
        UPDATE keeps
        SET views=@Views, shares=@Shares, keeps=@Keeps
        WHERE id=@Id;";
      }
      return _db.Execute(sqlStmt, keep);
    }

    //DELETE A KEEP
    public int Delete(int id)
    {
      return _db.Execute("DELETE FROM keeps WHERE id = @id", new { id });
    }
  }
}