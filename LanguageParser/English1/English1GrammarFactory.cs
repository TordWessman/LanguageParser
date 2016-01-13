using System;
using System.Collections.Generic;

namespace LanguageParser
{
	/// <summary>
	/// Creates a list of grammar parsers
	/// </summary>
	public class English1GrammarFactory
	{
		private GrammarContainer m_grammar;
		private IWordIdCounter m_counter;

		public English1GrammarFactory (GrammarContainer grammar, IWordIdCounter counter)
		{
			m_grammar = grammar; 
			m_counter = counter;
		}

		public IGrammarParser CreateGrammar() {
		
			List<IGrammarIdentifier> identifiers = new List<IGrammarIdentifier> ();

			List<DefaultGram> unidentified = new List<DefaultGram> ();

			English1AdverbIdentifier adverbs = null;
			English1AdjectiveIdentifier adjectives = null;

			foreach (DefaultGram gram in m_grammar.Grammar) {
			
				if (gram.Name == "noun") {

					identifiers.Add (new English1NounIdentifier (gram, m_counter));

				} else if (gram.Name == "verb") {

					identifiers.Add (new English1VerbIdentifier (gram, m_counter));

				} else if (gram.Name == "adjective") {

					adjectives = new English1AdjectiveIdentifier (gram, m_counter);

						identifiers.Add (adjectives);

				} else if (gram.Name == "adverb") {

					adverbs = new English1AdverbIdentifier (gram, m_counter);
					identifiers.Add (adverbs);

				}   else {

					unidentified.Add (gram);
				}
			}

			foreach (DefaultGram gram in unidentified) {
			
				identifiers.Add (new GenericGrammarIdentifier (unidentified, m_counter));

			}

			adjectives.ContainsWordDelegate = adverbs;
			adverbs.ContainsWordDelegate = adjectives;

			return new DefaultGrammarParser (identifiers, m_grammar.IgnoreIdentifiers, m_grammar.IgnoreWords);
		}


	}
}

