using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqRspLib
{
    public class ClientGamePiece
    {

        public string Name { get; set; }

        public string Owner { get; set; }

        public string Type { get; set; }

        public int RowCoordinate { get; set; }

        public int ColCoordinate { get; set; }
       

        public ClientGamePiece()
        {
            Name = "Unknown";
            Owner = "Unknown";
            Type = "Unknown";
            RowCoordinate = -1;
            ColCoordinate = -1;
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
            Console.WriteLine("Location [{0}, {1}]", RowCoordinate+1, ColCoordinate+1);
            Console.WriteLine("{0}----------------------------------", Environment.NewLine);
        }
    }
}
