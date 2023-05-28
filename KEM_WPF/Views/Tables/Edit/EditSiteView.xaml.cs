using KEM_WPF.Models;
using KEM_WPF.Models.Entity;
using KEM_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KEM_WPF.Views
{
    /// <summary>
    /// Interaction logic for EditProductView.xaml
    /// </summary>
    public partial class EditSiteView : UserControl
    {
        public EditSiteView()
        {
            InitializeComponent();
        }

        private void listBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = listBoxItem.SelectedItems;
            var lists = new List<EquipmentEntity>();
            foreach(var item in items)
            {
                var i = (EquipmentEntity)item;
                if(i.IsSelected)
                {
                    lists.Add(i);
                }
            }
            SiteManager.SetSelected(lists);
        }
    }
}
