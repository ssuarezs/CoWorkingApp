using Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Desk
    {
        public Guid DeskId { get; set; } = Guid.NewGuid();
        public string Number {get;set;}
        public string Description {get;set;}
        public DeskStatus DeskStatus { get; set; } = DeskStatus.Active;
    }
}
