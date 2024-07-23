using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
using SqlKata.Execution;
using SqlKata;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;


namespace GastroApi.Controllers
{
    [Route("api/GastroItems")]
    [ApiController]
    public class GastroItemsController : ControllerBase
    {
        private readonly QueryFactory _db;

        public GastroItemsController(QueryFactory db)
        {
            _db = db;
        }

        // GET: api/GastroItems
        [HttpGet]

        public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id, [FromQuery] string? name, [FromQuery] string? description)
        {
            var query = new Query("GastroItems");

            if (id.HasValue)
            {

                query = query.Where("Id", id);

            }
            else if (!name.IsNullOrEmpty())
            {
                query = query.WhereLike("DescriptionName", name);
            }
            else if (!description.IsNullOrEmpty())
            {
                query = query.WhereLike("Recipe", description);
            }

            var items = await _db.GetAsync<GastroItem>(query);
            if (items == null || !items.Any())
            {
                return NotFound();

            }
            else
            {
                return Ok(items);
            }
        }

        // POST: api/GastroItems
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<GastroItem>> PostGastroItem(long id, string name, string ingredients, string recipe, int time)
        {
            GastroItem item = new GastroItem
            {
                Id = id,
                DescriptionName = name,
                Ingredients = ingredients,
                Recipe = recipe,
                TimeToPrepare = time
            };


            var query = await _db.Query("GastroItems").InsertAsync(item);


            return CreatedAtAction(nameof(GetGastroItems), new { id = item.Id }, item);
        }

        // Other CRUD actions can be added here (PutGastroItem, DeleteGastroItem, etc.)


        [HttpPut("{id}")]
        public async Task<ActionResult<GastroItem>> PutGastroItem(long id, [FromQuery] string? name, [FromQuery] string? ingredients, [FromQuery] string? recipe, [FromQuery] int? time)
        {
            GastroItem item = new GastroItem
            {
                DescriptionName = name,
                Ingredients = ingredients,
                Recipe = recipe,
                TimeToPrepare = time
            };

            Dictionary<string, object> MapItem = NotNullItems(item);

            try
            {
                var affectedRows = await _db.Query("GastroItems").Where("Id", id).UpdateAsync(MapItem);

                if (affectedRows == 0)
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {

                throw;
            }


            return NoContent();

        }

        private Dictionary<string, object> NotNullItems(GastroItem gastroItem)
        {

            Dictionary<string, object> map = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(gastroItem.DescriptionName)) map["DescriptionName"] = gastroItem.DescriptionName;
            if (!string.IsNullOrEmpty(gastroItem.Ingredients)) map["Ingredients"] = gastroItem.Ingredients;
            if (!string.IsNullOrEmpty(gastroItem.Recipe)) map["Recipe"] = gastroItem.Recipe;
            if (gastroItem.TimeToPrepare.HasValue) map["TimeToPrepare"] = gastroItem.TimeToPrepare;


            return map;

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGastroItem(long id)
        {
            var query = new Query("GastroItems").Where("Id", id);

            try
            {
                var catchItem = await _db.FirstOrDefaultAsync<GastroItem>(query);

                if (catchItem is null)
                {
                    return NotFound();
                }
                _db.Query("GastroItems").Where("Id", id).Delete();
                return NoContent();
            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}

