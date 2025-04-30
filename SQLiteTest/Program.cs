using System;
using System.Linq;
using SQLiteTest.Models;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new WfDbContext())
            {
                if (db.Workflows.Count() == 0)
                {
                    int taskNo = 0;
                    var workflow = new Workflow { Id = 1, Name = "Workflow1" };
                    db.Workflows.Add(workflow);
                    for (int i = 0; i < 2; ++i)
                    {
                        var task = new Task { SubId = i + 1, Workflow = workflow, Name = $"task{taskNo++}" };
                        db.Tasks.Add(task);
                    }

                    workflow = new Workflow { Id = 10, Name = "Workflow10" };
                    db.Workflows.Add(workflow);
                    for (int i = 0; i < 3; ++i)
                    {
                        var task = new Task { SubId = i + 1, Workflow = workflow, Name = $"task{taskNo++}" };
                        db.Tasks.Add(task);
                    }

                    db.SaveChanges();
                    Console.WriteLine("Data has been added.");
                }
            }
            using (var db = new WfDbContext())
            {
                ShowWorkflow(db, 1);
                ShowWorkflow(db, 10);
                var task0 = db.Tasks.Find(1, 1);
                if (task0 != null)
                {
                    ShowTask(task0);
                }
                foreach (var task in db.Tasks)
                {
                    ShowTask(task);
                }
                ShowWorkflow(db, 1);
            }
        }

        private static void ShowWorkflow(WfDbContext db, int id)
        {
            var workflow = db.Workflows.Find(id);
            if (workflow != null)
            {
                Console.WriteLine("workflow.Name = {0}", workflow.Name);
                foreach (var task in workflow.Tasks)
                {
                    Console.WriteLine("  task: Workflow = {0}, SubId = {1}, Name = {2}", task.Workflow.Id, task.SubId, task.Name);
                }
            }
            else
            {
                Console.WriteLine("Workflow(Id = {0}) is not found.", id);
            }
        }

        private static void ShowTask(Task task)
        {
            Console.WriteLine("task: Workflow = {0}, SubId = {1}, Name = {2}", task.Workflow.Id, task.SubId, task.Name);
        }
    }
}
