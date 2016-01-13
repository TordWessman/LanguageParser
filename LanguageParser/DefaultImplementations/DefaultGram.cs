using System;
using System.Collections.Generic;

namespace LanguageParser
{
	/// <summary>
	/// Represents a default gramatical expression
	/// </summary>
	public class DefaultGram : IGram
	{
		public DefaultGram ()
		{
		}

		#region IGram implementation

		private IList<DefaultGram> m_children;

		private string m_name;

		private string m_key;

		private string[] m_classes;

		private DefaultGram m_parent;

		#endregion

		#region IGram implementation

		public IEnumerable<IGram> SubGrammar {
			get {
				return m_children;
			}
		}

		public IList<DefaultGram> Children {

			get {
				return m_children;
			}

			set {

				m_children = value;

				foreach (DefaultGram child in m_children) {
					child.m_parent = this;
				}
			}
		}

		public string Name {

			get {
				return m_name;
			}
			set {
				m_name = value;
			}
		}

		public string Description {

			get {
				return m_parent == null ? m_name : m_name + " " + m_parent.Description;
			}

		}

		public string Key {
			get {
				return m_parent == null ? m_key : m_parent.Key + m_key;
			}
			set {
				m_key = value;
			}
		}

		public string Classes {
			get {
				return m_classes;
			}
			set {
				m_classes = value;
			}

		}

		#endregion
	}
}

