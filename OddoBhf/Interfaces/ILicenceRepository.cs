namespace OddoBhf.Interfaces;

using OddoBhf.Dto;
using OddoBhf.Models;


public interface ILicenceRepository
{
    ICollection<GetLicenceDto> GetAllLicences();

    Licence GetLicenceById(int id);
    Licence GetLicenceBookedByUserId(int userId);

    void AddLicence(Licence licence);
    void UpdateLicence(Licence licence);
    void DeleteLicence(int id);
    public void SaveChanges();
}
