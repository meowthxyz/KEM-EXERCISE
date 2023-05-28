using KEM_WPF.Models;
using KEM_WPF.Models.Entity;
using System.Collections.Generic;


namespace KEM_WPF.ViewModels
{
    class EditEquipmentViewModel : EditItemModel<EquipmentEntity>
    {
        public EditEquipmentViewModel(EquipmentEntity item, bool newRecord, string itemName) : base(item, newRecord, itemName) { }

        private List<string> _categoryList;
        public List<string> CategoryList
        {
            get
            {
                if (_categoryList == null)
                {
                    _categoryList = new List<string>();
                    _categoryList.Add("Working");
                    _categoryList.Add("Not Working");
                }
                return _categoryList;
            }
            set { SetProperty(ref _categoryList, value); }
        }

        protected override bool Save(object parameter)
        {
            bool result = false;
            Item.user_id = UserManager._LoggedUser.user_id; //override user id before saving data
            if (NewRecord)
            {
                result = EquipmentManager.NewEquipment(Item);
            }
            else
            {
                result = EquipmentManager.ModifyEquipment(Item);
            }
            return result;
        }
    }
}
