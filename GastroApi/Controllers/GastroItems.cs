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
using Elastic.Clients.Elasticsearch;

namespace GastroApi.Controllers
{
    [Route("api/GastroItems")]
    [ApiController]
    public class GastroItemsController : ControllerBase
    {
        private readonly QueryFactory _db;
        private readonly ElasticsearchClient _elasticClient;

        //private readonly DbGastro _context;

        public GastroItemsController(QueryFactory db, ElasticsearchClient elasticClient)//, DbGastro context)
        {
            _db = db;
            _elasticClient = elasticClient;
        }

        // GET: api/GastroItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id, [FromQuery] string? name, [FromQuery] string? description)
        {
            if (id.HasValue)
            {
                // Use PostgreSQL for id lookup
                var query = new Query("gastroitems").Where("Id", id);
                var item = await _db.FirstOrDefaultAsync<GastroItem>(query);
                return item == null ? NotFound() : Ok(new[] { item });
            }
            else if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(description))
            {
                // Use Elasticsearch for text-based search
                var searchResponse = await _elasticClient.SearchAsync<GastroItemDocument>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                sh => sh.Match(m => m.Field(f => f.DescriptionName).Query(name)),
                                sh => sh.Match(m => m.Field(f => f.Recipe).Query(description))
                            )
                        )
                    )
                );

                if (!searchResponse.IsValidResponse)
                {
                    return StatusCode(500, "Error querying Elasticsearch");
                }

                var items = searchResponse.Documents.Select(d => new GastroItem
                {
                    id = d.Id,
                    data = new JsonRaw(System.Text.Json.JsonSerializer.Serialize(d))
                });

                return Ok(items);
            }

            // If no parameters provided, return all items from PostgreSQL
            var allItems = await _db.Query("gastroitems").GetAsync<GastroItem>();
            return Ok(allItems);
        }

        // POST: api/gastroitems

[HttpPost]
public async Task<ActionResult<GastroItem>> PostGastroItem([FromBody] GastroItemCreateModel model)
{
    GastroItem item = new GastroItem
    {
        id = model.Id,
        data = new JsonRaw(model.Itemino ?? new AdditionalItem())
    };

    try
    {
        // Insert into PostgreSQL
        var result = await _db.Query("gastroitems").InsertAsync(item);

        // Index in Elasticsearch
        var esDocument = new GastroItemDocument
        {
            Id = item.id,
            DescriptionName = model.Itemino?.DescriptionName ?? string.Empty,
            Ingredients = model.Itemino?.Ingredients ?? string.Empty,
            Recipe = model.Itemino?.Recipe ?? string.Empty,
            TimeToPrepare = model.Itemino?.TimeToPrepare
        };

        var indexResponse = await _elasticClient.IndexAsync(esDocument, i => i.Index("gastroitems"));

        if (!indexResponse.IsValidResponse)
        {
            // Log the error
            //_logger.LogError($"Failed to index document in Elasticsearch: {indexResponse.DebugInformation}");
        }

        return CreatedAtAction(nameof(GetGastroItems), new { id = item.id }, item);
    }
    catch (Exception ex)
    {
        // Handle exceptions
        //_logger.LogError($"An error occurred while processing the request: {ex.Message}");
        return StatusCode(500, "An error occurred while processing your request.");
    }
}

        
  [HttpPost("PostBulkGastroItem")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<GastroItem>>> PostBulkGastroItem([FromBody]List<GastroItem> items)
    {
        if (items == null || !items.Any())
        {
            return BadRequest("No items provided for insertion.");
        }

        try
        {
            var dataToInsert = items.Select(item => new
            {
                id = item.id,
                data = new JsonRaw(JsonConvert.SerializeObject(item.data))
            }).ToList();

            int affectedRows = await _db.Query("gastroitems").InsertAsync(dataToInsert);

            return CreatedAtAction(nameof(GetGastroItems), new { count = affectedRows }, null);
        }
        catch(Exception ex)
        {
            // Log the exception
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }



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


