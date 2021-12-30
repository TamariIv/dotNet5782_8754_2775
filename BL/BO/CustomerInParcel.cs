using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public override string ToString()
        {
            return string.Format
                ("id: {0} \nname: {1}\n", Id, Name);
        }
    }
}
