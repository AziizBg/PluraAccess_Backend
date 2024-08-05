using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface ISessionRepository
    {
        ICollection<Session> GetAllSessions();
        Session GetSessionById(int id);
        void AddSession(Session session);
        void UpdateSession(Session session);
        void DeleteSession(int id);
        void SaveChanges();
        ICollection<Session> GetSessionsByUserId(int user_id);



    }
}
