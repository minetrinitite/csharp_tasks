using System;
using System.Collections.Generic;
using System.Linq;



namespace coursetask1
{
    class Library : Owner//, ILibrary
    {
        private List<Subscription> readers = new List<Subscription>();

        public readonly int _maxBooks = 5;
        public readonly int _maxRare = 1;

        public event Action<Subscription> subscriberAdded1;
        public event Action<Book> bookAdded1;
        public event Action<Book, object> bookStateChanged1;

        private Action<Subscription> subscriberAdded = (Subscription s) => { Console.WriteLine("Subscriber " + s._name + " was added"); };
        private Action<Book> bookAdded = (Book b) => { Console.WriteLine("\"" + b._book + "\" was added to Library"); };
        private Action<Book, object> bookStateChanged = (Book b, object sender) => {
            Subscription sub = sender as Subscription;
            if (b._isGiven)
            {
                Console.WriteLine("\"" + b._book + "\" was given to " + sub._name);
            }
            else
            {
                Console.WriteLine("\"" + b._book + "\" was returned to Library from" + sub._name);
            }            
        };

        public Library(List<Book> core, List<Subscription> subs)
        {
            this.Owned = core;
            this.readers = subs;
        }

        public Library(Book newOne)
        {
            this.addBook(newOne);
        }

        public static int getHash(string author, string title)
        {
            return (author.GetHashCode() + title.GetHashCode());
        }

        public IBook this[string author, string title]
        {
            get
            {
                int h = getHash(author, title);
                foreach(Book b in this.getAllBooks())
                {
                    if (getHash(b._author, b._book) == h)
                        return b;
                }
                return null;
            }
        }
        
        public void addReader(Subscription newOne)
        {
            this.readers.Add(newOne);
            subscriberAdded?.Invoke(newOne);
        }

        public void addBook(Book newOne)
        {
            this.Owned.Add(newOne);
            bookAdded.Invoke(newOne);
        }

        public IEnumerable<Book> getAllBooks()
        {
            return this.Owned;
        }

        public IEnumerable<Book> getAvailableBooks()
        {
            return this.Owned.Where(book => book._isGiven.Equals(true) );
        }

        public IEnumerable<Subscription> getSubscribers()
        {
            return this.readers;
        }

        public IEnumerable<Book> searchBooksByAuthor(string author)
        {
            if (author.Length <= 2)
                return new List<Book>();
            Console.WriteLine("Look what i found by author request \"{0}\":", author);
            return this.Owned.Where(book => Program.isOverlap50(author, book._author));
        }

        public IEnumerable<Book> searchBooksByName(string name) 
        {
            if (name.Length <= 2)
                return new List<Book>();
            Console.WriteLine("Look what i found by title request \"{0}\":", name);
            return this.Owned.Where(book => Program.isOverlap50(name, book._book));
        }

        public IEnumerable<Book> getOccupiedBooks() 
        {
            return this.Owned.Where(book => book._isGiven);
        }

        public void giveBook(Book subject, Subscription reciever)
        {
            if (subject._isGiven)
            {
                Console.WriteLine("It seems that library have given this book to someone.");
                return;
            }
            if ((reciever.Owned.Count() >= this._maxBooks) | (reciever.hasOverdue()))
            {
                Console.WriteLine("It seems that this subscriber has too much books or has overdue books.");
                return;
            }
            if ((subject._rarity) && (reciever.hasRare()))
            {
                Console.WriteLine("It seems that this subscriber already has a rare book.");
                return;
            }

            int counter = 0; //to check the progress
            try
            {
                reciever.Owned.Add(subject);
                counter++;
                subject._owner = reciever;
                counter++;
                subject._isGiven = true;
                counter++;
                subject._timeGiven = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                counter++;
                bookStateChanged.Invoke(subject, reciever);
            }
            catch (Exception e) //rewind the changes
            {
                Console.WriteLine(e.Message);
                switch(counter)
                {
                    case 0:
                        Console.WriteLine(e.Message);
                        break;
                    case 1:
                        reciever.Owned.Remove(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 2:
                        reciever.Owned.Remove(subject);
                        subject._owner = this;
                        Console.WriteLine(e.Message);
                        break;
                    case 3:
                        reciever.Owned.Remove(subject);
                        subject._owner = this;
                        subject._isGiven = false;
                        Console.WriteLine(e.Message);
                        break;
                    case 4:
                        reciever.Owned.Remove(subject);
                        subject._owner = this;
                        subject._isGiven = false;
                        subject._timeGiven = 0;
                        Console.WriteLine(e.Message);
                        break;
                }
            }
        } 

        public void returnBook(Subscription returnee, Book subject)
        {
            if (!returnee.Owned.Contains(subject))
            {
                Console.WriteLine("It seems that this subscriber doesn't have this book.");
                return;
            }   

            int counter = 0; //to check the progress
            var timeTemp = subject._timeGiven; //to keep the time
            try
            {
                returnee.Owned.Remove(subject);
                counter++;
                subject._owner = this;
                counter++;
                subject._isGiven = false;
                counter++;
                subject._timeGiven = 0;
                counter++;
                bookStateChanged.Invoke(subject, returnee);
            }
            catch (Exception e) //rewind the changes
            {
                Console.WriteLine(e.Message);
                switch (counter)
                {
                    case 0:
                        Console.WriteLine(e.Message);
                        break;
                    case 1:
                        returnee.Owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 2:
                        subject._owner = returnee;
                        returnee.Owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 3:
                        subject._owner = returnee;
                        returnee.Owned.Add(subject);
                        subject._isGiven = true;
                        Console.WriteLine(e.Message);
                        break;
                    case 4:
                        subject._owner = returnee;
                        returnee.Owned.Add(subject);
                        subject._isGiven = true;
                        subject._timeGiven = timeTemp;
                        Console.WriteLine(e.Message);
                        break;
                }
            }
        }


    }
}
