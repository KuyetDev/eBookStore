using DataAccess;
using DataAccess.DTOs;
using DataAccess.DTOs.Authors;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAuthorRepository
    {
        IQueryable<Author> GetAuthors();
        CreateAuthorResponse AddAuthor(CreateAuthorRequest author);
        UpdateAuthorResponse UpdateAuthor(UpdateAuthorRequest updateAuthor, int id);
        bool DeleteAuthor(int id);
    }
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public CreateAuthorResponse AddAuthor(CreateAuthorRequest author)
        {

            try
            {
                var requestAuthor = new Author
                {
                    LastName = author.LastName,
                    FirstName = author.FirstName,
                    Address = author.Address,
                    Phone = author.Phone,
                    City = author.City,
                    State = author.State,
                    Email = author.Email,
                    Zip = author.Zip
                };

                _context.Authors.Add(requestAuthor);

                _context.SaveChanges();

                return new CreateAuthorResponse
                {
                    IsSuccess = true,
                    Status = "Success"
                };
            }
            catch (Exception e)
            {
                return new CreateAuthorResponse
                {
                    IsSuccess = true,
                    Status = e.ToString(),
                };
            }
        }

        public bool DeleteAuthor(int id)
        {
            var author = _context.Authors.FirstOrDefault(s => s.Id == id);
            if (author == null)
            {
                return false;
            }
            _context.Authors.RemoveRange(author);

            var bookAuthor = _context.BookAuthors.FirstOrDefault(b => b.AuthorId == id);
            if (bookAuthor != null)
            {
                _context.BookAuthors.Remove(bookAuthor);
                _context.SaveChanges();
                return true;
            }
            _context.SaveChanges();
            return true;
        }

        public IQueryable<Author> GetAuthors()
        {
            var authors = _context.Authors.AsQueryable();
            return authors;
        }

        public UpdateAuthorResponse UpdateAuthor(UpdateAuthorRequest updateAuthor, int id)
        {
            try
            {
                var author = _context.Authors.FirstOrDefault(s => s.Id == id);
                if (author == null)
                {
                    return new UpdateAuthorResponse
                    {
                        IsSuccess = false,
                        Status = "Author is null"
                    };
                }

                author.LastName = updateAuthor.LastName;
                author.FirstName = updateAuthor.FirstName;
                author.Address = updateAuthor.Address;
                author.Phone = updateAuthor.Phone;
                author.City = updateAuthor.City;
                author.State = updateAuthor.State;
                author.Email = updateAuthor.Email;
                author.Zip = updateAuthor.Zip;


                _context.Authors.Update(author);
                _context.SaveChanges();

                return
                     new UpdateAuthorResponse
                     {
                         IsSuccess = true,
                         Status = StatusEnum.Success.ToString(),
                     };
            }
            catch (Exception e)
            {
                return new UpdateAuthorResponse
                {
                    IsSuccess = false,
                    Status = e.ToString()
                };
            }
        }
    }
}
