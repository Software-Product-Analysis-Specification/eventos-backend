using eventos_backend.Data;
using eventos_backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eventos_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : ControllerBase
    {
        private readonly EventosDataContext _db;

        public ParticipantesController(EventosDataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var participantes = _db.Participantes;

            return Ok(participantes);
        }

        [HttpGet("{id}", Name = "GetParticipante")]
        public IActionResult GetParticipante(int id)
        {
            var participante = _db.Participantes.FirstOrDefault(e => e.Id == id);

            if (participante != null)
                return Ok(participante);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ParticipanteModel participante)
        {
            try
            {
                _db.Participantes.Add(participante);

                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(ParticipanteModel participante)
        {
            try
            {
                var oldParticipante = _db.Participantes.FirstOrDefault(e => e.Id == participante.Id);

                if (oldParticipante != null)
                {
                    oldParticipante.Nome = participante.Nome;
                    oldParticipante.Email = participante.Email;
                    oldParticipante.Whatsapp = participante.Whatsapp;

                    await _db.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("Registro não encontrado");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "DeleteParticipante")]
        public async Task<IActionResult> Delete(int id)
        {
            var participante = _db.Participantes.FirstOrDefault(e => e.Id == id);

            if (participante != null)
            {
                _db.Participantes.Remove(participante);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
