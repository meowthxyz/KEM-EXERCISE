using KEM_WPF.Models;
using KEM_WPF.Models.Entity;
using KEM_WPF.Notifications;
using KEM_WPF.Views;

namespace KEM_WPF.ViewModels
{
    public class SitesViewModel : TableModel<SiteEntity>
    {
        public SitesViewModel()
        {
            ItemName = "site";
            TableName = "Sites";
        }
        protected override void DeleteItem(object parameter)
        {
           
            string name = SelectedItem.description;
            if (SiteManager.DeleteSite(SelectedItem))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Site deleted", string.Format("Site serial number:{0}", name));
            }
            else
            {
                NotificationProvider.Error("Delete Site error", "This Site is set to one or more transactions.");
            }
        }

        protected override void EditItem(object parameter)
        {
            var Item = new SiteEntity();
            EntityCloner.CloneProperties<SiteEntity>(SelectedItem, Item);
            var EPVM = new EditSiteViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Site saved", string.Format("Site name:{0}", Item.description));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.site_id == p.site_id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            var Item = new SiteEntity();
            var EPVM = new EditSiteViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Site added", string.Format("Site name:{0}", Item.description));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.site_id == p.site_id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = SiteManager.GetList(UserManager._LoggedUser.user_id);
        }
    }

}
