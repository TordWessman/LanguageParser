using System;

namespace LanguageParser
{
	public class WordIdCounter : IWordIdCounter
	{
		private static readonly object m_lock = new object();
		private int m_counter;

		public WordIdCounter (int startSeed = 0)
		{
			m_counter = startSeed;
		}

		#region IWordIdCounter implementation

		public int GetNext {
			get {

				lock (m_lock) {

					return m_counter++;
				
				}
			}
		}

		#endregion
	}
}

