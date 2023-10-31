using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagementSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TblUser",
                columns: table => new
                {
                    Index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(20)", nullable: false),
                    Age = table.Column<short>(type: "smallint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(12)", nullable: false),
                    IsInit = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblUser", x => x.Index);
                });

            migrationBuilder.InsertData(
                table: "TblUser",
                columns: new[] { "Index", "Age", "IsInit", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, (short)10, true, "적길동", "01012341200" },
                    { 2, (short)15, true, "홍길동", "01012341201" },
                    { 3, (short)20, true, "황길동", "01012341202" },
                    { 4, (short)25, true, "록길동", "01012341203" },
                    { 5, (short)30, true, "청길동", "01012341204" },
                    { 6, (short)35, true, "남길동", "01012341205" },
                    { 7, (short)40, true, "자길동", "01012341206" },
                    { 8, (short)45, true, "백길동", "01012341207" },
                    { 9, (short)50, true, "회길동", "01012341208" },
                    { 10, (short)55, true, "흑길동", "01012341209" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblUser");
        }
    }
}
