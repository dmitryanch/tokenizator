using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tokenizator
{
	public class Word
	{
		private const string _pattern = @"(?<abbreviations>(?:[A-ZА-Я]\.)+|[A-ZА-Я]+(?![a-zа-я]))"            // abbreviations
			+ @"|(?<smiles>[\<\>]?[\:\;\=Xx][\-]?(?<smileBracketRight>[\p{Ps}\p{Pe}PpDdXx\*\<\>])+[_0\(]*\k<smileBracketRight>*|(?<smileBracketleft>[\p{Ps}\p{Pe}PpDdXx\*\<\>])\k<smileBracketleft>*[\-]?[\:\;\=Xx]|(?<=^|\s)\)+[_0\(]*\)+|(?<=^|\s)\(+[_0\)]*\(+)" // smiles (obsolete: (?<smiles>[\:\=\(\)\[\]\{\}_\-\>\<]+[\:\=\(\)\[\]\{\}_\-PpDdXx\<\>\*]+))
			+ @"|(?<brackets>(?<![\:\=\(])\((?![\:\=\(])|(?<![\:\=\)])\)(?![\:\=\)])|(?<![\:\=\[])\[(?![\:\=\[])|(?<![\:\=\]])\](?![\:\=\]])|(?<![\:\=\{])\{(?![\:\=\{])|(?<![\:\=\}])\}(?![\:\=\}]))"   // brackets
			+ @"|(?<email>[A-Za-z\d_\-\.]{2,}@[A-Za-z\d_\-\.]{2,}\.[A-Za-z]{2,})"		// e-mails
			//+ @"|(?<date>)"                                       // date
			//+ @"|(?<time>)"                                       // time
			//+ @"|(?<uri>)"                                       // uri
			+ @"|(?<currency>\p{Sc}(?:\s?\d+(?:[\.\,]\d+)?)?|\d+(?:[\.\,]\d+)?(?:\s*\p{Sc}))"                                       // currency
			+ @"|(?<number>\b\d{1,3}(?>(?<ksep>[\,\'\. ])?\d{3}(?>\k<ksep>\d{3})*)?(?>[\.\,]\d+)?(?>\s*%)?\b|\b\d+(?:[\.\,]\d+)?(?>\s*%)?\b)"                    // number, percent (obsolete: (?<number>\d+(?:[\.\,]\d+)?(?:\s*%)?))
			+ @"|(?<wordhyph>[a-zA-ZА-Яа-яёЁ\d]+[\-][a-zA-ZА-Яа-яёЁ\d]+)"       // words with hyphen 
			+ @"|(?<word>([a-zA-ZА-Яа-яёЁ\d]+))"                                        // just words
			+ @"|(?<punctuation>\p{P})"                                   // punctuation (obsolete: [`'""«»]|[!?\/\\\.,;:=]+)
			+ @"|(?<other>[^A-Za-zА-Яа-яЁё\d%\$#\/\s\r\t\n\p{P}]+)";					// others

		private Regex _regex;

		public Word()
		{
			_regex = new Regex(_pattern, RegexOptions.Compiled);
		}

		public string[] Tokenize(string text)
		{
			return _regex.Matches(text).Cast<Match>().Select(m => m.Value).ToArray();
		}

		public IEnumerable<string> Iterate(string text)
		{
			var match = _regex.Match(text);
			while(match.Success)
			{
				var value = match.Value;
				match = match.NextMatch();
				yield return value;
				
			}
		}
	}
}
