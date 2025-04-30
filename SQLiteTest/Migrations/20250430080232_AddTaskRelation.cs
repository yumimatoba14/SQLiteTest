using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteTest.Migrations
{
    public partial class AddTaskRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskRelations",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(nullable: false),
                    PrevTaskSubId = table.Column<int>(nullable: false),
                    NextTaskSubId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRelations", x => new { x.WorkflowId, x.PrevTaskSubId, x.NextTaskSubId });
                    table.ForeignKey(
                        name: "FK_TaskRelations_Tasks_WorkflowId_NextTaskSubId",
                        columns: x => new { x.WorkflowId, x.NextTaskSubId },
                        principalTable: "Tasks",
                        principalColumns: new[] { "WorkflowId", "SubId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskRelations_Tasks_WorkflowId_PrevTaskSubId",
                        columns: x => new { x.WorkflowId, x.PrevTaskSubId },
                        principalTable: "Tasks",
                        principalColumns: new[] { "WorkflowId", "SubId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskRelations_WorkflowId_NextTaskSubId",
                table: "TaskRelations",
                columns: new[] { "WorkflowId", "NextTaskSubId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskRelations_WorkflowId_PrevTaskSubId_NextTaskSubId",
                table: "TaskRelations",
                columns: new[] { "WorkflowId", "PrevTaskSubId", "NextTaskSubId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskRelations");
        }
    }
}
