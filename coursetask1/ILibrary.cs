using System.Collections.Generic;

namespace coursetask1
{
    interface ILibrary
    {
        //Добавлять новые книги
        //Получать списки всех книг, 
        //          книг, находящихся в библиотеке, и 
        //          книг на руках у абонентов
        //Искать книги по автору или названию
        //Выдавать книги абонентам если у них нет просроченных книг и не превышен лимит на максимальное число книг на руках
        //Возвращать книги обратно в библиотеку

        void addBook(Book newOne);

        List<Book> getAllBooks();

        List<Book> getAvailableBooks();

        List<Book> getOccupiedBooks();

        List<Book> searchBooksByAuthor(string author);

        List<Book> searchBooksByName(string name);

        void giveBook(Book subject, Subscription reciever);

        void returnBook(Subscription returnee, Book subject);
    }
}
