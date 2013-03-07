using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO.IsolatedStorage;


namespace Waetter_Mobile
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private int plz;
        public int Plz
        {
            get { return plz; }
            set
            {
                plz = value;
                OnPropertyChanged("Plz");
                var localSettings = IsolatedStorageSettings.ApplicationSettings;
                if (localSettings.Contains("plz"))
                    localSettings["plz"] = value;
                else
                    localSettings.Add("plz", value);

            }
        }

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged("ImageSource"); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged("IsLoading"); }
        }
        public MainViewModel()
        {
            var localSettings = IsolatedStorageSettings.ApplicationSettings;

            if (localSettings.Contains("plz"))
                Plz = (int)localSettings["plz"];
            else
                Plz = 6300;
                        
        }
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void LoadData()
        {
            IsLoading = true;

            var client = new WebClient();

            try
            {
                string webcontent = await client.DownloadStringTask(new Uri("http://meteo.search.ch/" + Plz));


                string uriString = "http://meteo.search.ch/images/chart/" + GetImageUrl(webcontent);
                var source = new BitmapImage(new Uri(uriString));
                
                ImageSource = source;
                ExtractDescription(webcontent);

            }
            catch (Exception e) { }
            finally { IsLoading = false; }
        }

        private string GetImageUrl(string webcontent)
        {
            try
            {
                string search = "<div class=\"meteo_chartcontainer\"><img src=\"/images/chart/";
                int index = webcontent.IndexOf(search);
                string substring = webcontent.Substring(index);
                substring = substring.Remove(0, search.Length);
                return substring.Substring(0, substring.IndexOf("\""));
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string searchAnchor = "Lokale Prognose für";
        private void ExtractDescription(string webcontent)
        {
            try
            {

                int index = webcontent.IndexOf(searchAnchor);
                string substring = webcontent.Substring(index);
                Description = substring.Substring(0, substring.IndexOf("<"));
            }
            catch (Exception)
            {
                SetErrorDescription("PLZ nicht gefunden");
            }
        }

        public void SetErrorDescription(string message)
        {
            Description = message;
            ImageSource = null;
        }

        
    }
}
