using OddoBhf.Data;
using OddoBhf.Interfaces;
using OddoBhf.Models;

namespace OddoBhf.Repositories
{
    public class LicenceRepository: ILicenceRepository
    {
        public readonly DataContext _context;

        public LicenceRepository(DataContext context)
        {
            _context = context;
        }

        public void AddLicence(Licence licence)
        {
            _context.Licences.Add(licence);
        }

        public void DeleteLicence(int id)
        {
            var licence = _context.Licences.Find(id);
            _context.Licences.Remove(licence);
        }

        public ICollection<Licence> GetAllLicences()
        {
            Console.WriteLine(_context.Licences.ToList());
            return _context.Licences.ToList();
        }

        public Licence GetLicenceById(int id)
        {
            return _context.Licences.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateLicence(Licence licence)
        {
            _context.Licences.Update(licence);
        }








    }
}
