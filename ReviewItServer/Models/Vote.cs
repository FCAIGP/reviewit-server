using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VoteId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string ReviewId { get; set; }
        public virtual Review Review { get; set; }
        public bool IsUpVote { get; set; }
    }
}
