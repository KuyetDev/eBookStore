using DataAccess;
using DataAccess.DTOs;
using DataAccess.DTOs.User;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetAllUser();
        bool DeleteUser(int id);
        CreateUserResponse AddUser(CreateUserRequest user);
        UpdateUserResponse UpdateUser(UpdateUserRequest updateUser, int id);
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public CreateUserResponse AddUser(CreateUserRequest user)
        {
            try
            {
                var requestUser = new User
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    HireDate = user.HireDate,
                    MiddleName = user.MiddleName,
                    Password = user.Password,
                    PubId = user.PubId,
                    RoleId = user.RoleId,
                    Source = user.Source,
                };

                _context.Users.Add(requestUser);

                _context.SaveChanges();

                return new CreateUserResponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString()
                };
            }
            catch (Exception e)
            {
                return new CreateUserResponse
                {
                    IsSuccess = false,
                    Status = StatusEnum.Failure.ToString() + e.Message
                };
            }
        }

        public bool DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return false;
            }
            _context.Remove(user);

            _context.SaveChanges();

            return true;
        }

        public IQueryable<User> GetAllUser()
        {
            return _context.Users.AsQueryable().Include(x=>x.Publisher).Include(x=>x.Role);
        }

        public UpdateUserResponse UpdateUser(UpdateUserRequest updateUser, int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(s => s.Id == id);
                if (user == null)
                {
                    return new UpdateUserResponse
                    {
                         IsSuccess= false,
                         Status = StatusEnum.Failure.ToString()
                    };
                }

                user.Email = updateUser.Email;
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.HireDate = updateUser.HireDate;
                user.MiddleName = updateUser.MiddleName;
                user.PubId = updateUser.PubId;
                user.RoleId = updateUser.RoleId;
                user.Source = updateUser.Source;

                _context.Update(user);
                _context.SaveChanges();

                return new UpdateUserResponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString()  
                };
            }
            catch (Exception e)
            {
                return new UpdateUserResponse
                {
                    IsSuccess = false,
                    Status = StatusEnum.Failure.ToString() + e.Message
                }; ;
            }
        }
    }
}
