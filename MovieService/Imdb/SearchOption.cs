namespace Imdb
{
    public class SearchOption
    {
        public string imdbId { get; set; }
        public string Title { get; set; }

        public SearchOption(string id, string title)
        {
            imdbId = id;
            Title = title;
        }
    }
}
