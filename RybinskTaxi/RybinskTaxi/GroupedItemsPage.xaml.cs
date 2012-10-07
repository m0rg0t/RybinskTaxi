using Callisto.Controls;
using RybinskTaxi.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Callisto;
using System.Collections.ObjectModel;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace RybinskTaxi
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class GroupedItemsPage 
//: RybinskTaxi.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;

            this.itemGridView.ItemsSource = sampleDataGroups.First().Items;
            this.itemListView.ItemsSource = sampleDataGroups.First().Items;
        }

        /// <summary>
        /// Вызывается при нажатии заголовка группы.
        /// </summary>
        /// <param name="sender">Объект Button, используемый в качестве заголовка выбранной группы.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри группы.
        /// </summary>
        /// <param name="sender">Объект GridView (или ListView, если приложение прикреплено),
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }*/

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var viewAboutPage = new SettingsCommand("", "О приложении", cmd =>
            {
                //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                var settingsFlyout = new SettingsFlyout();
                settingsFlyout.Content = new About(); //this is just a regular XAML UserControl


                settingsFlyout.HeaderText = "О приложении";


                settingsFlyout.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(viewAboutPage);

        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    var group1 = new SampleDataGroup("Group-1",
                            "Такси Рыбиснка",
                            "",
                            "Assets/DarkGray.png",
                            "");

                    _items.Add(new SampleDataItem("Group-1-Item-1",
                "Такси Эконом",
                "http://www.2rybinsk.ru/taxi-ekonom/",
                "Assets/taksi-ekonom1.jpg",
                "для домашних тел. 22-33-33, 22-26-66 для сотовых тел. 8-915-997-99-77, 8-906-526-777-6, 8-920-118-33-33, 93-33-09", "",
                group1));

                    _items.Add(new SampleDataItem("Group-1-Item-2",
                                        "Такси Рыбинск",
                                        "http://www.2rybinsk.ru/taxi-rybinsk/",
                                        "Assets/Rubinsk1.gif",
                                        "для домашних тел. 245-245, 246-246 \nдля сотовых тел. 8-905-139-05-05, 8-910-664-36-34, 8-902-330-88-88", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-3",
                                        "Такси Лада",
                                        "http://www.2rybinsk.ru/taxi-lada/",
                                        "Assets/taksi-lada1.jpg",
                                        "для домашних тел. 280-380 \nдля сотовых тел. 919-999,8-920-100-40-00, 8-910-666-60-50, 8-903-822-78-78", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-4",
                                        "Такси Марсель",
                                        "http://www.2rybinsk.ru/taxi-marsel/",
                                        "Assets/taksi-marsel1.jpg",
                                        "для домашних тел. 289-999, 25-33-33 \nдля сотовых тел. 909-888, 8-903-829-98-88, 8-920-105-88-88, 8-910-818-99-88", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-5",
                                        "Такси Круиз",
                                        "http://www.2rybinsk.ru/taxi-kruiz/",
                                        "Assets/kruiz1.jpg",
                                        "для сотовых тел. 95-45-65 и 8-920-107-22-22 \nдля домашних тел. 25-45-65, 25-15-15", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-6",
                                        "Такси Регион",
                                        "http://www.2rybinsk.ru/taxi-region/",
                                        "Assets/taksi-region1.jpg",
                                        "для домашних тел. 253-888 \nдля сотовых тел. 909-909", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-7",
                                        "Такси На Дубровку",
                                        "http://www.2rybinsk.ru/taxi-na-dybrovky/",
                                        "Assets/taksi-na-dybrovky1.jpg",
                                        "для домашних тел. 222-333, 266-201 \nдля сотовых тел. 8-915-966-55-55, 8-961-022-22-22, 900-006", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-8",
                                        "Такси Димон",
                                        "http://www.2rybinsk.ru/taxi-dimon/",
                                        "Assets/dimon-taxi1.jpg",
                                        "Стационарный телефон: 20-20-20 \nБилайн: 333-138, МТС: 595-615, Мегафон: 929-07-79-777", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-9",
                                        "Такси Пилот",
                                        "http://www.2rybinsk.ru/taxi-pilot/",
                                        "Assets/Pilot1.gif",
                                        "для домашних тел. 251-111 \nдля сотовых тел. 8-906-525-82-00", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-10",
                                        "Наше такси",
                                        "http://www.2rybinsk.ru/taxi-nashe/",
                                        "Assets/Nashe_taxi1.gif",
                                        "для домашних тел. 25-44-44 \nдля сотовых тел. 90-48-55", "",
                                        group1));

                    _items.Add(new SampleDataItem("Group-1-Item-11",
                            "Такси Экипаж",
                            "http://www.2rybinsk.ru/taxi-ekipazh/",
                            "Assets/ekipazh1.jpg",
                            "для домашних тел. 555-556, 558885, 227-227 \nдля сотовых тел. 8-905-134-41-41, 8-902-334-88-85", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-22",
                            "Такси Универсал",
                            "http://www.2rybinsk.ru/taxi-universal/",
                            "Assets/universal1.jpg",
                            "Такси Универсал", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-13",
                "Такси Недорогое",
                "http://www.2rybinsk.ru/taxi-nedorogoe/",
                "Assets/LightGray.png",
                "для домашних тел. 222-222 \nдля сотовых тел. 8-920-116-06-00, 8-905-636-11-44, 8-980-662-02-02", "",
                group1));

                    _items.Add(new SampleDataItem("Group-1-Item-14",
                "Такси Переборы",
                "http://www.2rybinsk.ru/taxi-perebori/",
                "Assets/LightGray.png",
                "для домашних тел. 598-522 \nдля сотовых тел. 8-906-525-82-00", "",
                group1));

                    _items.Add(new SampleDataItem("Group-1-Item-15",
                "Такси Городок",
                "http://www.2rybinsk.ru/taxi-gorodok/",
                "Assets/LightGray.png",
                "для домашних тел. 284-284 \nдля сотовых тел. 95-65-65", "",
                group1));

                    _items.Add(new SampleDataItem("Group-1-Item-16",
                "Такси Вояж",
                "http://www.2rybinsk.ru/taxi-voyaz/",
                "Assets/LightGray.png",
                "для домашних тел. 210-444, 222-323 \nдля сотовых тел. 8-905-639-94-99, 8-915-992-07-05", "",
                group1));

                    _items.Add(new SampleDataItem("Group-1-Item-17",
                            "Такси Галактика",
                            "http://www.2rybinsk.ru/taxi-galaktika/",
                            "Assets/LightGray.png",
                            "для домашних тел. 295-555, 280-606 \nдля сотовых тел. 919-000, 8-905-633-13-13, 8-915-986-06-06, 8-920-223-55-55", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-18",
                            "Такси Гора",
                            "http://www.2rybinsk.ru/taxi-gora/",
                            "Assets/LightGray.png",
                            "для домашних тел. 281-222, 219-596 \nдля сотовых тел. 930-900, 8-903-827-72-72", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-19",
                            "Такси Центр",
                            "http://www.2rybinsk.ru/taxi-centr/",
                            "Assets/LightGray.png",
                            "для домашних тел. 555-555 \nдля сотовых тел. 909-000, мтс - 0807 \n8-915-987-77-40, 8-961-021-91-31, 8-903-821-03-33, 8-920-117-72-27", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-20",
                            "Такси Паритет",
                            "http://www.2rybinsk.ru/taxi-paritet/",
                            "Assets/LightGray.png",
                            "для домашних тел. 262-633, 263-326, 262-628 \nдля сотовых тел. 8-910-826-82-82, 8-903-829-44-55, 8-920-225-27-50", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-21",
                            "Такси Визит",
                            "http://www.2rybinsk.ru/taxi-vizit/",
                            "Assets/LightGray.png",
                            "для домашних тел. 250-150 \nдля сотовых тел. 91-8888 8-915-999-03-83, 8-960-531-08-30, 8-920-220-35-3", "",
                            group1));

                    _items.Add(new SampleDataItem("Group-1-Item-22",
                            "Такси 911",
                            "http://www.2rybinsk.ru/taxi-911/",
                            "Assets/LightGray.png",
                            "для домашних тел. 219-911, 295-111 \nдля сотовых тел. 8-980-654-75-50, 8-920-652-03-40", "",
                            group1));
                }
                catch { };


                SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;

                //this.itemGridView.ItemsSource = _items;
                //this.itemListView.ItemsSource = _items;
            }
            catch { };
        }

        /*protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            //var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = null;
        }*/



    }
}
