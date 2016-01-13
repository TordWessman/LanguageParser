using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LanguageParser
{
	/// <summary>
	/// Identifies nouns in the English1 grammar
	/// </summary>
	public class English1NounIdentifier : BaseGrammarIdentifier
	{
		private DefaultGram m_noun;

		private string[] m_nuonIdentifiers;

		// Identifies irregular plural nouns
		private Regex m_pluralIdentifier = new Regex(@"\;(\ )*pl\.");

		// Body containing this will be ignored, since the word is a duplicate plural noun
		private Regex m_pluralCaseIgnore = new Regex(@"efn\:\ pl\.\ of\ ");
	

		public English1NounIdentifier (DefaultGram nuon, IWordIdCounter counter) : base (counter)
		{
			m_noun = nuon;
			m_nuonIdentifiers =  new string[] { "n.", "n.pl." , "n.sing."};
		}

		#region IGrammarIdentifier implementation

		public override bool CanParse (IWordData wordData)
		{
			return m_nuonIdentifiers.Contains (wordData.Descriptor);
		}

		public override IList<IWord> Identify (IWordData wordData)
		{
			List<IWord> alt = new List<IWord> ();

			if (m_pluralCaseIgnore.IsMatch (wordData.RawData)) {

				//Plural form only (i e wives) and will be included in the base noun (wife); thus, ignore

				return alt;

			}

			//string article = "the";

			DefaultGram pluralGrammar = m_noun.Children.Where (child => child.Name == "plural").First ();

			if (wordData.Descriptor == "n.pl.") {
			
				//Plural only nuon (ie scissors)

				alt.Add (new DefaultWord (wordData.Word, pluralGrammar, NextWordId));

				/*string definedPluralOnly = article + " " + wordData.Word;

				alt.Add (new DefaultWord (definedPluralOnly, pluralGrammar.Children.Where (child => child.Name == "articulated").First (), NextWordId));
				*/
				alt.AddRange (CreateGenitiveList (alt));

				return alt;

			} 

			string typeClass = wordData.Word [0].IsVowel () ? m_noun.Classes.Where (c => c == "an").First() : m_noun.Classes.Where (c => c == "a").First();

			alt.Add (new DefaultWord(wordData.Word, m_noun, NextWordId, typeClass ));

			// if n.sing., the word is conjugated the same in singular/plural

			string pluralWord = wordData.Descriptor == "n.sing." ? wordData.Word : getPlural (wordData);

			alt.Add (new DefaultWord(pluralWord, pluralGrammar, NextWordId));
			/*
			string undetermined = getUnDermined (wordData.Word);

			alt.Add (new DefaultWord(undetermined, m_noun.Children.Where( child => child.Name == "undetermined").First(), NextWordId));

			string definedSingular = article + " " + wordData.Word;

			alt.Add (new DefaultWord(definedSingular, m_noun.Children.Where( child => child.Name == "articulated").First(), NextWordId));

			string definedPlural = article + " " + pluralWord;

			alt.Add (new DefaultWord(definedPlural, pluralGrammar.Children.Where( child => child.Name == "articulated").First(), NextWordId));
			*/
			alt.AddRange (CreateGenitiveList (alt));

			return alt;
		
		}

		private IList<IWord> CreateGenitiveList(IList<IWord> words) {
		
			IList<IWord> alt = new List<IWord> ();

			foreach (IWord word in words) {
			
				DefaultGram genetive = ((DefaultGram) word.Gram).Children.Where (child => child.Name == "genetive").FirstOrDefault ();

				if (genetive != null) {
				
					string genetiveWord = getGenetive (word.Value);
					alt.Add (new DefaultWord (genetiveWord, genetive, NextWordId));
				}
			}

			return alt;

		}

		#endregion

		private string getPlural(IWordData wordData) {
		

			Match match = m_pluralIdentifier.Match (wordData.GetRow (2));
			string pluralWord = wordData.Word;

			if (match.Success) {

				//The word contains an irregular plural definiton

				pluralWord = wordData.GetRow (2).Substring (match.Index + match.Length);

				pluralWord = new Regex (@"[A-Za-z]+").Match (pluralWord).Value.ToLower();

			} else if (pluralWord.EndsWith("s")) {

				pluralWord += "es";

			} else if (pluralWord.EndsWith("y")) {

				pluralWord = pluralWord.Substring(0, pluralWord.Length - 2) + "ies";

			}  else {

				pluralWord += "s";
			
			}

			return pluralWord;
		}

		private string getUnDermined(string word) {

			if (word [0].IsVowel()) {

				return "an " + word;
			
			}

			return "a " + word;
		}

		private string getGenetive(string word) {
		

			if (word.LastChar () == 's') {
			
				return word + "'";

			}

			return word + "'s";
		}
	}




}

