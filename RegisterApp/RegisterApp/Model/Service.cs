using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace RegisterApp.Model
{
    [Table("Service")]
    public class Service
    {

        private int id;
        private string title;
        private string logo;
        private List<Section> sections;

        public Service() { }

        public Service(string title)
        {
            this.title = title;
            this.Sections = new List<Section>();
        }

        public string Title { get => title; set => title = value; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Section> Sections { get => sections; set => sections = value; }
        public string Logo { get => logo; set => logo = value; }

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
    }
}
