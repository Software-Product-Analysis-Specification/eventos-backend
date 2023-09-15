﻿using eventos_backend.Data;
using eventos_backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventos_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventosDataContext _db;

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

        [HttpGet("{id}", Name = "GetEvento")]
        public IActionResult GetEvento(int id)
        {
            var evento = _db.Eventos.FirstOrDefault(e => e.Id == id);

            if(evento != null)
                return Ok(id);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add(EventoModel evento)
        {
            try
            {
                _db.Eventos.Add(evento);
                await _db.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(EventoModel evento)
        {
            try
            {
                var oldEvento = _db.Eventos.FirstOrDefault(e => e.Id == evento.Id);

                if(oldEvento != null)
                {
                    oldEvento = evento;
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

        [HttpDelete]
        public async Task<IActionResult> Delete(EventoModel evento)
        {
            _db.Eventos.Remove(evento);
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
