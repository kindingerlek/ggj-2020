using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Utils.RichText.Markdown
{
    public static class MarkdownParser
    {
        [System.Serializable]
        public class TextStyle
        {
            [SerializeField] private float h1;
            [SerializeField] private float h2;
            [SerializeField] private float h3;
            [SerializeField] private float paragraph;

            public float H1 { get { return h1; } set { this.h1 = value;} }
            public float H2 { get { return h2; } set { this.h2 = value;} }
            public float H3 { get { return h3; } set { this.h3 = value;} }

            public float Paragraph
            {
                get
                {
                    return paragraph;
                }
                set
                {
                    this.paragraph = value;
                }
            }

            public TextStyle(float h1 = 40, float h2 = 28, float h3 = 20, float p = 16)
            {
                this.h1 = h1;
                this.h2 = h2;
                this.h3 = h3;

                this.paragraph = p;
            }
        }

        public static TextStyle textStyle =  new TextStyle();

        #region Parsers
        static MatchEvaluator parseComment = delegate (Match match)
        {
            return string.Empty;
        };

        static MatchEvaluator parseHeader = delegate (Match match)
        {
            string v = match.ToString().Trim();
            int level = Regex.Match(v, "(#+)").Value.Length - 1;
            float[] fontsize = { textStyle.H1, textStyle.H2, textStyle.H3 };

            return string.Format("<size={0}>{1}</size>\n", fontsize[level], v.Substring(level + 1).Trim());
        };

        static MatchEvaluator parseCombined = delegate (Match match)
        {
            return string.Format("<b><i>{0}</i></b>", match.Groups[2]);
        };

        static MatchEvaluator parseBold = delegate (Match match)
        {
            return string.Format("<b>{0}</b>", match.Groups[2]);
        };

        static MatchEvaluator parseItalic = delegate (Match match)
        {
            return string.Format("<i>{0}</i>", match.Groups[2]);
        };

        static MatchEvaluator parsePharagraph = delegate (Match match)
        {
            return string.Format("<size={0}>{1}</size>", textStyle.Paragraph, match.Groups[1]);
        };

        static MatchEvaluator parseList = delegate (Match match)
        {
            string line = match.Groups[1].Value;
            
            return string.Format("{0}• {1}",match.Groups[1], match.Groups[3]);
        };
        #endregion

        static Dictionary<string, MatchEvaluator> rules = new Dictionary<string, MatchEvaluator>()
        {
            { @"\/\/(.*)",                              parseComment },
            { @"(#{1,6})(.*)",                          parseHeader },
            { @"(\*_+)(.+)(_\*)",                       parseCombined },
            { @"(\*\*|__)(.*?)\1",                      parseBold },
            { @"(\*|_)(.*?)\1",                         parseItalic },
            { @"\n([A-Za-z0-9""]+[^\r\n]*)\n",          parsePharagraph },
            { @"^(\t*?)(\d\.|\*|\-|\+)\s?([^\n\r]*)",   parseList }
        };

        public static string Parse(string input, TextStyle style)
        {
            if (input == null)
                return input;

            textStyle = style;

            foreach (var rule in rules)
                input = Regex.Replace(input, rule.Key, rule.Value, RegexOptions.Multiline);

            return string.Format("<size={0}>{1}</size>", textStyle.Paragraph, input);
        }
    }
}





