﻿using Central_Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Interface
{
    public interface ITestService : IDisposable
    {
        Task<string> TestMethod();
    }
}