using System;
using System.Collections.Generic;
using System.Text;
using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Common
{
    public class TagHeader
    {
        public CommonBlockDelimiters CommonTagDelimiters { get; set; }

        public TagHeader()
        {
            CommonTagDelimiters = new CommonBlockDelimiters();
        }

        public TagHeader(CommonBlockDelimiters commonTagDelimiters)
        {
            CommonTagDelimiters = commonTagDelimiters;
        }
    }
}
