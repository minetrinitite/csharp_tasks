using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursetask1
{
    class Subscription : Owner, ISubscription
    {
        public string _name;
        public string _telNumber;

        public Subscription(string name, string number)
        {
            this._name = name;
            this._telNumber = number;
            this.owned = new List<Book>();
        }

        public List<Book> getBooks()
        {
            return this.owned;
        }

        public List<Book> getOverdueBooks()
        {
            List<Book> overdue = new List<Book>();

            foreach (Book book in this.owned)
            {
                if ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds > book._timeGiven + 1209600) //problem - this is seconds
                {
                    overdue.Add(book);
                }
            }
            return overdue;
        }

        public bool hasOverdue()
        {
            foreach (Book book in this.owned)
            {
                if ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds > book._timeGiven + 1209600) //problem - this is seconds
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasRare()
        {
            foreach (Book book in this.owned)
            {
                if (book._rarity)
                    return true;
            }
            return false;
        }

        public Dictionary<string, string> getProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("name", this._name);
            properties.Add("telephone", this._telNumber);

            return properties;
        }

        //public void printProperties()
        //{
        //    foreach (KeyValuePair<string, string> info in this.getProperties())
        //    {
        //        Console.WriteLine("{0}: {1}", info.Key, info.Value);
        //    }
        //}
    }
}
