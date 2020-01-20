namespace OnlineKladilnica.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TiketUtakmica
    {
        public int Id { get; set; }

        [DisplayName("Резултат на тим: ")]
        public decimal TimAResult { get; set; }

        [DisplayName("Резултат на тим: ")]
        public decimal TimBResult { get; set; }

        [DisplayName("Облог")]
        public decimal Oblog { get; set; }

        [DisplayName("Заработка")]
        public decimal Zarabotka { get; set; }

        [ForeignKey("Ticket")]
        public int TicketFk { get; set; }
        public virtual Ticket Ticket { get; set; }

        [ForeignKey("Utakmica")]
        public int UtakmicaFk { get; set; }
        public virtual Utakmica Utakmica { get; set; }
    }
}