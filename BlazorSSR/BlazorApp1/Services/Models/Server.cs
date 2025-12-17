using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Services.Models
{
    public class Server
    {
        public Server()
        {
            Random rand = new Random();
            int randomNumber = rand.Next(0, 2);
            IsOnline = randomNumber == 0 ? false : true;
        }

        public int Id { get; set; }
        public bool IsOnline { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? City { get; set; }
    }
}
