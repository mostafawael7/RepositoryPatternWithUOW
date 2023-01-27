using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOF.Core.Consts;
using RepositoryPatternWithUOF.Core.Interfaces;
using RepositoryPatternWithUOF.Core.Models;

namespace RepositoryPatternWithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBaseRepository<Book> _booksRepository;
        public BooksController(IBaseRepository<Book> booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet]
        public IActionResult GetById()
        {
            return Ok(_booksRepository.GetById(1));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_booksRepository.GetAll());
        }
        
        [HttpGet("GetByName")]
        public IActionResult GetByName()
        {
            return Ok(_booksRepository.Find(b => b.Title == "TestBook1", new[] { "Author" }));
        }

        [HttpGet("GetAllWithAuthors")]
        public IActionResult GetAllWithAuthors()
        {
            return Ok(_booksRepository.FindAll(b => b.Title.Contains("TestBook"), new[] { "Author" }));
        }

        [HttpGet("GetOrdered")]
        public IActionResult GetOrdered()
        {
            return Ok(_booksRepository.FindAll(b => b.Title.Contains("TestBook"), null, null, b => b.Id, OrderBy.Descending));
        }

        [HttpPost("AddOne")]
        public IActionResult AddOne()
        {
            return Ok(_booksRepository.Add(new Book { Title = "TestBook3", AuthorId = 1 }));
        }
    }
}
