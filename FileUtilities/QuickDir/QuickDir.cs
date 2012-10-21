//
// Copyright 2004-2005 Jim Wiese  All Rights Reserved
//
// This code is covered by the Creative Commons license (see license.html) or
// http://creativecommons.org/licenses/by-nc-sa/2.0/legalcode or
// http://creativecommons.org/licenses/by-nc-sa/2.0/ for more information.
// 
// I'm pretty liberal in giving out free commercial licenses, so please contact 
// me if you would like to use this code in a commercial 
// project: jcwiese@yahoo.com . 
//
using System;
using System.IO ;
using System.Threading ;
using System.Diagnostics ;
using System.Collections ;
using System.Reflection ;
using System.Windows.Forms ;

using CodeProject.Threading ;
using RJH.CommandLineHelper ;

namespace CodeProject.Utilities
{
	/// <summary>
	/// Class to quickly look for files / directories on a machine
	/// </summary>
	class QuickDir
	{
		bool			m_recurse		= false ;
		int				m_numThreads	= 3 ;
		bool			m_localPath		= false ;
		ThreadQueue		m_foldersToDo	= null ;
		bool			m_debug			= false ;

		public QuickDir( string commandLine ) 
		{
			Parser	parser	= new Parser( commandLine, this ) ;
			parser.Parse() ;
			
			//
			// Add the work to do
			//
			
			ArrayList	toDo	= new ArrayList() ;

			if ( parser.Parameters != null )
				foreach( string pathAndExpression in parser.Parameters ) 
				{
					toDo.Add( new LookFor( pathAndExpression ) ) ;
				}

			m_foldersToDo	= new ThreadQueue( toDo ) ;

		}

		public void		Run() 
		{
			// 
			// Create the threads
			//
			for( int numThreads = 0 ; numThreads < m_numThreads ; numThreads++ ) 
			{
				Thread	worker	= new Thread( new ThreadStart( ThreadMain ) ) ;

				m_foldersToDo.AddProducer( worker ) ;
				worker.Start() ;
			}
		}

		/// <summary>
		/// Main entrypoint for each of the threads
		/// </summary>
		void			ThreadMain() 
		{
			try 
			{
				while( true ) 
				{
					LookFor		toSearch = (LookFor)m_foldersToDo.Remove() ;

					if ( toSearch == null )
						continue; 

					//
					// Add the subdirectories if necessary
					//
					if ( Recurse ) 
					{
						string[]	subDirs	= Directory.GetDirectories( toSearch.Folder ) ;

						foreach( string subdir in subDirs )
						{ 
							m_foldersToDo.Add( new LookFor( subdir, toSearch.Expression), false ) ;
						}
					}

					//
					// Show the folders if no expression
					//
					if ( toSearch.Expression == "*" )
						foreach( string folder in Directory.GetDirectories( toSearch.Folder ) ) 
						{
							string	replaced	= folder.Replace(':', '$') ;

							// Prepend the machine name
							if ( replaced.StartsWith( @"\\" ) )
								Console.WriteLine( replaced ) ;
							else
								Console.WriteLine( @"\\{0}\{1}", Environment.MachineName, replaced ) ;
						}

					//
					// Look for files on this fodlder
					//

					foreach( string filePath in Directory.GetFiles( toSearch.Folder, toSearch.Expression ) ) 
					{
						if ( LocalPath )
							Console.WriteLine( filePath ) ;
						else 
						{
							string	replaced	= filePath.Replace(':', '$') ;

							// Prepend the machine name
							if ( replaced.StartsWith( @"\\" ) )
								Console.WriteLine( replaced ) ;
							else
								Console.WriteLine( @"\\{0}\{1}", Environment.MachineName, replaced ) ;
						}
					}
					
				}
			}
			catch( NoMoreException ) 
			{
				; // Standard exit
			}
			catch( Exception exp ) 
			{
				Trace.WriteLine( exp.Message + "\n" + exp.StackTrace, "Error" ) ;
			}
			finally 
			{
				m_foldersToDo.RemoveProducer() ;
			}
		}

		[CommandLineSwitch("debug", "Turn on debugging information")]
		public bool Debug 
		{
			get { return m_debug ; }
			set { m_debug = value ; }
		}

		[CommandLineSwitch("s", "Recurse the directory structure")]
		public bool	Recurse 
		{
			get { return m_recurse ; }
			set { m_recurse = value ; }
		}

		[CommandLineSwitch("threads", "Number of threads to run while searching")]
		public int	Threads 
		{
			get { return m_numThreads ; }
			set { m_numThreads = value ; }
		}

		[CommandLineSwitch("localPath", "Use the local path name and don't substitute the admin name")]
		public bool	LocalPath 
		{
			get { return m_localPath ; }
			set { m_localPath = value ; }
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			QuickDir	quick		=	null ;

			try 
			{
				string	path	= string.Empty ;

				if ( Assembly.GetEntryAssembly() != null && Assembly.GetEntryAssembly().Location != null )
					path	= Assembly.GetEntryAssembly().Location ;
				else if ( Assembly.GetCallingAssembly() != null && Assembly.GetCallingAssembly() != null )
					path	= Assembly.GetCallingAssembly().Location ;
				else if ( Assembly.GetExecutingAssembly() != null && Assembly.GetExecutingAssembly() != null )
					path	= Assembly.GetExecutingAssembly().Location ;

				quick	=
					new QuickDir( 
					 string.Format( 
								"\"{0}\" ",  
								path ) + " \"" + string.Join( "\" \"", args ) + "\"" 
					) ;
				
				quick.Run() ;
			}
			catch( Exception exp ) 
			{
				if ( quick != null && quick.Debug )
					Console.Error.WriteLine( "\nError: " + exp.Message + "\n" + exp.StackTrace ) ;
				else
					Console.Error.WriteLine( "\nError: " + exp.Message + "\n" ) ;
			}
		}

		public class LookFor 
		{
			public string	Folder		= string.Empty ;
			public string	Expression	= "*" ;
			
			public LookFor( string pathAndExpression ) 
			{
				if ( !Directory.Exists( pathAndExpression ) ) 
				{
					Folder	= 
						Environment.ExpandEnvironmentVariables( Path.GetDirectoryName( pathAndExpression ) );

					if ( !Directory.Exists( Folder ) )
						throw new ArgumentException( 
							string.Format("The specified folder {0} in {1} does not exist", Folder, pathAndExpression),
							"pathAndExpression" ) ;

					Expression	= Path.GetFileName( pathAndExpression ) ;

					if ( Expression == string.Empty )
						Expression = "*" ;
				}
				else
					Folder	= pathAndExpression ;
			}
			public LookFor( string folder, string expression ) 
			{
				Folder		= folder ;
				Expression	= expression ;
			}
		}
	}
}
