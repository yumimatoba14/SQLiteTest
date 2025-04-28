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
                    var workflow = new Workflow { Id = 1, Name = "Workflow1" };
                    db.Workflows.Add(workflow);

                    db.SaveChanges();
                    Console.WriteLine("Data has been added.");
                }
            }
            using (var db = new WfDbContext())
            {
                int id = 1;
                var workflow = db.Workflows.Find(id);
                if (workflow != null)
                {
                    Console.WriteLine("workflow.Name = {0}", workflow.Name);
                }
                else
                {
                    Console.WriteLine("Workflow(Id = {0}) is not found.", id);
                }
            }
        }
    }
}
