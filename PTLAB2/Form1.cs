using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTLAB2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            this.treeView1.Nodes.Clear();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Console.WriteLine(fbd.SelectedPath);

                string path = fbd.SelectedPath;
                DirectoryInfo DI = new DirectoryInfo(path);
                TreeNode mTreeNode = new TreeNode(DI.FullName, TraverseDirectory(DI));
                this.treeView1.Nodes.Add(mTreeNode);
                this.treeView1.ExpandAll();





            }
        }

        private TreeNode[] TraverseDirectory(DirectoryInfo root)
        {
            FileInfo[] files = root.GetFiles();
            DirectoryInfo[] directories = root.GetDirectories();

            TreeNode[] nodes = new TreeNode[(files.Length + directories.Length)];
            int index = 0;

            foreach (var directory in directories)
            {
                TreeNode dTreeNode = new TreeNode(directory.Name, TraverseDirectory(directory));
                nodes[index] = dTreeNode;
                index++;

            }
            foreach (var file in files)
            {

                TreeNode nTreeNode = new TreeNode(file.Name);
                nodes[index] = nTreeNode;
                index++;
            }


            return nodes;




        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
            TreeNode selectedFile = this.treeView1.SelectedNode;
           
            FileInfo file = new FileInfo(selectedFile.FullPath);
            toolStripStatusLabel1.Text = file.GetRahs();

            if (IsTxtFile(selectedFile.Text))
            {
                StreamReader sr = new StreamReader(selectedFile.FullPath);
                String text = sr.ReadToEnd();
                textBox1.Text = text;

            }

        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {

            TreeNode selectedFile = this.treeView1.SelectedNode;
            this.treeView1.LabelEdit = true;
            selectedFile.BeginEdit();
            //this.treeView1.BeforeLabelEdit += treeView1_BeforeLabelEdit;
           // this.treeView1.AfterLabelEdit += treeView1_AfterLabelEdit;
            
        
        
        }

        //private void treeView1_AfterLabelEdit(object sender,
        // System.Windows.Forms.NodeLabelEditEventArgs e)
        //{
        //    if (e.Label != null)
        //    {
        //        if (e.Label.Length > 0)
        //        {
        //            if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
        //            {
                      
        //                e.Node.EndEdit(false);
        //            }
        //            else
        //            {
        //                /* Cancel the label edit action, inform the user, and 
        //                   place the node in edit mode again. */
        //                e.CancelEdit = true;
        //                MessageBox.Show("Invalid tree node label.\n" +
        //                   "The invalid characters are: '@','.', ',', '!'",
        //                   "Node Label Edit");
        //                e.Node.BeginEdit();
        //            }
        //        }
        //        else
        //        {
        //            /* Cancel the label edit action, inform the user, and 
        //               place the node in edit mode again. */
        //            e.CancelEdit = true;
        //            MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
        //               "Node Label Edit");
        //            e.Node.BeginEdit();
        //        }
        //    }
        //}
      



        public static bool IsTxtFile(string text)
        {
           
            string pattern = @"^[a-z|A-Z|0-9]{1,200}\.[txt]{3}$";
            if (Regex.IsMatch(text, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool IsNull(TreeNode node)
        {
            if (node == null)
                throw new System.NullReferenceException();
            return true;
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedTreeNode = treeView1.SelectedNode;
            
            try
            {
                

                IsNull(selectedTreeNode);

                
            }
            catch (NullReferenceException)
            {
                {
                    Console.WriteLine("NULL");
                }

            }
            finally
            {
                if (selectedTreeNode != null)
                {
                    Console.WriteLine(selectedTreeNode.FullPath);
                    Form2 form = new Form2(selectedTreeNode);
                    form.Show();
               
                } 
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.treeView1.SelectedNode;
            string path = selectedNode.FullPath;

            FileAttributes selectedFileAttributes = File.GetAttributes(path);

            if ((selectedFileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                deleteFolder(path);
            }
            else
            {
                if ((selectedFileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    selectedFileAttributes = selectedFileAttributes & FileAttributes.ReadOnly;
                    File.SetAttributes(path, selectedFileAttributes);
                }
                File.Delete(path);
            }

            selectedNode.Parent.Nodes.Remove(selectedNode);
        }

        private void deleteFolder(string path)
        {
            DirectoryInfo currentCatalog = new DirectoryInfo(path);
            FileSystemInfo[] fsi = currentCatalog.GetFileSystemInfos();

            foreach (FileSystemInfo i in fsi)
            {

                if (i is DirectoryInfo)
                {
                    deleteFolder(i.FullName);
                }
                else if (i is FileInfo)
                {
                    FileAttributes selectedFileAttributes = File.GetAttributes(i.FullName);
                    if ((selectedFileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        selectedFileAttributes = selectedFileAttributes & FileAttributes.ReadOnly;
                        File.SetAttributes(i.FullName, selectedFileAttributes);
                    }
                    File.Delete(i.FullName);
                }
            }


            FileAttributes selectedDirectoryAttributes = File.GetAttributes(path);
            if ((selectedDirectoryAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                selectedDirectoryAttributes = selectedDirectoryAttributes & FileAttributes.ReadOnly;
                File.SetAttributes(path, selectedDirectoryAttributes);
            }
            Directory.Delete(path);

        }



    }

    static class Extensions
    {
        public static string GetRahs(this FileInfo x)
        {
            FileAttributes attr = File.GetAttributes(x.FullName);
            String temp = "";
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                temp += "r";
            else
                temp += "-";
            if ((attr & FileAttributes.Archive) == FileAttributes.Archive)
                temp += "a";
            else
                temp += "-";
            if ((attr & FileAttributes.Hidden) == FileAttributes.Hidden)
                temp += "h";
            else
                temp += "-";
            if ((attr & FileAttributes.System) == FileAttributes.System)
                temp += "s";
            else
                temp += "-";

            return temp;
        }

        public static string GetRahs(this DirectoryInfo x)
        {
            FileAttributes attr = File.GetAttributes(x.FullName);
            String temp = "";
            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                temp += "r";
            else
                temp += "-";
            if ((attr & FileAttributes.Archive) == FileAttributes.Archive)
                temp += "a";
            else
                temp += "-";
            if ((attr & FileAttributes.Hidden) == FileAttributes.Hidden)
                temp += "h";
            else
                temp += "-";
            if ((attr & FileAttributes.System) == FileAttributes.System)
                temp += "s";
            else
                temp += "-";

            return temp;
        }
    }
}
