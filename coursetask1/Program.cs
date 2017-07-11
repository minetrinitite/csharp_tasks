using System;
using System.Collections.Generic;
using System.Threading;

namespace coursetask1
{
    class Program
    {
        static void printDict(Dictionary<string, string> dict)
        {
            foreach (KeyValuePair<string, string> info in dict)
            {
                Console.WriteLine("{0}: {1}", info.Key, info.Value);
            }
        }

        static void printBookNames(List<Book> books)
        {
            foreach (Book info in books)
            {
                info.printName();
            }
        }

        static public bool isOverlap50(string f, string s) //>50% overlap
        {
            if (overlap(f, s) > 50)
                return true;
            return false;
        }

        static int overlap(string f, string s) //https://en.wikipedia.org/wiki/Overlap_coefficient
        {
            int overlap = 0;
            string big;
            string small; //also works as length
            if (Math.Max(f.Length, s.Length) == f.Length)
            {
                big = f;
                small = s;
            }
            else
            {
                big = s;
                small = f;
            }
            big.ToLower();
            small.ToLower();
            for(int i = 0; i <= big.Length - small.Length; i++)
            {
                int count = 0;
                string temp = big.Substring(i, small.Length);
                for (int j = 0; j < small.Length; j++)
                {
                    if (small[j] == temp[j])
                        count++;
                }
                if (count > overlap)
                    overlap = count;
            }
            return (Int32)((Convert.ToSingle(overlap) / Convert.ToSingle(small.Length))*100);
        }

        static void Main(string[] args)
        {
            //Peter Watts:        
            //Starfish
            //Maelstrom
            //βehemoth: β-Max
            //βehemoth: Seppuku - R
            //Blindsight - R
            //Echopraxia
            //Firefall

            //Thomas Metzinger:
            //The Ego Tunnel

            //Terry Pratchett:
            //The Science of Discworld I
            //The Science of Discworld II: The Globe
            //The Science of Discworld III: Darwin's Watch
            //The Science of Discworld IV: Judgement Day

            Book book1 = new Book("Peter Watts", "Starfish");
            Book book2 = new Book("Peter Watts", "Maelstrom");
            Book book3 = new Book("Peter Watts", "βehemoth: β-Max");
            Book book4 = new Book("Peter Watts", "βehemoth: Seppuku", true);
            Book book5 = new Book("Peter Watts", "Blindsight", true);
            Book book6 = new Book("Peter Watts", "Echopraxia");
            Book book7 = new Book("Peter Watts", "Firefall");
            Book book8 = new Book("Thomas Metzinger", "The Ego Tunnel");
            Book book9 = new Book("Terry Pratchett", "The Science of Discworld I");
            Book book10 = new Book("Terry Pratchett", "The Science of Discworld II: The Globe");
            Book book11 = new Book("Terry Pratchett", "The Science of Discworld III: Darwin's Watch");
            Book book12 = new Book("Terry Pratchett", "The Science of Discworld IV: Judgement Day");

            Subscription sub1 = new Subscription("Peter Watts", "+123654789");
            Subscription sub2 = new Subscription("Thomas Metzinger", "+321456789");
            Subscription sub3 = new Subscription("Terry Pratchett", "+123456789");

            List<Book> bpack1 = new List<Book>();
            List<Subscription> spack1 = new List<Subscription>();

            bpack1.Add(book1);
            bpack1.Add(book2);
            bpack1.Add(book3);
            bpack1.Add(book4);
            bpack1.Add(book5);
            bpack1.Add(book6);
            bpack1.Add(book7);
            bpack1.Add(book8);
            bpack1.Add(book9);
            bpack1.Add(book10);
            bpack1.Add(book11);
            //book 12 for add test

            spack1.Add(sub1);
            spack1.Add(sub2);
            //sub 3 for add test

            Library lib = new Library(bpack1, spack1);

            
            //show ALL books
            foreach (Book book in lib.getAllBooks())
            {
                printDict(book.getProperties());
            }

            //show subscribers
            foreach (Subscription sub in lib.getSubscribers())
            {
                printDict(sub.getProperties());
            }

            //add book
            lib.addBook(book12); 
            lib.owned[11].printName(); //The Science of Discworld IV: Judgement Day

            //add subscriber
            lib.addReader(sub3);
            printDict(lib.getSubscribers()[2].getProperties());  //Terry Pratchett, +123456789

            //search by author
            printBookNames(lib.searchBooksByAuthor("Metz")); //one
            printBookNames(lib.searchBooksByAuthor("Mehz")); //one

            printBookNames(lib.searchBooksByAuthor("Pratchett")); //list

            //search by title
            printBookNames(lib.searchBooksByName("The Science")); //four
            printBookNames(lib.searchBooksByName("Ehopraxa")); //one
            printBookNames(lib.searchBooksByName("opr")); //

            //give books
            lib.giveBook(book4, sub1);

            //give second rare book 
            lib.giveBook(book5, sub1); //has 1 rare book already
            printBookNames(sub1.getBooks()); //Behemoth: Seppuku
            
            //give books if owned amount is lesser than 5
            lib.giveBook(book6, sub1); //2 books
            lib.giveBook(book7, sub1); //3 books
            lib.giveBook(book8, sub1); //4 books
            lib.giveBook(book9, sub1); //5 books
            printBookNames(sub1.getBooks()); //prints 5 books
            lib.giveBook(book10, sub1); //mistake, has 5 books already
            printBookNames(sub1.getBooks()); //still 5 books

            //remove books
            lib.returnBook(sub1, book6);
            lib.returnBook(sub1, book6); //mistake, he hasn't this book

            Thread.Sleep(300000);
        }
    }
}
