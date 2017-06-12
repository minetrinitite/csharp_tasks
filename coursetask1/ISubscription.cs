using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursetask1
{
    interface ISubscription
    {
        //Получить основные свойства абонента(имя, телефон)
        //Получить список книг, которые находятся у него на руках
        //Получить список просроченных книг у него на руках

        Dictionary<string, string> getProperties();

        List<Book> getBooks();

        List<Book> getOverdueBooks();
    }
}
