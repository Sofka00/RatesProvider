﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RatesProvider.Application.Models
{
    public class Quota
    {
        public int Total { get; set; }
        public int Used { get; set; }
        public int Remaining { get; set; }
    }
}
