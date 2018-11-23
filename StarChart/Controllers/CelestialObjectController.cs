using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
        IActionResult GetById (int id) {

            var celestialobject = _context.CelestialObjects.Find(id);
            if (celestialobject == null)
                return NotFound();

            celestialobject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialobject);

        }

        [HttpGet("{name}",Name = "GetByName")]
        IActionResult GetByName (string name) {

            var celestialobject = _context.CelestialObjects.Where(e => e.Name == name);
            if (celestialobject == null)
                return NotFound();

            foreach(var c in celestialobject)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }
             
            return Ok(celestialobject);
        }

        [HttpGet("",Name = "GetAll")]
        IActionResult GetAll() {
            var celestialobject = _context.CelestialObjects;
            if (celestialobject == null)
                return NotFound();

            foreach (var c in celestialobject)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }

            return Ok(celestialobject);

        }
    }
}
