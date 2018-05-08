using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tokenizer.Test
{
	[TestClass]
	public class Word
	{
		private Tokenizer.Word _tokenizer;

		private bool EqualElementWise<T>(T[] seq1, T[] seq2)
		{
			if (seq1 == null || seq2 == null) return false;
			if (seq1.Length != seq2.Length) return false;
			for (var i = 0; i < seq1.Length; i++)
			{
				if (!seq1[i].Equals(seq2[i]))
				{
					return false;
				}
			}
			return true;
		}

		[TestInitialize]
		public void SetUp()
		{
			_tokenizer = new Tokenizer.Word();
		}

		#region Smiles
		[TestMethod]
		public void CheckSmiles_TruePositive()
		{
			var text = ":) (: =] :] ))) ((((((( =))_))) :)) (= :-) :-X :X >:( :*";
			var trueAnswer = new[] { ":)", "(:", "=]", ":]", ")))", "(((((((", "=))_)))", ":))", "(=", ":-)", ":-X", ":X", ">:(", ":*" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Abbreviation
		[TestMethod]
		public void CheckAbbreviation_TruePositive()
		{
			var text = "U.S.A. СССР РФ УЕФА UK";
			var trueAnswer = new[] { "U.S.A.", "СССР", "РФ", "УЕФА", "UK" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Currency
		[TestMethod]
		public void CheckCurrency_TruePositive()
		{
			var text = "10$ € 10 € 20.20 ₽99,8 8,99 ₽";
			var trueAnswer = new[] { "10$", "€ 10", "€ 20.20", "₽99,8", "8,99 ₽" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Numbers
		[TestMethod]
		public void CheckNumbers_TruePositive()
		{
			var text = "455554,564 1,000,100,100,900,000.2 87878.3454 19819898.0934 1 000 000 000.43 198198,000000";
			var trueAnswer = new[] { "455554,564", "1,000,100,100,900,000.2", "87878.3454", "19819898.0934", "1 000 000 000.43", "198198,000000" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Brackets
		[TestMethod]
		public void CheckBrackets_TruePositive()
		{
			var text = "() {} (1) (sdh) (sf sdg). (s1f 325? sdg sdg)!";
			var trueAnswer = new[] { "(", ")", "{", "}","(","1",")","(","sdh",")","(","sf","sdg", ")", ".", "(", "s1f", "325", "?", "sdg", "sdg", ")", "!"};
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Complex
		[TestMethod]
		public void Complex()
		{
			var text = "ПРОТОКОЛ, протокола, муж. (новогреч. protokollon - первый лист, к которому приклеивается следующий в свитке) (офиц.). Если речь идет о деловой среде, то протокол – это документ, описывающий происходящее событие (собрание, совещание, совет директоров и т.д.).";
			var trueAnswer = new[] { "ПРОТОКОЛ",",", "протокола", ",", "муж", ".", "(", "новогреч", ".", "protokollon", "-", "первый", "лист", ",", "к", "которому", "приклеивается", "следующий", "в", "свитке", ")", "(", "офиц", ".", ")", ".", "Если", "речь", "идет", "о", "деловой", "среде", ",", "то", "протокол", "–", "это", "документ", ",", "описывающий", "происходящее", "событие", "(", "собрание", ",", "совещание", ",", "совет", "директоров", "и", "т", ".", "д", ".", ")", "." };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion
	}
}
