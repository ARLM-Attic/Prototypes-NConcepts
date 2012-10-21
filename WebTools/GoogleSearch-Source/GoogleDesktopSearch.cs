using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Data;
using Microsoft.Win32;

namespace GoogleSearch
{
	internal class GoogleDesktopSearch: IGoogleSearch    
    {
        // This is a hardcoded value i coded in that only works on the devsearch computer
        // i coded in the SSID, because we are running it as a Web Apllication under IIS
        private const string SearchUrlRegistryPath = "S-1-5-21-436374069-115176313-725345543-500\\Software\\Google\\Google Desktop\\API";
		private const string SearchUrlKey = "search_url";		
		private const string FormatUrlPart = "format=xml";		
		private const string StartUrlPart = "start=";
		private const string RecordsPerPageUrlPart = "num=";

		private string searchUrl = null;
		private WebClient webClient = null;
		// Raw and XMLDocument is verified to work!
        private ResultTypes resultType = ResultTypes.Raw;
        private int resultsPerPage = 10;
	
		private void ReadSearchUrl() 
		{
			RegistryKey key = Registry.Users.OpenSubKey(SearchUrlRegistryPath);
			try 
			{
				if (key != null) 
				{
					searchUrl = Convert.ToString(key.GetValue(SearchUrlKey));
				}
				else 
				{
					throw new ApplicationException("Failed to read Search URL");
				}
			}
			finally 
			{
				if (key != null) 
				{
					key.Close();	
				}
			}
		}

