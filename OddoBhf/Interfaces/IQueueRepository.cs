using Microsoft.EntityFrameworkCore;
using OddoBhf.Models;

namespace OddoBhf.Interfaces
{
    public interface IQueueRepository
    {
        public void Add(Queue queue);

        public void Delete(int id);

        public ICollection<Queue> GetAll();
        public Queue GetById(int id);



        public void Update(Queue queue);

        public void SaveChanges();
    }
}
