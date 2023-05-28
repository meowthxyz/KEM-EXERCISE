using KEM_WPF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEM_WPF.Models.Entity
{
    public class SiteEntity : Site
    {
        public SiteEntity() 
        {
            this.RegisteredEquipments = new List<RegisteredEquipment>();
            this.SelectedRegisteredEquipments = new List<RegisteredEquipment>();
        }

        public List<RegisteredEquipment> RegisteredEquipments { get; set; }
        public List<RegisteredEquipment> SelectedRegisteredEquipments { get; set; }
    }
}
