using System;
using System.Collections.Generic;

namespace LanguageParser
{
	public abstract class BaseGrammarIdentifier : IGrammarIdentifier
	{
		private IWordIdCounter m_counter;

		public BaseGrammarIdentifier (IWordIdCounter counter)
		{
			m_counter = counter;
		}

		protected int NextWordId {

			get {
			
				return m_counter.GetNext;
			
			}
		}

		public abstract bool CanParse (IWordData wordData);

		public abstract IList<IWord> Identify(IWordData wordData);

		// override if required
		public virtual IList<IWord> Finish() { return new List<IWord>(); }

	}

}