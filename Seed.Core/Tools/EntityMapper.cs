using System;
using System.Collections.Generic;

namespace Seed.Core.Tools
{
    public static class EntityMapper
    {
        public static Dictionary<string, string> TableNames;

        static EntityMapper()
        {
            EntityMapper.TableNames = new Dictionary<string, string>();
            TableNames.Add("Menu", "menu");
            TableNames.Add("MenuAction", "menu_action");
            TableNames.Add("Role", "role");
            TableNames.Add("RoleMenuAction", "role_menu_action");
            TableNames.Add("Store", "store");
            TableNames.Add("StoreSys", "store_sys");
            TableNames.Add("Sys", "sys");
            TableNames.Add("User", "user");
            TableNames.Add("UserRole", "user_role");
        }

        public static string GetTableName(Type type)
        {
            foreach (var item in EntityMapper.TableNames)
            {
                if (item.Key == type.Name)
                    return item.Value;
            }
            return null;
        }

        public static bool Add(string key, string value)
        {
            if (EntityMapper.TableNames.ContainsKey(key))
                return false;
            EntityMapper.TableNames.Add(key, value);
            return true;
        }

        public static bool Remove(string key)
        {
            if (EntityMapper.TableNames.ContainsKey(key))
            {
                EntityMapper.TableNames.Remove(key);
                return true;
            }
            return false;
        }
    }
}
