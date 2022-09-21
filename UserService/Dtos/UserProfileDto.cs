namespace Identity.Dtos
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsBanned { get; set; }
    }
}
