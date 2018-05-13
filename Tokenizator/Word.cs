using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tokenizator
{
	public class Word
	{
		private const string _pattern =
@"(?<abbreviations>
	(?:[A-ZА-Я]\.)+
	|
	[A-ZА-Я]+
	(?![a-zа-я])
)
|
(?<smiles>
	[\<\>]?
	[\:\;\=Xx]
	[\-]?
	(?<smileBracketRight>[\p{Ps}\p{Pe}PpDdXx\*\<\>])+
	[_0\(]*															# misspelling
	\k<smileBracketRight>*
	|
	(?<smileBracketleft>[\p{Ps}\p{Pe}PpDdXx\*\<\>])
	\k<smileBracketleft>*
	[\-]?
	[\:\;\=Xx]
	|
	(?<=^|\s)\)+
	[_0\(]*															# misspelling
	\)+
	|
	(?<=^|\s)
	\(+
	[_0\)]*															# misspelling
	\(+
)
|
(?<brackets>
	(?<![\:\=\(])
	\(
	(?![\:\=\(])
	|
	(?<![\:\=\)])
	\)
	(?![\:\=\)])
	|
	(?<![\:\=\[])
	\[
	(?![\:\=\[])
	|
	(?<![\:\=\]])
	\]
	(?![\:\=\]])
	|
	(?<![\:\=\{])
	\{
	(?![\:\=\{])
	|
	(?<![\:\=\}])
	\}
	(?![\:\=\}])
)
|
(?<ip>
	\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.
	(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b
)
|
(?<email>
	[A-Za-z\d_\-\.]{2,}
	@
	[A-Za-z\d_\-\.]{2,}
	\.
	[A-Za-z]{2,}
)
|
(?<date>
	\d{1,2}
	(?<datesep>[\.\-\/])											# date separator
	\d{1,2}
	\k<datesep>														# same separator
	(?:\d{4}|\d{2})													# possible year part
	|
	(?:\d{4}|\d{2})													# possible year part
	(?<datesep1>[\.\-\/])											# date separator
	\d{1,2}
	\k<datesep1>													# same separator
	\d{1,2}
)
|
(?<time>
	(?:[01]?[0-9]|2[0-3])
	(?<timesep>[\:])												# time separator
	[0-5][0-9]
	(?:\k<timesep>[0-5][0-9])?										# optional seconds part after same separator
)
|
(?<absoluturi>
	(?=[\/]*)
	(?:
		(?:
			(?<scheme>https?|ftp|file|ldap|mailto|urn)
			\:\/\/\/?
		)?															# optional http(s) or file prefix
		(?:www\.|ftp\.)?											# optional www. or ftp. prefix
		(?<route>
			(?:
				[\w\-\.]+
				\w{2,20}\/
			)
			(?<subroute>
				(?:[\w\.]+\/)+
			)?
		)															# route
	)
	(?<query>
		\??
		(?:
			\&?
			[\w\-\%\;\.]+
			(?:
				\=
				[\w\-\%\;\.]+)?
			\/?
		)+
	)*																# query
	(?<fragment>\#[\w\-\%\;\.]+)?									# fragment
	(?=[\r\n\t\s\W])												# must be end of word
)
|
(?<currency>
	\p{Sc}
	(?:
		\s?
		\d+
		(?:[\.\,]\d+)?
	)?
	|
	\d+
	(?:[\.\,]\d+)?
	(?:\s*\p{Sc})
)
|
(?<number>
		[+-]?
		\b
			\d{1,3}
			(?>
				(?<ksep>
					[\,\'\. ]
				)?
				\d{3}
				(?>\k<ksep>\d{3})*
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
)
|
(?<wordhyph>
	[a-zA-ZА-Яа-яёЁ\d]+
	[\-]
	[a-zA-ZА-Яа-яёЁ\d]+
)
|
(?<word>
	([a-zA-ZА-Яа-яёЁ\d]+)
)
|
(?<punctuation>
	\p{P}
)
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
