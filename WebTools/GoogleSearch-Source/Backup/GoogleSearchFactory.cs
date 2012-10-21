//
// GoogleSearch
// Google Search API .NET wrapper
//
// Written by Eran Sandler (eran.sandler@gmail.com) 
// 
// Copyright (C) 2005   Eran Sandler
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
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
