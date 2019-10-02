using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SparkAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkAuto.TagHelpers
{
    //this tells the class that it is a taghelper
    [HtmlTargetElement("div",Attributes="page-model")] //asp-page-model is name when we use this tag helper
    //: TagHelper is built in that we need to inherit from
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory; //MVC Routing used to create URL

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory; //dependency injection for URL helper
        }

        [ViewContext] //specifically tag helper gains read access to ViewContext
        [HtmlAttributeNotBound] //limits tag helper to not write or set
        public ViewContext ViewContext { get; set; } //wrapper on page that has http post get. Package that shows everything going on in page

        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; } //next or previous page?
        public string PageClass { get; set; } //set initial class styles
        public string PageClassNormal { get; set; } //applies to first page only
        public string PageClassSelected { get; set; } //applies to selected page only

        //overriding process to help with creating href related to the execution of the TagHelper
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext); //dependency injection

            TagBuilder result = new TagBuilder("div");

            for (int i = 1; i <= PageModel.TotalPage; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                string url = PageModel.UrlParam.Replace(":", i.ToString()); //adds page number to link in replace of :
                tag.Attributes["href"] = url;

                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);

                tag.InnerHtml.Append(i.ToString()); //display page number as string after href tag 
                result.InnerHtml.Append(tag.ToString()); //closes the div tag
            }

            output.Content.AppendHtml(result.InnerHtml); //outputs to wherever tag helper is placed in our code
        }
    }
}
