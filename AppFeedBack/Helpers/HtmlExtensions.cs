using System.Text;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using AppFeedBack.Domain;

namespace AppFeedBack.Helpers
{
    public static class HtmlExtensions
    {
        private static TagBuilder CreateJumpButton(string innerHtml, int jumpSize, int page, Func<int, string> pageUrl)
        {
            var tag = new TagBuilder("a");
            tag.AddCssClass("btn btn-default");
            tag.InnerHtml = innerHtml;
            tag.MergeAttribute("href", pageUrl(page - jumpSize));

            if (page <= jumpSize)
            {
                tag.AddCssClass("disabled");
            }

            return tag;
        }

        private static TagBuilder CreateJumpButton(string innerHtml, int jumpSize, int page, int totalPages,
            Func<int, string> pageUrl)
        {
            var tag = new TagBuilder("a");
            tag.AddCssClass("btn btn-default");
            tag.InnerHtml = innerHtml;
            tag.MergeAttribute("href", pageUrl(page + jumpSize));

            if (page >= totalPages - jumpSize)
            {
                tag.AddCssClass("disabled");
            }

            return tag;
        }

        public static MvcHtmlString Pagination<T>(this HtmlHelper html, PagedList<T> list, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();            

            int page = list.Page;
            int totalPages = list.TotalPages;

            result.Append(CreateJumpButton("<<", 5, page, pageUrl));
            result.Append(CreateJumpButton("<", 1, page, pageUrl));

            int startIndex = page - 2 < 1 ? 1 : page - 2;
            int btnCount = startIndex + (totalPages > 5 ? 5 : totalPages) - 1;
            if (btnCount > totalPages) btnCount = totalPages;

            for (int i = startIndex; i <= btnCount; i++)
            {
                var tag = new TagBuilder("a");
                tag.InnerHtml = i.ToString();
                tag.MergeAttribute("href", pageUrl(i));
                tag.AddCssClass("btn btn-default");
                if (i == page)
                {
                    tag.AddCssClass("selected btn-primary");
                }

                result.Append(tag);
            }

            result.Append(CreateJumpButton(">", 1, page, btnCount, pageUrl));
            result.Append(CreateJumpButton(">>", 5, page, btnCount, pageUrl));

            return MvcHtmlString.Create(result.ToString());
        }
    }
}