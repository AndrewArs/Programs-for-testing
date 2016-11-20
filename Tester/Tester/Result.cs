using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Tester
{
    public partial class Result : Form
    {
        int globalIndex = 1;
        int questions;
        int rightAnswered;
        QuestionsList myTest;

        public Result(QuestionsList test)
        {
            InitializeComponent();

            foreach(Question qu in test.questions)
            {
                dataGridView1.Rows.Add(String.Format("{0}. {1}", globalIndex, qu.question), qu.isCorrectly());
                globalIndex++;
            }

            questions = test.Count();
            rightAnswered = test.questions.Where((a) => a.answered)
                .Where((a) => a.answeredString.Equals(a.rightAnswer)).Count();

            myTest = test;
            dataGridView1.ReadOnly = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (StreamWriter fs = new StreamWriter("result.txt", false))
            {
                fs.WriteLine(String.Format("Правильно отвечено: {0}", rightAnswered));
                fs.WriteLine(String.Format("Всего вопросов: {0}", questions));
                globalIndex = 1;

                foreach (Question question in myTest.questions)
                {
                   fs.WriteLine(String.Format("{0}. {1} - {2}", globalIndex,
                       question.question, question.isCorrectly()));

                    globalIndex++;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
