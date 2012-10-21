using System;
using System.Xml;
using System.IO;
using System.Text;
using GDSLibrary;

namespace GDSSampleApp
{
	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
			GDSService gs = new GDSService();
			
			gs.Server = "127.0.0.1";
			gs.Port = 4664;
			gs.SecurityToken = "p20rZU6jcNotPKO_TvMSGHgSh90";
			gs.TypeFilter = GDSItemTypes.All;
			
			Console.WriteLine("Please enter search keywords:");
			GDSResult rst = gs.Search(Console.ReadLine());
			
			Console.WriteLine(rst.Count + " results found.");
			foreach(GDSResultItem item in rst.Items){
				Console.WriteLine(item.Url);
			}

			Console.ReadLine();
			
		}
	}
}
