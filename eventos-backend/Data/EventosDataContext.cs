using eventos_backend.Model;
using Microsoft.EntityFrameworkCore;

namespace eventos_backend.Data
{
    public class EventosDataContext : DbContext
    {
        public EventosDataContext(DbContextOptions<EventosDataContext> options)
            : base(options)
        {
        }

        public DbSet<EventoModel> Eventos { get; set; }
        public DbSet<ParticipanteModel> Participantes { get; set; }
    }
}
