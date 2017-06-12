using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace coursetask1
{
    class Book : IBook
    {
        public string _book;
        public string _author;
        public bool _rarity;
        public bool _isGiven;
        public int _timeGiven;
        public Owner _owner;

        public Book(string author, string name, bool rare = false)
        {
            this._book = name;
            this._author = author;
            this._rarity = rare;
        }

        public Dictionary<string, string> getProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("name", this._book);
            properties.Add("author", this._author);
            properties.Add("rarity", Convert.ToString(this._rarity));

            return properties;
        }

        //public void printProperties() //for tests
        //{
        //    foreach (KeyValuePair<string, string> value in this.getProperties())
        //    {
        //        Console.WriteLine("{0} : {1}", value.Key, value.Value);
        //    }
        //}

        public void printName() //for tests
        {
            Console.WriteLine("Book Name: {0}", this._book);
        }

        public Owner checkWhere()
        {
            return _owner;
        }
    }
}
