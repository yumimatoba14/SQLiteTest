using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteTest.Migrations
{
    public partial class AddTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(nullable: false),
                    SubId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => new { x.WorkflowId, x.SubId });
                    table.ForeignKey(
                        name: "FK_Tasks_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_Id",
                table: "Workflows",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkflowId_SubId",
                table: "Tasks",
                columns: new[] { "WorkflowId", "SubId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Workflows_Id",
                table: "Workflows");
        }
    }
}
