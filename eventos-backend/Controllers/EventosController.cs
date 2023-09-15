using eventos_backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventos_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private EventosDataContext _db;

        public EventosController(EventosDataContext db) 
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var eventos = _db.Eventos;

            return Ok(eventos);
        }
    }
}
