using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI
{
    public static class Consts
    {
        public const string DbSchema = null;
        public const string DbTablePrefix = "NOM_STR_";
        public const int MaxUserIdLength = 128;
        public const int DateTimePrecision = 6; 
        public struct GeneralCategoryCode
        {
            public const string Gender = "Gender";
            public const string Education = "Education";
            public const string ColumnType = "ColumnType";
        }
    }
}
