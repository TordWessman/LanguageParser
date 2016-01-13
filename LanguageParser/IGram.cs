using System;
using System.Collections.Generic;

namespace LanguageParser
{

	public interface IGram
	{
		/// <summary>
		/// Sub types of the specified gramatical object
		/// </summary>
		/// <value>The sub grammar.</value>
		IEnumerable<IGram> SubGrammar { get; }

		/// <summary>
		/// Descriptional name of the object
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }

		/// <summary>
		/// The key used to identify the gramatical object in a dictionary
		/// </summary>
		/// <value>The key.</value>
		string Key { get; set; }

		/// <summary>
		/// Textual description of the gramatical object
		/// </summary>
		/// <value>The description.</value>
		string Description { get; }

		/// <summary>
		/// Specifies different classes of this word
		/// </summary>
		/// <value>The classes.</value>
		string[] Classes { get; }

	}

}

