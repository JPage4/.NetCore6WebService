using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.Migrations
{
    public partial class RefactorInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Foo = table.Column<string>(type: "TEXT", nullable: true),
                    Baz = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payloads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TS = table.Column<long>(type: "INTEGER", nullable: false),
                    Sender = table.Column<string>(type: "TEXT", nullable: false),
                    MessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SentFromIp = table.Column<string>(type: "TEXT", nullable: true),
                    Priority = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payloads_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Baz", "Foo" },
                values: new object[] { new Guid("1ce1fb75-d186-4700-a44a-b15382ca22ea"), "bang", "bar" });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Baz", "Foo" },
                values: new object[] { new Guid("a5f0285e-8739-4b60-887d-d5b829164dd0"), "whoop", "big" });

            migrationBuilder.InsertData(
                table: "Payloads",
                columns: new[] { "Id", "MessageId", "Priority", "Sender", "SentFromIp", "TS" },
                values: new object[] { new Guid("77194d53-0f5d-4868-bcf4-5adc693b6e62"), new Guid("a5f0285e-8739-4b60-887d-d5b829164dd0"), 0, "whoa very test", "4.3.2.1", 1684234873L });

            migrationBuilder.InsertData(
                table: "Payloads",
                columns: new[] { "Id", "MessageId", "Priority", "Sender", "SentFromIp", "TS" },
                values: new object[] { new Guid("f0411596-95f7-4553-b5bc-f90b91534506"), new Guid("1ce1fb75-d186-4700-a44a-b15382ca22ea"), 2, "testy-test-service", "1.2.3.4", 1530228282L });

            migrationBuilder.CreateIndex(
                name: "IX_Payloads_MessageId",
                table: "Payloads",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payloads");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
