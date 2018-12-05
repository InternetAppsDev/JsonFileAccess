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

        // == constant values ==
        private const string OUTPUT_FILE = "JsonDogs.txt";

        public MainPage()
        {
            InitializeComponent();
            if (dogs == null) dogs = new List<Dogs>();
            SetUpDogsLists();
            RefreshDogsListView();
        }

        private void SetUpDogsLists()
        {
            try
            {
                // read the data to a local file
                string path = Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, OUTPUT_FILE);
                using (var reader = new StreamReader(filename))
                {
                    string jsonText = reader.ReadToEnd();
                    // use a Json parser to deserialize the text in file
                    // to a list of object of type Dogs
                    dogs = JsonConvert.DeserializeObject<List<Dogs>>(jsonText);
                }
            }
            catch
            {
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
            } // end try catch
        }

        private void lvDogs_ItemSelected(object sender, 
                                    SelectedItemChangedEventArgs e)
        {
            Dogs current = (Dogs)e.SelectedItem;
            slOneDog.BindingContext = current;
            // need to set the button to be disable as setting the 
            // data through the BindingContext causes a TextChanged
            // event to fire on the Entry boxes
            btnUpdate.IsEnabled = false;
            slOneDog.IsVisible = true;
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            // save the data to a local file
            string path = Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, OUTPUT_FILE);

            using (var streamWriter = new StreamWriter(filename, false))
            {
                string jsonText = JsonConvert.SerializeObject(dogs);
                streamWriter.WriteLine(jsonText);
            }
        }

        private void btnRead_Clicked(object sender, EventArgs e)
        {
            SetUpDogsLists();
            RefreshDogsListView();
        }

        private void OneDogEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
        }

        private void btnUpdate_Clicked(object sender, EventArgs e)
        {

            foreach (var dog in dogs)
            {
                if (lblBreed.Text == dog.Breed)
                {
                    dog.Category = lblCategory.Text;
                    dog.Origin = lblOrigin.Text;
                    dog.Grooming = lblGrooming.Text;
                    dog.Exercise = lblExercise.Text;
                }
            }
            RefreshDogsListView();
        }

        private void RefreshDogsListView()
        {
            lvDogs.ItemsSource = null;
            lvDogs.ItemsSource = dogs;
        }
    }
}
