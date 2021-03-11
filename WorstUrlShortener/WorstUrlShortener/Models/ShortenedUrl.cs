namespace WorstUrlShortener.Models
{
	public class ShortenedUrl : BaseModel
	{
		public string ShortenService { get; set; }

		public string FullUrl { get; set; }

		public string ShortUrl { get; set; }
	}
}
