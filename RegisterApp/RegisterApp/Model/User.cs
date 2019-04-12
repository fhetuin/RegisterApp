using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterApp.Model
{
    [Table("Users")]
    public class User
    {
        private int id;
        private string login;
        private string password;
        private List<Result> results = new List<Result>();

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Result> Results { get => results; set => results = value; }
        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }

    }
}
