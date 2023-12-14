namespace Orc.Controls.Example.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.MVVM;
    using Orc.Theming;

    public class StaggeredPanelViewModel : ViewModelBase
    {
        public StaggeredPanelViewModel()
        {
            ColumnSpacing = 10;
            RowSpacing = 10;
            DesiredColumnWidth = 50;
            ItemWidth = 50;
        }

        protected override Task InitializeAsync()
        {
            Items = GetFontItems();
            return base.InitializeAsync();
        }

        public double DesiredColumnWidth { get; set; }

        public double ColumnSpacing { get; set; }

        public double RowSpacing { get; set; }

        public double ItemWidth { get; set; }

        public IEnumerable<FontItem> Items { get; set; }

        private List<FontItem> GetFontItems()
        {
            var items = new List<FontItem>();
            var ambulance = new FontImage(FontAwesome.Ambulance) { FontFamily =  nameof(FontAwesome) };
            var cab = new FontImage(FontAwesome.Cab) { FontFamily = nameof(FontAwesome) };
            var plane = new FontImage(FontAwesome.Plane) { FontFamily = nameof(FontAwesome) };
            var wheelchair = new FontImage(FontAwesome.Wheelchair) { FontFamily = nameof(FontAwesome) };
            var car = new FontImage(FontAwesome.Automobile) { FontFamily = nameof(FontAwesome) };
            var rocket = new FontImage(FontAwesome.Rocket) { FontFamily = nameof(FontAwesome) };
            var taxi = new FontImage(FontAwesome.Taxi) { FontFamily = nameof(FontAwesome) };
            var bicycle = new FontImage(FontAwesome.Bicycle) { FontFamily = nameof(FontAwesome) };
            var bus = new FontImage(FontAwesome.Bus) { FontFamily = nameof(FontAwesome) };
            var truck = new FontImage(FontAwesome.Truck) { FontFamily = nameof(FontAwesome) };

            for (int i = 0; i < 3; i++)
            {
                items.Add(new FontItem(ambulance.GetImageSource(), nameof(FontAwesome.Ambulance)));
                items.Add(new FontItem(cab.GetImageSource(), nameof(FontAwesome.Cab)));
                items.Add(new FontItem(plane.GetImageSource(), nameof(FontAwesome.Plane)));
                items.Add(new FontItem(wheelchair.GetImageSource(), nameof(FontAwesome.Wheelchair)));
                items.Add(new FontItem(car.GetImageSource(), nameof(FontAwesome.Automobile)));
                items.Add(new FontItem(rocket.GetImageSource(), nameof(FontAwesome.Rocket)));
                items.Add(new FontItem(taxi.GetImageSource(), nameof(FontAwesome.Taxi)));
                items.Add(new FontItem(bicycle.GetImageSource(), nameof(FontAwesome.Bicycle)));
                items.Add(new FontItem(bus.GetImageSource(), nameof(FontAwesome.Bus)));
                items.Add(new FontItem(truck.GetImageSource(), nameof(FontAwesome.Truck)));
            }

            return items;
        }

        public class FontItem
        {
            public FontItem(ImageSource image, string name)
            {
                Image = image;
                Name = name;
            }

            #region Properties
            public ImageSource Image { get; set; }
            public string Name { get; private set; }
            #endregion
        }
    }
}
