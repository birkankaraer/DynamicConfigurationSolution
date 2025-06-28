using ConfigurationLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConfigWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationRecordsController : ControllerBase
    {
        private readonly ConfigurationDbContext _dbContext;

        public ConfigurationRecordsController(ConfigurationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/ConfigurationRecords?filterName=site
        [HttpGet]
        public async Task<IActionResult> GetAll(string? filterName)
        {
            var query = _dbContext.ConfigurationRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterName))
            {
                query = query.Where(c => c.Name.Contains(filterName));
            }

            var list = await query.ToListAsync();
            return Ok(list);
        }

        // GET: api/ConfigurationRecords/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _dbContext.ConfigurationRecords.FindAsync(id);
            if (record == null) return NotFound();
            return Ok(record);
        }

        // POST: api/ConfigurationRecords
        [HttpPost]
        public async Task<IActionResult> Create(ConfigurationRecord newRecord)
        {
            _dbContext.ConfigurationRecords.Add(newRecord);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newRecord.Id }, newRecord);
        }

        // PUT: api/ConfigurationRecords/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ConfigurationRecord updatedRecord)
        {
            var existing = await _dbContext.ConfigurationRecords.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = updatedRecord.Name;
            existing.Type = updatedRecord.Type;
            existing.Value = updatedRecord.Value;
            existing.IsActive = updatedRecord.IsActive;
            existing.ApplicationName = updatedRecord.ApplicationName;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ConfigurationRecords/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _dbContext.ConfigurationRecords.FindAsync(id);
            if (existing == null) return NotFound();

            _dbContext.ConfigurationRecords.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
