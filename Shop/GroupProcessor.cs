using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shop
{
    public class GroupProcessor
    {
        public static List<Group> GetHierarchicalGroupList()
        {
            DataTable groupsTable = API.SelectAllFromTable("Product_group");

            List<Group> groups = new List<Group>();

            //Add main groups (groups without parent)

            foreach (DataRow row in groupsTable.Rows)
            {
                if (row["parent_group_id"] is DBNull)
                {
                    groups.Add(new Group
                    {
                        Id = (int)row["id"],
                        Name = row["name"].ToString()
                    });
                }
            }

            //Add other groups (with parents)

            while (groupsTable.Rows.Count > 0)
            {
                foreach (DataRow row in groupsTable.Rows)
                {
                    if (row["parent_group_id"] is DBNull)
                    {
                        groupsTable.Rows.Remove(row);
                        break;
                    }

                    var parentGroup = FindGroupById((int)row["parent_group_id"], groups);

                    if (parentGroup != null)
                    { 
                        if (parentGroup.Groups is null)
                            parentGroup.Groups = new List<Group>();

                        var childGroup = new Group
                        {
                            Id = (int)row["id"],
                            Name = row["name"].ToString()
                        };

                        parentGroup.Groups.Add(childGroup);

                        groupsTable.Rows.Remove(row);
                        break;
                    }
                }
            }
            return groups;
        }

        private static Group FindGroupById(int id, List<Group> groups)
        {
            if (groups == null)
                return null;

            var group = groups.FirstOrDefault(g => g.Id == id);

            if(group != null)
                return group;

            foreach (var g in groups)
            {
                var result = FindGroupById(id, g.Groups);
                if (!(result is null))
                    return result;
            }
            
            return null;
        }

        public static string GetGroupPathById(int id, List<Group> groups)
        {
            string path = "";

            if (groups is null)
                return path;

            var group = groups.FirstOrDefault(g => g.Id == id);

            if (group is null)
            {
                foreach (var g in groups)
                {
                    var localPath = GetGroupPathById(id, g.Groups);
                    if (localPath != "") 
                        path = $"{g.Name}/{localPath}" + path;
                }
            }
            else
            {
                path = group.Name + path;
            }
            
            return path;
        }
    }
}
