using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ApplicationLayer.Common {
	public class StringConverter {
		public static string ConvertToUnsign(string input) {
			// Loại bỏ dấu tiếng Việt
			string normalizedString = input.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder();

			foreach (char c in normalizedString) {
				var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
					stringBuilder.Append(c);
				}
			}

			string noDiacritics = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

			string result = Regex.Replace(noDiacritics, @"\s+", "-");

			return result.ToLower();
		}
	}
}
