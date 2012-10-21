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
	/// IGoogleSearch is the generic interface for executing google searches.
	/// Use the <see cref="GoogleSearchFactory"/> To create the relevant google search object.
	/// </summary>
	public interface IGoogleSearch 
	{
		/// <summary>
		/// The default result type that will be used when executing queries.
		/// See <see cref="ResultTypes"/> for more information on the types.
		/// </summary>
		ResultTypes ResultType { get; set; }
		/// <summary>
		/// The default number of results per page that will be retireved.
		/// </summary>
		int ResultsPerPage { get; set; }
		/// <summary>
		/// Executes a search.
		/// </summary>
		/// <param name="queryText">The query text to be used</param>
		/// <param name="startFrom">The search's internal index to start this Search Result page</param>
		/// <param name="resultType">The result type that will be used in this search. For more information on result types <see cref="ResultTypes"/>.</param>
		/// <returns>The IGoogleSearchResult interface. For more inforamation see <see cref="IGoogleSearchResult"/>.</returns>
		IGoogleSearchResult Search(string queryText, int startFrom, ResultTypes resultType);
		/// <summary>
		/// Executes a search.
		/// </summary>
		/// <param name="queryText">The query text to be used</param>
		/// <param name="resultType">The result type that will be used in this search. For more information on result types <see cref="ResultTypes"/>.</param>
		/// <returns>The IGoogleSearchResult interface. For more inforamation see <see cref="IGoogleSearchResult"/>.</returns>
		IGoogleSearchResult Search(string queryText, ResultTypes resultType);
		/// <summary>
		/// Executes a search.
		/// </summary>
		/// <param name="queryText">The query text to be used</param>
		/// <param name="startFrom">The search's internal index to start this Search Result page</param>
		/// <returns>The IGoogleSearchResult interface. For more inforamation see <see cref="IGoogleSearchResult"/>.</returns>
		IGoogleSearchResult Search(string queryText, int startFrom);
		/// <summary>
		/// Executes a search.
		/// </summary>
		/// <param name="queryText">The query text to be used</param>
		/// <returns>The IGoogleSearchResult interface. For more inforamation see <see cref="IGoogleSearchResult"/>.</returns>
		IGoogleSearchResult Search(string queryText);
	}
}
