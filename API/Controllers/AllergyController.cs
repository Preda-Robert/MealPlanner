using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AllergyController : BaseAPIController
    {
        private readonly IUnitOfServices _unitOfServices;

        public AllergyController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpPost]
        public async Task<ActionResult<AllergyDTO>> CreateAllergy(AllergyDTO allergyDTO)
        {
            var allergyServiceResult = await _unitOfServices.AllergyService.Create(allergyDTO);
            return allergyServiceResult;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AllergyDTO>>> GetAllergies()
        {
            var allergiesServiceResult = await _unitOfServices.AllergyService.GetAllAsync();
            return allergiesServiceResult;
        }


    }
}
