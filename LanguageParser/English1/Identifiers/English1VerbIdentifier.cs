using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LanguageParser
{
	public class English1VerbIdentifier : BaseGrammarIdentifier
	{
		private DefaultGram m_verb; 
		private string[] m_verbIdentifiers;

		private Regex m_ignoreObsolete = new Regex(@"Defn\:\ .+\[Obs");
		private Regex m_brackets = new Regex(@"\[(\ )*imp(.|\n)+\]");

		private Regex m_imperfectAndPast = new Regex(@"imp(\.)?(\ )*\&(\ )*p(\.)?(\ )*p(\.)?(\ )*[A-Za-z]+");
		private Regex m_imperfect = new Regex(@"imp(\.)?(\ )*[A-Za-z]+");
		private Regex m_past = new Regex(@"p(\.)?(\ )p(\.)?(\ )*[A-Za-z]+");
		private Regex m_progressive = new Regex(@"p(\.)?(\ )*(p)?r(\.)?(\ )*(\&(\ )*v(b)?\.(\ )*n(\.)?(\ )*)?[A-Za-z]{2,}");

		//Contain a list of words that was un-parseable. These words
		private IList<string> m_untrackedWords;

		private IList<string>[] m_trackedWords;

		public English1VerbIdentifier (DefaultGram verb, IWordIdCounter counter) : base(counter)
		{
			m_verb = verb;
			m_verbIdentifiers = new string[] { "v.t.", "v.i.", "v." };

			m_trackedWords = new List<string>[26];
			m_untrackedWords = new List<String> ();

			for (int i = 0; i < 26; i++) {

				m_trackedWords [i] = new List<string> ();

			}
		}

		private void AddTrackedWord(string word) {

			m_trackedWords [word [0] - 97].Add (word);

		}

		private bool HasTrackedWord(string word) {

			return m_trackedWords [(int)word [0] - 97].Contains (word);

		}

		#region IGrammarParser implementation

		public override bool CanParse (IWordData wordData) {

			return  m_verbIdentifiers.Contains (wordData.Descriptor);

		}

		int badCount;

		public override IList<IWord> Identify (IWordData wordData)
		{

			//If no sub grammar (imp. p.p etc) found, this verb is concidered to be an alteration of another verb and should be ignored.
			bool didMatchImp = false;
			bool didPast = false;
			bool didMatchProgressive = false;

			List<IWord> alt = new List<IWord> ();

			if (m_ignoreObsolete.IsMatch(wordData.RawData)) {

				//Marked as an obsolete word

				return alt;

			}

			if (m_brackets.IsMatch (wordData.RawData)) {
			
				//Brackets ( [imp. & p.p. Abashed; ...) found

				string bracketsContainer = m_brackets.Match (wordData.RawData).Value.Replace ('\n', ' ');

				IList<IWord> impAndPast = GetImperfectAndPast (bracketsContainer);

				if (impAndPast.Count > 0) {

					didMatchImp = true;
					didPast = true;

					alt.AddRange (impAndPast);

				} else {
				
					IList<IWord> imp = GetImperfect (bracketsContainer);

					if (imp.Count > 0) {
					
						didMatchImp = true;

						alt.AddRange (imp);
					}

					IList<IWord> past = GetPast (bracketsContainer);

					if (past.Count > 0) {

						didPast = true;

						alt.AddRange (past);
					}

				}

				IList<IWord> progressive = GetProgressive (bracketsContainer);

				if (progressive.Count > 0) {
				
					didMatchProgressive = true;

					alt.AddRange (progressive);

				} else {
				
					//Try to create one by guessing

					didMatchProgressive = true;

					alt.Add (new DefaultWord(CreateProgressive(wordData.Word),  m_verb.Children.Where (child => child.Name == "progressive").First (), NextWordId));

				}

			} else {
			
				//This is probably a non-consistent verb (it is a form ov another, parseable word and is ignored)

				//However, it might be a verb which requires manual conjugation. Need word list in order to check...

				if (!HasTrackedWord (wordData.Word)) {

					m_untrackedWords.Add (wordData.Word);
				
				}

				return alt;

			}

			if (!(didPast && didMatchImp && didMatchProgressive)) {

				if (!didPast) {

					badCount++;
					Console.WriteLine ("Unable to match PAST: " + wordData.Word);
				
				}

				if (!didMatchImp) {

					badCount++;
					Console.WriteLine ("Unable to match IMP: " + wordData.Word);

				}

				if (!didMatchProgressive) {

					badCount++;
					Console.WriteLine ("Unable to match PROGRESSIVE: " + wordData.Word);

				}


				return alt;

			}

			AddTrackedWord(wordData.Word);

			if (m_untrackedWords.Contains (wordData.Word)) {
			
				m_untrackedWords.Remove (wordData.Word);

			}

			alt.Add (new DefaultWord(wordData.Word, m_verb, NextWordId));

			string presentThird = GetPresentThird (wordData.Word);

			alt.Add (new DefaultWord(presentThird, m_verb.Children.Where( child => child.Name == "third").First(), NextWordId));

			return alt;

		}



		public override IList<IWord> Finish() { 

			List<IWord> alt = new List<IWord> ();

			foreach (string word in m_untrackedWords) {

				alt.AddRange (CreateManualConjugations (word));

			}

			//Console.WriteLine ("BADCOUNT: " + badCount);

			return alt;

		}

		#endregion

		private string GetPresentThird(string word) {

			return word + "s";

		}

		private IList<IWord> GetImperfectAndPast(string bracketsContainer) {

			IList<IWord> alt = new List<IWord> ();

			if (m_imperfectAndPast.IsMatch (bracketsContainer)) {

				string verbContainer = m_imperfectAndPast.Match (bracketsContainer).Value;
				string verb = new Regex(@"[A-Za-z]+$").Match(verbContainer).Value.ToLower ();
				alt.Add (new DefaultWord (verb, m_verb.Children.Where (child => child.Name == "imperfect").First (), NextWordId));
				alt.Add (new DefaultWord (verb, m_verb.Children.Where (child => child.Name == "past").First (), NextWordId));

			}

			return alt;
		}

		private IList<IWord> GetImperfect(string bracketsContainer) {

			IList<IWord> alt = new List<IWord> ();

			if (m_imperfect.IsMatch (bracketsContainer)) {

				string verbContainer = m_imperfect.Match (bracketsContainer).Value;
				string verb = new Regex(@"[A-Za-z]+$").Match(verbContainer).Value.ToLower ();
				alt.Add (new DefaultWord (verb, m_verb.Children.Where (child => child.Name == "imperfect").First (), NextWordId));

			}

			return alt;
		}

		private IList<IWord> GetPast(string bracketsContainer) {

			IList<IWord> alt = new List<IWord> ();

			if (m_past.IsMatch (bracketsContainer)) {

				string verbContainer = m_past.Match (bracketsContainer).Value;
				string verb = new Regex(@"[A-Za-z]+$").Match(verbContainer).Value.ToLower ();
				alt.Add (new DefaultWord (verb, m_verb.Children.Where (child => child.Name == "past").First (), NextWordId));

			}

			return alt;
		}

		private IList<IWord> GetProgressive(string bracketsContainer) {

			IList<IWord> alt = new List<IWord> ();

			if (m_progressive.IsMatch (bracketsContainer)) {

				string verbContainer = m_progressive.Match (bracketsContainer).Value;
				string verb = new Regex (@"[A-Za-z]+$").Match (verbContainer).Value.ToLower ();
				alt.Add (new DefaultWord (verb, m_verb.Children.Where (child => child.Name == "progressive").First (), NextWordId));
			} 

			return alt;

		}



		private string CreateProgressive(string verb) {

			return (verb.Last() == 'e' ? verb.Substring(0, verb.Length - 1) : verb) + "ing";

		}

		private string CreateImperfectAndPast(string verb) {


			return verb.Last() == 'y' ?  verb.Substring(0, verb.Length - 1) + "ied" :
				(verb.Last() == 'e' ? verb.Substring(0, verb.Length - 1) : verb) + "ed";

		}

		//Not used..
		private string CreateAdjective(string progressiveForm) {

			return progressiveForm + "ly";

		}

		/// <summary>
		/// Used if the verb did not contain any/sufficient conjugation information
		/// </summary>
		/// <returns>The manual conjugations.</returns>
		/// <param name="verb">Verb.</param>
		private IList<IWord> CreateManualConjugations(string verb) {

			IList<IWord> alt = new List<IWord> ();


			alt.Add (new DefaultWord(verb, m_verb, NextWordId));

			string presentThird = GetPresentThird (verb);

			alt.Add (new DefaultWord(presentThird, m_verb.Children.Where( child => child.Name == "third").First(), NextWordId));

			string imperfectAndPast = CreateImperfectAndPast (verb);
			string progressive = CreateProgressive (verb);

			alt.Add (new DefaultWord (progressive, m_verb.Children.Where (child => child.Name == "progressive").First (), NextWordId));
			alt.Add (new DefaultWord (imperfectAndPast, m_verb.Children.Where (child => child.Name == "imperfect").First (), NextWordId));
			alt.Add (new DefaultWord (imperfectAndPast, m_verb.Children.Where (child => child.Name == "past").First (), NextWordId));

			return alt;

		}

	}

}

