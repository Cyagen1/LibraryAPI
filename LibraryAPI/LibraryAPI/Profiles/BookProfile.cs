using AutoMapper;
using LibraryAPI.Models;

namespace LibraryAPI.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookForUpdate, Book>();
            CreateMap<BookForCreation, Book>();
        }
    }
}
