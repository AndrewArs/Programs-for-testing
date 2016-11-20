using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test_creator
{
    [Serializable]
    public class QuestionsList
    {
        [XmlArray("questions")]
        public List<Question> questions { get; set; }
        public int minutes { get; set; }
        public String name { get; set; }

        public QuestionsList() { }

        public QuestionsList(int minutes, String name)
        {
            questions = new List<Question>();
            this.name = name;
            this.minutes = minutes;
        }

        public int Count()
        {
            return questions.Count;
        }
    }
}
