using KEM_WPF.Data;
using KEM_WPF.Models.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KEM_WPF.Models
{
    public class EquipmentManager
    {
        public static List<EquipmentEntity> GetList(int userId)
        {
            if (userId == 0)
                return new List<EquipmentEntity>();
            else
            {
                var list = new List<EquipmentEntity>();
                using(var context = new KEMDbContext())
                {
                    var rs = context.Equipments.Where(w => w.user_id == userId).ToList();
                    foreach(var item in rs)
                    {
                        list.Add(new EquipmentEntity()
                        {
                            equipment_id = item.equipment_id,
                            user_id = item.user_id,
                            condition = item.condition,
                            description = item.description,
                            serial_number = item.serial_number
                        });
                    }
                }
                return list;
            }
                
        }

        public static bool NewEquipment(EquipmentEntity item)
        {
            try
            {
                using (var context = new KEMDbContext())
                {
                    var newItem = new Equipment();
                    newItem.equipment_id = item.equipment_id;
                    newItem.user_id = item.user_id;
                    newItem.condition = item.condition;
                    newItem.description = item.description;
                    newItem.serial_number = item.serial_number;
                    context.Equipments.Add(newItem);
                    context.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {

                return false;
            }
            return true;
        }
        public static bool DeleteEquipment(EquipmentEntity item)
        {
            try
            {
                using (var context = new KEMDbContext())
                {
                    var removeItem = context.Equipments.FirstOrDefault(f => f.equipment_id == item.equipment_id);
                    if(removeItem != null)
                    {
                        context.Equipments.Remove(removeItem);
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
        public static bool ModifyEquipment(EquipmentEntity item)
        {
            try
            {
                using (var context = new KEMDbContext())
                {
                    var updateItem = context.Equipments.FirstOrDefault(f => f.equipment_id == item.equipment_id);
                    if (updateItem != null)
                    {
                        updateItem.user_id = item.user_id;
                        updateItem.condition = item.condition;
                        updateItem.description = item.description;
                        updateItem.serial_number = item.serial_number;
                        context.Equipments.Update(updateItem);
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

        //public static List<ProductCategoryEntity> ListProductCategories()
        //{
        //    return ProductCategoryProvider.List(p => true);
        //}

        //public static bool NewProductCategory(ProductCategoryEntity category)
        //{
        //    return ProductCategoryProvider.Add(category);
        //}
        //public static bool DeleteProductCategory(ProductCategoryEntity category)
        //{
        //    return ProductCategoryProvider.Remove(category);
        //}
        //public static bool ModifyProductCategory(ProductCategoryEntity category)
        //{
        //    return ProductCategoryProvider.Modify(category);
        //}



    }
}
