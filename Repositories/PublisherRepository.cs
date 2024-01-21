using DataAccess;
using DataAccess.DTOs;
using DataAccess.DTOs.Publishers;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IPublisherRepository
    {
        IQueryable<Publisher> GetAllPublisher();
        bool DeletePublisher(int id);
        CreatePublisherResponse AddPublisher(CreatePublisherRequest publisher);
        UpdatePublisherResponse UpdatePublisher(UpdatePublisherRequest updatePublisher, int id);
    }
    public class PublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _context;

        public PublisherRepository(AppDbContext context)
        {
            _context = context;
        }

        public CreatePublisherResponse AddPublisher(CreatePublisherRequest publisher)
        {
            try
            {
                var requestPublisher = new Publisher
                {
                    City = publisher.City,
                    Country = publisher.Country,
                    PublisherName = publisher.PublisherName,
                    State = publisher.State,
                };

                _context.Publishers.Add(requestPublisher);

                _context.SaveChanges();

                return new CreatePublisherResponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString(),
                };
            }
            catch (Exception e)
            {
                return new CreatePublisherResponse
                {
                    IsSuccess = true,
                    Status = e.Message,
                };
            }
        }

        public bool DeletePublisher(int id)
        {
            var publisher = _context.Publishers.FirstOrDefault(c => c.PubId == id);
            if (publisher == null)
            {
                return false;
            }
            _context.Publishers.Remove(publisher);

            _context.SaveChanges();

            return true;
        }

        public IQueryable<Publisher> GetAllPublisher()
        {
            return _context.Publishers.AsQueryable();
        }

        public UpdatePublisherResponse UpdatePublisher(UpdatePublisherRequest updatePublisher, int id)
        {
            try
            {
                var publisher = _context.Publishers.FirstOrDefault(s => s.PubId == id);
                if (publisher == null)
                {
                    return new UpdatePublisherResponse
                    {
                        IsSuccess = false,
                        Status = StatusEnum.Failure.ToString(),
                    };
                }

                publisher.City = updatePublisher.City;
                publisher.Country = updatePublisher.Country;
                publisher.PublisherName = updatePublisher.PublisherName;
                publisher.State = updatePublisher.State;

                _context.Update(publisher);
                _context.SaveChanges();

                return new UpdatePublisherResponse
                {
                    IsSuccess = true,
                    Status = StatusEnum.Success.ToString(),
                };
            }
            catch (Exception e)
            {
                return new UpdatePublisherResponse
                {
                    IsSuccess = false,
                    Status = StatusEnum.Failure.ToString() + e.Message,
                }; 
            }
        }
    }
}
