using System;
using System.Collections.Generic;

namespace LanguageParser
{
	public interface IDictionary
	{
		void AddWord (IWord word);

		IList<IWord> Find(string word);

		void Print();
	}
}

