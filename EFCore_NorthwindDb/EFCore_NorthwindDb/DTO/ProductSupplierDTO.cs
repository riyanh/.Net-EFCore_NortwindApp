using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_NortwindDb.DTO
{
    //dto = data transfer object
    public class ProductSupplierDTO
    {
        public int? productId { get; set; }
        public string? productName { get; set; }
        public string? categoryName { get; set; }
        public string? companyName { get; set; }
        public string? address { get; set; }

    }
}
