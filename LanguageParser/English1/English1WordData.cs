using System;
using System.Text.RegularExpressions;

namespace LanguageParser
{
	public class English1WordData : IWordData
	{
		//		private Regex m_descriptorFinder = new Regex (@"[\ ]*([\ ]*[a-z]+\.[\ ]*(\&)?)+") ;
		private Regex m_descriptorFinder = new Regex (@"[\ ]+([a-z]+\.[\ ]*)+") ;
		private Regex m_obsolete = new Regex (@"Defn: .* \[(O|o)bs\.(\ )*\]");
		private Regex m_obsoleteIdentifier = new Regex (@"obs\.");

		private string m_word;
		private string m_wordData;
		private string m_identifier;

		public English1WordData (string word, string wordData, string[][] replace)
		{

			m_word = word.ToLower();
			m_wordData = wordData;

			string firstLine = wordData.Split ('\n')[2];

			foreach (string[] replacement in replace) {

				firstLine = firstLine.Replace (replacement[0], replacement[1]);
			
			}

			Match finderMatch = m_descriptorFinder.Match (firstLine);

			if (finderMatch.Success) {

				m_identifier = new Regex (@"([a-z]+\.[\ ]*)+").Match (finderMatch.Value).Value.Replace(@" ", "");

			} else {

				//Console.WriteLine ("FAILED TO IDENTIFY English1WordData.Descriptor: " + firstLine);
			
			}
			 
		}

		public bool IsObsolete {
			get {
				return m_obsolete.IsMatch(m_wordData.Replace('\n',' ')) ||
					(m_identifier != null && m_obsoleteIdentifier.IsMatch(m_identifier));
			}
		}


		#region IWordData implementation

		public string Word {

			get {

				return m_word;
			
			}
		
		}
		public string RawData {

			get {

				return m_wordData;
			
			}
		
		}
			
		public string Descriptor {

			get {
			
				return m_identifier;
			
			}

		}

		public string GetRow (int row)
		{

			return m_wordData.Split('\n')[row];
		
		}

		public int RowCount {
		
			get {

				return m_wordData.Split ('\n').Length;
			
			}
		
		}
		#endregion
	}
}

