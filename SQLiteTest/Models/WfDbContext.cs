//using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SQLiteTest.Models
{
    public class WfDbContext : DbContext
    {
        public DbSet<Workflow> Workflows { get; internal set; }

        public DbSet<Task> Tasks { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            // プロジェクト直下にデータベースファイルを配置するため、モジュールのパス起点のフルパスを用いる。
            // Update-Database はソリューションフォルダで実行されるのに対し、モジュールのデバッグ実行ではデフォルトでは出力フォルダを作業フォルダとして
            // 実行してしまうようで、相対パスでは同じ場所を指定することができない。
            string modulePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string filePath = System.IO.Path.Combine(modulePath, "../../../workflow.sqlite3");
            string connectionString = new SqliteConnectionStringBuilder { DataSource = filePath }.ToString();
            optionBuilder.UseSqlite(new SqliteConnection(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workflow>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<Task>(t =>
            {
                t.HasKey(x => new { x.WorkflowId, x.SubId });
                t.HasIndex(x => new { x.WorkflowId, x.SubId });
            });

            // 以下はなくても動く
#if false
            modelBuilder.Entity<Workflow>()
                .HasMany<Task>(x => x.Tasks)
                .WithOne(x => x.Workflow)
                .HasForeignKey(x => x.WorkflowId)
                .IsRequired();
#endif
        }
    }
}
