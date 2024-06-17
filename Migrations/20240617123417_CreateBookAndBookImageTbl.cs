using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_BookStore.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookAndBookImageTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books_List",
                columns: table => new
                {
                    Book_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    PublishDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Category_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books_List", x => x.Book_Id);
                    table.ForeignKey(
                        name: "FK_Books_List_Categories_Category_ID",
                        column: x => x.Category_ID,
                        principalTable: "Categories",
                        principalColumn: "Category_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books_List_Images",
                columns: table => new
                {
                    Image_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Book_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books_List_Images", x => x.Image_ID);
                    table.ForeignKey(
                        name: "FK_Books_List_Images_Books_List_Book_ID",
                        column: x => x.Book_ID,
                        principalTable: "Books_List",
                        principalColumn: "Book_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_List_Category_ID",
                table: "Books_List",
                column: "Category_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_List_Images_Book_ID",
                table: "Books_List_Images",
                column: "Book_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books_List_Images");

            migrationBuilder.DropTable(
                name: "Books_List");
        }
    }
}
