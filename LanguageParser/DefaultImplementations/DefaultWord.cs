using System;

namespace LanguageParser
{
	/// <summary>
	/// Represent default IWord implementation
	/// </summary>
	public class DefaultWord : IWord
	{
		private string m_value;
		private IGram m_gram;
		private int m_id;

		private string m_class;

		public DefaultWord (string value, DefaultGram gram, int id, string classType = null )
		{
			m_value = value;
			m_gram = gram;
			m_id = id;
			m_class = classType;
		}

		#region IWord implementation

		public int Id {
			get {
				return m_id;
			}
		}

		public string Value {
			get {
				return m_value;
			}
		}

		public IGram Gram {
			get {
				return m_gram;
			}
		}


		public string Class {

			get {

				return m_class;

			}

		}

		#endregion
	}
}

