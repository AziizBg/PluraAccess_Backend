using OddoBhf.Models;
using OddoBhf.Repositories;

namespace OddoBhf.Interfaces
{
    public interface IQueueService
    {
        public ICollection<Queue> GetAll();
        public Queue GetById(int id);
        public Queue GetByUserId(int id);

        public void Add(Queue queue);
        public bool IsUserInQueue(int userId);
        public int GetPosition(int userId);


        public void Update(Queue queue);
        
        public void Delete(int id);
        public void RemoveUser(int id);

    }
}
