﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCKK_APP_2023.Models.Enums;

namespace SCKK_APP_2023.Models
{
    public class LogCallModel
    {
        public ushort Identifier { get; set; }
        public DateTime CallTime { get; set; }
        public bool IsValidated { get; set; }
    }
}
