using System;
using System.Linq;

namespace Tokenizer.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var tokenizer = new Tokenizer.Word();
			var text = ":) (: =] :] ))) ((((((( =))_))) :)) (= :-) :-X :X >:( sdf";
			var trueAnswer = new[] { ":)", "(:", "=]", ":]", ")))", "(((((((", "=))_)))", ":))", "(=", ":-)", ":-X", ":X", ">:(" };
			var answer = tokenizer.Iterate(text).ToArray();
			System.Console.WriteLine(string.Join(',', answer));
			System.Console.ReadKey();
		}
	}
}
