using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;
using System.Numerics;

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
            _context.SaveChanges();

        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.Include(s => s.User).Include(s=>s.Licence).ToList();
        }

        public async Task<PaginatedList<Session>> GetSessionsByUserId(int user_id, int pageIndex, int pageSize)
        {
            var sessions = await _context.Sessions
                .Include(s => s.User)
                .Where(s => s.User.Id == user_id)
                .OrderByDescending(b => b.EndTime)
                .Skip(pageIndex  * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var length = await _context.Sessions.Include(s => s.User)
                .Where(s => s.User.Id == user_id).CountAsync();
            var totalPages = (int)Math.Ceiling(length / (double)pageSize);

            return new PaginatedList<Session>(sessions, pageIndex, totalPages, length);
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
