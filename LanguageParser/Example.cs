using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LanguageParser
{
	class MainClass
	{
		public static void PrintWordData(IList<IWord> words) {
		
			if (words.Count > 0 ) {

				foreach (IWord word in words) {

					if (word.Gram.Key.StartsWith("n.")) {

						Console.WriteLine (word.Value + " is " + (word.Gram.Description [0].IsVowel () ? "an " : "a ") + word.Gram.Description);

					} else {

						//Console.WriteLine ("ARGH: " + word.Value);
					}

				}


			} else {

				//Console.WriteLine ("UNABLE TO PARSE: " + wordCorpus.Word);

			}
		}

		public static void Main (string[] args)
		{


			GrammarContainer grammarContainer = new GrammarContainer("EnglishGrammar.json");

			IWordDataReader reader = new English1WordDataReader ("english.txt", grammarContainer.ReplaceIdentifiers);

			IDictionary dict = new DefaultDictionary ();

			int i = 0;
			English1GrammarFactory factory = new English1GrammarFactory (grammarContainer, new WordIdCounter(1));

			IGrammarParser grammar = factory.CreateGrammar();

			foreach (IWordData wordCorpus in reader) {
			
				IList<IWord> words = grammar.Parse (wordCorpus);

				foreach (IWord word in words) {
				
					dict.AddWord (word);

				}
				//PrintWordData (words);

				if (i++ == -100) {

					IList<IWord> postProcessed = grammar.PostProcess ();

					//PrintWordData (postProcessed);

					dict.Print ();
					Environment.Exit (0);
				}
			}

			Console.WriteLine ("done: " + i);

		}
	}
}
