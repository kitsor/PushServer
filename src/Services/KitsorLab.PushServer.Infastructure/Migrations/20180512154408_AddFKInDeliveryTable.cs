using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KitsorLab.PushServer.Infastructure.Migrations
{
    public partial class AddFKInDeliveryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_NotificationKey",
                schema: "pushServer",
                table: "Deliveries",
                column: "NotificationKey");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SubscriptionKey",
                schema: "pushServer",
                table: "Deliveries",
                column: "SubscriptionKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Notifications_NotificationKey",
                schema: "pushServer",
                table: "Deliveries",
                column: "NotificationKey",
                principalSchema: "pushServer",
                principalTable: "Notifications",
                principalColumn: "NotificationKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Subscriptions_SubscriptionKey",
                schema: "pushServer",
                table: "Deliveries",
                column: "SubscriptionKey",
                principalSchema: "pushServer",
                principalTable: "Subscriptions",
                principalColumn: "SubscriptionKey",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Notifications_NotificationKey",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Subscriptions_SubscriptionKey",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_NotificationKey",
                schema: "pushServer",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_SubscriptionKey",
                schema: "pushServer",
                table: "Deliveries");
        }
    }
}
