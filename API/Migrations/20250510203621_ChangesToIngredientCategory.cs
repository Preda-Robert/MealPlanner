using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ChangesToIngredientCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategory_CategoryId",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientCategory",
                table: "IngredientCategory");

            migrationBuilder.RenameTable(
                name: "IngredientCategory",
                newName: "IngredientCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientCategories",
                table: "IngredientCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId",
                table: "Ingredients",
                column: "CategoryId",
                principalTable: "IngredientCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientCategories_CategoryId",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientCategories",
                table: "IngredientCategories");

            migrationBuilder.RenameTable(
                name: "IngredientCategories",
                newName: "IngredientCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientCategory",
                table: "IngredientCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientCategory_CategoryId",
                table: "Ingredients",
                column: "CategoryId",
                principalTable: "IngredientCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
