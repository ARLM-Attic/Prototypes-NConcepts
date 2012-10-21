using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Collections;

namespace GDSLibrary
{
	internal delegate string AsyncResponse(Stream ResponseStream);
	
	[Flags]
	public enum GDSItemTypes 
	{
		None = 0,

		Email = 1,
		Chat  = 2,
		Web	  = 4,
		File  = 8,

		All = Email | Chat | Web | File
	}

	public struct GDSResult {

		/// <summary>
		/// GDS total results counter.
		/// </summary>
		/// <remarks>
		/// Valid only for GDSItemTypes.None and GDSItemTypes. All item filters.
		/// </remarks>
		public int Count;
		public GDSResultItem[] Items;
	}

	public struct GDSResultItem 
	{
		public int ID;
		public DateTime Time;
		public GDSItemTypes Type;
		
		public string Title;
		public string Url;
		public string Snippet;
		public string IconUrl;
		public string ThumbnailUrl;
		public string CacheUrl;
		public string From;
	}

	public sealed class GDSService
	{

		public GDSService(string SecurityToken, string Server, int Port, int PageSize, int Start) 
			: this(SecurityToken, Server, Port){
			this.PageSize = PageSize;
			this.StartIndex = StartIndex;
		}

		public GDSService(string SecurityToken, string Server, int Port)
			: this(SecurityToken) {
			this.Server = Server;
			this.Port = Port;
		}

		public GDSService(string SecurityToken){
			this.SecurityToken = SecurityToken;
		}

		public GDSService() { }

		public string Server 
		{
			get 
			{
				return m_GDS_Server;
			}
			set 
			{
				if(value != null && value != String.Empty)
					m_GDS_Server = value;
				else
					throw new Exception("Invalid server.");
			}
		}

		public int Port	
		{
			get 
			{
				return m_GDS_Port;
			}
			set 
			{
				if(value > 0 && value < 65535)
					m_GDS_Port = value;
				else
					throw new Exception("Invalid port");
			}
		}
		
		public string SecurityToken 
		{
			get 
			{
				return m_GDS_SecurityToken;
			}
			set 
			{
				if(value != null && value != String.Empty)
					m_GDS_SecurityToken = value;
				else
					throw new Exception("Invalid security token");
			}
		}

		public int Timeout 
		{
			get
			{
				return m_Timeout;
			}
			set 
			{
				if(value > 0)
					m_Timeout = value;
				else
					throw new Exception("Invalid timeout value.");
			}
		}

		public GDSItemTypes TypeFilter 
		{
			get
			{
				return m_TypeFilter;
			}
			set 
			{
				m_TypeFilter = value;
			}
		}

		public int PageSize {
			get {
				return m_GDS_PageSize;
			}
			set {
				if(value > 0)
					m_GDS_PageSize = value;
				else
					throw new Exception("Invalid page size.");
			}
		}

		public int StartIndex {
			get {
				return m_GDS_StartIndex;
			}
			set {
				if(value >= 0)
					m_GDS_StartIndex = value;
				else
					throw new Exception("Invalid item index.");
			}
		}

		// GDS specific parameters
		private string m_GDS_Server = "127.0.0.1";
		private int    m_GDS_Port = 4664;
		private string m_GDS_SecurityToken = String.Empty;
		private int    m_GDS_PageSize = 10;
		private int    m_GDS_StartIndex = 0;

		// Library specific
		private int m_AvgResultBytes = 670;	// ~ Avg 670 bytes per result
		private int m_NetBufferSize = 4096;	// 4 KB Network buffer size
		private int m_StackBound = 1048576 * 2; // ~ 2MB
		private int m_Timeout = 15;
		private GDSItemTypes m_TypeFilter = GDSItemTypes.None;

		private string GetResponse(Stream ResponseStream)
		{
			ArrayList Vector = new ArrayList(
				(m_AvgResultBytes * m_GDS_PageSize) + 2048 // Initial capacity
				);
	
			byte[] NetworkBuffer = new byte[m_NetBufferSize];
			byte[] FillBuffer;
			
			int BytesRead;
			while((BytesRead = ResponseStream.Read(NetworkBuffer, 0, m_NetBufferSize)) != 0)
			{
				if( (Vector.Count + BytesRead) < m_StackBound )
				{
					FillBuffer = new byte[BytesRead];
					
					Array.Copy(
						NetworkBuffer, // Source
						FillBuffer,    // Target
						BytesRead      // Length
						); 

					Vector.AddRange(FillBuffer); // Adding to vector
				}
				else 
				{
					Vector.Clear();
					Vector = null;
					break;
				}
			}

			if(Vector == null)
				throw new Exception("Memory buffer overflow.");

			return CreateValidUTF((byte[]) Vector.ToArray(typeof(byte)));
		}

		private string CreateValidUTF(byte[] RawData){

			for(int i = 0; i < RawData.Length; i++)
			{
				if( RawData[i] < 32 && RawData[i] != 10 && RawData[i] != 13)
					RawData[i] = 32;
			}

			UTF8Encoding UTF = new UTF8Encoding();
			return UTF.GetString(RawData);
		}

		private string GetContent(string URL)
		{
			string RESULT = null;

			HttpWebResponse Response;
			Stream ResponseStream;
			
			HttpWebRequest Request = (HttpWebRequest) HttpWebRequest.Create(URL);
			Request.Timeout = m_Timeout * 1000;
			
			Request.KeepAlive = false;

			try 
			{
				Response = (HttpWebResponse) Request.GetResponse();
			}
			catch
			{
				return null;
			}

			if(Response.StatusCode == HttpStatusCode.OK)
			{	
				ResponseStream = Response.GetResponseStream();
				
				// Async Operations
				AsyncResponse AsyncCall = new AsyncResponse(this.GetResponse); 
				IAsyncResult aResult = AsyncCall.BeginInvoke(ResponseStream, null, null);

				if(aResult.AsyncWaitHandle.WaitOne(m_Timeout * 1000, false))
				{
					if(aResult.IsCompleted)
						RESULT = AsyncCall.EndInvoke(aResult);
					else 
						AsyncCall.EndInvoke(aResult);
				}
				else
					AsyncCall.EndInvoke(aResult);
				
				ResponseStream.Close();
				
				if(RESULT != null)
					return RESULT;
				else
					return null;
			}
			else
			{
				return null;
			}
		}
		
