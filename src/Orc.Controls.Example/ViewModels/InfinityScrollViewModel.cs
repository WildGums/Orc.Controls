namespace Orc.Controls.Example.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using Catel.Collections;
    using Catel.MVVM;
    using Orc.Theming;

    public class InfinityScrollViewModel : ViewModelBase
    {
        private readonly List<FontItem> _source = GetFontItems();
        private readonly Random _randomizer = new Random(DateTime.Now.Millisecond);

        public InfinityScrollViewModel()
        {
            ListItems = new ObservableCollection<FontItem>();
            AddItems = new TaskCommand(OnAddItemsExecuteAsync);

            Count = 10;
            ListItems.AddRange(GetNextItems(30));
        }

        public ObservableCollection<FontItem> ListItems { get; set; }

        public TaskCommand AddItems { get; set; }

        public int Count { get; set; }

        private async Task OnAddItemsExecuteAsync()
        {
            ListItems.AddRange(GetNextItems(Count));
        }

        private List<FontItem> GetNextItems(int count)
        {
            var items = new List<FontItem>();

            for (int i = 0; i < count; i++)
            {
                var randomIndex = _randomizer.Next(0, _source.Count - 1);
                items.Add(_source[randomIndex]);
            }

            return items;
        }

        private static List<FontItem> GetFontItems()
        {
            var items = new List<FontItem>();
            var ambulance = new FontImage(FontAwesome.Ambulance) { FontFamily = nameof(FontAwesome) };
            var cab = new FontImage(FontAwesome.Cab) { FontFamily = nameof(FontAwesome) };
            var plane = new FontImage(FontAwesome.Plane) { FontFamily = nameof(FontAwesome) };
            var wheelchair = new FontImage(FontAwesome.Wheelchair) { FontFamily = nameof(FontAwesome) };
            var car = new FontImage(FontAwesome.Automobile) { FontFamily = nameof(FontAwesome) };
            var rocket = new FontImage(FontAwesome.Rocket) { FontFamily = nameof(FontAwesome) };
            var taxi = new FontImage(FontAwesome.Taxi) { FontFamily = nameof(FontAwesome) };
            var bicycle = new FontImage(FontAwesome.Bicycle) { FontFamily = nameof(FontAwesome) };
            var bus = new FontImage(FontAwesome.Bus) { FontFamily = nameof(FontAwesome) };
            var truck = new FontImage(FontAwesome.Truck) { FontFamily = nameof(FontAwesome) };

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
