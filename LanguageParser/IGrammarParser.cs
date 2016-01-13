using System;
using System.Collections.Generic;

namespace LanguageParser
{
	/// <summary>
	/// Contains complete grammar information and uses IGrammarIdentifiers to parse raw word data.
	/// </summary>
	public interface IGrammarParser
	{

		IList<IWord> Parse (IWordData wordData);
	
		/// <summary>
		/// Allows identifiers to post process any data retrieved during parsing;
		/// </summary>
		/// <returns>The process.</returns>
		IList<IWord> PostProcess();

	}
}

