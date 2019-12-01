

namespace esstp.Models.ViewModels
{

    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Nif { get; set; }
        public short? IsInactive { get; set; }
        public int? Role_id { get; set; }


        public User GetDbItem(User dbItem)
        {
            dbItem.Id = Id;
            dbItem.UserName = UserName;
            dbItem.Email = Email;
            dbItem.Nif = Nif;
            dbItem.IsInactive = IsInactive;
            dbItem.Role_id = Role_id;
            return dbItem;
        }
    }




}

