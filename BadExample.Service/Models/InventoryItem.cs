using System.Text.RegularExpressions;

namespace BadExample.Service.Models
{
    public class InventoryItem
    {
        private readonly Regex _nonAlphaRgx = new Regex("[^A-z]");
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = string.IsNullOrEmpty(value) ? "" : _nonAlphaRgx.Replace(value.Trim(), "");
            }
        }
        public int Id;
        public string Type;
        public int Amount;
        public decimal Cost;

        public bool IsValid()
        {
            return true;
        }
    }
}