using Android.Content;
using Android.Widget;
using WeatherSearch.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace WeatherSearch.Droid.CustomRenderer
{
    class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var searchView = base.Control as SearchView;

                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                ImageView searchViewIcon = (ImageView)searchView.FindViewById<ImageView>(searchIconId);
                searchViewIcon.SetImageDrawable(null);

            }
        }
    }
}