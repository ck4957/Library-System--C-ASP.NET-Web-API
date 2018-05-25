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
    public partial class PatronController : Controller
    {
        private readonly IPatronRepository _patronRepository;

        public PatronController(IPatronRepository patronRepository)
        {
            _patronRepository = patronRepository;
        }

        // GET: api/values
        
        [HttpGet]
        public IEnumerable<Patron> Get()
        {
            return _patronRepository.GetAll();
        }

        [HttpGet("{id}",Name = "GetPatron")]
        public IActionResult Get(int id)
        {
            var patron = _patronRepository.GetById(id);
            if (patron == null)
            {
                return NotFound();
            }

            return Ok(patron);
        }


        // POST book
        [HttpPost]
        public IActionResult Post([FromBody]Patron value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var createdPatron = _patronRepository.Add(value);
            return CreatedAtRoute("GetPatron", new { id = createdPatron.PatronId }, createdPatron);
        }


        // PUT book/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Patron value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var note = _patronRepository.GetById(id);
            if (note == null)
            {
                return NotFound();
            }
            value.PatronId = id;
            _patronRepository.Update(value);
            var response = new { MessageBody = "Successfully Updated" };
            return Ok(response);
        }

        [HttpPut("Checkout")]
        public IActionResult Put([FromBody]Patron patron,int bookid)
        {
            if (patron == null)
            {
                return BadRequest();
            }
            _patronRepository.Checkout(patron.PatronId, bookid);
            var pt = _patronRepository.GetById(patron.PatronId);
            return CreatedAtRoute("GetPatron", new { id = pt.PatronId }, pt);
            //return Ok(pt);
        }

        [HttpPut("Return")]
        public IActionResult BookReturn([FromBody]Patron patron, int bookid)
        {
            if (patron == null)
            {
                return BadRequest();
            }
            _patronRepository.BookReturn(patron.PatronId, bookid);
            var pt = _patronRepository.GetById(patron.PatronId);
            return CreatedAtRoute("GetPatron", new { id = pt.PatronId }, pt);

        }

        // DELETE book/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var patron = _patronRepository.GetById(id);
            if (patron == null)
            {
                return NotFound();
            }
            _patronRepository.Delete(patron);
            var response = new { MessageBody = "Successfully Deleted" };
            return Ok(response);
        }
    }
}
