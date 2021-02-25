﻿using System.Collections.Generic;

namespace XB.Astrea.Client.Config
{
    public record AstreaClientOptions
    {
        public const string ConfigurationSection = "AstreaClient";

        public string Url { get; set; }
        public string Version { get; set; }
        public double RetryPeriodInMin { get; set; } = 15;
        public double WaitingBeforeRetryInSec { get; set; } = 60;
        public int RiskThreshold { get; set; } = 5;
        public List<string> AcceptableTransactionTypes { get; set; }
    }
}
