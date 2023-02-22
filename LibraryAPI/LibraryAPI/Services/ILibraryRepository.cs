using LibraryAPI.Models;

namespace LibraryAPI.Services
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task AddBookAsync(Book book);

        Task<Book?> GetBookByIdAsync (int id);

        void DeleteBook(Book book);

        Task UpdateBookAsync(Book currentBook, BookForUpdate newBook);

    }
}
