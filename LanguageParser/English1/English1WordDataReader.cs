using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LanguageParser
{
	public class English1WordDataReader: IWordDataReader
	{

		private Regex m_newWordTitle = new Regex (@"^[A-Z](\ |-)?[A-Z]+$") ;

		private string m_lastWordTitle;
		private StreamReader m_reader;
		private string m_fileName;
		private string[][] m_replace;

		public English1WordDataReader (string fileName, string[][] replace)
		{
			if (!File.Exists (fileName)) {

				throw new IOException ("Dictionary not found: " + fileName);

			}

			m_fileName = fileName;
			m_replace = replace;

		}

		~English1WordDataReader() {
		
			if (m_reader != null) {
			}
		}



		#region IEnumerable implementation

		public IEnumerator<IWordData> GetEnumerator ()
		{
			string line = null;
			string wordData = "";

			using (m_reader = new StreamReader (m_fileName)) {
			
				while ((line = m_reader.ReadLine ()) != null) {

					Match match = m_newWordTitle.Match (line);

					if (match.Success) {

						if (m_lastWordTitle == null) {

							m_lastWordTitle = match.Value;

						} else {

							English1WordData wordDataObject = new English1WordData (m_lastWordTitle, wordData, m_replace);

							if (!wordDataObject.IsObsolete && !string.IsNullOrEmpty(wordDataObject.Descriptor)) {

								yield return wordDataObject;
							
							}
							 

							m_lastWordTitle = match.Value;

							wordData = "";

						}

					}

					wordData += "\n" + line;

				}

			}

		}

		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		#endregion
	}
}

