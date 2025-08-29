using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CatalogServiceAPI_Electric_Store.Helper;
public static class SlugHelper
{
    public static string Slugify(string phrase)
    {
        string str = phrase.ToLowerInvariant();

        // bỏ dấu tiếng Việt
        str = str.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in str)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        str = sb.ToString().Normalize(NormalizationForm.FormC);

        // thay khoảng trắng & ký tự đặc biệt thành -
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", "-").Trim('-');

        return str;
    }
}
