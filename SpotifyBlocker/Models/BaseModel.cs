using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyBlocker.Models
{
    class BaseModel
    {

        /// <summary>
        /// The string name of the model
        /// </summary>
        public string Name { get; set; }

        public ObservableCollection<string> Blocked { get; set; }

        public string Setting { get; set; }

        public string SelectedItem { get; set; }

        public BaseModel(string setting)
        {
            Setting = setting;
            List<string> blockedList = Properties.Settings.Default[Setting].ToString().Split(';').ToList();
            blockedList.RemoveAt(0);
            Blocked = new ObservableCollection<string>(blockedList);
            Add();
        }

        /// <summary>
        /// Deletes the base from the settings and from the obeservable collection
        /// </summary>
        public void Delete()
        {
            string oldSetting = Properties.Settings.Default[Setting].ToString();
            Properties.Settings.Default[Setting] = oldSetting.Replace($";{SelectedItem}", "");
            Properties.Settings.Default.Save();

            Blocked.Remove(SelectedItem);
        }

        /// <summary>
        /// Ads a base to the settings and the observable collection
        /// </summary>
        public void Add()
        {
            if (string.IsNullOrEmpty(Name)) {
                return;
            }
            string oldSetting = Properties.Settings.Default[Setting].ToString();
            Properties.Settings.Default[Setting] = $"{oldSetting};{Name}";

            Properties.Settings.Default.Save();
            Blocked.Add(Name);
        }
    }
}
