using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById (int id) {

            var celestialobject = _context.CelestialObjects.Find(id);
            if (celestialobject == null)
                return NotFound();

            celestialobject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialobject);

        }

        [HttpGet("{name}",Name = "GetByName")]
        public IActionResult GetByName (string name) {

            var celestialobject = _context.CelestialObjects.Where(e => e.Name == name);
            if (celestialobject.Count() == 0)
                return NotFound();

            foreach(var c in celestialobject)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }
             
            return Ok(celestialobject);
        }

        [HttpGet("",Name = "GetAll")]
        public IActionResult GetAll() {
            var celestialobject = _context.CelestialObjects;
            if (celestialobject.Count() == 0)
                return NotFound();

            foreach (var c in celestialobject)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }

            return Ok(celestialobject);

        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestial) {
            _context.CelestialObjects.Add(celestial);
            _context.SaveChanges();
            return CreatedAtRoute("GetById",new { id = celestial.Id} , celestial);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestial) {

            var celestialobject = _context.CelestialObjects.Find(id);
            if (celestialobject == null)
                return NotFound();

            celestialobject.Name = celestial.Name;
            celestialobject.OrbitalPeriod = celestial.OrbitalPeriod;
            celestialobject.OrbitedObjectId = celestial.OrbitedObjectId;

            _context.Update(celestialobject);
            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name) {
            var celestialobject = _context.CelestialObjects.Find(id);
            if (celestialobject == null)
                return NotFound();

            celestialobject.Name = name;

            _context.Update(celestialobject);
            _context.SaveChanges();


            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {

            var celestialobject = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);
            if (celestialobject.Count() == 0)
                return NotFound();

            foreach (var c in celestialobject)
            {
                _context.Remove(c);
            }

            _context.SaveChanges();

            return NoContent();

        }

    }
}
