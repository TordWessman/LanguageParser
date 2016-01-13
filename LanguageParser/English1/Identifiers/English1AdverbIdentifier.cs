using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;

namespace LanguageParser
{
	public class English1AdverbIdentifier : BaseGrammarIdentifier, IContainsWord
	{
		private DefaultGram m_gram;

		private IList<string> m_adverbs;

		public IContainsWord ContainsWordDelegate;

		public English1AdverbIdentifier  (DefaultGram adverb, IWordIdCounter counter) : base (counter)
		{

			m_gram = adverb;

			m_adverbs = new List<string> ();

		}

		#region implemented abstract members of BaseGrammarIdentifier
		public override bool CanParse (IWordData wordData)
		{

			return m_gram.Key == wordData.Descriptor;

		}

		public override System.Collections.Generic.IList<IWord> Identify (IWordData wordData)
		{
			if (ContainsWordDelegate == null) {

				throw new ApplicationException ("English1AdverbIdentifier needs a ContainsWordDelegate");
			}

			List<IWord> alt = new List<IWord> ();

			if (ContainsWordDelegate.Contains (wordData.Word)) {

				//Will create duplicates, but should be removed as a post-process
				//return alt;

			}

			alt.Add (new DefaultWord(wordData.Word, m_gram, NextWordId));

			//alt.AddRange(CreateDeterminedAndUndetermined(wordData.Word, m_gram));

			m_adverbs.Add (wordData.Word);

			return alt;
		}
		#endregion


		#region IContainsWord implementation
		public bool Contains (IWordData wordData)
		{

			return m_adverbs.Contains (wordData.Word);

		}

		public bool Contains (string word)
		{

			return m_adverbs.Contains (word);

		}
		#endregion

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

		private string getUnDermined(string word) {

			if (word [0].IsVowel()) {

				return "an " + word;

			}

			return "a " + word;

		}

	}
}

