using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursetask1
{
    class Owner
    {
        private List<Book> owned;

        internal List<Book> Owned
        {
            get
            {
                return owned;
            }

            set
            {
                owned = value;
            }
        }
    }
}
