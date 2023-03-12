using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployesWork.WebUI.Extensions
{
    public static class HTML_Exten
    {
        public static MvcHtmlString CheckBoxForNullable<Employe>(this HtmlHelper<Employe> helper, bool? b, string str)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "checkbox");
            tag.MergeAttribute("name", str);
            if (b.GetValueOrDefault())
                tag.MergeAttribute("checked", "checked");
            tag.MergeAttribute("value", "1");
            return new MvcHtmlString(tag.ToString());
        }
    }
}