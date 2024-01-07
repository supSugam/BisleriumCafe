﻿using BisleriumCafe.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Model
{
    public class TaskResponse:ITaskResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
