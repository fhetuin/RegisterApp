using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RegisterApp.Model
{
    [Table("Section")]
    public class Section
    {
        private int id;
        private string name;
        private List<Element> elements;
        private int serviceId;

        public Section() { }

        public Section(string name)
        {
            this.Name = name;
            elements = new List<Element>();
        }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Element> Elements { get => elements; set => elements = value; }
        public string Name { get => name; set => name = value; }
        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        [ForeignKey(typeof(Service))]
        public int ServiceId { get => serviceId; set => serviceId = value; }
    }
}
