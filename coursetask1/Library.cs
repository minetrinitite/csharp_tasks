using System;
using System.Collections.Generic;
using System.Linq;



namespace coursetask1
{
    class Library : Owner, ILibrary
    {
        private List<Subscription> readers = new List<Subscription>();
        private List<Book> given = new List<Book>();
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
            this.owned = core;
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
            this.owned.Add(newOne);
            bookAdded.Invoke(newOne);
        }

        public List<Book> getAllBooks()
        {
            return this.owned.Concat(this.given).ToList();
        }

        public List<Book> getAvailableBooks()
        {
            return this.owned;
        }

        public List<Subscription> getSubscribers()
        {
            return this.readers;
        }

        public List<Book> searchBooksByAuthor(string author)
        {
            List<Book> byAuthor = new List<Book>();
            if (author.Length <= 2)
                return byAuthor;
            foreach (Book book in this.getAllBooks())
            {
                if (Program.isOverlap50(author, book._author))
                    byAuthor.Add(book);
            }
            Console.WriteLine("Look what i found by author request \"{0}\":", author);
            return byAuthor;
        }

        public List<Book> searchBooksByName(string name) 
        {
            List<Book> byName = new List<Book>();
            if (name.Length <= 2)
                return byName;
            foreach (Book book in this.getAllBooks() )
            {
                if (Program.isOverlap50(name, book._book))
                    byName.Add(book);
            }
            Console.WriteLine("Look what i found by title request \"{0}\":", name);
            return byName;
        }

        public List<Book> getOccupiedBooks() 
        {
            return this.given;
        }

        public void giveBook(Book subject, Subscription reciever)
        {
            if (subject._isGiven)
            {
                Console.WriteLine("It seems that library have given this book to someone.");
                return;
            }
            if ((reciever.owned.Count() >= this._maxBooks) | (reciever.hasOverdue()))
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
                this.owned.Remove(subject);
                counter++;
                reciever.owned.Add(subject);
                counter++;
                subject._owner = reciever;
                counter++;
                subject._isGiven = true;
                counter++;
                subject._timeGiven = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                counter++;
                this.given.Add(subject);
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
                        this.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 2:
                        reciever.owned.Remove(subject);
                        this.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 3:
                        subject._owner = null;
                        reciever.owned.Remove(subject);
                        this.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 4:
                        subject._isGiven = false;
                        subject._owner = null;
                        reciever.owned.Remove(subject);
                        this.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 5:
                        subject._timeGiven = 0; 
                        subject._isGiven = false;
                        subject._owner = null;
                        reciever.owned.Remove(subject);
                        this.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 6:
                        Console.WriteLine(e.Message);
                        break;
                }
            }
        } 

        public void returnBook(Subscription returnee, Book subject)
        {
            if (!returnee.owned.Contains(subject))
            {
                Console.WriteLine("It seems that this subscriber doesn't have this book.");
                return;
            }   

            int counter = 0; //to check the progress
            try
            {
                returnee.owned.Remove(subject);
                counter++;
                this.owned.Add(subject);
                counter++;
                subject._owner = this;
                counter++;
                subject._isGiven = false;
                counter++;
                this.given.Remove(subject);
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
                        returnee.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 2:
                        this.owned.Remove(subject);
                        returnee.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 3:
                        subject._owner = returnee;
                        this.owned.Remove(subject);
                        returnee.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 4:
                        subject._isGiven = true;
                        subject._owner = returnee;
                        this.owned.Remove(subject);
                        returnee.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 5:
                        this.given.Add(subject); 
                        subject._isGiven = true;
                        subject._owner = returnee;
                        this.owned.Remove(subject);
                        returnee.owned.Add(subject);
                        Console.WriteLine(e.Message);
                        break;
                    case 6:
                        Console.WriteLine(e.Message);
                        break;
                }
            }
        }


    }
}
