namespace OnlineKladilnica.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Utakmica
    {
        public int Id { get; set; }

        [DisplayName("Име на утакмица")]
        public string ImeUtakmica { get; set; }

        [DisplayName("Термин")]
        public DateTime Vreme { get; set; }

        [DisplayName("Коефициент А")]
        public decimal CoefA { get; set; }

        [DisplayName("Коефициент B")]
        public decimal CoefB { get; set; }

        [ForeignKey("ATim")]
        public int? ATimeFk { get; set; }
        public virtual Tim ATim { get; set; }

        [ForeignKey("BTim")]
        public int? BTimeFk { get; set; }
        public virtual Tim BTim { get; set; }
    }
}