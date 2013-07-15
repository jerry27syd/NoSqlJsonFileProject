using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBudgetWPFApplicationDemo
{
    public class PbEntry : NoSqlJsonFileProject.NoSqlJsonFile<PbEntry>
    {
        public string CurrentDate { get; set; }
        public string ItemName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Amount { get; set; }
        public int Status { get; set; }
    }
}
