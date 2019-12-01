using System;
using esstp.Models.Services;

namespace esstp.Models.ViewModels
{
    public class ObserverModel
    {

        public int Id { get; set; }
        public string Action { get; set; }
        public Observer O { get; set; }

        public ObserverModel()
        {
            this.Id = 0;
            this.Action = string.Empty;
            this.O = null;
        }
    }
}
