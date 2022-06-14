using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entities
{
    public class ProfitabilityResponse
    {
        public int Id { get; set; }
        public decimal ProfitabilityPercentage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinalTIme { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}