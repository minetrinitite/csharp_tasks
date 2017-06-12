using System.Collections.Generic;

namespace coursetask1
{
    interface IBook
    {
        //Получить основные свойства книги(автор, название, редкость книги)
        //Узнать находится книга в библиотеке или у абонента(у какого именно абонента)
        //Узнать, когда книга была выдана, если она находится у абонента

        Dictionary<string, string> getProperties();

        Owner checkWhere();

        int whenGiven();
    }
}
