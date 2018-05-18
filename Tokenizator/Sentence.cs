using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tokenizator
{
    public class Sentence
    {
		private const string _pattern =
@"
(?<sentence>
	(?:
		[^\.\!\?\;\s]+
		(?:
			(?<=[\d\s])
			[\.]
			[^\.\!\?\s]+
		)?
		(?:
			\r?\n
			|
			\,?\s*
		)
	)+
	[^\.\!\?\;\s]+
	(?:
			(?<=[\d\s])
			[\.]
			(?=[\d\s])
			[^\.\!\?\s]+
	)?
	\s*
	(?:
		[\?\!]+[\""\'\»]?
		|
		\.[\""\'\»]?(?=[\s\r\n\t\wА-Яа-яЁё])
		|
		[\;]
		(?![\-]?[\p{Ps}\p{Pe}PpDdXx\*\<\>])
		|
		\S(?=\s*$)
	)
)";

		private Regex _regex;

		public Sentence()
		{
			_regex = new Regex(_pattern, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		}

		public string[] Tokenize(string text)
		{
			return _regex.Matches(text).Cast<Match>().Select(m => m.Value).ToArray();
		}

		public IEnumerable<string> Iterate(string text)
		{
			var match = _regex.Match(text);
			while (match.Success)
			{
				var value = match.Value;
				match = match.NextMatch();
				yield return value;

			}
		}
	}
}
