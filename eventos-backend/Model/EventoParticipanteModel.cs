using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace eventos_backend.Model
{
    [PrimaryKey(nameof(Evento), nameof(Participante))]
    public class EventoParticipanteModel
    {
        public int Evento { get; set; }
        [Key]
        public int Participante { get; set; }
    }
}
