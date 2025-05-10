using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ReconfiguredTheDatabases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanRecipe_MealPlans_MealPlanId",
                table: "MealPlanRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_AspNetUsers_UserId",
                table: "MealPlans");

            migrationBuilder.DropTable(
                name: "UserDietaryPreference");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_UserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "MealType",
                table: "MealPlanRecipe");

            migrationBuilder.DropColumn(
                name: "PlannedDate",
                table: "MealPlanRecipe");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DietaryPreferences");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DietaryPreferences");

            migrationBuilder.RenameColumn(
                name: "ShoppingListItemId",
                table: "ShoppingListItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ShoppingListId",
                table: "ShoppingList",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "Recipes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RecipeIngredientId",
                table: "RecipeIngredient",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RecipeCookwareId",
                table: "RecipeCookware",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MealPlanId",
                table: "MealPlans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MealPlanRecipeId",
                table: "MealPlanRecipe",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DietaryPreferenceId",
                table: "DietaryPreferences",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Recipes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "MealPlans",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MealPlanId",
                table: "MealPlanRecipe",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ServingTypeId",
                table: "MealPlanRecipe",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DietTypeId",
                table: "DietaryPreferences",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServingTypeId",
                table: "DietaryPreferences",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DietaryPreferencesId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Allergy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DietaryPreferencesId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allergy_DietaryPreferences_DietaryPreferencesId",
                        column: x => x.DietaryPreferencesId,
                        principalTable: "DietaryPreferences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DietType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeInstruction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    InstructionIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInstruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeInstruction_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServingType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberOfServings = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServingType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_PhotoId",
                table: "Recipes",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_ApplicationUserId",
                table: "MealPlans",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlanRecipe_ServingTypeId",
                table: "MealPlanRecipe",
                column: "ServingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DietaryPreferences_DietTypeId",
                table: "DietaryPreferences",
                column: "DietTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DietaryPreferences_ServingTypeId",
                table: "DietaryPreferences",
                column: "ServingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DietaryPreferencesId",
                table: "AspNetUsers",
                column: "DietaryPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_Allergy_DietaryPreferencesId",
                table: "Allergy",
                column: "DietaryPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInstruction_RecipeId",
                table: "RecipeInstruction",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DietaryPreferences_DietaryPreferencesId",
                table: "AspNetUsers",
                column: "DietaryPreferencesId",
                principalTable: "DietaryPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DietaryPreferences_DietType_DietTypeId",
                table: "DietaryPreferences",
                column: "DietTypeId",
                principalTable: "DietType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DietaryPreferences_ServingType_ServingTypeId",
                table: "DietaryPreferences",
                column: "ServingTypeId",
                principalTable: "ServingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanRecipe_MealPlans_MealPlanId",
                table: "MealPlanRecipe",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanRecipe_ServingType_ServingTypeId",
                table: "MealPlanRecipe",
                column: "ServingTypeId",
                principalTable: "ServingType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_AspNetUsers_ApplicationUserId",
                table: "MealPlans",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Photos_PhotoId",
                table: "Recipes",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DietaryPreferences_DietaryPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_DietaryPreferences_DietType_DietTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_DietaryPreferences_ServingType_ServingTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanRecipe_MealPlans_MealPlanId",
                table: "MealPlanRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlanRecipe_ServingType_ServingTypeId",
                table: "MealPlanRecipe");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_AspNetUsers_ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Photos_PhotoId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "Allergy");

            migrationBuilder.DropTable(
                name: "DietType");

            migrationBuilder.DropTable(
                name: "RecipeInstruction");

            migrationBuilder.DropTable(
                name: "ServingType");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_PhotoId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropIndex(
                name: "IX_MealPlanRecipe_ServingTypeId",
                table: "MealPlanRecipe");

            migrationBuilder.DropIndex(
                name: "IX_DietaryPreferences_DietTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropIndex(
                name: "IX_DietaryPreferences_ServingTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DietaryPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "ServingTypeId",
                table: "MealPlanRecipe");

            migrationBuilder.DropColumn(
                name: "DietTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropColumn(
                name: "ServingTypeId",
                table: "DietaryPreferences");

            migrationBuilder.DropColumn(
                name: "DietaryPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ShoppingListItem",
                newName: "ShoppingListItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ShoppingList",
                newName: "ShoppingListId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Recipes",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecipeIngredient",
                newName: "RecipeIngredientId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecipeCookware",
                newName: "RecipeCookwareId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MealPlans",
                newName: "MealPlanId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MealPlanRecipe",
                newName: "MealPlanRecipeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DietaryPreferences",
                newName: "DietaryPreferenceId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Recipes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "MealPlans",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MealPlans",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "MealPlans",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MealPlans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MealPlanId",
                table: "MealPlanRecipe",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MealType",
                table: "MealPlanRecipe",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDate",
                table: "MealPlanRecipe",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DietaryPreferences",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DietaryPreferences",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserDietaryPreference",
                columns: table => new
                {
                    UserDietaryPreferenceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DietaryPreferenceId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDietaryPreference", x => x.UserDietaryPreferenceId);
                    table.ForeignKey(
                        name: "FK_UserDietaryPreference_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDietaryPreference_DietaryPreferences_DietaryPreferenceId",
                        column: x => x.DietaryPreferenceId,
                        principalTable: "DietaryPreferences",
                        principalColumn: "DietaryPreferenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_UserId",
                table: "MealPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDietaryPreference_DietaryPreferenceId",
                table: "UserDietaryPreference",
                column: "DietaryPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDietaryPreference_UserId",
                table: "UserDietaryPreference",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlanRecipe_MealPlans_MealPlanId",
                table: "MealPlanRecipe",
                column: "MealPlanId",
                principalTable: "MealPlans",
                principalColumn: "MealPlanId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_AspNetUsers_UserId",
                table: "MealPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
