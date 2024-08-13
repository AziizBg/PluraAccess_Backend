using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Services
{
    public class SessionService:ISessionService
    {

        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public ICollection<Session> GetAllSessions()
        {
            return _sessionRepository.GetAllSessions().ToList();
        }
        public Session GetSessionById(int id)
        {
            return _sessionRepository.GetSessionById(id);
        }
        public async Task<ActionResult<PaginatedList<Session>>> GetSessionsByUserId(int user_id, int pageIndex = 1, int pageSize = 10)
        {
            return await _sessionRepository.GetSessionsByUserId(user_id, pageIndex, pageSize);
        }
        public void AddSession( Session session)
        {
            _sessionRepository.AddSession(session);
        }
        public void UpdateSession( Session session)
        {
            _sessionRepository.UpdateSession(session);

        }
        public void DeleteSession(int id)
        {
            _sessionRepository.DeleteSession(id);
        }

    }
}
