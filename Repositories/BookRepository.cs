using DataAccess;
using DataAccess.DTOs;
using DataAccess.DTOs.Books;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAllBook();
        bool DeleteBook(int id);
        CreateBookReponse AddBook(CreateBookRequest book);
        UpdateBookResponse UpdateBook(UpdateBookRequest updateBook, int id);
    }
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public CreateBookReponse AddBook(CreateBookRequest book)
        {
            try
            {
                var requestBook = new Book
                {
                    Title = book.Title,
                    Type = book.Type,
                    Notes = book.Notes,
                    Price = book.Price,
                    Royalty = book.Royalty,
                    YtdSales = book.YtdSales,
                    Advance = book.Advance,
                    PubId = book.PubId,
                    PublishedDate = book.PublishedDate
                };
                _context.Books.Add(requestBook);
                _context.SaveChanges();
                var author = _context.Authors.FirstOrDefault(s => s.Id == book.AuthorId);
                if (author == null)
                {
                    return new CreateBookReponse
                    {
                        IsSuccess = false,
                        Status = StatusEnum.Success.ToString()
                    };
                }

                {
                    var newBookAuthor = new BookAuthor
                    {
                        BookId = requestBook.Id,
                        AuthorId = book.AuthorId,
                        AuthorOrder = "",
                        RoyalityPercentage = 0,
                    };
                    _context.BookAuthors.Add(newBookAuthor);
                }
                _context.SaveChanges();

                return new CreateBookReponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString()
                };
            }
            catch (Exception e)
            {
                return new CreateBookReponse
                {
                    IsSuccess = false,
                    Status = StatusEnum.Failure.ToString() + e.ToString(),
                };
            }
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);

            _context.SaveChanges();

            return true;
        }

        public IQueryable<Book> GetAllBook()
        {   
            return _context.Books.AsQueryable().Include(x=>x.Publisher);
        }

        public UpdateBookResponse UpdateBook(UpdateBookRequest updateBook, int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(s => s.Id == id);
                if (book == null)
                {
                    return new UpdateBookResponse
                    {
                        IsSuccess = false,
                        Status = StatusEnum.Failure.ToString()  
                    };
                }

                book.Title = updateBook.Title;
                book.Price = updateBook.Price;
                book.Advance = updateBook.Advance;
                book.Notes = updateBook.Notes;
                book.Royalty = updateBook.Royalty;
                book.YtdSales = updateBook.YtdSales;
                book.PublishedDate = updateBook.PublishedDate;
                book.Type = updateBook.Type;
                book.PubId = updateBook.PubId;

                var newBook = _context.Books.Update(book);
                _context.SaveChanges();

                var bookAuthor = _context.BookAuthors.FirstOrDefault(s => s.BookId == id);

                if (bookAuthor != null)
                {
                    bookAuthor.AuthorId = updateBook.authorId;
                    bookAuthor.AuthorOrder = string.Empty;
                    bookAuthor.RoyalityPercentage = 0;

                    _context.Update(bookAuthor);
                }

                _context.SaveChanges();

                return new UpdateBookResponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString()
                };
            }
            catch (Exception e)
            {
                return new UpdateBookResponse
                {
                    IsSuccess = false,
                    Status = e.Message,
                }; 
            }
        }
    }
}
