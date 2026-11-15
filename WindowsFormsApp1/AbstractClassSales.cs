using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class AbstractClassSales
    {
        public int idProd;
        public int Quantity;
        public string Sizes;

        public AbstractClassSales(int idProd, int quant, string sizes) 
        { 
            this.idProd = idProd;
            this.Quantity = quant;
            this.Sizes = sizes;
        }
    }
}
