﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Context.Entities.UserNews
{
    public class Source
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }
        public Int32 PositiveRate {get; set; }
        public String URL { get; set; }

        public List<Article> Articles { get; set; }
    }
}