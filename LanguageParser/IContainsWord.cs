using System;

namespace LanguageParser
{
	public interface IContainsWord
	{
		bool Contains(IWordData wordData);
		bool Contains(string word);
	}
}

