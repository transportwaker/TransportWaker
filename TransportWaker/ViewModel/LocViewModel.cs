using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportWaker.ViewModel
{
    public class LocViewModel
    {
        public LocViewModel() 
        {
            Items = new List<LocTags>();
        }

        public List<LocTags> Items { get; set; }



    }
}
