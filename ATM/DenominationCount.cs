using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
namespace ATM
{
    public class DenominationCount
    {
        public int Denomination { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"{Count} x {Denomination} EUR";
        }
    }
}
