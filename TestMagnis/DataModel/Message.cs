using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMagnis.DataModel
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public string MessageText { get; set; }

        [Required]
        public DateTime MessageDate { get; set; }

        [Required]
        public int AccountId { get; set; }
    }
}
