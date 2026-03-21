using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return  Regex.IsMatch(text, @"^[a-zA-Z\s\-']+$");
            
        }
    }
}
