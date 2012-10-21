using System;
using System.IO;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace LabelHistory
{
    class Program
    {
        class ChangeSetLabels
        {
            public Changeset m_csChangeset;
            public System.Collections.ArrayList m_alLabels = new System.Collections.ArrayList();
        }

        static void Main(string[] args)
        {
            // Check and get the arguments.
            String path, scope;
            VersionControlServer sourceControl;
            GetPathAndScope(args, out path, out scope, out sourceControl);

            // Retrieve and print the label history for the file.
            VersionControlLabel[] labels = null;
            Item targetFile = null;
            System.Collections.IEnumerable history = null;

            System.Collections.SortedList slChangeSets = new System.Collections.SortedList();

            try
            {
                // The first three arguments here are null because we do not
                // want to filter by label name, scope, or owner.
                // Since we don't need the server to send back the items in
                // the label, we get much better performance by ommitting
                // those through setting the fourth parameter to false.

                DateTime dtStart = DateTime.Now;
                //scope = "$/PCSGlobal/DevTools/Authoring";
                //string labelfilter = "RadAuthor_20060524*";
                //VersionSpec vspec = VersionSpec.ParseSingleSpec("C23953", null);
                //labels = sourceControl.QueryLabels(labelfilter, scope, null, true, path, VersionSpec.Latest);
                labels = sourceControl.QueryLabels(null, scope, null, false, path, VersionSpec.Latest);
                //labels = sourceControl.QueryLabels(null, scope, null, true, path, VersionSpec.Latest);
                //labels = sourceControl.QueryLabels(labelfilter, scope, null, true, path, vspec);

                TimeSpan tsQueryLabels = DateTime.Now - dtStart;


                targetFile = sourceControl.GetItem(path);

                history = sourceControl.QueryHistory(path, VersionSpec.Latest, 0, RecursionType.None, null, null, null, 1000, true, false);
                
            }
            catch (TeamFoundationServerException e)
            {
                // We couldn't contact the server, the item wasn't found,
                // or there was some other problem reported by the server,
                // so we stop here.
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }

            if (labels.Length == 0)
            {
                Console.WriteLine("There are no labels for " + path);
            }
            else
            {
                


                foreach (Changeset c in history)
                {
                    ChangeSetLabels csl = new ChangeSetLabels();
                    csl.m_csChangeset = c;
                    slChangeSets[c.ChangesetId] = csl;

                    VersionControlLabel[] vclChangeSetLabels = null;

                    string s = c.ChangesetId.ToString();
                    VersionSpec vspec = new ChangesetVersionSpec(c.ChangesetId.ToString());
                    //VersionSpec vspec = VersionSpec.ParseSingleSpec("C" + c.ChangesetId.ToString(), null);
                    //labels = sourceControl.QueryLabels(labelfilter, scope, null, true, path, VersionSpec.Latest);
                    //labels = sourceControl.QueryLabels(labelfilter, scope, null, true, path, vspec);

                    DateTime dtStart = DateTime.Now;
                    vclChangeSetLabels = sourceControl.QueryLabels(null, scope, null, false, path, vspec);
                    TimeSpan tsQueryLabels = DateTime.Now - dtStart;
                    foreach (VersionControlLabel vclChangeSetLabel in vclChangeSetLabels)
                    {
                        VersionControlLabel[] vclEverything = null;
                        vclEverything = sourceControl.QueryLabels(vclChangeSetLabel.Name, scope, null, false, path, vspec);
                        //csl.m_alLabels.AddRange(vclEverything);
                    }
                    //csl.m_alLabels.AddRange(vclChangeSetLabels);
                }

                foreach (VersionControlLabel label in labels)
                {
                    // Display the label's name and when it was last modified.
                    Console.WriteLine("{0} ({1})", label.Name,
                                      label.LastModifiedDate.ToString("g"));
                    
                    foreach (Item item in label.Items)
                    {
                        if (item.ItemId != targetFile.ItemId)
                        {
                            //skip over any items that we don't care about
                            continue;
                        }

                        ChangeSetLabels csl = slChangeSets[item.ChangesetId] as ChangeSetLabels;
                        csl.m_alLabels.Add(label);
                    }
                    // For labels that actually have comments, display it.
                    if (label.Comment.Length > 0)
                    {
                        Console.WriteLine("   Comment: " + label.Comment);
                    }
                }

                //Reverse the order of everything as it is being displayed so that it looks like the VSS order
                System.Collections.ArrayList alReverseOrder = new System.Collections.ArrayList(slChangeSets.Values);
                alReverseOrder.Reverse();
                foreach (ChangeSetLabels csl in alReverseOrder)
                {
                    csl.m_alLabels.Reverse();
                    foreach (VersionControlLabel vcl in csl.m_alLabels)
                    {
                        Console.WriteLine("   Labeled: " + vcl.Name);
                    }
                    Console.WriteLine("   Checked-in: Changeset C" + csl.m_csChangeset.ChangesetId.ToString());
                    Console.WriteLine("     Comment: C" + csl.m_csChangeset.Comment.ToString());
                }
            }
        }

        private static void GetPathAndScope(String[] args,
                                            out String path, out String scope,
                                            out VersionControlServer sourceControl)
        {
            // This little app takes either no args or a file path and optionally a scope.
            if (args.Length > 2 ||
                args.Length == 1 && args[0] == "/?")
            {
                Console.WriteLine("Usage: labelhist");
                Console.WriteLine("       labelhist path [label scope]");
                Console.WriteLine();
                Console.WriteLine("With no arguments, all label names and comments are displayed.");
                Console.WriteLine("If a path is specified, only the labels containing that path");
                Console.WriteLine("are displayed.");
                Console.WriteLine("If a scope is supplied, only labels at or below that scope will");
                Console.WriteLine("will be displayed.");
                Console.WriteLine();
                Console.WriteLine("Examples: labelhist c:\\projects\\secret\\notes.txt");
                Console.WriteLine("          labelhist $/secret/notes.txt");
                Console.WriteLine("          labelhist c:\\projects\\secret\\notes.txt $/secret");
                Environment.Exit(1);
            }

            // Figure out the server based on either the argument or the
            // current directory.
            WorkspaceInfo wsInfo = null;
            if (args.Length < 1)
            {
                path = null;
            }
            else
            {
                path = args[0];
                try
                {
                    if (!VersionControlPath.IsServerItem(path))
                    {
                        wsInfo = Workstation.Current.GetLocalWorkspaceInfo(path);
                    }
                }
                catch (Exception e)
                {
                    // The user provided a bad path argument.
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }

            TeamFoundationServer tfs = null;

            if (wsInfo == null)
            {
                wsInfo = Workstation.Current.GetLocalWorkspaceInfo(Environment.CurrentDirectory);
            }

            // Stop if we couldn't figure out the server.
            if (wsInfo == null)
            {
                //Console.Error.WriteLine("Unable to determine the server.");
                //Environment.Exit(1);
                //tfs = TeamFoundationServerFactory.GetServer("http://tfsappserver:8080");
                tfs = TeamFoundationServerFactory.GetServer("http://tfs.radiantsystems.com:8080");
            }
            else
            {
                tfs = TeamFoundationServerFactory.GetServer(wsInfo.ServerUri.AbsoluteUri);
            }
            //    TeamFoundationServerFactory.GetServer(wsInfo.ServerName);
            // RTM: wsInfo.ServerUri.AbsoluteUri);
            sourceControl = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));

            // Pick up the label scope, if supplied.
            scope = VersionControlPath.RootFolder;
            if (args.Length == 2)
            {
                // The scope must be a server path, so we convert it here if
                // the user specified a local path.
                if (!VersionControlPath.IsServerItem(args[1]))
                {
                    Workspace workspace = wsInfo.GetWorkspace(tfs);
                    scope = workspace.GetServerItemForLocalItem(args[1]);
                }
                else
                {
                    scope = args[1];
                }
            }
        }
    }
}
