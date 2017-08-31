using System;
using System.Collections.Generic;
using System.Linq;

namespace coursetask1
{
    class Subscription : Owner, ISubscription
    {
        public string _name;
        public string _telNumber;

        public IBook this[int index]
        {
            get
            {
                return this.Owned[index];
            }
        }


        public Subscription(string name, string number)
        {
            this._name = name;
            this._telNumber = number;
            this.Owned = new List<Book>();
        }

        public IEnumerable<Book> getBooks()
        { 
            return this.Owned;
        }

        public IEnumerable<Book> getOverdueBooks()
        {
            return this.Owned.Where(book => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds > book._timeGiven + 1209600); //problem - it is seconds
        }

        public bool hasOverdue()
        {
            if (this.getOverdueBooks().Count() > 0)
                return true;
            return false;
        }

        public bool hasRare()
        {
            if (this.Owned.Where(book => book._rarity).Count() > 0)
                return true;
            return false;
        }

        public Dictionary<string, string> getProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("name", this._name);
            properties.Add("telephone", this._telNumber);

            return properties;
        }
    }
}
