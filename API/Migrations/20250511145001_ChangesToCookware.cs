using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToCookware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cookwares");

            migrationBuilder.CreateTable(
                name: "RecipeCookware",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CookwareId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCookware", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeCookware_Cookwares_CookwareId",
                        column: x => x.CookwareId,
                        principalTable: "Cookwares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeCookware_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCookware_CookwareId",
                table: "RecipeCookware",
                column: "CookwareId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeCookware_RecipeId",
                table: "RecipeCookware",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeCookware");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Cookwares",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
