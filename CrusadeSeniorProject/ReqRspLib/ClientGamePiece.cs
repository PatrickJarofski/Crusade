using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public class ClientGamePiece
    {
        public const int DEFAULT_INT_VALUE = -1;
        public const string DEFAULT_STRING_VALUE = "Unknown";

        public string Name { get; set; }

        public string Owner { get; set; }

        public string Type { get; set; }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int Move { get; set; }

        public int MinAttackRange { get; set; }

        public int MaxAttackRange { get; set; }

        public ClientGamePiece()
        {
            Name = DEFAULT_STRING_VALUE;
            Owner = DEFAULT_STRING_VALUE;
            Type = DEFAULT_STRING_VALUE;
            RowCoordinate = DEFAULT_INT_VALUE;
            ColCoordinate = DEFAULT_INT_VALUE;
            Attack = DEFAULT_INT_VALUE;
            Defense = DEFAULT_INT_VALUE;
            Move = DEFAULT_INT_VALUE;
            MinAttackRange = DEFAULT_INT_VALUE;
            MaxAttackRange = DEFAULT_INT_VALUE;
        }       


        public void print(Guid clientId)
        {
            string owner;
            if (clientId.ToString() == Owner)
                owner = "You";
            else
                owner = "Opponent";

            Console.WriteLine("{0}----------------------------------", Environment.NewLine);
            Console.WriteLine("Name:    {0}", Name);
            Console.WriteLine("Owner:   {0}", owner);
            Console.WriteLine("Atk/Def: {0}/{1}", Attack, Defense);
            Console.WriteLine("Move:    {0}", Move);
            Console.WriteLine("Range:   {0} - {1}", MinAttackRange, MaxAttackRange);
            Console.WriteLine("Location [{0}, {1}]", RowCoordinate+1, ColCoordinate+1);
            Console.WriteLine("----------------------------------{0}", Environment.NewLine);
        }
    }
}
