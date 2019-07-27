using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public class PasswordOptions
    {
        public struct PasswordItem
        {
            public char Char { get; set; }
            public int Frequency { get; set; }

            public PasswordItem(char c, int freq)
            {
                Char = c;
                Frequency = freq;
            }
        }

        public PasswordItem[] Alphabet { get; set; }
        public int NumChars { get; set; }

        public static readonly PasswordOptions Standard = new PasswordOptions()
        {
            Alphabet = new PasswordItem[]
            {
                new PasswordItem('a', 1),
                new PasswordItem('b', 1),
                new PasswordItem('c', 1),
                new PasswordItem('d', 1),
                new PasswordItem('e', 1),
                new PasswordItem('f', 1),
                new PasswordItem('g', 1),
                new PasswordItem('h', 1),
                new PasswordItem('i', 1),
                new PasswordItem('j', 1),
                new PasswordItem('k', 1),
                new PasswordItem('l', 1),
                new PasswordItem('m', 1),
                new PasswordItem('n', 1),
                new PasswordItem('o', 1),
                new PasswordItem('p', 1),
                new PasswordItem('q', 1),
                new PasswordItem('r', 1),
                new PasswordItem('s', 1),
                new PasswordItem('t', 1),
                new PasswordItem('u', 1),
                new PasswordItem('v', 1),
                new PasswordItem('w', 1),
                new PasswordItem('x', 1),
                new PasswordItem('y', 1),
                new PasswordItem('z', 1),

                new PasswordItem('A', 2),
                new PasswordItem('B', 2),
                new PasswordItem('C', 2),
                new PasswordItem('D', 2),
                new PasswordItem('E', 2),
                new PasswordItem('F', 2),
                new PasswordItem('G', 2),
                new PasswordItem('H', 2),
                new PasswordItem('I', 2),
                new PasswordItem('J', 2),
                new PasswordItem('K', 2),
                new PasswordItem('L', 2),
                new PasswordItem('M', 2),
                new PasswordItem('N', 2),
                new PasswordItem('O', 2),
                new PasswordItem('P', 2),
                new PasswordItem('Q', 2),
                new PasswordItem('R', 2),
                new PasswordItem('S', 2),
                new PasswordItem('T', 2),
                new PasswordItem('U', 2),
                new PasswordItem('V', 2),
                new PasswordItem('W', 2),
                new PasswordItem('X', 2),
                new PasswordItem('Y', 2),
                new PasswordItem('Z', 2),

                new PasswordItem('0', 3),
                new PasswordItem('1', 3),
                new PasswordItem('2', 3),
                new PasswordItem('3', 3),
                new PasswordItem('4', 3),
                new PasswordItem('5', 3),
                new PasswordItem('6', 3),
                new PasswordItem('7', 3),
                new PasswordItem('8', 3),
                new PasswordItem('9', 3),

                new PasswordItem('-', 4),
                new PasswordItem('_', 4),
                new PasswordItem('@', 4),
                new PasswordItem('*', 4),
                new PasswordItem('/', 4),
                new PasswordItem('\\', 4),
                new PasswordItem('%', 4),
                new PasswordItem('&', 4),
                new PasswordItem('#', 4),
                new PasswordItem('!', 4),
                new PasswordItem('?', 4),
                new PasswordItem(',', 4),
                new PasswordItem('.', 4),
                new PasswordItem(';', 4),
                new PasswordItem(':', 4)
            },
            NumChars = 8
        };

        public static readonly PasswordOptions Reinforced = new PasswordOptions()
        {
            Alphabet = new PasswordItem[]
            {
                new PasswordItem('a', 1),
                new PasswordItem('b', 1),
                new PasswordItem('c', 1),
                new PasswordItem('d', 1),
                new PasswordItem('e', 1),
                new PasswordItem('f', 1),
                new PasswordItem('g', 1),
                new PasswordItem('h', 1),
                new PasswordItem('i', 1),
                new PasswordItem('j', 1),
                new PasswordItem('k', 1),
                new PasswordItem('l', 1),
                new PasswordItem('m', 1),
                new PasswordItem('n', 1),
                new PasswordItem('o', 1),
                new PasswordItem('p', 1),
                new PasswordItem('q', 1),
                new PasswordItem('r', 1),
                new PasswordItem('s', 1),
                new PasswordItem('t', 1),
                new PasswordItem('u', 1),
                new PasswordItem('v', 1),
                new PasswordItem('w', 1),
                new PasswordItem('x', 1),
                new PasswordItem('y', 1),
                new PasswordItem('z', 1),

                new PasswordItem('A', 2),
                new PasswordItem('B', 2),
                new PasswordItem('C', 2),
                new PasswordItem('D', 2),
                new PasswordItem('E', 2),
                new PasswordItem('F', 2),
                new PasswordItem('G', 2),
                new PasswordItem('H', 2),
                new PasswordItem('I', 2),
                new PasswordItem('J', 2),
                new PasswordItem('K', 2),
                new PasswordItem('L', 2),
                new PasswordItem('M', 2),
                new PasswordItem('N', 2),
                new PasswordItem('O', 2),
                new PasswordItem('P', 2),
                new PasswordItem('Q', 2),
                new PasswordItem('R', 2),
                new PasswordItem('S', 2),
                new PasswordItem('T', 2),
                new PasswordItem('U', 2),
                new PasswordItem('V', 2),
                new PasswordItem('W', 2),
                new PasswordItem('X', 2),
                new PasswordItem('Y', 2),
                new PasswordItem('Z', 2),

                new PasswordItem('0', 3),
                new PasswordItem('1', 3),
                new PasswordItem('2', 3),
                new PasswordItem('3', 3),
                new PasswordItem('4', 3),
                new PasswordItem('5', 3),
                new PasswordItem('6', 3),
                new PasswordItem('7', 3),
                new PasswordItem('8', 3),
                new PasswordItem('9', 3),

                new PasswordItem('-', 4),
                new PasswordItem('_', 4),
                new PasswordItem('@', 4),
                new PasswordItem('*', 4),
                new PasswordItem('/', 4),
                new PasswordItem('\\', 4),
                new PasswordItem('%', 4),
                new PasswordItem('&', 4),
                new PasswordItem('#', 4),
                new PasswordItem('!', 4),
                new PasswordItem('?', 4),
                new PasswordItem(',', 4),
                new PasswordItem('.', 4),
                new PasswordItem(';', 4),
                new PasswordItem(':', 4),
                new PasswordItem('²', 4),

                new PasswordItem('ù', 5),
                new PasswordItem('é', 5),
                new PasswordItem('è', 5),
                new PasswordItem('$', 5),
                new PasswordItem('µ', 5),
                new PasswordItem('£', 5),
                new PasswordItem('¤', 5),
                new PasswordItem('}', 5),
                new PasswordItem('{', 5),
                new PasswordItem('[', 5),
                new PasswordItem(']', 5),
                new PasswordItem('\'', 5),
                new PasswordItem('(', 5),
                new PasswordItem(')', 5),
                new PasswordItem('à', 5),
                new PasswordItem('^', 5),
                new PasswordItem('ç', 5),
                new PasswordItem('"', 5),
                new PasswordItem('~', 5),
                new PasswordItem('§', 5),
                new PasswordItem('<', 5),
                new PasswordItem('>', 5),
            },
            NumChars = 24
        };
    }
}
