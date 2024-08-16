using Microsoft.AspNetCore.Mvc;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Services
{
    public class QueueService:IQueueService
    {
        private readonly IQueueRepository _queueRepository;

        public QueueService(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public ICollection<Queue> GetAll()
        {
            return _queueRepository.GetAll().ToList();
        }
        public Queue GetById(int id)
        {
            return _queueRepository.GetById(id);
        }

        public void Add(Queue queue)
        {
            _queueRepository.Add(queue);
        }
        public void Update(Queue queue)
        {
            _queueRepository.Update(queue);

        }
        public void Delete(int id)
        {
            _queueRepository.Delete(id);
        }
    }
}
