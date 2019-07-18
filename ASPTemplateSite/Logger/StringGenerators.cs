using NLipsum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ASPTemplateSite.Logger
{
    public class StringGenerators
    {
        static Random random = new Random();
        public static string GenerateString(int length)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static string GenerateContent(int length)
        {   
            return LipsumGenerator.Generate(length);
        }

        public static string GenerateTitle()
        {
            return LipsumGenerator.Generate(1, Features.Sentences, 
                                            Sentence.Short.FormatString, Lipsums.ChildHarold);
        }
    }
}