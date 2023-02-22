using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository ?? throw new ArgumentNullException(nameof(libraryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var result = await _libraryRepository.GetAllBooksAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _libraryRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(BookForCreation bookToAdd)
        {
            var newBook = _mapper.Map<Book>(bookToAdd);
            await _libraryRepository.AddBookAsync(newBook);
            return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var result = await _libraryRepository.GetBookByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteBook(result);
            
            return Ok($"The book with the id {id} has been deleted.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody]BookForUpdate newBook)
        {
            var currentBook = await _libraryRepository.GetBookByIdAsync(id);
            if (currentBook == null)
            {
                return NotFound();
            }
            await _libraryRepository.UpdateBookAsync(currentBook, newBook);
            return NoContent();
        }
    }
}
