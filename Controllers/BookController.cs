using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projectTwo.Models;

namespace projectTwo
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;

        public BookController(BookContext context)
        {
            _context = context;

            if (_context.BookItems.Count()==0)
            {
                // create book if empty or delete it too!
                _context.BookItems.Add(new BookItem { Name = "New Book here" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookItem>>> GetBookItem(long id)
        {
            return await _context.BookItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookItem>> GetBookItems(long id)
        {
            var BookItem = await _context.BookItems.FindAsync(id);

            if (BookItem == null)
            {
                return NotFound();
            }
            return BookItem;
        }
        [HttpPost]
        public async Task<ActionResult<BookItem>> PostBookItem(BookItem Item)
        {
            _context.BookItems.Add(Item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookItem), new { id = Item.Id}, Item);
        }
    }
}