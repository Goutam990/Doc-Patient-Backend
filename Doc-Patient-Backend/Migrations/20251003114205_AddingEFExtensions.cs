using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doc_Patient_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddingEFExtensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enquiry",
                columns: table => new
                {
                    enquiryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enquiryTypeId = table.Column<int>(type: "int", nullable: false),
                    enquiryStatusId = table.Column<int>(type: "int", nullable: false),
                    customerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiry", x => x.enquiryId);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryStatuses",
                columns: table => new
                {
                    statusIs = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryStatuses", x => x.statusIs);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryTypes",
                columns: table => new
                {
                    typeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryTypes", x => x.typeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enquiry");

            migrationBuilder.DropTable(
                name: "EnquiryStatuses");

            migrationBuilder.DropTable(
                name: "EnquiryTypes");
        }
    }
}
