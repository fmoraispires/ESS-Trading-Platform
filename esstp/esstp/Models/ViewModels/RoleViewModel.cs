

namespace esstp.Models.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleValue { get; set; }
        public short? IsInactive { get; set; }


        public Role GetDbItem(Role dbItem)
        {
            dbItem.Id = Id;
            dbItem.RoleName = RoleName;
            dbItem.RoleValue = RoleValue;
            dbItem.IsInactive = IsInactive;
            return dbItem;
        }

    }
}
