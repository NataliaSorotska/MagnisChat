using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMagnis.DataModel

{
    class MessageManager
    {
        public void SaveMessage(Message message)
        {

            using (ChatContext context = new ChatContext())
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }
        }
        public IEnumerable<Message> GetLastMessages(int count)
        {
            using (ChatContext context = new ChatContext())
            {
                Message[] lastMessages = context.Messages
                    .OrderByDescending(m => m.MessageDate)
                    .Take(count).
                    ToArray();
                return lastMessages;
            }
        }
    }
}