		private string NormalizeQueryText(string queryText) 
		{
			StringBuilder sb = new StringBuilder(queryText);
			
			for (int i = 0; i < sb.Length; i++) 
			{
				if (sb[i] == ' ') 
				{
					sb[i] = '+';
				}
			}

			return sb.ToString();
		}
		private string BuildRequestUrl(string queryText) 
		{
			return searchUrl + NormalizeQueryText(queryText) + "&" + FormatUrlPart;
		}
		private string RunQuery(string queryText, int startFrom) 
		{
			string url = BuildRequestUrl(queryText);

			//
			// If we actually got a worthwhile startFrom value.
			//
			if (startFrom > -1) 
			{
				url += "&" + StartUrlPart + startFrom.ToString();
			}
			
			url += "&" + RecordsPerPageUrlPart + resultsPerPage.ToString();

			Stream stream = webClient.OpenRead(url);
			try 
			{
				StreamReader reader = new StreamReader(stream);
				string queryResult = reader.ReadToEnd();
                                          
				return queryResult;
			}
			finally
			{
				stream.Close();
			}
		}
		private string UglyStrangeCharacterHack(string text) 
		{
			StringBuilder sb = new StringBuilder(text);
			int i = 0;
			while (i < sb.Length)
			{
				if ((sb[i] == 0x1C) || (sb[i] == 0x1D))
				{
					sb.Remove(i, 1);
				}

				i++;
			}

			return sb.ToString();
		}
		private XmlDocument ConvertToXmlDocument(string result) 
		{
			try 
			{		
				XmlDocument xmlDocument = new XmlDocument();				
				xmlDocument.PreserveWhitespace = true;
				
				//
				// For some reason, sometimes the result contains characters which 
				// XmlDocument refuses to accept and throws an excpetions.
				// 
				// Current the character whose values are 0x1C and 0x1D are the 
				// only onces I have encountered.
				//
				// This is an ugly hack the removes them.
				//
				xmlDocument.LoadXml(UglyStrangeCharacterHack(result));
				return xmlDocument;
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}
		private XmlNode GetNextNonWhiteSpaceSibling(XmlNode node) 
		{
			XmlNode temp = node.NextSibling;
			while ((temp != null) && (temp.NodeType == XmlNodeType.Whitespace)) 
			{
				temp = temp.NextSibling;
			}

			return temp;
		}
		private DataSet ConvertToDataSet(string result) 
		{
			XmlDocument xmlDocumnet = ConvertToXmlDocument(result);
			
			DataSet dataSet = new DataSet("GoogleSearchResults");

			XmlNode resultsNode = xmlDocumnet.FirstChild.NextSibling.NextSibling;
			XmlNode resultNode = GetNextNonWhiteSpaceSibling(resultsNode.FirstChild);

			XmlNode resultFieldNode = null;
			DataTable table = null;
			DataRow row = null;
			DataColumn column = null;
			
			table = dataSet.Tables.Add();

			while (resultNode != null) 
			{
				row = table.NewRow();
				resultFieldNode = GetNextNonWhiteSpaceSibling(resultNode.FirstChild);
			
				if (resultFieldNode != null) 
				{					
					while (resultFieldNode != null) 
					{												
						column = table.Columns[resultFieldNode.Name];
						if (column == null) 
						{
							if (resultFieldNode.Name == "time") 
							{
								column = table.Columns.Add(resultFieldNode.Name, typeof(DateTime));
							}
							else 
							{
								column = table.Columns.Add(resultFieldNode.Name, typeof(string));
							}
						}

						if (resultFieldNode.Name == "time") 
						{							
							row[column] = DateTime.FromFileTime(Convert.ToInt64(resultFieldNode.FirstChild.Value));
						}
						else 
						{
							row[column] = resultFieldNode.FirstChild.Value;
						}

						resultFieldNode = GetNextNonWhiteSpaceSibling(resultFieldNode);
					}
					table.Rows.Add(row);
				}
				
				resultNode = GetNextNonWhiteSpaceSibling(resultNode);
			}

			return dataSet;
		}
		private object ConvertResult(string result, ResultTypes resultType) 
		{
			switch (resultType) 
			{
				case ResultTypes.Raw:
					return result;
				case ResultTypes.XmlDocument:				
					return ConvertToXmlDocument(result);
				case ResultTypes.DataSet:
					return ConvertToDataSet(result);
				default:
					throw new ApplicationException("Unknown result Type. Cannot convert.");
			}
		}
		private int GetResultsCount(XmlDocument xmlDocumentResult, ResultTypes resultType) 
		{
			// TODO: Add a more efficient way to get the results count depending on the result type.
			// TODO: Add all the necessasry checks to validate the XML. This is ugly code for now.
			XmlNode resultsNode = GetNextNonWhiteSpaceSibling(xmlDocumentResult.FirstChild);
			return Convert.ToInt32(resultsNode.Attributes[0].Value);
		}
		private void CommonConstructor() 
		{
			ReadSearchUrl();
			webClient = new WebClient(); 
		}
		private GoogleDesktopSearch()
		{
			CommonConstructor();
		}
		public static IGoogleSearch Create() 
		{
			return new GoogleDesktopSearch();
		}
		public GoogleDesktopSearch(ResultTypes resultType) 
		{
			CommonConstructor();
			this.resultType = resultType;
		}
		public ResultTypes ResultType 
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
		public IGoogleSearchResult Search(string queryText, int startFrom, ResultTypes resultType) 
		{
			string tempResult = RunQuery(queryText, startFrom);	
			
			XmlDocument xmlDocument = ConvertToXmlDocument(tempResult);
			int resultsCount = GetResultsCount(xmlDocument, resultType);
			
			object result = ConvertResult(tempResult, resultType);			
			
			IGoogleSearchResult gsResult = new GoogleSearchResult(this, resultsCount, startFrom, resultsPerPage, queryText, result, resultType);
			
			return gsResult;
		}		
		public IGoogleSearchResult Search(string queryText, ResultTypes resultType) 
		{
			return Search(queryText, -1, resultType);
		}
		public IGoogleSearchResult Search(string queryText, int startFrom) 
		{			
			return Search(queryText, startFrom, this.resultType);
		}
		public IGoogleSearchResult Search(string queryText) 
		{			
			return Search(queryText, -1, this.resultType);
		}		
	}
}
