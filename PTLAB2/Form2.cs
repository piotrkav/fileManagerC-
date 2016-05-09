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
    public partial class Form2 : Form
    {
        private TreeNode selectedNode;
        public Form2(TreeNode selectedNode)
        {
            InitializeComponent();
            this.selectedNode = selectedNode;
        }

        public bool Validate(string text)
        {
            string pattern = @"^[a-z|A-Z|0-9]{1,8}\.[txt|htm|php]{3}$";
            if (Regex.IsMatch(text, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {



            string path = selectedNode.FullPath;// +"\\" + textBox1.Text;

            bool ifDir = Directory.Exists(path);

            if (!ifDir)
            {
                path = selectedNode.Parent.FullPath;
            }

            path += "\\" + textBox1.Text;

            if (this.radioButton1.Checked)//file
            {
                if (!Validate(textBox1.Text))
                {
                    this.Close();
                    return;
                }

                //using (File.Create(sciezka)) { };
                File.Create(path).Dispose();


            }
            else
            {
                Directory.CreateDirectory(path);
            }

            FileAttributes attr = File.GetAttributes(path);
            if (checkBox1.Checked)
            {
                attr = attr | FileAttributes.ReadOnly;
            }
            if (checkBox2.Checked)
            {
                attr = attr | FileAttributes.Archive;
            }
            if (checkBox3.Checked)
            {
                attr = attr | FileAttributes.Hidden;
            }
            if (checkBox4.Checked)
            {
                attr = attr | FileAttributes.System;
            }

            File.SetAttributes(path, attr);


            TreeNode child = new TreeNode(textBox1.Text);

            if (ifDir)
            {
                selectedNode.Nodes.Add(child);
            }
            else
            {
                selectedNode.Parent.Nodes.Add(child);
            }


            this.Close();

        }


    }
}
