using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Tester
{
    public partial class Form1 : Form
    {
        int globalIndex = 0;
        QuestionsList test;
        XmlSerializer formatter = new XmlSerializer(typeof(QuestionsList));
        int minutes;
        int seconds;
        bool testEnded = true;

        public Form1()
        {
            InitializeComponent();

            ToolStripMenuItemAboutProject.Click += About_Click;
            ToolStripMenuItemHelp.Click += Help_Click;
            ToolStripMenuItemOpen.Click += Open_Click;
        }
        
        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа для тестовой оценки знаний\nХарьков 2016", "О программе");
        }

        private void Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выбрав вариант ответа, нажмите кнопку \"Ответить\"" +
                "\nВы можете перемещаться по вопросам в тесте нажимая кнопки \"Вперед\" и \"Назад\"\n" +
                "Нажав кнопку \"Завершить\" вы завершаете тест и на экран выводится результат", "Помощь");
        }

        private void Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Тестовые файлы|*.xml";

            if (ofd.ShowDialog() == DialogResult.Cancel)
                return;

            using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
            {
                test = (QuestionsList)formatter.Deserialize(fs);
            }

            label1.Text = test.name;
            minutes = test.minutes - 1;
            seconds = 60;

            if (MessageBox.Show("Запустить тест?", "", MessageBoxButtons.OK) == DialogResult.OK)
            {
                testEnded = false;
                PlaceAnswers();
                buttonAnswer.Enabled = true;
                timer1.Start();
                labelProgress.Text = String.Format("0/{0}", test.Count());
            }
        }

        private void PlaceAnswers()
        {
            Random rd = new Random();

            textBoxQuestion.Text = test.questions.ElementAt(globalIndex).question;

            switch (rd.Next(1, 4))
            {
                case 1:
                    {
                        textBox1.Text = test.questions.ElementAt(globalIndex).answer1;

                        if (rd.Next(2, 4) == 2)
                        {
                            textBox2.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox3.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                        else
                        {
                            textBox3.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox2.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                    }
                    break;
                case 2:
                    {
                        textBox2.Text = test.questions.ElementAt(globalIndex).answer1;

                        if (rd.Next(1, 3) == 1)
                        {
                            textBox1.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox3.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                        else
                        {
                            textBox3.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox1.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                    }
                    break;
                case 3:
                    {
                        textBox3.Text = test.questions.ElementAt(globalIndex).answer1;

                        if (rd.Next(1, 3) == 1)
                        {
                            textBox1.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox2.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                        else
                        {
                            textBox2.Text = test.questions.ElementAt(globalIndex).rightAnswer;
                            textBox1.Text = test.questions.ElementAt(globalIndex).answer2;
                        }
                    }
                    break;
            }

            if (textBox1.Text.Equals(test.questions.ElementAt(globalIndex).answeredString))
                radioButton1.Checked = true;
            else if (textBox2.Text.Equals(test.questions.ElementAt(globalIndex).answeredString))
                radioButton2.Checked = true;
            else radioButton3.Checked = true;
        }

        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                buttonAnswer.Enabled = false;
                test.questions.ElementAt(globalIndex).answered = true;

                if (radioButton1.Checked)
                {
                    test.questions.ElementAt(globalIndex).answeredString = textBox1.Text;
                }
                else if (radioButton2.Checked)
                {
                    test.questions.ElementAt(globalIndex).answeredString = textBox2.Text;
                }
                else
                {
                    test.questions.ElementAt(globalIndex).answeredString = textBox3.Text;
                }

                labelProgress.Text = String.Format("{0}/{1}",
                    test.questions.Where((a) => a.answered == true).Count(), test.Count());

                buttonNext_Click(null, null);
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if ((globalIndex - 1) >= 0)
            {
                globalIndex--;

                if (test.questions.ElementAt(globalIndex).answered)
                    buttonAnswer.Enabled = false;
                else buttonAnswer.Enabled = true;

                PlaceAnswers();
            }
            else return;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (test.Count() - 1 >= (globalIndex + 1))
                {
                    globalIndex++;

                    if (test.questions.ElementAt(globalIndex).answered)
                        buttonAnswer.Enabled = false;
                    else buttonAnswer.Enabled = true;

                    PlaceAnswers();
                }
                else return;
            }
            catch(Exception)
            {
                return;
            }
            
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            if (!testEnded)
            {
                testEnded = true;
                timer1.Stop();
                Result result = new Result(test);

                result.ShowDialog();
            }
            else return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds -= 1;
            if(seconds == -1)
            {
                if(minutes > 0)
                {
                    minutes -= 1;
                    seconds = 59;
                }
                else
                {
                    buttonEnd_Click(null, null);
                }
            }

            labelTimer.Text = String.Format("{0}:{1}", minutes, seconds);

        }
    }
}
