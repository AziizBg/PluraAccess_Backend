using Microsoft.AspNetCore.Mvc;
using OddoBhf.Models;


namespace OddoBhf.Interfaces
{
     public interface ISessionService
    {
         public ICollection<Session> GetAllSessions();
         public Session GetSessionById(int id);
         public Task<ActionResult<PaginatedList<Session>>> GetSessionsByUserId(int user_id, int pageIndex = 1, int pageSize = 10);
         public void AddSession(Session session);
         public void UpdateSession(Session session);
         public void ExtendSession(Session session);
         public void DeleteSession(int id);

    }
}
