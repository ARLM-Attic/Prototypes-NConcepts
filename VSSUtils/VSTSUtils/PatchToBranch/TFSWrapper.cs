using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;
using System.IO;

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
            Workspace workspace;
            GetPathAndScope(szFile, out sourceControl, out workspace);
            
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
                                     out VersionControlServer sourceControl,
                                     out Workspace workspace)
        {

            // Figure out the server based on either the argument or the
            // current directory.
            WorkspaceInfo wsInfo = null;
            workspace = null;
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
                workspace = wsInfo.GetWorkspace(tfs);
                scope = workspace.GetServerItemForLocalItem(szFile);
            }
            else
            {
                scope = szFile;
            }
        }

        //Creates a BatchScriptCreater and does the merging or makes a file
        public void CreateBranchAndMerge(System.Collections.ICollection historyAndLabelList, String szPatchPath, String szBranchPath, String szTrunkPath, VersionControlLabel trunkLabel, String szBatchFileName)
        {
            VersionControlServer sourceControl = null;
            Workspace workspace = null;
            
            //
            GetPathAndScope(szPatchPath, out sourceControl, out workspace);

        }
    }
}
        /**
        private class BatchScriptCreater
        {
            //member variables:
            System.Collections.ICollection m_cHistoryAndLabelList;
            String m_szPatchPath;
            String m_szBranchPath;
            String m_szTrunkPath;
            String m_szServer;
            String m_szBatchFileName;

            VersionControlLabel m_oTrunkLabel;
            FileInfo m_fiBatchScriptFile;
            StreamWriter m_swFileWriter;
            bool m_bFileInfoInitialized;
            bool m_bEcho;

            VersionControlServer m_oSourceControl;
            Workspace m_oWorkspace;


            public BatchScriptCreater(System.Collections.ICollection historyAndLabelList, String patchPath, String branchPath, String trunkPath, String server, String batchFileName, VersionControlLabel trunkLabel)
            {
                m_bFileInfoInitialized = true;
                m_bEcho = true;

                try
                {
                    // if no file name is passed in, we can continue.
                    if (m_szBatchFileName != null)
                    {
                        m_szBatchFileName = batchFileName;
                    }
                    else
                    {
                        //use default name!
                        m_szBatchFileName = "PatchToBranch_" + trunkLabel.Name + ".bat";
                        //popup a screen to  show that this name is used.
                        System.Windows.Forms.MessageBox.Show("File output will be named " + m_szBatchFileName);
                    }

                    //if we're missing any other information, we cannot continue
                    if (historyAndLabelList != null)
                    {
                        m_cHistoryAndLabelList = historyAndLabelList;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (patchPath != null)
                    {
                        m_szPatchPath = patchPath;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (m_szBranchPath != null)
                    {
                        m_szBranchPath = branchPath;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (m_szTrunkPath != null)
                    {
                        m_szTrunkPath = trunkPath;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (m_szServer != null)
                    {
                        m_szServer = server;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (m_oTrunkLabel != null)
                    {
                        m_oTrunkLabel = trunkLabel;
                    }
                    else
                    {
                        m_bFileInfoInitialized = false;
                    }

                    if (m_bFileInfoInitialized)
                    {
                     //   GetPathAndScope(m_szPatchPath, out m_oSourceControl, out m_oWorkspace);
                     //   m_fiBatchScriptFile = new FileInfo(m_szBatchFileName); //set up a file
                     //   m_swFileWriter = m_fiBatchScriptFile.CreateText(); //Create the file and get an object to write to it
                    }
                    else
                    {
                        //Pop up a box explaining what fields are missing:
                        String szMissingFieldMessage = "The batch file cannot be created for the following reasons:";
                        if (historyAndLabelList == null)
                        {
                            szMissingFieldMessage += "No history/labels have been retrieved; please select a patch folder and click \"View History\" before creating batch file.";
                        }
                        else
                        {
                            //all other messages can be included together
                            if (patchPath == null)
                            {
                                szMissingFieldMessage += "Error:  No patch folder is currently selected.\n";
                            }
                            if (branchPath == null)
                            {
                                szMissingFieldMessage += "Error: No branch path has been specified.\n";
                            }
                            if (trunkPath == null)
                            {
                                szMissingFieldMessage += "Error: No trunk path has been specified.\n";
                            }
                            if (trunkLabel == null)
                            {
                                szMissingFieldMessage += "Error: No initial trunk label has been specified.\n";
                            }
                            if (server == null)
                            {
                                szMissingFieldMessage += "Error: No server has been selected.";
                            }
                            szMissingFieldMessage += "    Please correct and attempt again.";
                        }

                        System.Windows.Forms.MessageBox.Show(szMissingFieldMessage);
                    }
                }
                catch(Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

            public void CreateBatchScript(System.Collections.ICollection historyAndLabelList, String patchPath, String branchPath, String trunkPath, String server, String batchFileName, VersionControlLabel trunkLabel)
            {
                if (m_bFileInfoInitialized)
                    CreateBatchScript( historyAndLabelList, patchPath, branchPath, trunkPath, server, batchFileName, trunkLabel, true);
            }

            public void CreateBatchScript(System.Collections.ICollection historyAndLabelList, String patchPath, String branchPath, String trunkPath, String server, String batchFileName, VersionControlLabel trunkLabel, bool bEchoMergeInfo)
            {

                if (!m_bFileInfoInitialized)
                {
                    //notify that no file was created; exit
                    System.Windows.Forms.MessageBox.Show("Error; no file will be created.  Please be certain that all fields are entered.");
                    return;
                }
                
                //variables to keep track of information:
                bool bBaselessMergeDone = false;
                m_bEcho = bEchoMergeInfo;

                //Write the introductory information to the batch script
                m_swFileWriter.WriteLine("REM THIS SCRIPT CREATED BY PatchToBranch " + DateTime.UtcNow.ToShortDateString() + m_swFileWriter.NewLine);
                m_swFileWriter.WriteLine("REM ** Please note that for this script to perform properly, it should be run");
                m_swFileWriter.WriteLine("REM **  from the workspace folder that maps to the project within VSTS and TF");
                m_swFileWriter.WriteLine("REM **  must be in the PATH" + m_swFileWriter.NewLine + m_swFileWriter.NewLine);
                m_swFileWriter.WriteLine("REM  This script will create a branch by the following algorithm:");
                m_swFileWriter.WriteLine("REM   1.) Create a branch from the original source at the specified label.");
                m_swFileWriter.WriteLine("ECHO       Trunk from which Patch was created:");
                m_swFileWriter.WriteLine("ECHO         " + trunkPath);
                m_swFileWriter.WriteLine("ECHO       Trunk Label from which Patch was created: ");
                m_swFileWriter.WriteLine("ECHO         " + trunkLabel.Name);
                m_swFileWriter.WriteLine("ECHO       Patch location:                           ");
                m_swFileWriter.WriteLine("ECHO         " + patchPath);
                m_swFileWriter.WriteLine("ECHO       New branch location:                      ");
                m_swFileWriter.WriteLine("ECHO         " + branchPath);
                m_swFileWriter.WriteLine("REM   2.) This file will import each changeset from the Patch folder to the");
                m_swFileWriter.WriteLine("REM       branch independently, applying labels at the approprate times.");
                m_swFileWriter.WriteLine("REM       The first changeset to be imported will be merged from the Patch");
                m_swFileWriter.WriteLine("REM       as a baseless merge; subsequent changesets will be merged as");
                m_swFileWriter.WriteLine("REM       standard changeset merges.");
                m_swFileWriter.WriteLine("REM   3.) Whenever a label is reached in the changeset/label list, a label");
                m_swFileWriter.WriteLine("REM       with the same name will be applied to the new patch folder; (for ");
                m_swFileWriter.WriteLine("REM       example, if a label X was applied  to the patch after changeset 123");
                m_swFileWriter.WriteLine("REM       and before changeset 124, this script will first merge changeset 123,");
                m_swFileWriter.WriteLine("REM       then apply a label with the same name as label X, then merge in 124." + m_swFileWriter.NewLine + m_swFileWriter.NewLine);

                m_swFileWriter.WriteLine("echo *** Begin Branch Creation ***");
                if (m_bEcho)
                {
                    m_swFileWriter.WriteLine("@ECHO ON");
                }
                else
                {
                    m_swFileWriter.WriteLine("@ECHO OFF");
                }

                //now, scroll through list and add command line options.
                foreach (object versionItem in historyAndLabelList)
                {
                    if (versionItem is VersionControlLabel)
                    {
                        CreateLabelCommand(versionItem as VersionControlLabel);
                    }
                    if (versionItem is Changeset)
                    {
                        if (bBaselessMergeDone)
                        {
                            CreateMergeCommand(versionItem as Changeset);
                            bBaselessMergeDone = true;
                        }
                        else
                        {
                            CreateBaselessMergeCommand(versionItem as Changeset);
                        }
                    }
                }
            }

            private void CreateBaselessMergeCommand(Changeset changeset)
            {
                //Write a string out!
                String szCommandString = "tf merge /version:" + changeset.ChangesetId.ToString() + "~" + changeset.ChangesetId.ToString() + "/force ";
                szCommandString += m_szPatchPath + "/baseless" + m_szBranchPath + "/recursive";



            }

            private void CreateMergeCommand(Changeset changeset)
            {
            }

            private void CreateLabelCommand(VersionControlLabel label)
            {
            }
        }
    }
}
*/
