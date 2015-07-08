namespace People
{
    public class Person
    {
        public Gender Gender { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public void CreatePerson(int age)
        {
            var person = new Person();
            person.Age = age;
            if (age % 2 == 0)
            {
                person.Name = "John";
                person.Gender = Gender.Male;
            }
            else
            {
                person.Name = "Jane";
                person.Gender = Gender.Female;
            }
        }
    }
}
