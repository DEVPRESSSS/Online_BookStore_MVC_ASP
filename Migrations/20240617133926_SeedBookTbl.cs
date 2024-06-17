using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_BookStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedBookTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books_List",
                columns: new[] { "Book_Id", "Category_ID", "Description", "ISBN", "Price", "PublishDate", "Publisher", "Stock", "Title" },
                values: new object[] { 1, 2, "Programming subject", "KS454574JDHFBS", 50f, new DateOnly(2023, 12, 10), "Sir Ervin Santos", 15, "DS101" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books_List",
                keyColumn: "Book_Id",
                keyValue: 1);
        }
    }
}
