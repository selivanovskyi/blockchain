using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainProject.model
{
    class Token
    {
        public string Name;
        public BigInteger Balance;
        

        public Token(string name, BigInteger balance)
        {
            Name = name;
            Balance = balance;
        }
    }
}
