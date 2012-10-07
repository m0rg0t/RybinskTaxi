using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;

// Модель данных, определяемая этим файлом, служит типичным примером строго типизированной
// модели, которая поддерживает уведомление при добавлении, удалении или изменении членов. Выбранные
// имена свойств совпадают с привязками данных из стандартных шаблонов элементов.
//
// Приложения могут использовать эту модель в качестве начальной точки и добавлять к ней дополнительные элементы или полностью удалить и
// заменить ее другой моделью, соответствующей их потребностям.

namespace RybinskTaxi.Data
{
    /// <summary>
    /// Базовый класс объектов <see cref="SampleDataItem"/> и <see cref="SampleDataGroup"/>, который
    /// определяет свойства, общие для обоих объектов.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : RybinskTaxi.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Универсальная модель данных элементов.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Универсальная модель данных групп.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Предоставляет подмножество полной коллекции элементов, привязываемой из объекта GroupedItemsPage
            // по двум причинам: GridView не виртуализирует большие коллекции элементов и оно
            // улучшает работу пользователей при просмотре групп с большим количеством
            // элементов.
            //
            // Отображается максимальное число столбцов (12), поскольку это приводит к заполнению столбцов сетки
            // сколько строк отображается: 1, 2, 3, 4 или 6

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Создает коллекцию групп и элементов с жестко заданным содержимым.
    /// 
    /// SampleDataSource инициализируется подстановочными данными, а не реальными рабочими
    /// данными, чтобы пример данных был доступен как во время разработки, так и во время выполнения.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            try
            {
                
                if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            }
            catch { };
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Такси Рыбиснка",
                    "",
                    "Assets/DarkGray.png",
                    "");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Такси Эконом",
                    "http://www.2rybinsk.ru/taxi-ekonom/",
                    "Assets/taksi-ekonom1.jpg",
                    "для домашних тел. 22-33-33, 22-26-66 для сотовых тел. 8-915-997-99-77, 8-906-526-777-6, 8-920-118-33-33, 93-33-09", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                                "Такси Рыбинск",
                                "http://www.2rybinsk.ru/taxi-rybinsk/",
                                "Assets/Rubinsk1.gif",
                                "для домашних тел. 245-245, 246-246 \nдля сотовых тел. 8-905-139-05-05, 8-910-664-36-34, 8-902-330-88-88", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                                "Такси Лада",
                                "http://www.2rybinsk.ru/taxi-lada/",
                                "Assets/taksi-lada1.jpg",
                                "для домашних тел. 280-380 \nдля сотовых тел. 919-999,8-920-100-40-00, 8-910-666-60-50, 8-903-822-78-78", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                                "Такси Марсель",
                                "http://www.2rybinsk.ru/taxi-marsel/",
                                "Assets/taksi-marsel1.jpg",
                                "для домашних тел. 289-999, 25-33-33 \nдля сотовых тел. 909-888, 8-903-829-98-88, 8-920-105-88-88, 8-910-818-99-88", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                                "Такси Круиз",
                                "http://www.2rybinsk.ru/taxi-kruiz/",
                                "Assets/kruiz1.jpg",
                                "для сотовых тел. 95-45-65 и 8-920-107-22-22 \nдля домашних тел. 25-45-65, 25-15-15", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                                "Такси Регион",
                                "http://www.2rybinsk.ru/taxi-region/",
                                "Assets/taksi-region1.jpg",
                                "для домашних тел. 253-888 \nдля сотовых тел. 909-909", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                                "Такси На Дубровку",
                                "http://www.2rybinsk.ru/taxi-na-dybrovky/",
                                "Assets/taksi-na-dybrovky1.jpg",
                                "для домашних тел. 222-333, 266-201 \nдля сотовых тел. 8-915-966-55-55, 8-961-022-22-22, 900-006", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                                "Такси Димон",
                                "http://www.2rybinsk.ru/taxi-dimon/",
                                "Assets/dimon-taxi1.jpg",
                                "Стационарный телефон: 20-20-20 \nБилайн: 333-138, МТС: 595-615, Мегафон: 929-07-79-777", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                                "Такси Пилот",
                                "http://www.2rybinsk.ru/taxi-pilot/",
                                "Assets/Pilot1.gif",
                                "для домашних тел. 251-111 \nдля сотовых тел. 8-906-525-82-00", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                                "Наше такси",
                                "http://www.2rybinsk.ru/taxi-nashe/",
                                "Assets/Nashe_taxi1.gif",
                                "для домашних тел. 25-44-44 \nдля сотовых тел. 90-48-55", "",
                                group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-11",
                    "Такси Экипаж",
                    "http://www.2rybinsk.ru/taxi-ekipazh/",
                    "Assets/ekipazh1.jpg",
                    "для домашних тел. 555-556, 558885, 227-227 \nдля сотовых тел. 8-905-134-41-41, 8-902-334-88-85", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-22",
                    "Такси Универсал",
                    "http://www.2rybinsk.ru/taxi-universal/",
                    "Assets/universal1.jpg",
                    "Такси Универсал", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-13",
        "Такси Недорогое",
        "http://www.2rybinsk.ru/taxi-nedorogoe/",
        "Assets/LightGray.png",
        "для домашних тел. 222-222 \nдля сотовых тел. 8-920-116-06-00, 8-905-636-11-44, 8-980-662-02-02", "",
        group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-14",
        "Такси Переборы",
        "http://www.2rybinsk.ru/taxi-perebori/",
        "Assets/LightGray.png",
        "для домашних тел. 598-522 \nдля сотовых тел. 8-906-525-82-00", "",
        group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-15",
        "Такси Городок",
        "http://www.2rybinsk.ru/taxi-gorodok/",
        "Assets/LightGray.png",
        "для домашних тел. 284-284 \nдля сотовых тел. 95-65-65", "",
        group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-16",
        "Такси Вояж",
        "http://www.2rybinsk.ru/taxi-voyaz/",
        "Assets/LightGray.png",
        "для домашних тел. 210-444, 222-323 \nдля сотовых тел. 8-905-639-94-99, 8-915-992-07-05", "",
        group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-17",
                    "Такси Галактика",
                    "http://www.2rybinsk.ru/taxi-galaktika/",
                    "Assets/LightGray.png",
                    "для домашних тел. 295-555, 280-606 \nдля сотовых тел. 919-000, 8-905-633-13-13, 8-915-986-06-06, 8-920-223-55-55", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-18",
                    "Такси Гора",
                    "http://www.2rybinsk.ru/taxi-gora/",
                    "Assets/LightGray.png",
                    "для домашних тел. 281-222, 219-596 \nдля сотовых тел. 930-900, 8-903-827-72-72", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-19",
                    "Такси Центр",
                    "http://www.2rybinsk.ru/taxi-centr/",
                    "Assets/LightGray.png",
                    "для домашних тел. 555-555 \nдля сотовых тел. 909-000, мтс - 0807 \n8-915-987-77-40, 8-961-021-91-31, 8-903-821-03-33, 8-920-117-72-27", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-20",
                    "Такси Паритет",
                    "http://www.2rybinsk.ru/taxi-paritet/",
                    "Assets/LightGray.png",
                    "для домашних тел. 262-633, 263-326, 262-628 \nдля сотовых тел. 8-910-826-82-82, 8-903-829-44-55, 8-920-225-27-50", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-21",
                    "Такси Визит",
                    "http://www.2rybinsk.ru/taxi-vizit/",
                    "Assets/LightGray.png",
                    "для домашних тел. 250-150 \nдля сотовых тел. 91-8888 8-915-999-03-83, 8-960-531-08-30, 8-920-220-35-3", "",
                    group1));

            group1.Items.Add(new SampleDataItem("Group-1-Item-22",
                    "Такси 911",
                    "http://www.2rybinsk.ru/taxi-911/",
                    "Assets/LightGray.png",
                    "для домашних тел. 219-911, 295-111 \nдля сотовых тел. 8-980-654-75-50, 8-920-652-03-40", "",
                    group1));
            this.AllGroups.Add(group1);

        }
    }
}
