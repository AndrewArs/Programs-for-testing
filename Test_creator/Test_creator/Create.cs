using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_creator
{
    public partial class Create : Form
    {
        public int minutes;
        public String fileName;
        public String testName;

        public Create()
        {
            InitializeComponent();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            minutes = (int)numericUpDown1.Value;
            fileName = textBox1.Text + ".xml";
            testName = textBoxName.Text;

            if (File.Exists(fileName))
            {
                MessageBox.Show("Файл с таким именем уже существует", "Ошибка");
                return;
            }
            else File.Create(fileName);

            this.DialogResult = DialogResult.OK;
        }

        private void Create_Load(object sender, EventArgs e)
        {

        }
    }
}
