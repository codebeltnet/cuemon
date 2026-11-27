---
uid: Cuemon.Extensions.AspNetCore.Mvc.Rendering.HtmlHelperExtensions
example:
- *content
---

The following example demonstrates how to conditionally render content based on the current controller and action using `UseWhenView`, and based on the current Razor Page using `UseWhenPage`. The example uses a minimal `IHtmlHelper` implementation so it can run outside an MVC view.

```csharp
// Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cuemon.Extensions.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

var routeData = new RouteData();
routeData.Values["controller"] = "Home";
routeData.Values["action"] = "Index";
routeData.Values["page"] = "Contact";

var helper = new HtmlHelperStub
{
    ViewContext = new ViewContext
    {
        HttpContext = new DefaultHttpContext(),
        RouteData = routeData,
        ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()),
        Writer = TextWriter.Null
    }
};

Console.WriteLine(helper.UseWhenView("Index", "Home", () => "<div>Welcome to the Home Page</div>"));
Console.WriteLine(helper.UseWhenPage("Contact", () => "Contact Us"));

public sealed class HtmlHelperStub : IHtmlHelper
{
    public ViewContext ViewContext { get; set; } = null!;
    public ViewDataDictionary ViewData { get; } = new(new EmptyModelMetadataProvider(), new ModelStateDictionary());
    public Html5DateRenderingMode Html5DateRenderingMode { get; set; }
    public string IdAttributeDotReplacement => ".";
    public IModelMetadataProvider MetadataProvider { get; } = new EmptyModelMetadataProvider();
    public ITempDataDictionary TempData => throw new NotSupportedException();
    public UrlEncoder UrlEncoder { get; } = UrlEncoder.Default;
    public object ViewBag => throw new NotSupportedException();
    public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent AntiForgeryToken() => throw new NotSupportedException();
    public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes) => throw new NotSupportedException();
    public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData) => throw new NotSupportedException();
    public string DisplayName(string expression) => throw new NotSupportedException();
    public string DisplayText(string expression) => throw new NotSupportedException();
    public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData) => throw new NotSupportedException();
    public string Encode(object value) => throw new NotSupportedException();
    public string Encode(string value) => throw new NotSupportedException();
    public void EndForm() => throw new NotSupportedException();
    public string FormatValue(object value, string format) => throw new NotSupportedException();
    public string GenerateIdFromName(string fullName) => throw new NotSupportedException();
    public IEnumerable<SelectListItem> GetEnumSelectList() => throw new NotSupportedException();
    public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct => throw new NotSupportedException();
    public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType) => throw new NotSupportedException();
    public IHtmlContent Hidden(string expression, object value, object htmlAttributes) => throw new NotSupportedException();
    public string Id(string expression) => throw new NotSupportedException();
    public IHtmlContent Label(string expression, string labelText, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes) => throw new NotSupportedException();
    public string Name(string expression) => throw new NotSupportedException();
    public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData) => throw new NotSupportedException();
    public IHtmlContent Password(string expression, object value, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent Raw(string value) => throw new NotSupportedException();
    public IHtmlContent Raw(object value) => throw new NotSupportedException();
    public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData) => throw new NotSupportedException();
    public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes) => throw new NotSupportedException();
    public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag) => throw new NotSupportedException();
    public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag) => throw new NotSupportedException();
    public string Value(string expression, string format) => throw new NotSupportedException();
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="6ae3c029a56668c172933d01"><a href="~/run-actions/6ae3c029a56668c172933d01/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/6ae3c029a56668c172933d01/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
