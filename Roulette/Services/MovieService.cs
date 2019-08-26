using Newtonsoft.Json.Linq;
using Roulette.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Services
{
    class MovieService
    {
        public static readonly int MinSearchLength = 5;
        private const string Url = "https://www.omdbapi.com/?apikey=[_your_api_key_here_]&s=";              

        private HttpClient _client = new HttpClient();

        public async Task<IEnumerable<Movie>> SearchByTitle(string title)
        {           
            
            if (title.Length < MinSearchLength)
                return Enumerable.Empty<Movie>();            

            var response = await _client.GetAsync($"{Url}{title}*");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return Enumerable.Empty<Movie>();

            var content = await response.Content.ReadAsStringAsync();

            //Code below is based on info provided on https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.htm
            //Using LINQ to JSON you can extract the pieces of JSON you want to deserialize before passing them to the Json.NET serializer....

            JObject search = JObject.Parse(content);

            if(!search.ContainsKey("Search"))
                return Enumerable.Empty<Movie>();

            // get JSON result objects into a list
            IList<JToken> results = search["Search"].Children().ToList();



            IList<Movie> movies = new List<Movie>();

            // serialize JSON results into .NET objects
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                Movie movie = result.ToObject<Movie>();
                movies.Add(movie);
            }

            return movies;           
            
        }
    }
}
