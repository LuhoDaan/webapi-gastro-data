using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
using GastroApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;

namespace GastroApi.Controllers
{
    [Route("api/GastroItems")]
    [ApiController]
    public class GastroItemsController : ControllerBase
    {
        private readonly GastroContext _context;

        public GastroItemsController(GastroContext context)
        {
            _context = context;
        }

        // GET: api/GastroItems
        [HttpGet]

        public async Task<ActionResult<IEnumerable<GastroItem>>> GetGastroItems([FromQuery] long? id,[FromQuery] string? name,[FromQuery] string? description)
        {
            IQueryable<GastroItem> query = _context.GastroItems;

            if(id.HasValue){

                query = query.Where(item => item.Id.Equals(id));

            }
            else if (!name.IsNullOrEmpty()){
                query = query.Where(item => item.DescriptionName.Contains(name));
            }
            else if (!description.IsNullOrEmpty()){
                query = query.Where(item => item.Recipe.Contains("description"));
            }

           var items = await query.ToListAsync();
            if (items == null || !items.Any() ){
                return NotFound();

            }
            else {
                return Ok(items);
            }
        }

        // POST: api/GastroItems
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<GastroItem>> PostGastroItem(GastroItem GastroItem)
        {
            _context.GastroItems.Add(GastroItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGastroItems), new { id = GastroItem.Id }, GastroItem);
        }

        // Other CRUD actions can be added here (PutGastroItem, DeleteGastroItem, etc.)
    

    [HttpPut("{id}")]
    public async Task<IActionResult> PutGastroItem(long id, GastroItem GastroItem)
    {
        if (id != GastroItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(GastroItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GastroItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    private bool GastroItemExists(long id)
{
    return _context.GastroItems.Any(e => e.Id == id);
}

    [HttpDelete]
    public async Task<IActionResult> DeleteGastroItem([FromQuery] long? id, [FromQuery] string name)
    {
        IQueryable<GastroItem> query = _context.GastroItems;

        if(id.HasValue){
            query = query.Where(e => e.Id.Equals(id));
        }

        else if(!name.IsNullOrEmpty()){

            query = query.Where(e => e.DescriptionName.Equals(name));

        }

        if ( !await query.AnyAsync())
        {
            return NotFound();
        }
    else{

        GastroItem item = await query.FirstOrDefaultAsync();
        _context.GastroItems.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }}
}
}