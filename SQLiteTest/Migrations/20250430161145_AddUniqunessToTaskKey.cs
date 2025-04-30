using Microsoft.EntityFrameworkCore.Migrations;

namespace SQLiteTest.Migrations
{
    public partial class AddUniqunessToTaskKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_WorkflowId_SubId",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkflowId_SubId",
                table: "Tasks",
                columns: new[] { "WorkflowId", "SubId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_WorkflowId_SubId",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkflowId_SubId",
                table: "Tasks",
                columns: new[] { "WorkflowId", "SubId" });
        }
    }
}
