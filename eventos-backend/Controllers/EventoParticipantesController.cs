using eventos_backend.Data;
using eventos_backend.Data.Migrations;
using eventos_backend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

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

        [HttpDelete("{evento}/{participante}", Name = "DeleteEventoParticipante")]
        public async Task<IActionResult> Delete(int evento, int participante)
        {
            var eventoParticipante = _db.EventoParticipantes.FirstOrDefault(e => e.Evento == evento && e.Participante == participante);

            if (eventoParticipante != null)
            {
                _db.EventoParticipantes.Remove(eventoParticipante);
                await _db.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{evento}/{participante}", Name = "SendInvite")]
        public async Task<IActionResult> SendInvite(int evento, int participante)
        {
            var eventoParticipante = _db.EventoParticipantes.FirstOrDefault(e => e.Evento == evento && e.Participante == participante);

            if (eventoParticipante != null)
            {
                var eventoDb = _db.Eventos.FirstOrDefault(e => e.Id == evento);
                var participanteDb = _db.Participantes.FirstOrDefault(p => p.Id == participante);

                if(eventoDb != null && participanteDb != null)
                {
                    var restClient = new RestClient("https://graph.facebook.com/v17.0/132331503289028");
                    var restRequest = new RestRequest("/messages", Method.Post);

                    restRequest.AddHeader("Authorization", $"Bearer {Environment.GetEnvironmentVariable("METATOKEN")}");

                    var message = new JObject
                    {
                        ["messaging_product"] = "whatsapp",
                        ["to"] = participanteDb.Whatsapp,
                        ["type"] = "template",
                        ["template"] = new JObject
                        {
                            ["name"] = "cracha_evento",
                            ["language"] = new JObject
                            {
                                ["code"] = "pt_BR"
                            },
                            ["components"] = new JArray
                            {
                                new JObject
                                {
                                    ["type"] = "header",
                                    ["parameters"] = new JArray
                                    {
                                        new JObject
                                        {
                                            ["type"] = "image",
                                            ["image"] = new JObject
                                            {
                                                ["link"] = "https://i.ibb.co/3dSvYhz/qrcode.png"
                                            }
                                        }
                                    }
                                },
                                new JObject
                                {
                                    ["type"] = "body",
                                    ["parameters"] = new JArray
                                    {
                                        new JObject
                                        {
                                            ["type"] = "text",
                                            ["text"] = eventoDb.Nome
                                        },
                                        new JObject
                                        {
                                            ["type"] = "text",
                                            ["text"] = participanteDb.Nome
                                        },
                                        new JObject
                                        {
                                            ["type"] = "text",
                                            ["text"] = eventoDb.Data?.ToString("dd/MM/yyyy HH:mm")
                                        },
                                        new JObject
                                        {
                                            ["type"] = "text",
                                            ["text"] = participanteDb.Email
                                        }
                                    }
                                }
                            }
                        }
                    };

                    restRequest.AddJsonBody(message.ToString());

                    var response = restClient.Execute(restRequest);

                    Console.WriteLine("STATUSX " + response.StatusCode);
                    Console.WriteLine(response.Content);
                }
            }

            return Ok();
        }
    }
}
