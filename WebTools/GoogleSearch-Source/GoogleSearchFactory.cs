using System;

namespace GoogleSearch
{
	/// <summary>
	/// GoogleSearchFactory gives easier access to create the various supported Google search objects.
	/// </summary>
	public class GoogleSearchFactory
	{
		private GoogleSearchFactory()
		{
		}
		/// <summary>
		/// Creates a new GoogleDesktopSearch object.
		/// </summary>
		/// <returns>the newly created GoogleDesktopSearch object</returns>
		public static IGoogleSearch CreateGoogleDesktopSearch() 
		{
			return GoogleDesktopSearch.Create();
		}

		/// <summary>
		/// Creates a new GoogleWebSearch object.
		/// </summary>
		/// <param name="licenseKey">License key required by Google. 
		/// For more information go to http://www.google.com/apis/</param>
		/// <returns>the newly created GoogleWebSearch object</returns>
		public static IGoogleSearch CreateGoogleWebSearch(string licenseKey) 
		{
			return GoogleWebSearch.Create(licenseKey);
		}

		/// <summary>
		/// Creates a new GoogleWebSearch object
		/// </summary>
		/// <param name="licenseKey">License key required by Google.
		/// For more information go to http://www.google.com/apis/</param>
		/// <param name="filter">Enable/Disable automatic filtering.
		/// For more information go to http://www.google.com/apis/reference.html#2_1</param>
		/// <param name="restrict">Retricts the search to a subset of the Google Web Index.
		/// For more information go to http://www.google.com/apis/reference.html#2_4.
		/// To not use, pass an empty string.</param>
		/// <param name="safeSearch">Enable/Disable safe SafeSearch to filter adult content.
		/// For more information go to http://www.google.com/apis/reference.html#2_6</param>
		/// <param name="languageRestrict">Retricts the search to documents with one or more languages.
		/// For default usage, pass an empty string.</param>
		/// <returns></returns>
		public static IGoogleSearch CreateGoogleWebSearch(string licenseKey, bool filter, string restrict, bool safeSearch, string languageRestrict) 
		{
			return GoogleWebSearch.Create(licenseKey, filter, restrict, safeSearch, languageRestrict);
		}
	}
}
