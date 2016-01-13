using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LanguageParser
{
	/// <summary>
	/// Contains/Loads the raw grammar.
	/// </summary>
	public class GrammarContainer
	{
		private IList<DefaultGram> m_grammar;
		private string[] m_ignore;
		private string[][] m_replace;
		private string[] m_ignoreWords;

		public GrammarContainer () {}

		public GrammarContainer (string fileName)
		{

			if (!File.Exists (fileName)) {
				throw new IOException ("Grammar file not found: " + fileName);
			}

			GrammarContainer gram = JsonConvert.DeserializeObject<GrammarContainer> (File.ReadAllText (fileName));

			m_grammar = gram.Grammar;
			m_ignore = gram.IgnoreIdentifiers;
			m_replace = gram.ReplaceIdentifiers;
			m_ignoreWords = gram.IgnoreWords;

		}
		public IList<DefaultGram> Grammar {
			get {
				return m_grammar;
			}
			set {
				m_grammar = value;
			}
		}

		public string[] IgnoreIdentifiers {
			get {
				return m_ignore;
			}
			set {
				m_ignore = value;
			}
		}

		public string[][] ReplaceIdentifiers {
			get {
				return m_replace;
			}
			set {
				m_replace = value;
			}
		}

		public string[] IgnoreWords {
			get {
				return m_ignoreWords;
			}
			set {
				m_ignoreWords = value;
			}
		}

	}
}

