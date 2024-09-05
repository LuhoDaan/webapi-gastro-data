// using Microsoft.AspNetCore.Mvc;
// //using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// //using Microsoft.EntityFrameworkCore;
// using SqlKata.Execution;
// using SqlKata;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Microsoft.AspNetCore.Http.HttpResults;
// using System.Text.Json;
// using GastroApi.Models;
// using Newtonsoft.Json;
// using GastroApi.Services;

// namespace GastroApi.Controllers
// {
//     [Route("api/v0/dish")]
//     [ApiController]
//     public class DishController : ControllerBase
//     {

//         private readonly ILogger<DishController> _logger;
//         private readonly QueryFactory _db;

//         //private readonly DbGastro _context;

//         public DishController(QueryFactory db, ILogger<DishController> logger)//, DbGastro context)
//         {
//             _db = db;
//             _logger = logger;
//         }

//                 // GET: api/GastroItems
//                 [HttpGet]
//                 public async Task<ActionResult<IEnumerable<Order>>> GetDish()
//                 {
//                         // If no parameters provided, return all items from PostgreSQL
//                         var allItems = await _db.Query("Dish").GetAsync<Order>();
//                         return Ok(allItems);
//                     }
                

//         //         // POST: api/gastroitems

//         //         [HttpPost]
//         //         public async Task<ActionResult<Order>> PostGastroItem(long ext_id, [FromBody] AdditionalItem? itemino)
//         //         {
//         //             GastroItem item = new GastroItem
//         //             {
//         //                 id = id,
//         //                 data = new JsonRaw(itemino)
//         //             };

//         //             var result = await _db.Query("gastroitems").InsertAsync(item);

//         //             return CreatedAtAction(nameof(GetGastroItems), new { id = item.id }, item);
//         //         }


//         //         [HttpPost]

//         //         public async Task<ActionResult<Order>> PostOrder(long ext_id, [FromBody] OrderDto item)
//         //         {

//         //             using Microsoft.AspNetCore.Mvc;
//         //             //using System.Threading.Tasks;
//         //             using Microsoft.EntityFrameworkCore;
//         //             using Microsoft.IdentityModel.Tokens;
//         //             using Microsoft.AspNetCore.Authorization;
//         //             using Microsoft.AspNetCore.Authentication.JwtBearer;
//         //             //using Microsoft.EntityFrameworkCore;
//         //             using SqlKata.Execution;
//         //             using SqlKata;
//         //             using Microsoft.CodeAnalysis.CSharp.Syntax;
//         //             using Microsoft.AspNetCore.Http.HttpResults;
//         //             using System.Text.Json;
//         //             using GastroApi.Models;
//         //             using Newtonsoft.Json;
//         //             using GastroApi.Services;

//         // namespace GastroApi.Controllers
//         //     {
//         //         [Route("api/v0/Dish")]
//         //         [ApiController]
//         //         public class DishController : ControllerBase
//         //         {

//         //             private readonly ILogger<DishController> _logger;
//         //             private readonly QueryFactory _db;

//         //             //private readonly DbGastro _context;

//         //             public DishController(QueryFactory db, ILogger<DishController> logger)//, DbGastro context)
//         //             {
//         //                 _db = db;
//         //                 _logger = logger;
//         //             }

//         //             // GET: api/GastroItems
//         //             [HttpGet]
//         //             public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id, [FromQuery] string? description, [FromQuery] string? recipe)
//         //             {

//         //                 var query = new Query("gastroitems");

//         //                 if (id.HasValue)
//         //                 {
//         //                     // Use PostgreSQL for id lookup
//         //                     query = query.Where("id", id);
//         //                     var item = await _db.FirstOrDefaultAsync<GastroItem>(query);
//         //                     return item == null ? NotFound() : Ok(new[] { item });
//         //                 }
//         //                 else if (!string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(recipe))
//         //                 {
//         //                     var orSpec = new DescriptionOrRecipe(description, recipe);
//         //                     query = orSpec.ApplySpecification(query);
//         //                     var items = await _db.GetAsync(query);
//         //                     return !items.Any() ? NotFound() : Ok(items);
//         //                 }
//         //                 else if (!string.IsNullOrEmpty(description) && string.IsNullOrEmpty(recipe))
//         //                 {
//         //                     var descSpec = new DescriptionSpecification(description);
//         //                     query = descSpec.ApplySpecification(query);
//         //                     var items = await _db.GetAsync(query);
//         //                     return !items.Any() ? NotFound() : Ok(items);
//         //                 }

