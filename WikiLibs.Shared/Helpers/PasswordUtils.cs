using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public class PasswordUtils
    {
        private static char RandomChar(PasswordOptions options, Random rand, int freq)
        {
            var res = options.Alphabet.OrderBy(e => e.Frequency).Where(e => e.Frequency == freq);
            var id = rand.Next(0, res.Count() - 1);

            return (res.ElementAt(id).Char);
        }

        public static string NewPassword(PasswordOptions options)
        {
            var rand = new Random();
            string res = "";
            int curFreq = 1;
            var copy = options.NumChars;

            while (copy > 0)
            {
                res += RandomChar(options, rand, curFreq);
                --copy;
                ++curFreq;
                if (!options.Alphabet.Any(e => e.Frequency == curFreq))
                    curFreq = 1;
            }
            return (res);
        }
    }
}
