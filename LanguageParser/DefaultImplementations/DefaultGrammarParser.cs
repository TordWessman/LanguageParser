using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageParser
{
	/// <summary>
	/// Default implementation for grammar parsing
	/// </summary>
	public class DefaultGrammarParser: IGrammarParser
	{
		private IList<IGrammarIdentifier> m_identifiers; 
		private string[] m_ignore;
		private string[] m_ignoreWords;

		public DefaultGrammarParser (IList<IGrammarIdentifier> identifiers, string[] ignore, string[] ignoreWords)
		{

			m_identifiers = identifiers;

			m_ignore = ignore;

			m_ignoreWords = ignoreWords;
		
		}

		#region IGrammar implementation

		public IList<IWord> Parse (IWordData wordData)
		{
			List<IWord> words = new  List<IWord> ();

			if (m_ignore.Contains (wordData.Descriptor)) {
			
				//The identifier should not be used (it's probably redundant)
				return words;

			} else if (m_ignoreWords.Contains (wordData.Word)) {

				//The word should not be used. It might parsed separately
				return words;

			}

			bool didParse = false;

			foreach (IGrammarIdentifier identifier in m_identifiers) {

				if (identifier.CanParse(wordData)) {
				
					didParse = true;

					words.AddRange(identifier.Identify(wordData));

				}

			}

			if (!didParse && !String.IsNullOrEmpty(wordData.Descriptor)) {
			
				Console.WriteLine ("Unable to find identifier for: " + wordData.Word + " id: " + wordData.Descriptor);

			} 

			return words; 
		}


		public IList<IWord> PostProcess() {
		
			List<IWord> words = new  List<IWord> ();

			foreach (IGrammarIdentifier identifier in m_identifiers) {

				words.AddRange(identifier.Finish());

			}

			return words; 

		}

		#endregion
	}

}

