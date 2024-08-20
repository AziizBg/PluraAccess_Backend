using Microsoft.EntityFrameworkCore;
using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface IQueueRepository
    {
        public void Add(Queue queue);

        public bool IsUserInQueue(int userId);
        public int GetPosition(int userId);
        public Queue GetFirst();
        public void Delete(int id);
        public ICollection<Queue> GetAll();
        public Queue GetById(int id);
        public Queue GetByUserId(int id);

        public void Update(Queue queue);

        public void SaveChanges();
    }
}
