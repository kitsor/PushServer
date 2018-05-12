using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KitsorLab.PushServer.Infastructure.Migrations
{
    public partial class AddScheduledOnInDeliveryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_Status_CreatedOn",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledOn",
                schema: "pushServer",
                table: "Deliveries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_Status_ScheduledOn",
                schema: "pushServer",
                table: "Deliveries",
                columns: new[] { "Status", "ScheduledOn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_Status_ScheduledOn",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ScheduledOn",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_Status_CreatedOn",
                schema: "pushServer",
                table: "Deliveries",
                columns: new[] { "Status", "CreatedOn" });
        }
    }
}
