using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LanguageParser
{
	/// <summary>
	/// Identifies nouns in the English1 grammar
	/// </summary>
	public class English1AdjectiveIdentifier : BaseGrammarIdentifier, IContainsWord
	{
		private DefaultGram m_gram;

		private string[] m_identifiers = { "a.", "a.superl."};

		private Regex m_brackets = new Regex(@"\[(\ )*(c|C)ompar(.|\n)+\]");
		private Regex m_comparative = new Regex(@"ompar(\.)?(\ )*[A-Za-z]+");
		private Regex m_superlative = new Regex(@"uper(l)?(\.)?(\ )*[A-Za-z]+");

		public IContainsWord ContainsWordDelegate;

		private IList<string> m_adjectives;

		public English1AdjectiveIdentifier (DefaultGram adjective, IWordIdCounter counter) : base (counter)
		{

			m_gram = adjective;

			m_adjectives = new List<string> ();
		
		}

		#region IContainsWord implementation
		public bool Contains (IWordData wordData)
		{

			return m_adjectives.Contains (wordData.Word);

		}

		public bool Contains (string word)
		{

			return m_adjectives.Contains (word);

		}

		#endregion


		#region IGrammarIdentifier implementation

		public override bool CanParse (IWordData wordData)
		{

			return m_identifiers.Contains (wordData.Descriptor);
		
		}

		public override IList<IWord> Identify (IWordData wordData)
		{
			if (ContainsWordDelegate == null) {
			
				throw new ApplicationException ("English1AdjectiveIdentifier needs a ContainsWordDelegate");
			}

			List<IWord> alt = new List<IWord> ();

			if (wordData.Descriptor == "a.superl.") {
			
				//Supqrlative only adjective (ie aftermost)
			
				DefaultGram gram = m_gram.Children.Where (child => child.Name == "superlative").First ();

				alt.Add (new DefaultWord (wordData.Word, gram, NextWordId));

				//alt.AddRange(CreateArticulated(wordData.Word,gram));

				return alt;

			}

			if (m_brackets.IsMatch (wordData.RawData)) {

				//Brackets ( [Compa...) found

				string bracketsContainer = m_brackets.Match (wordData.RawData).Value.Replace ('\n', ' ');

				IList<IWord> comp = GetComparative (bracketsContainer);

				if (comp.Count > 0) {

					alt.AddRange (comp);

				} else {

					Console.WriteLine ("Unable to match Comparative for: " + wordData.Word);

				}

				IList<IWord> sup = GetSuperlative (bracketsContainer);

				if (sup.Count > 0) {

					alt.AddRange (sup);

				} else {

					Console.WriteLine ("Unable to match Superlative for: " + wordData.Word);

				}


			} else {

				//Console.WriteLine ("Unable to match brackets for: " + wordData.Word);

			}


			alt.Add (new DefaultWord(wordData.Word, m_gram, NextWordId));

			//alt.AddRange(CreateDeterminedAndUndetermined(wordData.Word, m_gram));

			alt.AddRange (CreateAdjective (wordData));


			foreach (IWord word in alt) {
			
				m_adjectives.Add (word.Value);
			
			}

			return alt;

		}

		#endregion

		private IList<IWord> CreateAdjective(IWordData wordData) {

			//There will be non-functional adjectives here

			List<IWord> alt = new List<IWord> ();

			string adjective = wordData.Word + "ly";

			if (wordData.Word [wordData.Word.Length - 1] == 'y') {

				adjective = wordData.Word + "ily";

			} /* else if (wordData.Word.Substring(wordData.Word.Length -2, 2) == "le") {

				adjective = wordData.Word.Substring(wordData.Word.Length -1) + "ly";

			}*/

			if (ContainsWordDelegate.Contains (adjective)) {
			
				return alt;

			}

			DefaultGram gram = m_gram.Children.Where (child => child.Name == "adjective").First ();

			alt.Add (new DefaultWord(adjective, gram, NextWordId));
			
			//alt.AddRange (CreateDeterminedAndUndetermined (adjective, gram));

			return alt;

		}

		private string getUnDermined(string word) {

			if (word [0].IsVowel()) {

				return "an " + word;

			}

			return "a " + word;

		}

		private IList<IWord> CreateDeterminedAndUndetermined (string word, DefaultGram mother)
		{
			string article = "the";

			IList<IWord> alt = new List<IWord> ();

			string undetermined = getUnDermined (word);

			alt.Add (new DefaultWord(undetermined, mother.Children.Where( child => child.Name == "undetermined").First(), NextWordId));

			string definedSingular = article + " " + word;

			alt.Add (new DefaultWord(definedSingular, mother.Children.Where( child => child.Name == "articulated").First(), NextWordId));

			return alt;
		}

		private IList<IWord> CreateArticulated (string word, DefaultGram mother)
		{
			string article = "the";

			IList<IWord> alt = new List<IWord> ();

			string definedSingular = article + " " + word;

			alt.Add (new DefaultWord(definedSingular, mother.Children.Where( child => child.Name == "articulated").First(), NextWordId));

			return alt;
		}

		private IList<IWord> GetComparative(string bracketsContainer) {

			List<IWord> alt = new List<IWord> ();

			if (m_comparative.IsMatch (bracketsContainer)) {

				string adjContainer = m_comparative.Match (bracketsContainer).Value;

				string adj = new Regex(@"[A-Za-z]+$").Match(adjContainer).Value.ToLower ();

				DefaultGram gram = m_gram.Children.Where (child => child.Name == "comparative").First ();

				alt.Add (new DefaultWord (adj, gram, NextWordId));
			
				//alt.AddRange(CreateDeterminedAndUndetermined(adj,gram));
			
			}
			//Console.WriteLine ("---- - - -");
			//MainClass.PrintWordData (alt);

			return alt;
		}

		private IList<IWord> GetSuperlative(string bracketsContainer) {

			List<IWord> alt = new List<IWord> ();

			if (m_superlative.IsMatch (bracketsContainer)) {

				string adjContainer = m_superlative.Match (bracketsContainer).Value;

				string adj = new Regex(@"[A-Za-z]+$").Match(adjContainer).Value.ToLower ();

				DefaultGram gram = m_gram.Children.Where (child => child.Name == "superlative").First ();

				alt.Add (new DefaultWord (adj, gram, NextWordId));

				//alt.AddRange(CreateArticulated(adj,gram));

			}

			return alt;
		}

	}

}

