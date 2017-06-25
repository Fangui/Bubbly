using System;
using System.IO;

namespace ItectoBot2
{
    internal class DataContractJsonSerializer
    {
        private Type type;

        public DataContractJsonSerializer(Type type)
        {
            this.type = type;
        }

        internal User ReadObject(MemoryStream ms)
        {
            throw new NotImplementedException();
        }
    }
}