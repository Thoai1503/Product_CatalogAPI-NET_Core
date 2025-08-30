using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogServiceAPI_Electric_Store.Helper
{
    public static class StringHelper
    {
        public static string RemoveVietnameseDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string[] vietnameseChars = {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

            for (int i = 1; i < vietnameseChars.Length; i++)
            {
                for (int j = 0; j < vietnameseChars[i].Length; j++)
                {
                    text = text.Replace(vietnameseChars[i][j], vietnameseChars[0][i - 1]);
                }
            }
            return text;
        }
    }
}