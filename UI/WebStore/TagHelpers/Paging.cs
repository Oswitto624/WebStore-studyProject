﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.ViewModels;

namespace WebStore.TagHelpers;

public class Paging : TagHelper
{
    private readonly IUrlHelperFactory _UrlHelperFactory;

    public PageViewModel PageModel { get; set; } = null!;

    public string PageAction { get; set; } = null!;

    [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
    public Dictionary<string, object> PageUrlValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    [ViewContext, HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public Paging(IUrlHelperFactory UrlHelperFactory) => _UrlHelperFactory = UrlHelperFactory;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var ul = new TagBuilder("ul");
        ul.AddCssClass("pagination");

        var url_helper = _UrlHelperFactory.GetUrlHelper(ViewContext);

        for (var i = 1; i <= PageModel.TotalPages; i++)
            ul.InnerHtml.AppendHtml(CreateElement(i, url_helper));

        output.Content.AppendHtml(ul);

    }

    private TagBuilder CreateElement(int PageNumber, IUrlHelper Url)
    {
        var li = new TagBuilder("li");
        var a = new TagBuilder("a");
        a.InnerHtml.AppendHtml(PageNumber.ToString());

        if (PageNumber == PageModel.Page)
            li.AddCssClass("active");

        PageUrlValues["PageNumber"] = PageNumber;
        a.Attributes["href"] = Url.Action(PageAction, PageUrlValues);

        foreach (var (key, value) in PageUrlValues.Select(v => (v.Key, Value: v.Value?.ToString())).Where(v => v.Value is { Length: > 0 }))
            a.MergeAttribute($"data-{key}", value);

        li.InnerHtml.AppendHtml(a);
        return li;
    }
}
