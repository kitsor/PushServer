using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KitsorLab.PushServer.Infastructure.Migrations
{
	public partial class Init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
					name: "pushServer");

			migrationBuilder.CreateTable(
					name: "Deliveries",
					schema: "pushServer",
					columns: table => new
					{
						DeliveryKey = table.Column<long>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						NotificationKey = table.Column<long>(nullable: false),
						SubscriptionKey = table.Column<long>(nullable: false),
						Status = table.Column<byte>(nullable: false, defaultValue: (byte)0),
						CreatedOn = table.Column<DateTime>(nullable: false),
						UpdatedOn = table.Column<DateTime>(nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Deliveries", x => x.DeliveryKey);
					});

			migrationBuilder.CreateTable(
					name: "Notifications",
					schema: "pushServer",
					columns: table => new
					{
						NotificationKey = table.Column<long>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						NotificationId = table.Column<Guid>(nullable: false),
						Title = table.Column<string>(maxLength: 50, nullable: false),
						Message = table.Column<string>(maxLength: 100, nullable: false),
						Url = table.Column<string>(maxLength: 255, nullable: true),
						IconUrl = table.Column<string>(maxLength: 150, nullable: true),
						ImageUrl = table.Column<string>(maxLength: 150, nullable: true),
						CreatedOn = table.Column<DateTime>(nullable: false),
						UpdatedOn = table.Column<DateTime>(nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Notifications", x => x.NotificationKey);
					});

			migrationBuilder.CreateTable(
					name: "Subscriptions",
					schema: "pushServer",
					columns: table => new
					{
						SubscriptionKey = table.Column<long>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						UserId = table.Column<string>(maxLength: 32, nullable: false),
						Endpoint = table.Column<string>(nullable: true),
						PublicKey = table.Column<string>(maxLength: 90, nullable: true),
						Token = table.Column<string>(maxLength: 25, nullable: true),
						DeviceToken = table.Column<string>(maxLength: 255, nullable: true),
						Type = table.Column<byte>(nullable: false),
						IP = table.Column<long>(nullable: true),
						CreatedOn = table.Column<DateTime>(nullable: false),
						UpdatedOn = table.Column<DateTime>(nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionKey);
					});

			migrationBuilder.CreateIndex(
					name: "IX_Deliveries_Status_CreatedOn",
					schema: "pushServer",
					table: "Deliveries",
					columns: new[] { "Status", "CreatedOn" });

			migrationBuilder.CreateIndex(
					name: "IX_Notifications_NotificationId",
					schema: "pushServer",
					table: "Notifications",
					column: "NotificationId",
					unique: true);

			migrationBuilder.CreateIndex(
					name: "IX_Subscriptions_UserId",
					schema: "pushServer",
					table: "Subscriptions",
					column: "UserId",
					unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "Deliveries",
					schema: "pushServer");

			migrationBuilder.DropTable(
					name: "Notifications",
					schema: "pushServer");

			migrationBuilder.DropTable(
					name: "Subscriptions",
					schema: "pushServer");
		}
	}
}
