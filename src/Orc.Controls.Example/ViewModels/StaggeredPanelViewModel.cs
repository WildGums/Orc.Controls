namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.MVVM;
    using Catel.Reflection;
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

            items.Add(new FontItem(ambulance.GetImageSource(), ambulance.ItemName));
            items.Add(new FontItem(cab.GetImageSource(), cab.ItemName));
            items.Add(new FontItem(plane.GetImageSource(), plane.ItemName));
            items.Add(new FontItem(wheelchair.GetImageSource(), wheelchair.ItemName));
            items.Add(new FontItem(car.GetImageSource(), car.ItemName));
            items.Add(new FontItem(rocket.GetImageSource(), rocket.ItemName));
            items.Add(new FontItem(taxi.GetImageSource(), taxi.ItemName));
            items.Add(new FontItem(bicycle.GetImageSource(), bicycle.ItemName));
            items.Add(new FontItem(bus.GetImageSource(), bus.ItemName));
            items.Add(new FontItem(truck.GetImageSource(), truck.ItemName));

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
