using Microsoft.SmallBasic.Library;
using System;
using ZS;


namespace ZS
{
	/// <summary>
	/// Some api functions.
	/// </summary>
	[SmallBasicType]
	public static class ZSApi
	{
		
		/// <summary>
		/// Retrieves the public IP address of the user.
		/// </summary>
		/// <returns>The public IP address as a string.</returns>
		public static Primitive GetPublicIP()
		{
			try {
				return ZSNetwork.Get("https://api.ipify.org");
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves detailed information about an IP address.
		/// </summary>
		/// <param name="ip">The IP address to look up. Use "auto" for the current IP.</param>
		/// <returns>Formatted location details as a string.</returns>
		public static Primitive GetIPDetails(Primitive ip)
		{
			try {
				string url = "http://ip-api.com/json/" + ip;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the current weather for a given city.
		/// </summary>
		/// <param name="city">The city name (e.g., "London").</param>
		/// <returns>The weather report as a string.</returns>
		public static Primitive GetWeather(Primitive city)
		{
			try {
				string url = "https://wttr.in/" + city + "?format=%C+%t";
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the current time for a specified timezone.
		/// </summary>
		/// <param name="timezone">The timezone (e.g., "Europe/London").</param>
		/// <returns>The current time in the given timezone.</returns>
		public static Primitive GetTimeByZone(Primitive timezone)
		{
			try {
				string url = "http://worldtimeapi.org/api/timezone/" + timezone;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves a random programming-related joke.
		/// </summary>
		/// <returns>A joke as a string.</returns>
		public static Primitive GetRandomJoke()
		{
			try {
				string url = "https://v2.jokeapi.dev/joke/Any?format=txt";
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves the exchange rate between two currencies.
		/// </summary>
		/// <param name="fromCurrency">The base currency (e.g., "USD").</param>
		/// <param name="toCurrency">The target currency (e.g., "EUR").</param>
		/// <returns>The exchange rate as a string.</returns>
		public static Primitive GetExchangeRate(Primitive fromCurrency, Primitive toCurrency)
		{
			try {
				string url = "https://api.exchangerate-api.com/v4/latest/" + fromCurrency;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Shortens a given URL using TinyURL.
		/// </summary>
		/// <param name="longUrl">The long URL to shorten.</param>
		/// <returns>The shortened URL.</returns>
		public static Primitive GetShortURL(Primitive longUrl)
		{
			try {
				string url = "https://tinyurl.com/api-create.php?url=" + longUrl;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves a random inspirational quote.
		/// </summary>
		/// <returns>A quote as a string.</returns>
		public static Primitive GetRandomQuote()
		{
			try {
				string url = "https://zenquotes.io/api/random";
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves a random fun fact.
		/// </summary>
		/// <returns>A random fact as a string.</returns>
		public static Primitive GetRandomFact()
		{
			try {
				string url = "https://uselessfacts.jsph.pl/random.txt?language=en";
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves COVID-19 statistics for a given country.
		/// </summary>
		/// <param name="country">The country name (e.g., "USA").</param>
		/// <returns>COVID-19 statistics as a string.</returns>
		public static Primitive GetCovidStats(Primitive country)
		{
			try {
				string url = "https://disease.sh/v3/covid-19/countries/" + country;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		/// <summary>
		/// Retrieves information about a country.
		/// </summary>
		/// <param name="country">The country name (e.g., "India").</param>
		/// <returns>Country details as a string.</returns>
		public static Primitive GetCountryInfo(Primitive country)
		{
			try {
				string url = "https://restcountries.com/v3.1/name/" + country;
				return ZSNetwork.Get(url);
			} catch (Exception ex) {
				ZSUtilities.OnError(ex);
				return "Error: " + ex.Message;
			}
		}

		
		
		
		
		
		
		
		
		
		
		
	}
}