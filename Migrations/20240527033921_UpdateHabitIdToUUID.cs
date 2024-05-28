using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XTracker.Migrations
{
    public partial class UpdateHabitIdToUUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remover a restrição de chave estrangeira que faz referência à coluna Id na tabela Habits
            migrationBuilder.DropForeignKey(
                name: "FK_DayHabits_Habits_HabitId",
                table: "DayHabits");
            
            migrationBuilder.DropForeignKey(
                name: "FK_HabitWeekDays_Habits_HabitId",
                table: "HabitWeekDays");

            // Antes de alterar a coluna Id na tabela Habits, remover a restrição de chave estrangeira que faz referência a essa coluna na tabela DayHabits
            migrationBuilder.DropIndex(
                name: "IX_DayHabits_HabitId",
                table: "DayHabits");

            migrationBuilder.DropIndex(
                name: "IX_HabitWeekDays_HabitId",
                table: "HabitWeekDays");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Habits",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "UUID()",
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<Guid>(
                name: "HabitId",
                table: "HabitWeekDays",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "HabitId",
                table: "DayHabits",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Adicionar de volta os índices após a alteração
            migrationBuilder.CreateIndex(
                name: "IX_DayHabits_HabitId",
                table: "DayHabits",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitWeekDays_HabitId",
                table: "HabitWeekDays",
                column: "HabitId");

            // Adicionar de volta as restrições de chave estrangeira após a alteração
            migrationBuilder.AddForeignKey(
                name: "FK_DayHabits_Habits_HabitId",
                table: "DayHabits",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HabitWeekDays_Habits_HabitId",
                table: "HabitWeekDays",
                column: "HabitId",
                principalTable: "Habits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Habits",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "UUID()")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "HabitId",
                table: "HabitWeekDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "HabitId",
                table: "DayHabits",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
        }
    }
}
