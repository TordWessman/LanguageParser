using System;

namespace LanguageParser
{
	public interface IWord
	{
		int Id { get; }
		string Value { get; }
		IGram Gram { get; }
		string Class { get; }
	}
}

