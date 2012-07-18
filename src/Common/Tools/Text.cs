using System.Collections;

namespace Common.Tools
{
    class Text
    {
        // Methods
        public static string OnlyAlphanumeric(string text)
        {
            string str = "";
            foreach (char ch in text)
            {
                if ((((ch >= 'A') && (ch <= 'Z')) || ((ch >= 'a') && (ch <= 'z'))) || ((ch >= '0') && (ch <= '9')))
                {
                    str = str + ch;
                }
            }
            return str;
        }

        public static string Q(object text)
        {
            return Q(text.ToString());
        }

        public static string Q(string text)
        {
            return text.Replace("'", "''");
        }

        public static string Replicate(string sourceString, int numberOfRepetitions)
        {
            return string.Concat(ArrayList.Repeat(sourceString, numberOfRepetitions).ToArray());
        }

    }
}
