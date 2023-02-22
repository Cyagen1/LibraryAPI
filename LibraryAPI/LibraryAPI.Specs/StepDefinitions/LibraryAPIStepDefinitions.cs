using AutoMapper;
using LibraryAPI.Controllers;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace LibraryAPI.Specs.StepDefinitions
{
    [Binding]
    public class LibraryAPIStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LibraryController _controller;
        private readonly Mock<ILibraryRepository> _repositoryMock = new Mock<ILibraryRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public LibraryAPIStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _controller = new LibraryController(_repositoryMock.Object, _mapperMock.Object);
        }

        [Given(@"that I have some books:")]
        public void GivenThatIHaveSomeBooks(Table table)
        {
            var books = table.CreateSet<Book>();
            _scenarioContext.Add("books", books);
        }

        [When(@"I send a GET request for all books")]
        public async void WhenISendAGETRequestForAllBooks()
        {
            var books = _scenarioContext.Get<IEnumerable<Book>>("books");
            _repositoryMock.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);
            var response = await _controller.GetAllBooks();
            _scenarioContext.Add("GetAllBooksResponse", response);
        }

        [Then(@"I should get all books with status code (.*)")]
        public void ThenIShouldGetAllBooksWithStatusCode(int expectedStatusCode)
        {
            var response = _scenarioContext.Get<ActionResult<IEnumerable<Book>>>("GetAllBooksResponse");
            var books = _scenarioContext.Get<IEnumerable<Book>>("books");
            var result = response.Result as OkObjectResult;
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(books, result.Value);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
        }

        [Given(@"that I have a book")]
        public void GivenThatIHaveABook()
        {
            var book = new Book
            {
                Id = 1,
                Title = "TestBook",
                Description = "This is a test description",
                Author = "TestAuthor"
            };
            _scenarioContext.Add("book", book);
        }

        [When(@"I send a GET request with the given book id")]
        public async void WhenISendAGETRequestWithTheGivenBookId()
        {
            var book = _scenarioContext.Get<Book>("book");
            _repositoryMock.Setup(x => x.GetBookByIdAsync(book.Id)).ReturnsAsync(book);
            var response = await _controller.GetBookById(book.Id);
            _scenarioContext.Add("GetBookByIdResponse", response);
        }

        [Then(@"I should recieve that book with the status code (.*)")]
        public void ThenIShouldRecieveThatBookWithTheStatusCode(int expectedStatusCode)
        {
            var response = _scenarioContext.Get<ActionResult<Book>>("GetBookByIdResponse");
            var book = _scenarioContext.Get<Book>("book");
            var result = response.Result as OkObjectResult;
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
            Assert.AreEqual(book, result.Value);
        }

        [When(@"I send a PUT request with new data for the given book")]
        public async void WhenISendAPUTRequestWithNewDataForTheGivenBook()
        {
            var book = _scenarioContext.Get<Book>("book");
            var bookForUpdate = new BookForUpdate
            {
                Title = "NewTestTitle",
                Description = "NewTestDescription",
                Author = "NewTestAuthor"
            };
            _repositoryMock.Setup(x => x.UpdateBookAsync(book, bookForUpdate));
            _repositoryMock.Setup(x => x.GetBookByIdAsync(book.Id)).ReturnsAsync(book);
            var response = await _controller.UpdateBook(book.Id, bookForUpdate);
            _scenarioContext.Add("UpdateBookResponse", response);
            _scenarioContext.Add("bookForUpdate", bookForUpdate);
        }

        [Then(@"that given book should be updated with a status code (.*)")]
        public void ThenThatGivenBookShouldBeUpdatedWithAStatusCode(int expectedStatusCode)
        {
            var book = _scenarioContext.Get<Book>("book");
            var bookForUpdate = _scenarioContext.Get<BookForUpdate>("bookForUpdate");
            var response = _scenarioContext.Get<IActionResult>("UpdateBookResponse");
            _repositoryMock.Verify(x => x.UpdateBookAsync(book, bookForUpdate), Times.Once);
            Assert.AreEqual(expectedStatusCode, ((IStatusCodeActionResult)response).StatusCode);
        }

        [When(@"I send a DELETE request with the given book id")]
        public async void WhenISendADELETERequestWithTheGivenBookId()
        {
            var book = _scenarioContext.Get<Book>("book");
            _repositoryMock.Setup(x => x.GetBookByIdAsync(book.Id)).ReturnsAsync(book);
            _repositoryMock.Setup(x => x.DeleteBook(book));
            var response = await _controller.DeleteBook(book.Id);
            _scenarioContext.Add("DeleteBookResponse", response);
        }

        [Then(@"the book should be deleted with the status code (.*)")]
        public void ThenTheBookShouldBeDeletedWithTheStatusCode(int expectedStatusCode)
        {
            var book = _scenarioContext.Get<Book>("book");
            var response = _scenarioContext.Get<IActionResult>("DeleteBookResponse");
            _repositoryMock.Verify(x => x.DeleteBook(book), Times.Once);
            Assert.AreEqual(expectedStatusCode, ((IStatusCodeActionResult)response).StatusCode);
        }
    }
}
