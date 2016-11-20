using System;

namespace Test_creator
{
    [Serializable]
    public class Question
    {
        public String question { get; set; }
        public String rightAnswer { get; set; }
        public String answer1 { get; set; }
        public String answer2 { get; set; }

        public Question()
        { }

        public Question(String question, String rightAnswer, String answer1, String answer2)
        {
            this.question = question;
            this.rightAnswer = rightAnswer;
            this.answer1 = answer1;
            this.answer2 = answer2;
        }
    }
}
