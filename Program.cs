using Newtonsoft.Json;

namespace LinqLesson
{
    class Program
    {
        static double Distance(Person Person1, Person Person2)
        {

            var distance = Math.Acos((
                           Math.PI / 180.0 * 60 * 1.1515 
                         * Math.Sin(Person1.Latitude)
                         * Math.Sin(Person2.Latitude)
                         + Math.Cos(Person1.Latitude)
                         * Math.Cos(Person2.Latitude) 
                         * Math.Cos(Person2.Longitude - Person1.Longitude)));
            return distance;
        }
        static void Main(string[] args)
        {
            var persons = JsonConvert.DeserializeObject<IEnumerable<Person>>(File.ReadAllText("data.json"));


            var north = persons.Max(NP => NP.Latitude + " " + NP.Name);

            var south = persons.Min(SP => SP.Latitude + " " + SP.Name);

            var west = persons.Max(WP => WP.Longitude + " " + WP.Name);

            var east = persons.Min(EP => EP.Longitude + " " + EP.Name);

            Console.WriteLine("North  - " + north + "\n" + "South  - " + south + 
                              "West  - "  + west + "East  - " + east);
            ////////////////////////////////////////////////////////////////////////
            var distantMax = persons.SelectMany(p1 => persons.Select(p2 => new {
                Person1 = p1,
                Person2 = p2,
                distance = Distance(p1, p2)
            })).Where(d => d.Person1 != d.Person2)
                .Max(d => d.distance);
            Console.WriteLine("The max distance between 2 persons is: " + distantMax);


            var distantMin = persons.SelectMany(p1 => persons.Select(p2 => new
            {
                Person1 = p1,
                Person2 = p2,
                distance = Distance(p1, p2)
            })).Where(d => d.Person1 != d.Person2)
                .Min(d => d.distance);
            Console.WriteLine("The min distance between 2 persons is: " + distantMin);
            ////////////////////////////////////////////////////////////////////////
            var theSame = persons.SelectMany(p3 => persons.Select(p4 => new
            {
                Person3 = p3,
                Person4 = p4,
                the_Same = p3.About.Split(' ').Intersect(p4.About.Split(' ')).Count()
            })).Where(d => d.Person3 != d.Person4)
                .Max(d => d.the_Same);
            Console.WriteLine("How many persons with the same letters? --> " + theSame);
            ////////////////////////////////////////////////////////////////////////
            var friends = persons
                .SelectMany(person => person.Friends, (person, friend) => new { 
                    FriendName = friend.Name, 
                    PersonName = person.Name 
                }).GroupBy(f => f.FriendName)
                  .Where(f => f.Count() > 1)
                  .ToList();
            foreach (var similar in friends)
            {
                Console.WriteLine($"{similar} is common friend for:");
                foreach (var person in similar)
                {
                    Console.WriteLine(person.PersonName);
                }
                Console.WriteLine();
            }
        }
    }
}
//checked
