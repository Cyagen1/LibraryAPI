using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public LibraryRepository(LibraryContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateBookAsync(Book currentBook, BookForUpdate newBook)
        {
            _mapper.Map(newBook, currentBook);
            await _context.SaveChangesAsync();
        }
    }
}
