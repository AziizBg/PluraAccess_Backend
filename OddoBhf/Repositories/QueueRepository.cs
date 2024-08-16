using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;

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

        public void Delete(int id)
        {
            var queue= _context.Sessions.Find(id);
            _context.Sessions.Remove(queue);
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
