using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Common;

namespace History
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            m_cbServerName.Items.Add("http://tfsappserver:8080");
            m_cbServerName.Items.Add("http://tfs.radiantsystems.com:8080");
            m_cbServerName.SelectedIndex = 0;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // TFSWrapper.ChangeSetLabelObject csl = e.Node.Tag as TFSWrapper.ChangeSetLabelObject;
            VersionControlLabel vcl = e.Node.Tag as VersionControlLabel;
            Changeset cs = e.Node.Tag as Changeset;

            if (cs != null)
            {
                m_tbComments.Text = "Date:\t\t" + cs.CreationDate.ToString() + "\r\nCommited by:\t" + cs.Committer + "\r\nUser:\t\t" + cs.Owner + "\r\nComment:\r\n" + cs.Comment;
                m_tbUser.Text = cs.Owner;
                m_tbDate.Text = cs.CreationDate.ToString();
                foreach (Change change in cs.Changes)
                {
                    this.textBox1.Text = "Item:\r\n\t" + change.Item.ServerItem.ToString() + "\n\rChange Type:\t" + change.ChangeType.ToString();
                }
            }
            else if (vcl != null)
            {
                m_tbComments.Text = "Labeled as " + vcl.Name + "on date " + vcl.LastModifiedDate + "\r\nComment: " + vcl.Comment;
                m_tbUser.Text = vcl.OwnerName;
                m_tbDate.Text = vcl.LastModifiedDate.ToString();
            }
            else
            {
                m_tbComments.Text = "";
                m_tbUser.Text = "";
                m_tbDate.Text = "";
            }
        }

        private void m_btnViewHistory_Click(object sender, EventArgs e)
        {
            TFSWrapper tfs = new TFSWrapper(m_cbServerName.SelectedItem.ToString());

            System.Collections.ICollection cslFileHistory;
            cslFileHistory = tfs.GetHistory(m_tbFile.Text);

            m_tvChangeSets.BeginUpdate();
            m_tvChangeSets.Nodes.Clear();

            TreeNode tnRoot = new TreeNode(m_tbFile.Text);
            m_tvChangeSets.Nodes.Add(tnRoot);

            foreach (object csl in cslFileHistory)
            {
                if (csl == null)
                {
                    continue;
                }
                if (csl is Changeset)
                {
                    TreeNode tnChangeSet = new TreeNode((csl as Changeset).ChangesetId.ToString() + "    " + (csl as Changeset).CreationDate.ToString());
                    tnChangeSet.Tag = (csl as Changeset);
                    tnRoot.Nodes.Add(tnChangeSet);
                }
                else if (csl is VersionControlLabel)
                {
                    TreeNode tnLabel = new TreeNode("    Labeled: " + (csl as VersionControlLabel).Name + "    " + (csl as VersionControlLabel).LastModifiedDate.ToString());
                    tnLabel.Tag = (csl as VersionControlLabel);
                    tnRoot.Nodes.Add(tnLabel);
                }
            }

            m_tvChangeSets.EndUpdate();
        }

    }
}