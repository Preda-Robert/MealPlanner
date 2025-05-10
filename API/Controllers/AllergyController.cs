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
        public async Task<IActionResult> CreateAllergy(AllergyDTO allergyDTO)
        {
            var allergyServiceResult = await _unitOfServices.Service<Allergy, AllergyDTO>().Create(allergyDTO);
            return allergyServiceResult != null ? Ok(allergyServiceResult) : BadRequest("Error creating allergy");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllergies()
        {
            var allergiesServiceResult = await _unitOfServices.Service<Allergy, AllergyDTO>().GetAllAsync();
            return allergiesServiceResult != null ? Ok(allergiesServiceResult) : NotFound("No allergies found");
        }


    }
}
