using System.ComponentModel.DataAnnotations;

namespace MiniChatApp.Models
{
    public class Zprava
    {

        public int Id { get; set; } 

        [Required]
        public string UzivatelskeJmeno { get; set; } = "";

        [Required]
        public string Text { get; set; } = "";
        
        public DateTime Cas { get; set; } = DateTime.Now;
        }

    
}
