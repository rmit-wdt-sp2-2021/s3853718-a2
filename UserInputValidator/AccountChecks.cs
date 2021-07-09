﻿using System;
using System.Collections.Generic;

namespace DataValidator
{
    public static class AccountChecks
    {
        private static readonly decimal _checkingMinimum = 200;
        private static readonly decimal _savingsMinimum = 0;
        private static readonly decimal _checkingMinimumOpening = 500;
        private static readonly decimal _savingsMinimumOpening = 100;
        private static readonly decimal _atmFee = 0.1M;
        private static readonly decimal _transferFee = 0.2M;
        private static readonly int _freeTransactions = 4;
        public static bool ExceedsSavingsMin(this decimal t)
        {
            if (t > _savingsMinimum)
                return true;
            else
                return false;
        }
        public static bool ExceedsCheckingMin(this decimal t)
        {
            if (t > _checkingMinimum)
                return true;
            else
                return false;
        }
        public static bool ExceedsSavingsOpening(this decimal t)
        {
            if (t > _savingsMinimumOpening)
                return true;
            else
                return false;
        }
        public static bool ExceedsCheckingOpening(this decimal t)
        {
            if (t > _checkingMinimumOpening)
                return true;
            else
                return false;
        }
        public static decimal GetSavingsMin() => _savingsMinimum;

        public static decimal GetCheckingsMin() => _checkingMinimum;

        public static decimal GetATMFee() => _atmFee;

        public static decimal GetTransferFee() => _transferFee;

        public static int GetFreeTransacionLimit() => _freeTransactions;


        public static string DateDisplayFormat(this DateTime time) => time.ToString("dd-MM-yyyy : HH:mm:ss");

        public static DateTime GetCurrentTimeFormatted() => DateTime.UtcNow;
    }
}
