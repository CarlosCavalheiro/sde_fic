using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{

    public static class EnumHtmlHelper
    {

        public static MvcHtmlString DropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return DropDownListFor(htmlHelper, expression, null, null);
        }

        public static MvcHtmlString DropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel)
        {
            return DropDownListFor(htmlHelper, expression, optionLabel, null);
        }

        public static MvcHtmlString DropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            return DropDownListFor(htmlHelper, expression, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string optionLabel, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = Nullable.GetUnderlyingType(typeof(TEnum)) ?? typeof(TEnum);
            IEnumerable<TEnum> enumValues = Enum.GetValues(enumType).Cast<TEnum>();
            IEnumerable<SelectListItem> items = enumValues.Select(e => new SelectListItem { Text = e.ToString(), Value = e.ToString(), Selected = e.Equals(metadata.Model) });
            if (optionLabel != null)
            {
                new[] { new SelectListItem { Text = optionLabel } }.Concat(items);
            }
            return htmlHelper.DropDownListFor(expression, items, optionLabel, htmlAttributes);
        }
    }
}