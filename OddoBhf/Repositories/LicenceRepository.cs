using Microsoft.EntityFrameworkCore;
using OddoBhf.Data;
using OddoBhf.Dto;
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
            _context.SaveChanges();

        }

        public void DeleteLicence(int id)
        {
            var licence = _context.Licences.Find(id);
            _context.Licences.Remove(licence);
            _context.SaveChanges();

        }

        public ICollection<GetLicenceDto> GetAllLicences()
        {
            return _context.Licences.Include(l=>l.CurrentSession).ThenInclude(s=>s.User)
                .Select(l=> new GetLicenceDto
                {
                    Id = l.Id,
                    CurrentSession = l.CurrentSession,
                    Email = l.Email,
                    BookedByUserId = l.BookedByUserId,
                    BookedUntil = l.BookedUntil,
                })
                .ToList();
        }

        public Licence GetLicenceById(int id)
        {
            return _context.Licences.Include(l => l.CurrentSession).First(l => l.Id == id);
        }

        public void UpdateLicence(Licence licence)
        {
            _context.Licences.Update(licence);
            _context.SaveChanges();

        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }








    }
}
