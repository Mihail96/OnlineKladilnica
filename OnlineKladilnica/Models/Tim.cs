using System.ComponentModel;

namespace OnlineKladilnica.Models
{
    public class Tim
    {
        public int Id { get; set; }

        [DisplayName("Име на тим")]
        public string Ime { get; set; }
    }
}