//         //                 else if (!string.IsNullOrEmpty(recipe) && string.IsNullOrEmpty(description))
//         //                 {
//         //                     var recipeSpec = new RecipeSpecification(recipe);
//         //                     query = recipeSpec.ApplySpecification(query);
//         //                     var items = await _db.GetAsync(query);
//         //                     return !items.Any() ? NotFound() : Ok(items);
//         //                 }

//         //                 else
//         //                 {

//         //                     // If no parameters provided, return all items from PostgreSQL
//         //                     var allItems = await _db.Query("gastroitems").GetAsync<GastroItem>();
//         //                     return Ok(allItems);
//         //                 }
//         //             }

//         //             // POST: api/gastroitems

//         //             [HttpPost]
//         //             public async Task<ActionResult<Order>> PostGastroItem(long ext_id, [FromBody] AdditionalItem? itemino)
//         //             {
//         //                 GastroItem item = new GastroItem
//         //                 {
//         //                     id = id,
//         //                     data = new JsonRaw(itemino)
//         //                 };

//         //                 var result = await _db.Query("gastroitems").InsertAsync(item);

//         //                 return CreatedAtAction(nameof(GetGastroItems), new { id = item.id }, item);
//         //             }


//         [HttpPost]

//         public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDto orderDto) // does it make sense here that I removed the id from the parameters ?
//         {

//             Order order = new Order
//             {
//                 Uuid = orderDto.Uuid,
//                 StartTime = DateTime.UtcNow, // set it to current time
//                 EstimatedTime = orderDto.EstimatedTime, // this value later on should be set automatically depending on queues, RKA availability etc... 
//                 OrderType = orderDto.OrderType,
//                 PreOrderTime = orderDto.PreOrderTime,
//                 OrderCategory = orderDto.OrderCategory,
//                 Priority = orderDto.Priority,
//                 Cost = orderDto.Cost,
//                 //Dishes = orderDto.Dishes, 

//                 // Set default values for properties not in OrderDto
//                 //Id = 0, // This will be set by the database
//                 CompletionTime = null,
//                 MappedStatus = Dishtatus.Preparing // Assuming new Dish start with 'Preparing' status
//             };

//             // the following is basically done since i need to insert my enums as strings in the Db and hence create another object
//             var orderForDb = new
//             {
//                 order.Uuid,
//                 order.StartTime,
//                 order.EstimatedTime,
//                 OrderType = order.OrderType.ToString(), // Explicit conversion
//                 order.PreOrderTime,
//                 order.OrderCategory,
//                 order.Priority,
//                 order.Cost,
//                 order.CompletionTime,
//                 MappedStatus = order.MappedStatus.ToString() // Explicit conversion
//             };

//             var insertedId = await _db.Query("Dish").InsertGetIdAsync<long>(orderForDb); // I've found that if I substitute orderforDb with the poper order object i get a lastval error from the db when trying to insert the entry
//             order.Id = insertedId;

//             return CreatedAtAction(nameof(GetDish),new {id = insertedId}, order);
//         }


//         //             [HttpPut("{id}")]
//         //             public async Task<ActionResult<GastroItem>> PutGastroItem(long id, [FromBody] AdditionalItem? itemino)
//         //             {


//         //                 Dictionary<string, object> MapItem = NotNullItems(itemino);
//         //                 _logger.LogInformation("Updating fields: {Fields}", string.Join(", ", MapItem.Keys));

//         //                 try
//         //                 {
//         //                     var affectedRows = await _db.Query("gastroitems").Where("id", id).UpdateAsync(MapItem);

//         //                     if (affectedRows == 0)
//         //                     {
//         //                         _logger.LogInformation("Gastro Item with id={Id} was not found", id);
//         //                         return NotFound();
//         //                     }


