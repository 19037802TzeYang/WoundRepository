#pragma checksum "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dc11201cc876c73670aff27abf08095aef549c49"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Wound_Index), @"mvc.1.0.view", @"/Views/Wound/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\_ViewImports.cshtml"
using System.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\_ViewImports.cshtml"
using WoundImgRepo.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dc11201cc876c73670aff27abf08095aef549c49", @"/Views/Wound/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"098bfe6577a708acfc48009a15d76c6fa36aa205", @"/Views/_ViewImports.cshtml")]
    public class Views_Wound_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<WoundRecord>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/datatables/css/jquery.dataTables.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/datatables/js/jquery.dataTables.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Wound", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
  
    Layout = "_Wound";

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
 if (TempData["Msg"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div");
            BeginWriteAttribute("class", " class=\"", 99, "\"", 139, 3);
            WriteAttributeValue("", 107, "alert", 107, 5, true);
            WriteAttributeValue(" ", 112, "alert-", 113, 7, true);
#nullable restore
#line 7 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
WriteAttributeValue("", 119, TempData["MsgType"], 119, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        ");
#nullable restore
#line 8 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
   Write(TempData["Msg"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n");
#nullable restore
#line 10 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            DefineSection("MoreScripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "dc11201cc876c73670aff27abf08095aef549c497287", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dc11201cc876c73670aff27abf08095aef549c498465", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

    <script>
        $(document).ready(function () {
            $('#jsWoundsTable').DataTable({
                ordering: true,
                paging: true,
                searching: true,
                info: true,
                lengthChange: true,
                pageLength: 5
            });
        });
    </script>
");
            }
            );
            WriteLiteral(@"
<style>
    img {
        width: 200px;
        height: 200px;
        object-fit: cover;
    }
</style>

<h2>Wounds</h2>
<table id=""jsWoundsTable"" class=""table table-bordered table-condensed table-hover table-striped"">
    <tr>
        <th scope=""col"" width=""20%"">User Id</th>
        <th scope=""col"" width=""20%"">Name</th>
        <th scope=""col"" width=""20%"">Wound Stage</th>
        <th scope=""col"" width=""20%"">Remarks</th>
        <th scope=""col"" width=""20%"">Wound Category</th>
        <th scope=""col"" width=""20%"">Wound Location</th>
        <th scope=""col"" width=""20%"">Original Wound Image</th>
        
        <th scope=""col"" width=""20%"">Tissue</th>
        <th scope=""col"" width=""20%"">Version</th>
        <th scope=""col"" width=""20%"">Image Id</th>
        <th scope=""col"" width=""20%"">User Id</th>
        <th scope=""col"" width=""20%"">Edit / Delete</th>
    </tr>
    <tbody>
");
#nullable restore
#line 56 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
         foreach (WoundRecord w in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr scope=\"row\">\r\n                <td width=\"20%\">");
#nullable restore
#line 59 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 60 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundname);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 61 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundstage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 62 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundremarks);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 63 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundcategoryname);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 64 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundlocationname);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "dc11201cc876c73670aff27abf08095aef549c4913204", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 2074, "~/photos/", 2074, 9, true);
#nullable restore
#line 65 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
AddHtmlAttributeValue("", 2083, w.imagefile, 2083, 12, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</td>\r\n                \r\n                <td width=\"20%\">");
#nullable restore
#line 67 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.tissuename);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 68 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.versionname);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 69 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.imageid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
#nullable restore
#line 70 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                           Write(w.woundid);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dc11201cc876c73670aff27abf08095aef549c4916056", async() => {
                WriteLiteral("Details");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 71 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
                                                                                 WriteLiteral(w.woundid);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</td>\r\n                <td width=\"20%\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dc11201cc876c73670aff27abf08095aef549c4918532", async() => {
                WriteLiteral("Edit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" |  ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dc11201cc876c73670aff27abf08095aef549c4919892", async() => {
                WriteLiteral("Delete");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_6.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 74 "C:\Users\19044593\source\repos\WoundRepository\ProjectFYP\MyCode\WoundImgRepo\WoundImgRepo\Views\Wound\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<WoundRecord>> Html { get; private set; }
    }
}
#pragma warning restore 1591
