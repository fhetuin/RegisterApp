using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterApp.Model
{
    [Table("Value")]
    public class Value
    {
        private int id;
        private int elementId;
        private string strValue;

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        [ForeignKey(typeof(Element))]
        public int ElementId { get => elementId; set => elementId = value; }
        public string StrValue { get => strValue; set => strValue = value; }

        public override string ToString()
        {
            return StrValue;
        }
    }
}
