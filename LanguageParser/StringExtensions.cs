using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace LanguageParser
{
	public static class CharExtensions
	{
		private static char[] WOVELS = {'a', 'e', 'i', 'o' , 'u' };

		public static bool IsVowel(this char self) {
		
			return WOVELS.Contains (self);
		}


	}

	public static class StringExtensions {

		public static char LastChar(this string self) {
		
			return self [self.Length - 1];
		
		}
	}
}

