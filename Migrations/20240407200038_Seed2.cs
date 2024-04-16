using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace XTracker.Migrations
{
    /// <inheritdoc />
    public partial class Seed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DayHabits",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DayHabits",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DayHabits",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DayHabits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Days",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 1,
                column: "WeekDay",
                value: 0);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 2,
                column: "WeekDay",
                value: 1);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 3,
                column: "WeekDay",
                value: 2);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 4,
                column: "HabitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 5,
                column: "HabitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 6,
                column: "HabitId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 1, 6 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 2, 3 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 2, 4 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 2, 5 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 15,
                column: "HabitId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Title" },
                values: new object[] { new DateTime(2024, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ler 30 minutos" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Days",
                columns: new[] { "Id", "Date" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2023, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2023, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 1,
                column: "WeekDay",
                value: 1);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 2,
                column: "WeekDay",
                value: 2);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 3,
                column: "WeekDay",
                value: 3);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 4,
                column: "HabitId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 5,
                column: "HabitId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 6,
                column: "HabitId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 1 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 2 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 4 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 3, 5 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 4, 1 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 4, 2 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "HabitId", "WeekDay" },
                values: new object[] { 5, 4 });

            migrationBuilder.UpdateData(
                table: "HabitWeekDays",
                keyColumn: "Id",
                keyValue: 15,
                column: "HabitId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Habits",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Title" },
                values: new object[] { new DateTime(2023, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dormir 8h" });

            migrationBuilder.InsertData(
                table: "Habits",
                columns: new[] { "Id", "CreatedAt", "Title" },
                values: new object[,]
                {
                    { 4, new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ler 30 minutos" },
                    { 5, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meditar" }
                });

            migrationBuilder.InsertData(
                table: "DayHabits",
                columns: new[] { "Id", "DayId", "HabitId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 3, 1 },
                    { 4, 3, 2 }
                });
        }
    }
}
