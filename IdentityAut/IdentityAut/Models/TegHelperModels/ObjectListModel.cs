using System;
using System.Collections.Generic;

namespace Business_Logic.Models.TegHelperModels
{
    public class ObjectListModel
    {
        public IEnumerable<string> ArticlePreviews { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
