using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class Profitability
    {
        public int Id { get; set; }
        public decimal ProfitabilityPercentage { get; set; }
        public string StartTime { get; set; }
        public string FinalTIme { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}