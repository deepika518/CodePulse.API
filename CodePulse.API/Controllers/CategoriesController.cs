using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Controllers
{
    // https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        //Post
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> createCategory([FromBody] CreateCategoryRequestDTO request)
        {
            //Map DTO to Domain Models
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);
            

            //Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        //Get: /https://localhost:7277/api/categories?query=html&sortBy=name&sortDirection=desc
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            //we talk to the DB via repositories
            var categories = await categoryRepository
                .GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);
            
            //Map Domain model to DTO (var categories is Domain model which can't be exposed to cliwent side

            var response = new List<CategoryDto>();
            foreach(var category in categories)
            {
                response.Add(new CategoryDto 
                { 
                    Id = category.Id, 
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }
            return Ok(response);
        }

        //Get: /https://localhost:7277/api/categories/{id}
        [HttpGet]
        [Route("{id:guid}")]

        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            var existingcategory = await categoryRepository.GetById(id);

            if (existingcategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDto 
            {
                Id = existingcategory.Id,
                Name = existingcategory.Name,
                UrlHandle = existingcategory.UrlHandle
            };

            return Ok(response);
        }

        //Put: https://localhost:7277/api/categories/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDTO request)
        {
            // convert DTO to Domain model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category = await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }
            //Convert Domain model to DTO

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);

        }

        //Delete: https://localhost:7277/api/categories/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeteteAsync(id);

            if(category is null)
            {
                return NotFound();
            }
            //convert domain to DTO

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        //GET: https://localhost:7277/api/categories/count
        [HttpGet]
        [Route("count")]
        //[Authorize(Roles = "Writer")]
       
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await categoryRepository.GetCount();
            return Ok(count);
        }


    }
}
