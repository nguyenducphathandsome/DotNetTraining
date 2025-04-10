using System.Data;
using BPMaster.Domains.Entities;
using Common.Databases;
using Common.Repositories;
using Dapper;
using Dapper.Contrib.Extensions;
using DocumentFormat.OpenXml.Spreadsheet;
using DotNetTraining.Domains.Entities;
using iText.StyledXmlParser.Jsoup.Select;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNetTraining.Repositories
{
    public class UserRepository(IDbConnection connection) : SimpleCrudRepository<User, Guid>(connection)
    {
        public async Task<List<User>> GetAllUsers()
        {
            var sql = SqlCommandHelper.GetSelectSql<User>();
            var result = await connection.QueryAsync<User>(sql);
            return result.ToList();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var param = new { Id = id };
            var sql = SqlCommandHelper.GetSelectSqlWithCondition<User>(new { Id = id });
            return await GetOneByConditionAsync(sql, param);

        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var sql = "SELECT * FROM users WHERE email = @Email";
                var parameters = new { Email = email };
                return await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetUserByEmail: {e.Message}");
                return null;
            }
        }

        public async Task<User?> Create(User user)
        {
            return await CreateAsync(user);
        }

        public async Task<User?> UpdateUser(User user)
        {
            return await UpdateAsync(user);
        }

        public async Task DeleteUser(User user)
        {
            await DeleteAsync(user);
        }
        public async Task<User?> GetUserRoleByUserID(Guid userId)
        {
            try
            {
                var sql = "SELECT * FROM users WHERE id = @UserId";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = userId });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetByUsernameAsync: {e.Message}");
                return null;
            }
        }

        public async Task<int> CountUsers()
        {
            var query = "SELECT COUNT(*) FROM Users";
            return await _connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<List<User>> GetUsersWithPagination(int offset, int pageSize)
        {
            var query = @"
            SELECT * FROM Users
            ORDER BY CreatedAt DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            return (await _connection.QueryAsync<User>(query, new { Offset = offset, PageSize = pageSize })).ToList();
        }

    }
}
