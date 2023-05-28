using KEM_WPF.Models;
using KEM_WPF.Models.Entity;
using KEM_WPF.Notifications;
using KEM_WPF.Views;

namespace KEM_WPF.ViewModels
{
    public class EquipmentsViewModel : TableModel<EquipmentEntity>
    {
        public EquipmentsViewModel()
        {
            ItemName = "equipment";
            TableName = "Equipments";
        }
        protected override void DeleteItem(object parameter)
        {
           
            string name = SelectedItem.serial_number;
            if (EquipmentManager.DeleteEquipment(SelectedItem))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Equipment deleted", string.Format("Equipment serial number:{0}", name));
            }
            else
            {
                NotificationProvider.Error("Delete equipment error", "This equipment is set to one or more transactions.");
            }
        }

        protected override void EditItem(object parameter)
        {
            var Item = new EquipmentEntity();
            EntityCloner.CloneProperties<EquipmentEntity>(SelectedItem, Item);
            var EPVM = new EditEquipmentViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Equipment saved", string.Format("Equipment name:{0}", Item.serial_number));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.equipment_id == p.equipment_id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            var Item = new EquipmentEntity();
            Item.condition = "Working"; //default value: Working
            var EPVM = new EditEquipmentViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Equipment added", string.Format("Equipment name:{0}", Item.serial_number));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.equipment_id == p.equipment_id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = EquipmentManager.GetList(UserManager._LoggedUser.user_id);
        }
    }

}
