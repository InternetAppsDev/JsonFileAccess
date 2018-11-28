using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace FileAccessSample
{
    public partial class MainPage : ContentPage
    {
        // global list to store the data
        // for CRUD
        List<Dogs> dogs;

        public MainPage()
        {
            InitializeComponent();
            SetUpLists();
            // set the dogs list as the source for the list view.
            lvDogs.ItemsSource = dogs;
        }

        private void SetUpLists()
        {
            if (dogs != null) return;
            //list is not created, instantiate and fill
            dogs = new List<Dogs>();
            // need a link to the assembly (dll) to get the file
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            // create a stream to access the file
            Stream stream = assembly.GetManifestResourceStream(
                                "FileAccessSample.Data.DogsJson.txt");
            // create a stream reader to read from the stream
            using (var reader = new StreamReader(stream))
            {
                string jsonText = reader.ReadToEnd();
                // use a Json parser to deserialize the text in file
                // to a list of object of type Dogs
                dogs = JsonConvert.DeserializeObject<List<Dogs>>(jsonText);

            }
        }

        private void lvDogs_ItemSelected(object sender, 
                                    SelectedItemChangedEventArgs e)
        {
            Dogs current = (Dogs)e.SelectedItem;
        }
    }
}
