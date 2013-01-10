using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Xxf.Web.UI.Control
{
    public partial class SetSortPropertyFrm : Form
    {
        public SetSortPropertyFrm()
        {
            InitializeComponent();
        }
        public string SelectedProperty { get; set; }
        private  Type[] types;
        private void SetSortPropertyFrm_Load(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                Assembly dll = Assembly.LoadFile(file);
                types = dll.GetTypes().Where(s => s.IsClass == true).ToArray();
                listBox1.DataSource = types;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type type =types[ listBox1.SelectedIndex];
            listBox2.DataSource = type.GetProperties().Select (s=>s.Name ).ToList ();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedProperty = listBox2.SelectedValue.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
