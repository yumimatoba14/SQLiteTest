using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SQLiteTest.Models;

namespace SQLiteTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new WfDbContext())
            {
                db.Database.Migrate();
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
                    ConnectTask(db, 1, 1, 2);

                    workflow = new Workflow { Id = 10, Name = "Workflow10" };
                    db.Workflows.Add(workflow);
                    for (int i = 0; i < 3; ++i)
                    {
                        var task = new Task { SubId = i + 1, Workflow = workflow, Name = $"task{taskNo++}" };
                        db.Tasks.Add(task);
                    }
                    ConnectTask(db, 10, 1, 3);
                    ConnectTask(db, 10, 2, 3);

                    db.SaveChanges();
                    Console.WriteLine("Data has been added.");
                }

                {
                    int taskNo = 100;
                    var anotherWorkflow = new Workflow { Id = 100, Name = "Workflow to be deleted" };
                    db.Workflows.Add(anotherWorkflow);
                    for (int i = 0; i < 3; ++i)
                    {
                        var task = new Task { SubId = i + 1, Workflow = anotherWorkflow, Name = $"task{taskNo++}" };
                        db.Tasks.Add(task);
                    }
                    ConnectTask(db, 100, 1, 2);
                    ConnectTask(db, 100, 2, 3);
                    db.SaveChanges();
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

                var anotherWorkflow = db.Workflows.Find(100);
                if (anotherWorkflow == null)
                {
                    throw new InvalidProgramException("Unexpected result");
                }
                var t = db.Tasks.Find(100, 1);
                DeleteTaskCascade(db, t);
                db.SaveChanges();

                foreach(var t2 in anotherWorkflow.Tasks.ToList())
                {
                    DeleteTaskCascade(db, t2);
                }
                db.Remove(anotherWorkflow);
                db.SaveChanges();
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
            Console.WriteLine("  PrevTasks = [{0}]", string.Join(", ", task.PrevTaskRelations.Select(x => x.PrevTaskSubId).ToList()));
            Console.WriteLine("  NextTasks = [{0}]", string.Join(", ", task.NextTaskRelations.Select(x => x.NextTaskSubId).ToList()));
        }

        private static TaskRelation ConnectTask(WfDbContext db, int workflowId, int prevTaskId, int nextTaskId)
        {
            var prevTask = db.Tasks.Find(workflowId, prevTaskId);
            var nextTask = db.Tasks.Find(workflowId, nextTaskId);
            if (prevTask == null)
            {
                throw new ArgumentException($"prevTaskId({prevTaskId}) is not found.");
            }
            if (nextTask == null)
            {
                throw new ArgumentException($"nextTaskId({nextTaskId}) is not found.");
            }
            var rel = new TaskRelation {
                WorkflowId = workflowId, PrevTaskSubId = prevTaskId, NextTaskSubId = nextTaskId,
                PrevTask = prevTask, NextTask = nextTask };
            db.TaskRelations.Add(rel);
            return rel;
        }

        private static void DeleteTaskCascade(WfDbContext db, Task task)
        {
            foreach (var rel in task.PrevTaskRelations)
            {
                db.Remove(rel);
            }
            foreach (var rel in task.NextTaskRelations)
            {
                db.Remove(rel);
            }
            task.Workflow.Tasks.Remove(task);
            db.Remove(task);
        }
    }
}
