using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebAPIApp6AM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        Efcore6amContext _db;
        public CategoryController(Efcore6amContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetProducts", Summary = "Return a list of Product")]
        public IEnumerable<Category> GetCategories()
        {
            return _db.Categories;
        }
    }
}
