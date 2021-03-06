using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models.ViewModels;

namespace UTCollisionApp.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-changer")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory uhf;

        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }

        public PageInfo PageChanger { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            if (PageChanger.CurrentPage + 10 > PageChanger.TotalPages)
            {
                for (int i = PageChanger.CurrentPage; i <= PageChanger.TotalPages; i++)
                {
                    TagBuilder tb = new TagBuilder("a");

                    tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i });
                    if (PageClassesEnabled)
                    {
                        tb.AddCssClass(PageClass);
                        tb.AddCssClass(i == PageChanger.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tb.AddCssClass(PageClass);
                    tb.InnerHtml.Append(i.ToString());

                    final.InnerHtml.AppendHtml(tb);
                }
            }
            else
            {
                for (int i = PageChanger.CurrentPage; i <= PageChanger.CurrentPage + 10; i++)
                {
                    TagBuilder tb = new TagBuilder("a");

                    tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i });
                    if (PageClassesEnabled)
                    {
                        tb.AddCssClass(PageClass);
                        tb.AddCssClass(i == PageChanger.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tb.AddCssClass(PageClass);
                    tb.InnerHtml.Append(i.ToString());

                    final.InnerHtml.AppendHtml(tb);
                }
            }
           

            tho.Content.AppendHtml(final.InnerHtml);
        }
    }
}
