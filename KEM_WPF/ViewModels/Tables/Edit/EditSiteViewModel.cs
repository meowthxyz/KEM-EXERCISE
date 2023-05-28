using KEM_WPF.Data;
using KEM_WPF.Models;
using KEM_WPF.Models.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace KEM_WPF.ViewModels
{
    class EditSiteViewModel : EditItemModel<SiteEntity>
    {
        public EditSiteViewModel(SiteEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName) { }

        public List<EquipmentEntity> Items
        {
            get
            {
                var _categoryList = EquipmentManager.GetList(UserManager._LoggedUser.user_id).ToList();
                if(Item.RegisteredEquipments.Count > 0)
                {
                    foreach(var c in _categoryList)
                    {
                        var check = Item.RegisteredEquipments.FirstOrDefault(f => f.equipment_id == c.equipment_id);
                        if(check != null)
                        {
                            c.IsSelected = true;
                        }
                    }
                }
                return _categoryList;
            }
        }

        protected override bool Save(object parameter)
        {
            bool result = false;
            var selected = SiteManager._selected.Distinct().ToList();
            foreach(var s in selected)
            {
                if(s.IsSelected)
                {
                    Item.SelectedRegisteredEquipments.Add(new RegisteredEquipment()
                    {
                        equipment_id = s.equipment_id,
                        site_id = Item.site_id,
                    });
                }
            }
            Item.user_id = UserManager._LoggedUser.user_id; //override user id before saving data
            if (NewRecord)
            {
                result = SiteManager.NewSite(Item);
            }
            else
            {
                result = SiteManager.ModifySite(Item);
            }
            return result;
        }
    }
}
