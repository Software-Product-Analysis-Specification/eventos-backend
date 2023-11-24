using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eventos_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class EventoParticipantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventoParticipantes",
                columns: table => new
                {
                    Evento = table.Column<int>(type: "int", nullable: false),
                    Participante = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoParticipantes", x => new { x.Evento, x.Participante });
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoParticipantes");
        }
    }
}
