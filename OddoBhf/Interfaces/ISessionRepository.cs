using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface ISessionRepository
    {
        public ICollection<Session> GetAllSessions();
        public Task<PaginatedList<Session>> GetSessionsByUserId(int user_id, int pageIndex, int pageSize);
        public Session GetSessionById(int id);
        public void AddSession(Session session);
        public void UpdateSession(Session session);
        public void DeleteSession(int id);
        public void SaveChanges();
        //ICollection<Session> GetSessionsByUserId(int user_id);




    }
}
