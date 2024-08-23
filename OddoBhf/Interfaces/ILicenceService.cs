using Microsoft.AspNetCore.Mvc;
using OddoBhf.Dto;
using OddoBhf.Models;
using OddoBhf.Repositories;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace OddoBhf.Interfaces
{
     public interface ILicenceService
    {
         public Licence GetLicenceById(int id);
         public Licence GetLicenceBookedByUserId(int id);

         public ICollection<GetLicenceDto> GetLicences();
         public void CreateLicence([FromBody] Licence licence);

         public void UpdateLicence(Licence licence);
        public Session ExtendLicence(int id);

         public Task<Licence> TakeLicence(int id, OpenPluralsightDto dto);
          public Task<Session> ReturnLicence(int id, ReturnLicenceDto dto);
          public Task<Licence> CancelRequestLicence(int id);

         public void DeleteLicence(int id);
    }
}
