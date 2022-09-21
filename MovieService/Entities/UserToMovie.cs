using Identity.Models;

namespace MovieService.Entities
{
    public class UserToMovie
    {
        public string ApplicationUserId { get; set; }        

        public int MovieId { get; set; }

        public bool IsLiked { get; set; }

        public bool IsDisLiked { get; set; }

        public bool IsWatched { get; set; }
    }
}
