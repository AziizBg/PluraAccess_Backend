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
        }

        public void DeleteSession(int id)
        {
            var session = _context.Sessions.Find(id);
            _context.Sessions.Remove(session);
        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.ToList();
        }

        public Session GetSessionById(int id)
        {
            return _context.Sessions.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
        }

        //get session by licence id
        public Session GetSessionByLicenceId(int licenceId)
        {
            return _context.Sessions.FirstOrDefault(s => s.LicenceId == licenceId);
        }






    }
}
