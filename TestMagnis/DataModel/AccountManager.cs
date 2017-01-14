using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestMagnis.DataModel
{
    class AccountManager
    {
        public bool RegisterAccount(Account account)
        {
            using (ChatContext context = new ChatContext())
            {
                bool isExisted = context.Accounts.Any(a => a.Name == account.Name);
                if (!isExisted)
                {
                    context.Accounts.Add(account);
                    context.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }

            }
        }


        public Account GetAccount(string name, string password)
        {
            using (var context = new ChatContext())
            {
                Account person = context.Accounts.FirstOrDefault(p => p.Name == name && p.Password == password);
                return person;
            }
        }
    }
}

