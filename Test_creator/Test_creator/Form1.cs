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
using System.Xml.Serialization;

namespace Test_creator
{
    public partial class Form1 : Form
    {
        String filePath;
        int globalIndex = 0;
        QuestionsList questions;
        XmlSerializer formatter = new XmlSerializer(typeof(QuestionsList));

        public Form1()
        {
            InitializeComponent();

            label.Text = "Вопрос " + (globalIndex + 1);
        }

        //вызывается при загрузке формы
        private void Form1_Load(object sender, EventArgs e)
        {
            Create create = new Create();

            if (create.ShowDialog() == DialogResult.OK)
            {
                questions = new QuestionsList(create.minutes, create.testName);
                this.filePath = create.fileName;
            }
            else this.Close();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (globalIndex <= 0 )
            {
                return;
            }
            else
            {
                globalIndex--;

                label.Text = "Вопрос " + (globalIndex + 1);

                textBoxQuestion.Text = questions.questions.ElementAt(globalIndex).question;
                textBox1.Text = questions.questions.ElementAt(globalIndex).rightAnswer;
                textBox2.Text = questions.questions.ElementAt(globalIndex).answer1;
                textBox3.Text = questions.questions.ElementAt(globalIndex).answer2;
                radioButton1.Checked = true;
                questions.questions.Remove(questions.questions.ElementAt(globalIndex));
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            label.Text = "Вопрос " + (globalIndex + 2);

            if (radioButton1.Checked)
            {
                questions.questions.Add(
                    new Question(textBoxQuestion.Text, textBox1.Text, textBox2.Text, textBox3.Text));
            }
            else if (radioButton2.Checked)
            {
                questions.questions.Add(
                    new Question(textBoxQuestion.Text, textBox2.Text, textBox1.Text, textBox3.Text));
            }
            else
            {
                questions.questions.Add(
                    new Question(textBoxQuestion.Text, textBox3.Text, textBox1.Text, textBox2.Text));
            }

            globalIndex++;

            if (questions.Count() - 1 >= globalIndex + 1)
            {
                textBoxQuestion.Text = questions.questions.ElementAt(globalIndex + 1).question;
                textBox1.Text = questions.questions.ElementAt(globalIndex + 1).rightAnswer;
                textBox2.Text = questions.questions.ElementAt(globalIndex + 1).answer1;
                textBox3.Text = questions.questions.ElementAt(globalIndex + 1).answer2;
                radioButton1.Checked = true;
                questions.questions.Remove(questions.questions.ElementAt(globalIndex + 1));
            }
            else
            {
                textBoxQuestion.Text = null;
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                radioButton1.Checked = true;
            }
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                formatter.Serialize(fs, questions);
            }
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
