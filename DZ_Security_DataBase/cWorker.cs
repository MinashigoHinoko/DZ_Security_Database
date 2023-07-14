using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_Security_DataBase
{
    public class cWorker
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name} - {ID}";  // or just "{ID}" if you want
        }

        public override bool Equals(object obj)
        {
            // If the object is null, return false
            if (obj == null)
                return false;

            // If the object cannot be cast to cWorker, return false
            cWorker otherWorker = obj as cWorker;
            if (otherWorker == null)
                return false;

            // Return true if the ID fields match
            return this.ID.Equals(otherWorker.ID);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}
