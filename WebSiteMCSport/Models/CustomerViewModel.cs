﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteMCSport.Models
{
    public class CustomerViewModel
    {
        public string avarta { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Payment { get; set; }
    }
}