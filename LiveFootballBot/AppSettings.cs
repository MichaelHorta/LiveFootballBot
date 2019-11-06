using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot
{
    public class AppSettings
    {
        public UnidadeditorialAPI UnidadeditorialAPI { get; set; }
    }

    public class UnidadeditorialAPI
    {
        public string Events { get; set; }
        public string Event { get; set; }
    }
}
