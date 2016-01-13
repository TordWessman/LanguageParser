using System;

namespace LanguageParser
{
	/// <summary>
	/// Represent a word in it's raw format
	/// </summary>
	/// 
	public interface IWordData
	{
		/// <summary>
		/// The base name of the word
		/// </summary>
		/// <value>The word.</value>
		string Word { get; }

		/// <summary>
		/// All the text extracted for the word except the word title
		/// </summary>
		/// <value>The raw data.</value>
		string RawData {get; }

		/// <summary>
		/// Returns the specified row of the raw data
		/// </summary>
		/// <returns>The row.</returns>
		/// <param name="">.</param>
		string GetRow(int row);

		/// <summary>
		/// Returns the number of rows in the raw data
		/// </summary>
		/// <value>The row count.</value>
		int RowCount {get;}

		/// <summary>
		/// Textual representation of the base identifier for the word class
		/// </summary>
		/// <value>The descriptor.</value>
		string Descriptor { get; }
	}
}

