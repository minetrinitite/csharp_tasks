﻿using System;
using System.Collections.Generic;
//using NUnit.Framework;


namespace coursetask1
{
    class Book : IBook
    {
        //TODO: book indexation
        public string _book;
        public string _author;
        public bool _rarity;
        public bool _isGiven;
        public int _timeGiven;
        public Owner _owner;

        public Book(string author = "no author", string name = "no name", bool rare = false)
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
        
        public void printName() //for tests
        {
            Console.WriteLine("Book Name: {0}", this._book);
        }

        public Owner checkWhere()
        {
            return _owner;
        }

        public int whenGiven()
        {
            if (this._isGiven)
                return this._timeGiven;
            return 0;
        }
    }
}
