using KEM_WPF.Data;
using KEM_WPF.Models.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KEM_WPF.Models
{
    public class SiteManager
    {
        public static List<EquipmentEntity> _selected = new List<EquipmentEntity>();
        public static List<SiteEntity> GetList(int userId, int siteId = 0)
        {
            if (userId == 0)
                return new List<SiteEntity>();
            else
            {
                var list = new List<SiteEntity>();
                using(var context = new KEMDbContext())
                {
                    var rs = context.Sites.Where(w => w.user_id == userId).ToList();
                    if(siteId > 0)
                    {
                        rs = context.Sites.Where(w => w.user_id == userId && w.site_id == siteId).ToList();
                    }
                    foreach(var item in rs)
                    {
                        list.Add(new SiteEntity()
                        {
                            site_id = item.site_id,
                            user_id = item.user_id,
                            active = item.active,
                            description = item.description,
                            RegisteredEquipments = context.RegisteredEquipments.Where(w => w.site_id == item.site_id).ToList()

                        });
                    }
                }
                return list;
            }
        }

        public static void SetSelected(List<EquipmentEntity> selection)
        {
            _selected = new List<EquipmentEntity>();
            foreach (var s in selection)
            {
                _selected.Add(s);
            }
        }

        public static bool NewSite(SiteEntity item)
        {
            try
            {
                using (var context = new KEMDbContext())
                {
                    var newItem = new Site();
                    newItem.site_id = item.site_id;
                    newItem.user_id = item.user_id;
                    newItem.description = item.description;
                    newItem.active = item.active;
                    context.Sites.Add(newItem);
                    context.SaveChanges();

                    foreach(var re in item.SelectedRegisteredEquipments)
                    {
                        var reItem = new RegisteredEquipment();
                        reItem.site_id = newItem.site_id;
                        reItem.equipment_id = re.equipment_id;
                        context.RegisteredEquipments.Add(reItem);
                        context.SaveChanges();
                    }
                }
            }
            catch (System.Exception ex)
            {

                return false;
            }
            return true;
        }
        public static bool DeleteSite(SiteEntity item)
        {
            try
            {
                using (var context = new KEMDbContext())
                {
                    var removeItem = context.Sites.FirstOrDefault(f => f.site_id == item.site_id);
                    if(removeItem != null)
                    {
                        var removeReItems = context.RegisteredEquipments.Where(f => f.site_id == removeItem.site_id);

                        foreach(var reItem in removeReItems)
                        {
                            context.RegisteredEquipments.Remove(reItem);
                            context.SaveChanges();
                        }

                        context.Sites.Remove(removeItem);
                        context.SaveChanges();
                    }
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }
        public static bool ModifySite(SiteEntity item)
        {
            try
            {
                var siteItem = Update(item);

                using (var context = new KEMDbContext())
                {
                    var removeReItems = context.RegisteredEquipments.Where(f => f.site_id == item.site_id);

                    foreach (var reItem in removeReItems)
                    {
                        RemoveRegisteredEquipment(reItem);
                    }
                    var reqs = item.SelectedRegisteredEquipments.Distinct().ToList();
                    foreach (var re in reqs)
                    {
                        var reItem = new RegisteredEquipment();
                        reItem.site_id = siteItem.site_id;
                        reItem.equipment_id = re.equipment_id;
                        SaveRegisteredEquipment(reItem);
                    }
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }

        private static Site Update(SiteEntity item)
        {
            var updateItem = new Site();
            using (var context = new KEMDbContext())
            {
                updateItem = context.Sites.FirstOrDefault(f => f.site_id == item.site_id);
                if (updateItem != null)
                {
                    updateItem.user_id = item.user_id;
                    updateItem.description = item.description;
                    updateItem.active = item.active;

                    context.Sites.Update(updateItem);
                    context.SaveChanges();
                }
            }
            return updateItem;
        }

        private static void RemoveRegisteredEquipment(RegisteredEquipment item)
        {
            using (var context = new KEMDbContext())
            {
                context.RegisteredEquipments.Remove(item);
                context.SaveChanges();
            }
        }

        private static void SaveRegisteredEquipment(RegisteredEquipment item)
        {
            using (var context = new KEMDbContext())
            {
                context.RegisteredEquipments.Add(item);
                context.SaveChanges();
            }
        }
    }
}
