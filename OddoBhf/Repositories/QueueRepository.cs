using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Diagnostics.Eventing.Reader;

namespace OddoBhf.Repositories
{
    public class QueueRepository:IQueueRepository
    {
        private readonly DataContext _context;

        public QueueRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Queue queue)
        {

            _context.Queue.Add(queue);
            _context.SaveChanges();

        }

        public bool IsUserInQueue(int userId)
        {
            return _context.Queue.Any(q=>q.User.Id == userId);
        }
        public int GetPosition(int userId)
        {
            var queue = _context.Queue.Include(q=>q.User).OrderBy(q => q.RequestedAt).ToList();
            var position = queue
                .Select((q, index) => new { q.User.Id, Position = index + 1 })
                .FirstOrDefault(s => s.Id == userId)?.Position;
            if (position != null)
                return (int) position;
            return 0;
        
        }

        public void Delete(int id)
        {
            var queue= _context.Queue.Find(id);
            _context.Queue.Remove(queue);
            _context.SaveChanges();

        }

        public ICollection<Queue> GetAll()
        {
            return _context.Queue.Include(q=>q.User).ToList();
        }
        public Queue GetById(int id)
        {
            return _context.Queue.Include(q => q.User).First(s => s.Id == id);
        }
        public Queue GetByUserId(int id)
        {
            return _context.Queue.Include(q => q.User).First(s => s.User.Id == id);
        }


        public void Update(Queue queue)
        {
            _context.Queue.Update(queue);
            _context.SaveChanges();

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
