using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebAPI.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string AccountNumber { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AccountHolder { get; set; }
        [Required]
        public int BankID { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [Required]
        public string IFSC { get; set; }
        [Column(TypeName = "nvarchar(450)")]
        public string UserID { get; set; }
    }
}
