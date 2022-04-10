using System;
using System.Collections.Generic;

namespace EFCore_NortwindDb
{
    public partial class CurrentProductList
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
