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
           // _context = context;
        }

        // GET: api/GastroItems
        [HttpGet]

        public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id, [FromQuery] string? name, [FromQuery] string? description)
        {
            var query = new Query("gastroitems");

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

        // POST: api/gastroitems
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<GastroItem>> PostGastroItem(long id,  [FromBody] AdditionalItem? itemino)
        {
            //var jsondata = additionalItem != null ? JsonConvert.SerializeObject(additionalItem): null;

            GastroItem item = new GastroItem
            {
                Id = id,
                Data = new JsonRaw(itemino)

            };

            // _context.gastroitems.Add(item);
            // await _context.SaveChangesAsync();

            var result = await _db.Query("gastroitems").InsertAsync(item);

            return CreatedAtAction(nameof(GetGastroItems), new { id = item.Id }, item);
        }

        // Other CRUD actions can be added here (PutGastroItem, DeleteGastroItem, etc.)


        [HttpPut("{id}")]
        public async Task<ActionResult<GastroItem>> PutGastroItem(long id, [FromBody] AdditionalItem? itemino)
        {


            Dictionary<string, object> MapItem = NotNullItems(itemino);

            try
            {
                var affectedRows = await _db.Query("gastroitems").Where("Id", id).UpdateAsync(MapItem);

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
            var query = new Query("gastroitems").Where("Id", id);

            try
            {
                var catchItem = await _db.FirstOrDefaultAsync<GastroItem>(query);

                if (catchItem is null)
                {
                    return NotFound();
                }
                _db.Query("gastroitems").Where("Id", id).Delete();
                return NoContent();
            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}