//         //                 }
//         //                 catch (Exception e)
//         //                 {

//         //                     throw;
//         //                 }


//         //                 return NoContent();

//         //             }

//         //             private Dictionary<string, object> NotNullItems(AdditionalItem? itemino)
//         //             {

//         //                 Dictionary<string, object> map = new Dictionary<string, object>();

//         //                 if (!string.IsNullOrEmpty(itemino.DescriptionName)) map["DescriptionName"] = itemino.DescriptionName;
//         //                 if (!string.IsNullOrEmpty(itemino.Ingredients)) map["Ingredients"] = itemino.Ingredients;
//         //                 if (!string.IsNullOrEmpty(itemino.Recipe)) map["Recipe"] = itemino.Recipe;
//         //                 if (itemino.TimeToPrepare.HasValue) map["TimeToPrepare"] = itemino.TimeToPrepare;


//         //                 return map;

//         //             }


//         //             [HttpDelete("{id}")]
//         //             public async Task<IActionResult> DeleteGastroItem(long id)
//         //             {

//         //                 try
//         //                 {
//         //                     var affectedRows = await _db.Query("gastroitems").Where("id", id).DeleteAsync();

//         //                     if (affectedRows == 0)
//         //                     {
//         //                         return NotFound();
//         //                     }
//         //                     _logger.LogInformation("Succesfully deleted Gastro Item with id = {Id}", id);
//         //                     return NoContent();
//         //                 }
//         //                 catch (Exception e)
//         //                 {
//         //                     return StatusCode(500, "An error occured while processing your request");
//         //                 }

//         //             }


//         //         }
//         //     }




//         //     var result = await _db.Query("Dish").InsertAsync(order);

//         //             return CreatedAtAction(nameof(GetGastroItems), new {id = ext_id
//         // }, order)
//         //         }


//         //         [HttpPut("{id}")]
//         // public async Task<ActionResult<GastroItem>> PutGastroItem(long id, [FromBody] AdditionalItem? itemino)
//         // {


//         //     Dictionary<string, object> MapItem = NotNullItems(itemino);
//         //     _logger.LogInformation("Updating fields: {Fields}", string.Join(", ", MapItem.Keys));

//         //     try
//         //     {
//         //         var affectedRows = await _db.Query("gastroitems").Where("id", id).UpdateAsync(MapItem);

//         //         if (affectedRows == 0)
//         //         {
//         //             _logger.LogInformation("Gastro Item with id={Id} was not found", id);
//         //             return NotFound();
//         //         }


//         //     }
//         //     catch (Exception e)
//         //     {

//         //         throw;
//         //     }


//         //     return NoContent();

//         // }

//         // private Dictionary<string, object> NotNullItems(AdditionalItem? itemino)
//         // {

//         //     Dictionary<string, object> map = new Dictionary<string, object>();

//         //     if (!string.IsNullOrEmpty(itemino.DescriptionName)) map["DescriptionName"] = itemino.DescriptionName;
//         //     if (!string.IsNullOrEmpty(itemino.Ingredients)) map["Ingredients"] = itemino.Ingredients;
//         //     if (!string.IsNullOrEmpty(itemino.Recipe)) map["Recipe"] = itemino.Recipe;
//         //     if (itemino.TimeToPrepare.HasValue) map["TimeToPrepare"] = itemino.TimeToPrepare;


//         //     return map;

//         // }


//         // [HttpDelete("{id}")]
//         // public async Task<IActionResult> DeleteGastroItem(long id)
//         // {

//         //     try
//         //     {
//         //         var affectedRows = await _db.Query("gastroitems").Where("id", id).DeleteAsync();

//         //         if (affectedRows == 0)
//         //         {
//         //             return NotFound();
//         //         }
//         //         _logger.LogInformation("Succesfully deleted Gastro Item with id = {Id}", id);
//         //         return NoContent();
//         //     }
//         //     catch (Exception e)
//         //     {
//         //         return StatusCode(500, "An error occured while processing your request");
//         //     }

//         // }


//     }
// }


