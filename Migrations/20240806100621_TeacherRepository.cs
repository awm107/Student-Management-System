using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentManagement.Migrations
{
    public partial class TeacherRepository : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Age = table.Column<long>(nullable: false),
                    Subject = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Photopath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Age", "Email", "Gender", "Name", "Photopath", "Subject" },
                values: new object[] { 1, 35L, "maria@gmail.com", 1, "Maria", null, 0 });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Age", "Email", "Gender", "Name", "Photopath", "Subject" },
                values: new object[] { 2, 46L, "liam@gmail.com", 0, "Liam", null, 2 });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Age", "Email", "Gender", "Name", "Photopath", "Subject" },
                values: new object[] { 3, 32L, "george@gmail.com", 0, "George", null, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
