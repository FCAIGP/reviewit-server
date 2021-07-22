using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewItServer.ViewModels
{
    public class VotesView
    {
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string UserId { get; set; }
        public string ReviewId { get; set; }
        public VotesView(int upVotes, int downVotes)
        {
            Upvotes = upVotes;
            Downvotes = downVotes;
        }
    }
}
