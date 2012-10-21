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
using System.Xml;
using System.Data;

namespace GoogleSearch
{
	internal class GoogleWebSearch: IGoogleSearch
	{
		private const string SummaryFieldName = "summary";
		private const string URLFieldName = "URL";
		private const string SnippetFieldName = "snippet";
		private const string TitleFieldName = "title";
		private const string CachedSizeFieldName = "cachedSize";
		private const string RelatedInformationPresentFieldName = "relatedInformationPresent";
		private const string HostNameFieldName = "hostName";
		private const string DirectoryCategoryFieldName = "directoryCategory";
		private const string DirectoryTitleFieldName = "directoryTitle";
		
		private string licenseKey = null;
		private bool filter = false;
		private string restrict = string.Empty;
		private bool safeSearch = false;
		private string languageRestrict = string.Empty;
		private ResultTypes resultType = ResultTypes.DataSet;
		
		// Currently, Google Web Search API is limited to returning a max of 10 results 
		// per query so currently there is no point in changing this value to more than
		// 10, but perhaps it will change in time depending on the license key supplied.
		private int resultsPerPage = 10;
		
		private GoogleWebSearch(string licenseKey, bool filter, string restrict, bool safeSearch, string languageRestrict)
		{
			this.licenseKey = licenseKey;
			this.filter = filter;
			this.restrict = restrict;
			this.safeSearch = safeSearch;
			this.languageRestrict = languageRestrict;
		}

		private XmlNode AddElement(XmlNode parentNode, string elementName, string elementValue) 
		{
			XmlNode tempNode = parentNode.OwnerDocument.CreateElement(elementName);

			if (elementValue != null) 
			{
				XmlText text = parentNode.OwnerDocument.CreateTextNode(elementValue);
				tempNode.AppendChild(text);
			}

			parentNode.AppendChild(tempNode);

			return tempNode;
		}
		private XmlDocument ConvertToXmlDocument(Google.GoogleSearchResult searchResult) 
		{
			XmlDocument xmlDocument = new XmlDocument();
			
			XmlNode resultsNode = xmlDocument.CreateElement("results");
			XmlAttribute resultsCount = xmlDocument.CreateAttribute("count");
			resultsCount.Value = searchResult.estimatedTotalResultsCount.ToString();
			resultsNode.Attributes.Append(resultsCount);

			xmlDocument.AppendChild(resultsNode);

			XmlNode resultNode;
			Google.ResultElement resultElement;

			for (int i = 0; i < searchResult.resultElements.Length; i++) 
			{
				resultElement = searchResult.resultElements[i];
				resultNode = AddElement(resultsNode, "result", null);

				AddElement(resultNode, SummaryFieldName, resultElement.summary);
				AddElement(resultNode, URLFieldName, resultElement.URL);
				AddElement(resultNode, SnippetFieldName, resultElement.snippet);
				AddElement(resultNode, TitleFieldName, resultElement.title);
				AddElement(resultNode, CachedSizeFieldName, resultElement.cachedSize);
				AddElement(resultNode, RelatedInformationPresentFieldName, resultElement.relatedInformationPresent.ToString());
				AddElement(resultNode, HostNameFieldName, resultElement.hostName);
				AddElement(resultNode, DirectoryCategoryFieldName, resultElement.directoryCategory.fullViewableName);
				AddElement(resultNode, DirectoryTitleFieldName, resultElement.directoryTitle);

				resultsNode.AppendChild(resultNode);
			}

			return xmlDocument;
		}
		private DataSet ConvertToDataSet(Google.GoogleSearchResult searchResult) 
		{
			DataSet dataSet = new DataSet("GoogleWebSearchResults");			
			DataTable table = dataSet.Tables.Add();

			table.Columns.Add(SummaryFieldName, typeof(string));
			table.Columns.Add(URLFieldName, typeof(string));
			table.Columns.Add(SnippetFieldName, typeof(string));
			table.Columns.Add(TitleFieldName, typeof(string));
			table.Columns.Add(CachedSizeFieldName, typeof(string));
			table.Columns.Add(RelatedInformationPresentFieldName, typeof(bool));
			table.Columns.Add(HostNameFieldName, typeof(string));
			table.Columns.Add(DirectoryCategoryFieldName, typeof(string));
			table.Columns.Add(DirectoryTitleFieldName, typeof(string));

			Google.ResultElement resultElement;
			DataRow row = null;

			for (int i = 0; i < searchResult.resultElements.Length; i++) 
			{
				row = table.NewRow();
				resultElement = searchResult.resultElements[i];
				
				row[SummaryFieldName] = resultElement.summary;
				row[URLFieldName] = resultElement.URL;
				row[SnippetFieldName] = resultElement.snippet;
				row[TitleFieldName] = resultElement.title;
				row[CachedSizeFieldName] = resultElement.cachedSize;
				row[RelatedInformationPresentFieldName] = resultElement.relatedInformationPresent;
				row[HostNameFieldName] = resultElement.hostName;
				row[DirectoryCategoryFieldName] = resultElement.directoryCategory.fullViewableName;
				row[DirectoryTitleFieldName] = resultElement.directoryTitle;
				
				table.Rows.Add(row);
			}

			return dataSet;
		}
		private object ConvertResult(Google.GoogleSearchResult searchResult, ResultTypes resultType) 
		{
			switch (resultType) 
			{
				case ResultTypes.Raw:
					return searchResult;
				case ResultTypes.XmlDocument:
					return ConvertToXmlDocument(searchResult);
				case ResultTypes.DataSet:
					return ConvertToDataSet(searchResult);
				default:
					throw new ApplicationException("Unknown result Type. Cannot convert.");
			}
		}

		public static IGoogleSearch Create(string licenseKey) 
		{
			return new GoogleWebSearch(licenseKey, false, string.Empty, false, string.Empty);
		}
		public static IGoogleSearch Create(string licenseKey, bool filter, string restrict, bool safeSearch, string languageRestrict) 
		{
			return new GoogleWebSearch(licenseKey, filter, restrict, safeSearch, languageRestrict);
		}
		#region IGoogleSearch Members

		public GoogleSearch.ResultTypes ResultType
		{
			get
			{
				return resultType;
			}
			set
			{
				resultType = value;
			}
		}

		public int ResultsPerPage
		{
			get
			{
				return resultsPerPage;
			}
			set
			{
				resultsPerPage = value;
			}
		}

		public IGoogleSearchResult Search(string queryText, int startFrom, GoogleSearch.ResultTypes resultType)
		{
			Google.GoogleSearchService service = new Google.GoogleSearchService();			
			Google.GoogleSearchResult tempSearchResult = service.doGoogleSearch(licenseKey, queryText, startFrom, 10, filter, restrict, safeSearch, languageRestrict, string.Empty, string.Empty);
						
			object convertedResult = ConvertResult(tempSearchResult, resultType);

			GoogleSearchResult searchResult = new GoogleSearchResult(this, tempSearchResult.estimatedTotalResultsCount, startFrom, 10, queryText, convertedResult, resultType);

			return searchResult;
		}

		IGoogleSearchResult GoogleSearch.IGoogleSearch.Search(string queryText, GoogleSearch.ResultTypes resultType)
		{
			return Search(queryText, 0, resultType);	
		}

		IGoogleSearchResult GoogleSearch.IGoogleSearch.Search(string queryText, int startFrom)
		{
			return Search(queryText, startFrom, resultType);
		}

		IGoogleSearchResult GoogleSearch.IGoogleSearch.Search(string queryText)
		{
			return Search(queryText, 0, resultType);
		}

		#endregion
	}
}
