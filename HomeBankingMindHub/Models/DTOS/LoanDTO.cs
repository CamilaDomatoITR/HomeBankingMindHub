﻿namespace HomeBankingMindHub.Models.DTOS
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }
        public string Type { get; set; }
    }
}
