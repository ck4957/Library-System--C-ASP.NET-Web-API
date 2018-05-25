using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlyBook.Models;
using OnlyBook.Repository;

namespace OnlyBook.Controllers
{
    [Route("[controller]")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Author> Get()
        {
            return _authorRepository.GetAll();
        }
        [HttpGet("{id}",Name = "GetAuthor")]
        public IActionResult Get(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpGet("AuthorName/{authorName}")]
        public IActionResult GetByAuthorName(string authorName)
        {
            var author = _authorRepository.GetByName(authorName);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }


        // POST book
        [HttpPost]
        public IActionResult Post([FromBody]Author value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdAuthor = _authorRepository.Add(value);

            return CreatedAtRoute("GetAuthor", new { id = createdAuthor.AuthorId }, createdAuthor);
        }


        [HttpPut("AddBooktoAuthor")]
        public IActionResult AddBktoAuthor([FromBody]Author value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            //value.AuthorId = id;
            _authorRepository.AddBooktoAuthor(value);
            var note = _authorRepository.GetById(value.AuthorId);

            //var response = new { MessageBody = "Successfully Added Book to Author" };
            //return Ok(response);
            return CreatedAtRoute("GetAuthor", new { id = note.AuthorId }, note);

        }



        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Author value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var note = _authorRepository.GetById(id);
            if (note == null)
            {
                return NotFound();
            }
            value.AuthorId = id;
            _authorRepository.Update(value);
            var response = new { MessageBody = "Successfully Updated" };
            return Ok(response);
        }


        // DELETE book/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author == null)
            {
                return NotFound();
            }
            _authorRepository.Delete(author);
            var response = new { MessageBody = "Successfully Deleted" };
            return Ok(response);
        }
    }
}
