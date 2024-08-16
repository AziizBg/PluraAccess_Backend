using OddoBhf.Models;
using OddoBhf.Repositories;

namespace OddoBhf.Interfaces
{
    public interface IQueueService
    {
        public ICollection<Queue> GetAll();
        public Queue GetById(int id);

        public void Add(Queue queue);
        public void Update(Queue queue);
        
        public void Delete(int id);
    }
}
