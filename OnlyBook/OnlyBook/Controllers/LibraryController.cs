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
    public class LibraryController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibraryController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Library> Get()
        {
            return _libraryRepository.GetAll();
        }
        [HttpGet("{id}", Name = "GetLibrary")]
        public IActionResult Get(int id)
        {
            var library = _libraryRepository.GetById(id);
            if (library == null)
            {
                return NotFound();
            }

            return Ok(library);
        }


        // POST library
        [HttpPost]
        public IActionResult Post([FromBody]Library value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdLibrary = _libraryRepository.Add(value);
            return CreatedAtRoute("GetLibrary", new { id = createdLibrary.LibraryId }, createdLibrary);
        }


        // PUT library/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Library value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var note = _libraryRepository.GetById(id);
            if (note == null)
            {
                return NotFound();
            }
            value.LibraryId = id;
            _libraryRepository.Update(value);
            var response = new { MessageBody = "Successfully Updated" };
            return Ok(response);
        }


        // DELETE library/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var library = _libraryRepository.GetById(id);
            if (library == null)
            {
                return NotFound();
            }
            _libraryRepository.Delete(library);
            var response = new { MessageBody = "Successfully Deleted" };
            return Ok(response);
        }
    }
}