		private string ComposeUrl(string Keywords)
		{

			if(m_GDS_SecurityToken == String.Empty)
				throw new Exception("Security token is not set.");

			Keywords = Keywords.Replace(' ','+');
			Keywords = Keywords.Replace("\"","%22");
			
			string URL = "http://";
			
			URL += m_GDS_Server;
			URL += ":";
			URL += Convert.ToString(m_GDS_Port);
			URL += "/search&s=";
			URL += m_GDS_SecurityToken;
			URL += "?q=";
			URL += Keywords;
			URL += "&format=xml";
			URL += "&num=";
			URL += Convert.ToString(m_GDS_PageSize);
			URL += "&start=";
			URL += Convert.ToString(m_GDS_StartIndex);
		
			return URL;
		}

		private string PrepareXPath()
		{

			if( m_TypeFilter == GDSItemTypes.None)
				return "/results/result/category/parent::node()";

			bool HasValue = false;
			string Filter = String.Empty;

			if( (m_TypeFilter & GDSItemTypes.Email) != 0 )
			{
				Filter = ". = 'email'";
				HasValue = true;
			}
			
			if( (m_TypeFilter & GDSItemTypes.Chat) != 0 )
			{
				if(HasValue)
					Filter += " or . = 'chat'";
				else
				{
					Filter = ". = 'chat'";
					HasValue = true;
				}
			}

			if( (m_TypeFilter & GDSItemTypes.Web) != 0 )
			{
				if(HasValue)
					Filter += " or . = 'web'";
				else
				{
					Filter = ". = 'web'";
					HasValue = true;
				}
			}

			if( (m_TypeFilter &  GDSItemTypes.File) != 0 )
			{
				if(HasValue)
					Filter += " or . = 'file'";
				else
				{
					Filter = ". = 'file'";
					HasValue = true;
				}
			}
		
			return "/results/result/category["+Filter+"]/parent::node()";
		}
		
		
		public GDSResult Search(string Keywords){
			
			string URL = ComposeUrl(Keywords);

			XmlDocument doc = new XmlDocument();
			
			string XML = this.GetContent(URL);
 
			if(XML == null || XML == String.Empty)
				throw new Exception("Can't fetch data form GDS.");

			try 
			{
				doc.LoadXml(XML);
			}
			catch {
				throw new Exception("Error parsing XML response.");
			}

			XmlNodeList ResultNodes = doc.SelectNodes(
				this.PrepareXPath()
				);
			
			ArrayList vector = new ArrayList(10);
			foreach(XmlNode ResultNode in ResultNodes)
			{
				GDSResultItem item = new GDSResultItem();

				try {
					// URL
					XmlNode UrlNode = ResultNode.SelectSingleNode("url");
					item.Url = UrlNode.InnerText;
				} catch {}
				
				try {
					// Title
					XmlNode TitleNode = ResultNode.SelectSingleNode("title");
					item.Title = TitleNode.InnerText;
				} 
				catch {}

				try {
					// Snippet
					XmlNode SnippetNode = ResultNode.SelectSingleNode("snippet");
					item.Snippet = SnippetNode.InnerText;
				} catch {}

				try {
					// ID
					XmlNode IdNode = ResultNode.SelectSingleNode("id");
					item.ID = Convert.ToInt32(IdNode.InnerText);
				} catch {}

				try {
					// Time
					XmlNode TimeNode = ResultNode.SelectSingleNode("time");
					item.Time = DateTime.FromFileTime(Convert.ToInt64(TimeNode.InnerText));
				} catch {}
				
				try {
					// IconUrl
					XmlNode IconNode = ResultNode.SelectSingleNode("icon");
					item.IconUrl = IconNode.InnerText;
				} catch {}
				
				try {
					// CacheUrl
					XmlNode CacheNode = ResultNode.SelectSingleNode("cache_url");
					item.CacheUrl = CacheNode.InnerText;
				} catch {}

				try {
					// ThumbnailUrl
					XmlNode ThumbnailNode = ResultNode.SelectSingleNode("thumbnail");
					item.ThumbnailUrl = ThumbnailNode.InnerText;
				} catch {}
				
				try {
					// From
					XmlNode FromNode = ResultNode.SelectSingleNode("from");
					item.From = FromNode.InnerText;
				} catch {}

				try {
					// Type
					XmlNode CategoryNode = ResultNode.SelectSingleNode("category");
					switch(CategoryNode.InnerText)
					{
						case "file":
							item.Type = GDSItemTypes.File;
							break;
						case "email":
							item.Type = GDSItemTypes.Email;
							break;
						case "chat":
							item.Type = GDSItemTypes.Chat;
							break;
						case "web":
							item.Type = GDSItemTypes.Web;
							break;
					}
				} catch {}

				vector.Add(item);
			}

			XmlNode CountNode = doc.SelectSingleNode("/results");
			XmlAttribute CountAttribute = CountNode.Attributes["count"];

			GDSResult rst = new GDSResult();

			rst.Items = (GDSResultItem[]) vector.ToArray(typeof(GDSResultItem));
			rst.Count = Convert.ToInt32(CountAttribute.Value);
			
			return rst;
		}
	}
}