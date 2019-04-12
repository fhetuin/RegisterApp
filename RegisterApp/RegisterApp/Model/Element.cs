using Android.Widget;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RegisterApp.Model
{
    [Table("Element")]
    public class Element
    {

        private int id;
        private int sectionId;
        private bool mandatory;
        private List<Value> values;
        private string type;

        public Element() { }

        public Element(bool mandatory, List<Value> values, string type)
        {
            this.Mandatory = mandatory;
            this.Type = type;
            this.Values = values; 
        }

        public bool Mandatory { get => mandatory; set => mandatory = value; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Value> Values { get => values; set => values = value; }
        public string Type { get => type; set => type = value; }
        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        [ForeignKey(typeof(Section))]
        public int SectionId { get => sectionId; set => sectionId = value; }

    }
}
