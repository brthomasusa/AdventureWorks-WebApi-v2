using System.ComponentModel.DataAnnotations.Schema;

namespace AdventureWorks.Models.Base
{
    [NotMapped]
    public class LookupItem
    {
        public LookupItem(string value, string txt)
        {
            Value = value;
            Text = txt;
        }

        public string Value { get; }

        public string Text { get; }
    }
}