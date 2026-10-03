namespace Windows.ApplicationModel.Resources.Core {}
namespace Windows.Foundation {}
namespace Windows.Foundation.Collections {}
namespace Windows.Foundation.Metadata {public class WebHostHiddenAttribute:System.Attribute {}}
namespace Windows.UI.Xaml.Data {}
namespace Windows.UI.Xaml.Media {public class ImageSource {}}
namespace Windows.UI.Xaml.Media.Imaging {public class BitmapImage:Windows.UI.Xaml.Media.ImageSource {public BitmapImage(System.Uri uri){}}}
namespace Windows.ApplicationModel.Activation {}
namespace Windows.UI.Xaml.Controls.Primitives {}
namespace Windows.UI.Xaml.Input {}
namespace Windows.UI.Xaml.Navigation {}
namespace Windows.UI.Xaml {public class RoutedEventArgs:System.EventArgs {}public class FrameworkElement {public object DataContext;}public static class VisualStateManager {public static bool GoToState(object page,string state,bool transitions){return true;}}}
namespace Windows.UI.Xaml.Controls {public class SelectionChangedEventArgs:System.EventArgs {public System.Collections.Generic.List<object> AddedItems=new System.Collections.Generic.List<object>();}public class ItemClickEventArgs:System.EventArgs {public object ClickedItem;}}
namespace RybinskTaxi.Common {public class LayoutAwarePage {public System.Collections.Generic.Dictionary<string,object> DefaultViewModel=new System.Collections.Generic.Dictionary<string,object>();public TestFrame Frame=new TestFrame();protected virtual void LoadState(object p,System.Collections.Generic.Dictionary<string,object>s){} }public class TestFrame {public bool CanGoBack;public void GoBack(){throw new System.Exception("Navigation outside test");}public void Navigate(System.Type t,object p=null){throw new System.Exception("Navigation outside test");}}}
namespace RybinskTaxi {public class GroupedItemsPage {} public class ItemDetailPage {}public sealed partial class TaxiSearch {void InitializeComponent(){} public TestViewSource filtersViewSource=new TestViewSource();public class TestViewSource {public TestView View;}public class TestView {public void MoveCurrentTo(object o){}}public void TestSearch(object query){LoadState(query,null);var f=(System.Collections.IEnumerable)DefaultViewModel["Filters"];var e=new Windows.UI.Xaml.Controls.SelectionChangedEventArgs();foreach(var filter in f){e.AddedItems.Add(filter);break;}Filter_SelectionChanged(this,e);}public int TestCount {get {foreach(Filter f in (System.Collections.IEnumerable)DefaultViewModel["Filters"])return f.Count;return -1;}}}}
