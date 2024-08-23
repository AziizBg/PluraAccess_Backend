namespace OddoBhf.Interfaces;

using OddoBhf.Dto;
using OddoBhf.Models;


public interface ILicenceRepository
{
    public ICollection<GetLicenceDto> GetAllLicences();

    public Licence GetLicenceById(int id);
    public Licence GetLicenceBookedByUserId(int userId);

    public void AddLicence(Licence licence);
    public void UpdateLicence(Licence licence);
    public void DeleteLicence(int id);
    public void SaveChanges();
}
