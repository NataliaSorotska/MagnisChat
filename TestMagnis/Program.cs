using TestMagnis.DataModel;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace TestMagnis
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ChatContext context = new ChatContext())
            {
                Account account = new Account();
                account.Name = "Natalia";
                account.Password = "1111";

                var accountManager = new AccountManager();
                accountManager.RegisterAccount(account);
              //  Console.ReadLine();
                Account acc = accountManager.GetAccount("Natalia","1111");
                if (acc != null)
                {
                    Console.WriteLine(acc.Name);
                }
                else
                {
                    Console.WriteLine("sorry");
                    Console.ReadLine();
                    return;
                }

                Message message;
                var messagemanager = new MessageManager();

                for (int i = 0; i <1; i++)
                {
                    message = new Message();
                    message.MessageText = "Hello my Darling. Soon our Holiday" + i.ToString();
                    message.MessageDate = DateTime.UtcNow;
                    message.AccountId = account.AccountId;

                    messagemanager.SaveMessage(message);
                    Task.Delay(1000).Wait();
                }

                var messages = messagemanager.GetLastMessages(500).ToList();
                messages.ForEach(x => Console.WriteLine($"{x.MessageId} {x.AccountId} {x.MessageText} {x.MessageDate}"));

                Console.ReadLine();
                Console.ReadLine();
            }
        }
    }
}
