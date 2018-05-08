using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tokenizator
{
	public class Word
	{
		private const string _pattern =
			// abbreviations
			@"
(?<abbreviations>
	(?:[A-ZА-Я]\.)+|[A-ZА-Я]+(?![a-zа-я])
)"
			// smiles
			+ @"
|
(?<smiles>
	[\<\>]?[\:\;\=Xx][\-]?(?<smileBracketRight>[\p{Ps}\p{Pe}PpDdXx\*\<\>])+[_0\(]*\k<smileBracketRight>*|(?<smileBracketleft>[\p{Ps}\p{Pe}PpDdXx\*\<\>])\k<smileBracketleft>*[\-]?[\:\;\=Xx]|(?<=^|\s)\)+[_0\(]*\)+|(?<=^|\s)\(+[_0\)]*\(+
)"
			// brackets
			+ @"
|
(?<brackets>
	(?<![\:\=\(])\((?![\:\=\(])|(?<![\:\=\)])\)(?![\:\=\)])|(?<![\:\=\[])\[(?![\:\=\[])|(?<![\:\=\]])\](?![\:\=\]])|(?<![\:\=\{])\{(?![\:\=\{])|(?<![\:\=\}])\}(?![\:\=\}])
)"
			// ip-address
			+ @"
|
(?<ip>
	\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b
)"
			// e-mails
			+ @"
|
(?<email>
	[A-Za-z\d_\-\.]{2,}@[A-Za-z\d_\-\.]{2,}\.[A-Za-z]{2,}
)"
			// date
			+ @"
|
(?<date>
	\d{2}(?<datesep>[\:\-\/])\d{2}\k<datesep>(?:\d{4}|\d{2})
)"
			// time
			+ @"
|
(?<time>
	([01]?[0-9]|2[0-3])(?<timesep>[\:\-\.])[0-5][0-9](?:\k<timesep>[0-5][0-9])?
)"
			// uri
			+ @"
|
(?<absoluturi>
	(?=[\/]*)
	(?:
		(?:(?<scheme>https?|ftp|file|ldap|mailto|urn)\:\/\/\/?)?			# optional http(s) or file prefix
		(?:www\.|ftp\.)?													# optional www. or ftp. prefix
		(?<route>
			(?:[\w\-\.]+\w{2,20}\/)
			(?<subroute>(?:[\w\.]+\/)+)?
		)		# route
	)
	(?<query>\??(?:\&?[\w\-\%\;\.]+(?:\=[\w\-\%\;\.]+)?\/?)+)*				# query
	(?<fragment>\#[\w\-\%\;\.]+)?											# fragment
	(?=[\r\n\t\s\W])														# must be end of word
)"
			// currency
			+ @"
|
(?<currency>
	\p{Sc}(?:\s?\d+(?:[\.\,]\d+)?)?|\d+(?:[\.\,]\d+)?(?:\s*\p{Sc})
)"
			// number, percent
			+ @"
|
(?<number>
		[+-]?
		\b
			\d{1,3}
			(?>
				(?<ksep>[\,\'\. ])?\d{3}(?>\k<ksep>\d{3})*
			)?
			(?>[\.\,]\d+)?
			(?>\s*%)?
		\b
		|
		[+-]?
		\b
			\d+
			(?:[\.\,]\d+)?
			(?>\s*%)?
		\b
)"
			// words with hyphen 			
			+ @"
|
(?<wordhyph>
	[a-zA-ZА-Яа-яёЁ\d]+[\-][a-zA-ZА-Яа-яёЁ\d]+
)"
			// just words
			+ @"
|
(?<word>
	([a-zA-ZА-Яа-яёЁ\d]+)
)"
			// punctuation 
			+ @"
|
(?<punctuation>
	\p{P}
)"
			// others
			+ @"
|
(?<other>
	[^A-Za-zА-Яа-яЁё\d%\$#\/\s\r\t\n\p{P}]+
)";

		private Regex _regex;

		public Word()
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
