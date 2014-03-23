using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SnippetCache.WebUI.HtmlHelpers
{
    public static class BooleanHelpers
    {
        public static MvcHtmlString BooleanDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return BooleanDropDownListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString BooleanDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string emptyText)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            bool? value = null;

            if (metadata != null && metadata.Model != null)
            {
                if (metadata.Model is bool)
                    value = (bool) metadata.Model;
            }

            List<SelectListItem> items = emptyText != null
                                             ? new List<SelectListItem>
                                                   {
                                                       new SelectListItem {Text = emptyText, Value = String.Empty},
                                                       new SelectListItem
                                                           {
                                                               Text = "Yes",
                                                               Value = "True",
                                                               Selected = (value.HasValue && value.Value)
                                                           },
                                                       new SelectListItem
                                                           {
                                                               Text = "No",
                                                               Value = "False",
                                                               Selected = (value.HasValue && value.Value == false)
                                                           }
                                                   }
                                             : new List<SelectListItem>
                                                   {
                                                       new SelectListItem
                                                           {
                                                               Text = "Yes",
                                                               Value = "True",
                                                               Selected = (value.HasValue && value.Value)
                                                           },
                                                       new SelectListItem
                                                           {
                                                               Text = "No",
                                                               Value = "False",
                                                               Selected = (value.HasValue && value.Value == false)
                                                           }
                                                   };
            return htmlHelper.DropDownListFor(expression, items);
        }
    }
}