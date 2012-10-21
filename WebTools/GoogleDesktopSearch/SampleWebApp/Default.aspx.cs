using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GDSLibrary;

namespace googleye
{
	public class WebForm1 : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox Keywords;
		protected System.Web.UI.WebControls.DataList Results;
		protected System.Web.UI.WebControls.Label Counter;
		protected System.Web.UI.WebControls.Button Search;
	
		private ICollection GenerateDatasource(string Keywords, out int Count){

			DataTable dt = new DataTable();
			DataRow dr;
 
			dt.Columns.Add(new DataColumn("Title", typeof(string)));
			dt.Columns.Add(new DataColumn("Snippet", typeof(string)));
			dt.Columns.Add(new DataColumn("Url", typeof(string)));
			dt.Columns.Add(new DataColumn("HighlightedUrl", typeof(string)));

			
			// Library use
			GDSService gs = new GDSService();

			gs.Server = "127.0.0.1";
			gs.Port = 4664;
			gs.SecurityToken = ConfigurationSettings.AppSettings["SecurityToken"];
			gs.TypeFilter = GDSItemTypes.File;
			gs.Timeout = Convert.ToInt32(ConfigurationSettings.AppSettings["Timeout"]);
			gs.PageSize = Convert.ToInt32(ConfigurationSettings.AppSettings["PageSize"]);
			gs.StartIndex = 0;
			
			GDSResult rst = gs.Search(Keywords);
			
			int Counter = 0;

			foreach(GDSResultItem item in rst.Items)
			{
				if(Convert.ToBoolean(ConfigurationSettings.AppSettings["UseDirFilter"]))
				{
					if(item.Url.IndexOf(ConfigurationSettings.AppSettings["SearchDir"]) != -1)
					{
						dr = dt.NewRow();
				
						dr[0] = item.Title;
						dr[1] = item.Snippet;
						dr[2] = item.Url;
	
						dr[3] = "http://mermaid.cms.com.ua/bluegoogle/office.asp?file=";
						dr[3] += item.Url.Replace(ConfigurationSettings.AppSettings["SearchDir"], String.Empty);
						dr[3] += "&words=";
						dr[3] += Keywords.Replace(" ", "&words=");
						dr[3] += "&case=0";
						
						dt.Rows.Add(dr);

						Counter++;
					}
				}
				else
				{
					dr = dt.NewRow();
				
					dr[0] = item.Title;
					dr[1] = item.Snippet;
					dr[2] = item.Url;
				
					dt.Rows.Add(dr);
					
					Counter++;
				}
			}

			//////////////////////////////
		
			Count = Counter;

			DataView dv = new DataView(dt);
			return dv;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				Results.Visible = false;
			}
			else
			{
				int Count = 0;
				Results.DataSource = GenerateDatasource(Request["Keywords"],out Count);
				Results.DataBind();
				Results.Visible = true;
				
				Counter.Text = Count + " results found.";
				Counter.Visible = true;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
