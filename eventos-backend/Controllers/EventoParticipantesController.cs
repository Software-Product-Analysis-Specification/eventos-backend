using eventos_backend.Data;
using eventos_backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eventos_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoParticipantesController : ControllerBase
    {
        private readonly EventosDataContext _db;

        public EventoParticipantesController(EventosDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var eventos = _db.EventoParticipantes
                .Join(_db.Eventos, ep => ep.Evento, e => e.Id, (ep, e) => new { EventoParticipante = ep, Evento = e })
                .Join(_db.Participantes, ep => ep.EventoParticipante.Participante, p => p.Id, (ep, p) => new { ep.Evento, Participante = p });

            return Ok(eventos);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventoParticipanteModel eventoParticipante)
        {
            try
            {
                _db.EventoParticipantes.Add(eventoParticipante);

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
