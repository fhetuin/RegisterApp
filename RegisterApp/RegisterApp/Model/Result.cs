using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RegisterApp.Model
{
    [Table("Results")]
    public class Result
    {
        private int id;
        private int elementId;
        private int userId;
        private int serviceId;
        private string resultat;
        private bool? isChecked;

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        [ForeignKey(typeof(Element))]
        public int ElementId { get => elementId; set => elementId = value; }
        [ForeignKey(typeof(User))]
        public int UserId { get => userId; set => userId = value; }
        [ForeignKey(typeof(Service))]
        public int ServiceId { get => serviceId; set => serviceId = value; }
        public string Resultat { get => resultat; set => resultat = value; }
        public bool? IsChecked { get => isChecked; set => isChecked = value; }

        public override string ToString()
        {
            return resultat;
        }
    }
}
