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
	/// Result types for the searches you perform.
	/// </summary>
	public enum ResultTypes: uint 
	{
		/// <summary>
		/// The Raw result as returned from Google.
		/// </summary>
		Raw = 1,
		/// <summary>
		/// An XmlDocument for easier navigation and parsing.
		/// </summary>
		XmlDocument = 2,
		/// <summary>
		/// Return the result as a DataSet. 		
		/// </summary>
		DataSet = 3
	}
	
	/// <summary>
	/// IGoogleSearchResult is the search results container for google searches.
	/// </summary>
	public interface IGoogleSearchResult 
	{
		/// <summary>
		/// The total number of results for this search.
		/// </summary>
		int ResultsCount { get; }
		/// <summary>
		/// The number of results displayed per page. 
		/// This is the number of results contained in this search result object.
		/// </summary>
		int ResultsPerPage { get; }
		/// <summary>
		/// The search's result record index that indicates where the search started from.
		/// </summary>
		int StartedFrom { get; }
		/// <summary>
		/// The query used to obtain this result.
		/// </summary>
		string Query { get; }
		/// <summary>
		/// The query result. The type of the result depends on the <see cref="ResultType"/>.
		/// </summary>
		object QueryResult { get; }
		/// <summary>
		/// Can be one of the type defined in <see cref="ResultTypes"/>
		/// </summary>
		ResultTypes ResultType { get; }
		/// <summary>
		/// Returns the next page of results.
		/// Will return NULL when there is no next page available.
		/// </summary>
		IGoogleSearchResult NextPage { get; }
		/// <summary>
		/// Returns the previous page of results.
		/// Will return NULL when there is no previous page available.
		/// </summary>
		IGoogleSearchResult PreviousPage { get; }
		/// <summary>
		/// Returns true if there is a next page available.
		/// </summary>
		bool HasNext { get; }
		/// <summary>
		/// Returns true if there is a previous page available.
		/// </summary>
		bool HasPrevious { get; }
	}
	
	internal class GoogleSearchResult: IGoogleSearchResult
	{
		private int resultsCount;
		private int startedFrom;
		private string query;
		private object queryResult;
		private ResultTypes resultType;
		private int resultsPerPage = 10;
		private IGoogleSearch googleSearch = null;
		
		public GoogleSearchResult(IGoogleSearch googleSearch, int resultsCount, int startedFrom, int resultsPerPage, string query, object queryResult, ResultTypes resultType)
		{
			this.googleSearch = googleSearch;
			this.resultsCount = resultsCount;
			this.startedFrom = startedFrom;
			this.resultsPerPage = resultsPerPage;
			this.query = query;
			this.queryResult = queryResult;
			this.resultType = resultType;
		}
		public int ResultsCount 
		{
			get 
			{
				return resultsCount;
			}
		}
		public int ResultsPerPage 
		{
			get 
			{
				return resultsPerPage;
			}
		}
		public int StartedFrom 
		{
			get 
			{
				return startedFrom;
			}
		}
		public string Query 
		{
			get 
			{
				return query;
			}
		}
		public object QueryResult 
		{
			get 
			{
				return queryResult;
			}
		}
		public ResultTypes ResultType 
		{
			get 
			{
				return resultType;
			}
		}
		public IGoogleSearchResult NextPage 
		{
			get 
			{				
				if (HasNext) 
				{
					return googleSearch.Search(query, startedFrom + resultsPerPage, resultType);
				}
				else 
				{
					return null;
				}
			}
		}
		public IGoogleSearchResult PreviousPage 
		{
			get 
			{
				if (HasPrevious) 
				{
					return googleSearch.Search(query, startedFrom - resultsPerPage, resultType);
				}
				else 
				{
					return null;
				}				
			}
		}
		public bool HasNext 
		{
			get 
			{
				return (startedFrom + resultsPerPage) < resultsCount;
			}
		}
		public bool HasPrevious 
		{
			get 
			{
				return (startedFrom - resultsPerPage) > 0;
			}
		}
	}
}
