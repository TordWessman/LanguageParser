using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageParser
{
	public class WordContainer { 
	
		public IWord Word;
		public List<WordContainer> Children;

		public WordContainer(IWord word) {

			Word = word;
			Children = new List<WordContainer> ();
		
		}

		public void Add(WordContainer container) {

			for (int i = 0; i < Children.Count; i++) {
			
				if (Children [i].IsParentOf (container)) {
				
					Children [i].Add (container);

					return;
				
				}
		
				if (Children [i].IsChildOf (container)) {
				
					container.Add (Children [i]);
					Children.RemoveAt (i);
					i--;

				}

			}

			Children.Add (container);

		}

		public bool IsParentOf(WordContainer newWord) {
		
			if (newWord.Word.Value.Length < Word.Value.Length ) {

				return false;

			}

			return Word.Value == newWord.Word.Value.Substring(0,Word.Value.Length);

		}

		public bool IsChildOf(WordContainer newWord) {

			if (newWord.Word.Value.Length > Word.Value.Length ) {
			
				return false;

			}

			return newWord.Word.Value == Word.Value.Substring(0,newWord.Word.Value.Length);

		}

		public IList<WordContainer> Matches(WordContainer word) {
		
			IList<WordContainer> matches = new List<WordContainer> ();

			foreach (WordContainer child in Children) {
			
				if (word.IsChildOf (child)) {

					return child.Matches (word);

				} else if (word.Word.Value == child.Word.Value) {
				
					matches.Add (word);

				}
			
			}

			return matches;

		}


		public void Print(int trav) {
		
			for (int i = 0; i < trav; i++) {

				Console.Write("-");
			
			}
		
			Console.WriteLine (Word.Value);

			foreach (WordContainer container in Children) {
			
				container.Print (trav + 1);

			}
		}

	}

	public class DefaultDictionary : IDictionary
	{
		private WordContainer m_tree;

		public DefaultDictionary ()
		{
			m_tree = new WordContainer (new DefaultWord ("", null, 0));
		}

		#region IDictionary implementation

		public void AddWord (IWord word)
		{

			m_tree.Add (new WordContainer(word));
		
		}

		public IList<IWord> Find (string word)
		{
			IList<IWord> matches = new List<IWord> ();

			foreach (WordContainer container in m_tree.Matches(new WordContainer(new DefaultWord(word, null, 0)))) {
			
				matches.Add (container.Word);
			}

			return matches;
		}

		public void Print() {
		
			//Console.WriteLine (m_tree.Children[0].Children.Count);
			m_tree.Print (0);

		}

		#endregion
	}
}

