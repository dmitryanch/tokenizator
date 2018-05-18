using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer.Test
{
	[TestClass]
	public class Sentence
	{
		private Tokenizator.Sentence _tokenizer;

		private static bool EqualElementWise<T>(T[] seq1, T[] seq2)
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
			_tokenizer = new Tokenizator.Sentence();
		}

		#region Test cases
		[TestMethod]
		public void WithoutPunctuationSigns_TruePositive()
		{
			var text = "Анализ, проделанный с помощью (SLOCCount, автор David A Wheeler) показывает, что GCC содержит больше 5 миллионов строк";
			var trueAnswer = new[] { "Анализ, проделанный с помощью (SLOCCount, автор David A Wheeler) показывает, что GCC содержит больше 5 миллионов строк" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void NoWhitespaceBetweenSentence_Russian_TruePositive()
		{
			var text = "Вначале создаётся порт, содержащий все компоненты.Создание прототипа важно для того, чтобы выявить наиболее проблемные места при полном портировании, и должно занять несколько месяцев. ";
			var trueAnswer = new[] { "Вначале создаётся порт, содержащий все компоненты.", "Создание прототипа важно для того, чтобы выявить наиболее проблемные места при полном портировании, и должно занять несколько месяцев." };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void NoWhitespaceBetweenSentence_English_TruePositive()
		{
			var text = "You get the idea: the possible number of combinations that the regex engine will try for each line where the 12th field does not start with a P is huge.All this would take a long time if you ran this regex on a large CSV file where most rows don't have a P at the start of the 12th field.";
			var trueAnswer = new[] { "You get the idea: the possible number of combinations that the regex engine will try for each line where the 12th field does not start with a P is huge.", "All this would take a long time if you ran this regex on a large CSV file where most rows don't have a P at the start of the 12th field." };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void ContainsDifferentSymbols_English_TruePositive()
		{
			var text = "Новая архитектура с большой пользовательской базой, - \" : + = ` ~ 12 1.2 @ должна поддерживать C и C++, и для bare metal, и для Linux.";
			var trueAnswer = new[] { "Новая архитектура с большой пользовательской базой, - \" : + = ` ~ 12 1.2 @ должна поддерживать C и C++, и для bare metal, и для Linux." };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void SingleWordSentence_TruePositive()
		{
			var text = "Светало. Ночь. Truth";
			var trueAnswer = new[] { "Светало.","Ночь.", "Truth" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void SemicolonSentence_TruePositive()
		{
			var text = "1) first sentence; 2) second; 3) третье предложение; 4) четвертое;";
			var trueAnswer = new[] { "1) first sentence;", "2) second;", "3) третье предложение;", "4) четвертое;" };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}

		[TestMethod]
		public void SentenceWithQuotes_TruePositive()
		{
			var text = @"Ценные советы Анатолия Ломаченко во многом помогли «Хай-Теку» собраться после нокдауна и финишировать лучшего легковеса на планете. Наверное, только отец мог начать разговор со слов «Я тебе что говорил?» ""Статус председателя Счетной палаты по закону о Счетной палате — это уровень первого вице-премьера"", — напомнил Кудрин после заседания фракции ЕР.";
			var trueAnswer = new[] { "Ценные советы Анатолия Ломаченко во многом помогли «Хай-Теку» собраться после нокдауна и финишировать лучшего легковеса на планете.", "Наверное, только отец мог начать разговор со слов «Я тебе что говорил?»", @"""Статус председателя Счетной палаты по закону о Счетной палате — это уровень первого вице-премьера"", — напомнил Кудрин после заседания фракции ЕР." };
			var answer = _tokenizer.Tokenize(text);
			var enumerable = _tokenizer.Iterate(text).ToArray();
			Assert.IsTrue(EqualElementWise(trueAnswer, answer));
			Assert.IsTrue(EqualElementWise(trueAnswer, enumerable));
		}
		#endregion

		#region Performance Test
		
		#endregion
	}
}
