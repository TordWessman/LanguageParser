using System;
using System.Collections.Generic;

namespace LanguageParser
{
	/// <summary>
	/// Parses all non-complex gramatical objects
	/// </summary>
	public class GenericGrammarIdentifier: BaseGrammarIdentifier
	{
		private IList<DefaultGram> m_grammar;

		public GenericGrammarIdentifier (IList<DefaultGram> simpleGrammar, IWordIdCounter counter) : base (counter)
		{

			m_grammar = simpleGrammar;
		
		}

		#region IGrammarParser implementation

		public override bool CanParse (IWordData wordData)
		{
			foreach (DefaultGram gram in m_grammar) {
			
				if (gram.Key == wordData.Descriptor) {

					return true;
				
				}

			}

			return false;

		}

		public override IList<IWord> Identify (IWordData wordData)
		{
			IList<IWord> list = new List<IWord> ();

			foreach (DefaultGram gram in m_grammar) {

				if (gram.Key == wordData.Descriptor) {

					list.Add (new DefaultWord(wordData.Word, gram, NextWordId));

				}

			}

			return list;

		}

		#endregion
	}
}

