using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Roulette.Model;
using Roulette.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Roulette.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MoviesPage : ContentPage
	{

        private MovieService _movieService = new MovieService();

        //private ObservableCollection<Movie> _movies;

        public static readonly BindableProperty IsSearchingProperty =
            BindableProperty.Create("IsSearching", typeof(bool), typeof(MoviesPage));

        public bool IsSearching {
            get
            {
                return (bool)GetValue(IsSearchingProperty);
            }
            set
            {
                SetValue(IsSearchingProperty, value);
            }
        }

        
        public MoviesPage()
		{
            BindingContext = this;
            InitializeComponent();
            
		}

        private async void MovieList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var movie = e.SelectedItem as Movie;
            movieList.SelectedItem = null;

            await Navigation.PushAsync(new DetailPage(movie));

        }

        async private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null || e.NewTextValue.Length < MovieService.MinSearchLength)
                return;
            

            await FindMovies(e.NewTextValue);

        }

        async private Task FindMovies(string title)
        {
            try
            {
                IsSearching = true;

                var items = await _movieService.SearchByTitle(title);

                movieList.ItemsSource = items;
                movieList.IsVisible = items.Any();
                //_movies = new ObservableCollection<Movie>(items);

                //movieList.ItemsSource = _movies;
                //movieList.IsVisible = _movies.Any();
                notFound.IsVisible = !movieList.IsVisible;

               
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Error", "Could not retrieve the list of movies." + " Exception details: " + ex.Message, "OK");
                
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}