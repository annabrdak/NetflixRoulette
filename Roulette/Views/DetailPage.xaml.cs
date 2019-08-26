using Roulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Roulette.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailPage : ContentPage
	{
        private Movie _movie;

		public DetailPage (Movie movie)
		{
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            _movie = movie;

            BindingContext = _movie;

			InitializeComponent();
		}


	}
}