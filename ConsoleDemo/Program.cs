using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NoSqlJsonFileProject;


namespace ConsoleDemo
{
    internal class Program
    {   
        private static void Main(string[] args)
        {
            var student = new Student {Email = "jerry27syd@gmail.com", FirstName = "Jerry", LastName = "Liang"};


            var x = Subject.List();

            Subject subject = new Subject {Title1 = "Math",};
            subject.Save();

            Console.ReadKey();
        }
    }

    [DataContract]
    internal class Student : NoSqlJsonFile<Student>
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public List<Guid> SubjectId { get; set; }
    }

    [DataContract]
    internal class Subject : NoSqlJsonFile<Subject>
    {
        [DataMember]
        public string Title1 { get; set; }
        [DataMember]
        public List<Guid> SubjectId { get; set; }
    }
}