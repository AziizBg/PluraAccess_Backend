namespace OddoBhf.Interfaces;
using OddoBhf.Models;


public interface ILicenceRepository
{
    ICollection<Licence> GetAllLicences();

    Licence GetLicenceById(int id);
    void AddLicence(Licence licence);
    void UpdateLicence(Licence licence);
    void DeleteLicence(int id);
}
