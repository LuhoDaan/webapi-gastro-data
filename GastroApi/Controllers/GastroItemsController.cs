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
using System.Text.Json;
using GastroApi.Models;
using Newtonsoft.Json;

namespace GastroApi.Controllers
{
    [Route("api/GastroItems")]
    [ApiController]
    public class GastroItemsController : ControllerBase
    {
        private readonly QueryFactory _db;

        //private readonly DbGastro _context;

        public GastroItemsController(QueryFactory db)//, DbGastro context)
        {
            _db = db;
        }

        // GET: api/GastroItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id, [FromQuery] string? description, [FromQuery] string? recipe)
        {

            if (id.HasValue)
            {
                // Use PostgreSQL for id lookup
                var query = new Query("gastroitems").Where("id", id);
                var item = await _db.FirstOrDefaultAsync<GastroItem>(query);
                return item == null ? NotFound() : Ok(new[] { item });
            }
            else if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(recipe))
            {
                 var firstquery = new Query("gastroitems").WhereRaw("((data ->> 'DescriptionName') :: text ILIKE ? OR  (data ->> 'Recipe') :: text ILIKE ? )", $"%{description}%", $"%{recipe}%");
                 var items = await _db.GetAsync(firstquery);
                 return !items.Any() ? NotFound() : Ok(items);
            }
            else if (!string.IsNullOrEmpty(description) && string.IsNullOrEmpty(recipe))
            {
                 var firstquery = new Query("gastroitems").WhereRaw("(data ->> 'DescriptionName') :: text ILIKE ? ", $"%{description}%");
                 var items = await _db.GetAsync(firstquery);
                 return !items.Any() ? NotFound() : Ok(items);
            }

            else if (!string.IsNullOrEmpty(recipe) && string.IsNullOrEmpty(description))
            {
                 var firstquery = new Query("gastroitems").WhereRaw("(data ->> 'Recipe') :: text ILIKE ? ", $"%{recipe}%");
                 var items = await _db.GetAsync(firstquery);
                 return !items.Any() ? NotFound() : Ok(items);
            }

            else{

            // If no parameters provided, return all items from PostgreSQL
            var allItems = await _db.Query("gastroitems").GetAsync<GastroItem>();
            return Ok(allItems);
        }}

        // POST: api/gastroitems



        [HttpPut("{id}")]
        public async Task<ActionResult<GastroItem>> PutGastroItem(long id, [FromBody] AdditionalItem? itemino)
        {


            Dictionary<string, object> MapItem = NotNullItems(itemino);

            try
            {
                var affectedRows = await _db.Query("gastroitems").Where("id", id).UpdateAsync(MapItem);

                if (affectedRows == 0)
                {
                    return NotFound();
                }


            }
            catch (Exception e)
            {

                throw;
            }


            return NoContent();

        }

        private Dictionary<string, object> NotNullItems(AdditionalItem? itemino)
        {

            Dictionary<string, object> map = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(itemino.DescriptionName)) map["DescriptionName"] = itemino.DescriptionName;
            if (!string.IsNullOrEmpty(itemino.Ingredients)) map["Ingredients"] = itemino.Ingredients;
            if (!string.IsNullOrEmpty(itemino.Recipe)) map["Recipe"] = itemino.Recipe;
            if (itemino.TimeToPrepare.HasValue) map["TimeToPrepare"] = itemino.TimeToPrepare;


            return map;

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGastroItem(long id)
        {
            var query = new Query("gastroitems").Where("id", id);

            try
            {
                var catchItem = await _db.FirstOrDefaultAsync<GastroItem>(query);

                if (catchItem is null)
                {
                    return NotFound();
                }
                _db.Query("gastroitems").Where("id", id).Delete();
                return NoContent();
            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}


