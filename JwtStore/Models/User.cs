namespace JwtStore.Models
{
    public record User
    {
        public int id;

        public User(int id, string name, string email, string image, string password, string[] roles)
        {
            this.id = id;
            Name = name;
            Email = email;
            Image = image;
            Password = password;
            Roles = roles;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; } 
    }
}
