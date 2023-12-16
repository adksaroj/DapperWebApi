using Dapper;
using DapperWebApi.Data;
using DapperWebApi.Models;
using System.Text;

namespace DapperWebApi.Repo
{
    public interface IVillaRepo
    {
        Task<int> Create(Villa villa);
        Task<int> Delete(int id);
        Task<Villa> Get(int id);
        Task<List<Villa>> GetAll();
        Task<int> Update(int id, Villa villa);
    }

    public class VillaRepo : IVillaRepo
    {
        private readonly DapperDbContext _dbContext;

        public VillaRepo(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Villa>> GetAll()
        {
            var query = "SELECT * FROM [dbo].[Villas]";

            return (await _dbContext.GetDbConnection().QueryAsync<Villa>(query)).ToList();
        }


        public async Task<Villa> Get( int id)
        {
            var query = "SELECT * FROM [dbo].[Villas] WHERE Id = @id";

            return await _dbContext.GetDbConnection().QuerySingleOrDefaultAsync<Villa>(query, new {id});
        }

        public async Task<int> Create(Villa villa)
        {
            StringBuilder columnsSb = new();
            StringBuilder valuesSb = new();
            foreach (var prop in typeof(Villa).GetProperties())
            {
                if (prop.Name == "Id") continue;
                columnsSb.Append($"[{prop.Name}],");
                valuesSb.Append($"@{prop.Name},");
            }
            var query = $"INSERT INTO [dbo].[Villas] ({columnsSb.ToString().TrimEnd(',')}) VALUES ({valuesSb.ToString().TrimEnd(',')})";

            return await _dbContext.GetDbConnection().ExecuteAsync(query, villa);
        }

        public async Task<int> Update(int id,Villa villa)
        {
            villa.Id = id;
            var updatePartSb = new StringBuilder();
            foreach (var prop in typeof(Villa).GetProperties())
            {
                if (prop.Name == "Id") continue;
                updatePartSb.Append(prop.Name).Append($" = @{prop.Name},");
            }

            var query = $"UPDATE [dbo].[Villas] SET {updatePartSb.ToString().TrimEnd(',')} WHERE Id = @id";
            return await _dbContext.GetDbConnection().ExecuteAsync(query, villa);
        }

        public async Task<int> Delete(int id)
        {
            var query = "DELETE FROM [dbo].[Villas] WHERE Id = @id";
            return await _dbContext.GetDbConnection().ExecuteAsync(query, new {id});
        }
    }

}
