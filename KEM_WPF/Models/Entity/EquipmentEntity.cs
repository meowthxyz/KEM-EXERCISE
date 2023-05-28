using KEM_WPF.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEM_WPF.Models.Entity
{
    public class EquipmentEntity : Equipment
    {
        public EquipmentEntity() { }

        public bool IsSelected { get; set; }

    }
}
