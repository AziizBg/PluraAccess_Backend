using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Repositories
{
    public class SessionRepository:ISessionRepository
    {
        private readonly DataContext _context;

        public SessionRepository(DataContext context)
        {
            _context = context;
        }

        public void AddSession(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();

        }

        public void DeleteSession(int id)
        {
            var session = _context.Sessions.Find(id);
            _context.Sessions.Remove(session);
        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.Include(s => s.User).Include(s=>s.Licence).ToList();  
        }

        public Session GetSessionById(int id)
        {
            return _context.Sessions.Include(s => s.User).Include(s => s.Licence).First(s => s.Id == id);
        }


        public void UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            _context.SaveChanges();

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }







    }
}
