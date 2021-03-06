using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
namespace RadVSTSHelper
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            // Retrieve the event objects from the automation model.
            EnvDTE.Events events = _applicationObject.Events;
            // Send event messages to the Output window.
            OutputWindow outputWindow = (OutputWindow)_applicationObject.Windows.Item(Constants.vsWindowKindOutput).Object;
            //Constants.vsWindowKindMainWindow;

            outputWindowPane = outputWindow.OutputWindowPanes.Add("DTE Event Information");
            // Retrieve the event objects from the automation model.
            events.SelectionEvents.OnChange += new _dispSelectionEvents_OnChangeEventHandler(this.SelectionEvents_OnChange);
            events.get_CommandEvents("{00000000-0000-0000-0000-000000000000}", 0).AfterExecute += new _dispCommandEvents_AfterExecuteEventHandler(this.AfterExecuteEventHandler);
            winEvents = 
            (EnvDTE.WindowEvents)events.get_WindowEvents(null);
            
            // Connect to each delegate exposed from each object 
            // retrieved above.
            winEvents.WindowActivated += new 
            _dispWindowEvents_WindowActivatedEventHandler
            (this.WindowActivated);
            winEvents.WindowClosing += new  
            _dispWindowEvents_WindowClosingEventHandler
            (this.WindowClosing);
            winEvents.WindowCreated += new  
            _dispWindowEvents_WindowCreatedEventHandler
            (this.WindowCreated);
            winEvents.WindowMoved += new 
            _dispWindowEvents_WindowMovedEventHandler
            (this.WindowMoved);

		}

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}

        public void SelectionEvents_OnChange()
        {
            Log("Selection Change", "");

        }

        public void BeforeExecuteEventHandler (string Guid, int ID,Object CustomIn,Object CustomOut,
                                               ref bool CancelDefault)
        {
            Log("Id", ID.ToString());
            //Log("Guid", Guid);
            //Log("In", CustomIn.ToString());
            //Log("Out", CustomOut.ToString());
        }

        public void AfterExecuteEventHandler(string Guid, int ID, Object CustomIn, Object CustomOut)
        {
            Log("Id", ID.ToString());
            //Log("Guid", Guid);
            //Log("In", CustomIn.ToString());
            //Log("Out", CustomOut.ToString());
        }

        // Window-related events.
        public void WindowClosing(EnvDTE.Window closingWindow)
        {
            outputWindowPane.OutputString
("WindowEvents::WindowClosing\n");
            outputWindowPane.OutputString("\tWindow: " +
closingWindow.Caption + "\n");
        }

        public void WindowActivated(EnvDTE.Window gotFocus,
        EnvDTE.Window lostFocus)
        {
            outputWindowPane.OutputString
("WindowEvents::WindowActivated\n");
            outputWindowPane.OutputString("\tWindow receiving focus: "
+ gotFocus.Caption + "\n");
            outputWindowPane.OutputString("\tWindow that lost focus: "
+ lostFocus.Caption + "\n");
            if (gotFocus.Caption == "Source Control Explorer")
            {
                foreach (Command c in _applicationObject.Commands)
                {
                    if (c.Name.Contains("History"))
                    {
                        Log("Command Name", c.Name);
                        Log("  Command Id", c.ID.ToString());
                    }
                }
                Log("Kind", gotFocus.Kind);
                Log("ObjectKind", gotFocus.ObjectKind);
                if (gotFocus.Object == null)
                {
                    Log("Object", "Is null");
                }
                else
                {
                    Log("ObjectType", gotFocus.Object.GetType().ToString());
                }
                if (gotFocus.Document == null)
                {
                    Log("Doc", "Is null");
                }
                else
                {
                    Log("Doc", gotFocus.Document.ToString());
                    Log("DocName", gotFocus.Document.Name);
                }
                //gotFocus.
                
                //outputWindowPane.OutputString("\tSelection: " + gotFocus.Selection.ToString() + "\n");
                //outputWindowPane.OutputString("\tProject: " + gotFocus.Project.Name + "\n");
                //outputWindowPane.OutputString("\tProjectItem: " + gotFocus.ProjectItem.Name + "\n");
            }
        }

        private void Log(string szCaption, string szStringToLog)
        {
            outputWindowPane.OutputString("\t" + szCaption + ":" + szStringToLog + "\n");
        }

        public void WindowCreated(EnvDTE.Window window)
        {
            outputWindowPane.OutputString
            ("WindowEvents::WindowCreated\n");
            outputWindowPane.OutputString("\tWindow: " + window.Caption
+ "\n");
        }

        public void WindowMoved(EnvDTE.Window window, int top, int
        left, int width, int height)
        {
            outputWindowPane.OutputString
            ("WindowEvents::WindowMoved\n");
            outputWindowPane.OutputString("\tWindow: " + window.Caption
+ "\n");
            outputWindowPane.OutputString("\tLocation: (" +
top.ToString() + " , " + left.ToString() + " , " +
width.ToString() + " , " + height.ToString() + ")\n");
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private EnvDTE.WindowEvents winEvents;
        private OutputWindowPane outputWindowPane;
        
	}
}