using Application.Settings;
using System.Text;
using AutoMapper;
using Common.Application.CustomAttributes;
using Common.Services;
using DotNetTraining.Domains.Dtos;
using DotNetTraining.Domains.Models;
using DotNetTraining.Domains.Entities;
using DotNetTraining.Repositories;
using Newtonsoft.Json;
using System.Data;
using iText.Commons.Actions.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.Forms.Fields.Merging;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.AspNetCore.Identity;
using Common.Application.Exceptions;
using Common.Application.Models;
using Common.Application.Settings;
using Common.Utilities;
using Microsoft.Extensions.Configuration;
using Utilities;
using DotNetTraining.Requests;
using Domain.Enums;
using Microsoft.Extensions.Options;
using DotNetTraining.Common.Application.Models;
namespace DotNetTraining.Services
{
  
    [ScopedService]
    public class UserService(IServiceProvider services, ApplicationSetting setting, IDbConnection connection, IConfiguration configuration, IOptions<JwtTokenSetting> jwtOptions) : BaseService(services)
    {
        private readonly UserRepository _repo = new(connection);
        private readonly IConfiguration _configuration = configuration;
        private readonly JwtTokenSetting _jwtTokenSetting = jwtOptions.Value;

        public async Task<(List<UserModel>, PaginationModel)> GetAllUsers(int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            // Lấy tổng số người dùng
            var totalUsers = await _repo.CountUsers();

            // Lấy danh sách người dùng với phân trang
            var users = await _repo.GetUsersWithPagination(offset, pageSize);

            // Chuyển đổi dữ liệu sang UserModel
            var result = _mapper.Map<List<UserModel>>(users);

            // Tạo đối tượng phân trang
            var pagination = new PaginationModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalUsers
            };

            return (result, pagination);
        }


        public async Task<UserModel?> GetUserById(Guid userId)
        {
            var existingUser = await _repo.GetUserById(userId);
            if (existingUser == null)
            {
                throw new Exception("user not exist");
            }
            // map entity to Dto
            var dto = _mapper.Map<UserModel>(existingUser);

            return dto;

        }

        public async Task<User?> UpdateUser(Guid userId, UserDto userDto)
        {
            var existingUser = await _repo.GetUserById(userId);
            if (existingUser == null)   
            {
                throw new Exception(" id not found"); // User không tồn tại
            }

            var hasher = new HashingWithKeyService(_configuration);
            userDto.Password = hasher.HashPassword(userDto.Password);
            var user = _mapper.Map(userDto, existingUser); 

            var updatedUser = await _repo.UpdateUser(user);

            return user;
        }
        public async Task DeleteUser(Guid userId)
        {
            var existingUser = await _repo.GetUserById(userId);

            if (existingUser == null)
            {
                throw new Exception("user not exist"); // User không tồn tại
            }

            await _repo.DeleteUser(existingUser);
        }

        public async Task<User?> CreateUser(UserDto newUser)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _repo.GetUserByEmail(newUser.Email);
            if (existingUser != null)
            {
                throw new Exception("email đã tồn tại"); // Email đã tồn tại
            }
            // Tạo đối tượng User
            var user = _mapper.Map<User>(newUser);
            user.Id = Guid.NewGuid();

            var hasher = new HashingWithKeyService(_configuration);
            user.Password = hasher.HashPassword(newUser.Password);
            // Gọi repository để lưu vào DB
            return await _repo.Create(user);
        }

        public async Task<string> AuthenticateAsync(LoginRequest request)
        {
            var user = await _repo.GetUserByEmail(request.Email);
            if (user == null || user.Status == UserStatus.Deleted)
            {
                throw new NonAuthenticateException("The account does not exist in the system. Please contact the admin to have the account added.");
            }

            if (user.Status != UserStatus.Active)
            {
                throw new NonAuthenticateException("Account is not active. Please contact the administrator.");
            }

            var hashingService = new HashingWithKeyService(_configuration);
            if (hashingService.VerifyPassword(user.Password, request.Password))
            {
                user.LastLoggedIn = DateTime.Now;
                try
                {
                    await _repo.UpdateAsync(user);
                    var authenticatedUser = _mapper.Map<AuthenticatedUserModel>(user);

                    var userRole = await _repo.GetUserRoleByUserID(user.Id); // role của user
                    var rolestring = userRole.ToString();

                    return JwtUtil.CreateJwtToken(_jwtTokenSetting, authenticatedUser, rolestring);
                }
                catch (Exception)
                {
                    return null!;
                }
            }
            throw new NonAuthenticateException();
        }


    }
}
