using System;
using System.Data.Entity;
using System.Linq;
using TestMagnis.DataModel;

namespace TestMagnis
{
    public class ChatContext : DbContext
    {
        public ChatContext()
            : base("name=ChatContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ChatContext>());
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Message> Messages { get; set; } 
    }
}
