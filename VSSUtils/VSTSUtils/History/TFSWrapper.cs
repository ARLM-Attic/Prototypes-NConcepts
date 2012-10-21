using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace History
{
    class TFSWrapper
    {
        public class ChangeSetLabels
        {
            public bool m_bIsUsed = false;
            public Changeset m_csChangeset;
            public System.Collections.ArrayList m_alLabels = new System.Collections.ArrayList();
        }

        public class ChangeSetLabelObject
        {
            public DateTime m_dtCreationDate;
            public bool m_bIsLabel;
            public Changeset m_oChangeset = null;
            public VersionControlLabel m_oLabel = null;

            public ChangeSetLabelObject() { }
            public ChangeSetLabelObject(Changeset oChangeset)
            {
                m_dtCreationDate = oChangeset.CreationDate;
                m_bIsLabel = false;
                m_oChangeset = oChangeset;
            }
            public ChangeSetLabelObject(VersionControlLabel oLabel)
            {
                m_dtCreationDate = oLabel.LastModifiedDate;
                m_bIsLabel = true;
                m_oLabel = oLabel;
            }

        }

        string m_szServerAddress = null;

        public TFSWrapper(string szServer)
        {
            m_szServerAddress = szServer;
        }

        public System.Collections.ICollection GetHistory(string szFile)
        {
            VersionControlServer sourceControl;
            GetPathAndScope(szFile, out sourceControl);

            // Retrieve and print the label history for the file.
            VersionControlLabel[] labels = null;
            ChangeSetLabelObject[] ChangesetLabelObjects = new ChangeSetLabelObject[10000];
            Item targetFile = null;
            System.Collections.IEnumerable history = null;
            System.Collections.SortedList slChangeSetsAndLabels = new System.Collections.SortedList();
            System.Collections.SortedList slChangeSets = new System.Collections.SortedList();

            try
            {
                targetFile = sourceControl.GetItem(szFile);

                //Query labels seems to return labels that correspond to the file and nothing filtered related the version spec
                //So we need to query everything and then filter ourselves.
                //Hint: The there is a TimeSpan here because doing that is slow.
                DateTime dtStart = DateTime.Now;
                labels = sourceControl.QueryLabels(null, szFile, null, true, szFile, VersionSpec.Latest);
                TimeSpan tsQueryLabels = DateTime.Now - dtStart;

                history = sourceControl.QueryHistory(szFile, 
                                                     VersionSpec.Latest,
                                                     0,
                                                     RecursionType.Full,
                                                     null,
                                                     null,
                                                     null,
                                                     10000,
                                                     true,
                                                     false);
                TimeSpan tsAnotherTimeSpan = DateTime.Now - dtStart;
            }
            catch (TeamFoundationServerException e)
            {
                // We couldn't contact the server, the item wasn't found,
                // or there was some other problem reported by the server,
                // so we stop here.
                System.Windows.Forms.MessageBox.Show(e.Message);
                return slChangeSets.Values;
            }

            if (labels.Length == 0)
            {
                Console.WriteLine("There are no labels for " + szFile);
                return slChangeSets.Values;
            }
            else
            {
                //first, sort all the changesets into a list
                foreach (Changeset c in history)
                {
                    ChangeSetLabels csl = new ChangeSetLabels();
                    csl.m_csChangeset = c;
                    slChangeSets[c.ChangesetId] = csl;
                    slChangeSetsAndLabels[c.CreationDate] = c;
                }

                foreach (VersionControlLabel l in labels)
                {
                    slChangeSetsAndLabels[l.LastModifiedDate] = l;
                }
            }
            return slChangeSetsAndLabels.Values;
        }

        private void GetPathAndScope(string szFile,
                                     out VersionControlServer sourceControl)
        {

            // Figure out the server based on either the argument or the
            // current directory.
            WorkspaceInfo wsInfo = null;
            if (!VersionControlPath.IsServerItem(szFile))
            {
                wsInfo = Workstation.Current.GetLocalWorkspaceInfo(szFile);
            }

            TeamFoundationServer tfs = null;

            if (wsInfo == null)
            {
                wsInfo = Workstation.Current.GetLocalWorkspaceInfo(Environment.CurrentDirectory);
            }

            if (wsInfo != null)
            {
                //Just in case our local file is hooked to a different server
                m_szServerAddress = wsInfo.ServerUri.AbsoluteUri;
            }

            tfs = TeamFoundationServerFactory.GetServer(m_szServerAddress);

            sourceControl = (VersionControlServer)tfs.GetService(typeof(VersionControlServer));

            // Pick up the label scope, if supplied.
            string scope = VersionControlPath.RootFolder;
            // The scope must be a server path, so we convert it here if
            // the user specified a local path.
            if (!VersionControlPath.IsServerItem(szFile))
            {
                Workspace workspace = wsInfo.GetWorkspace(tfs);
                scope = workspace.GetServerItemForLocalItem(szFile);
            }
            else
            {
                scope = szFile;
            }
        }
    }
}
