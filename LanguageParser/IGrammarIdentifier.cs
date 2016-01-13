using System;
using System.Collections.Generic;

namespace LanguageParser
{
	public interface IGrammarIdentifier
	{
		/// <summary>
		/// <c>true</c> if this instance can parse the specified wordData; otherwise, <c>false</c>.
		/// </summary>
		/// <returns><c>true</c> if this instance can parse the specified wordData; otherwise, <c>false</c>.</returns>
		/// <param name="wordData">Word data.</param>
		bool CanParse (IWordData wordData);

		/// <summary>
		/// Tries to extract a set of word conjugations from the wordData
		/// </summary>
		/// <param name="wordData">Word data.</param>
		IList<IWord> Identify(IWordData wordData);

		/// <summary>
		/// Calls the grammar identifier after all words have been parsed. This will allow the parser to perform any post-processing of all the input data it have received
		/// </summary>
		IList<IWord> Finish ();
	}
}

