namespace OnlineKladilnica.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        [ForeignKey("User")]
        public string UserFk { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int Id { get; set; }

        [DisplayName("Креирана")]
        public DateTime Created { get; set; }

        [DisplayName("Платено?")]
        public bool Platena { get; set; }

        public ICollection<TiketUtakmica> TiketUtakmicas { get; set; }
    }
}