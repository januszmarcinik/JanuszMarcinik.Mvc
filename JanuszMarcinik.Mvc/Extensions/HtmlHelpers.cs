﻿using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using TwitterBootstrapMVC.BootstrapMethods;
using TwitterBootstrapMVC.Controls;

namespace JanuszMarcinik.Mvc
{
    public static class HtmlHelpers
    {
        #region SubmitLink()
        public static MvcHtmlString SubmitLink(this HtmlHelper html, string text, string formId)
        {
            var tag = new TagBuilder("a");

            tag.MergeAttribute("href", string.Format("javascript: document.getElementById('{0}').submit()", formId));
            tag.InnerHtml = text;

            return MvcHtmlString.Create(tag.ToString());
        }
        #endregion

        #region Action()
        public static MvcHtmlString Action(this HtmlHelper htmlHelper, ActionMap result)
        {
            return htmlHelper.Action(result.ActionName, result.ControllerName, result.RouteValues);
        }
        #endregion

        #region ListGroupItem()
        public static BootstrapActionLink ListGroupItem<TModel>(this BootstrapBase<TModel> bootstrap, string text, ActionResult result) where TModel : class
        {
            return bootstrap.ActionLink(text, result).Class("list-group-item");
        }
        #endregion

        #region ButtonsFooter()
        public static MvcHtmlString ButtonsFooter(this HtmlHelper htmlHelper, params IHtmlString[] controls)
        {
            var footer = new TagBuilder("div");
            footer.AddCssClass("navbar-fixed-bottom");
            footer.AddCssClass("navbar-default");
            footer.AddCssClass("navbar");

            var container = new TagBuilder("div");
            container.AddCssClass("container-fluid");

            var buttons = new TagBuilder("p");
            buttons.AddCssClass("navbar-text");

            var builder = new StringBuilder();
            foreach (var control in controls)
            {
                builder.AppendLine(control.ToString());
            }

            buttons.InnerHtml = builder.ToString();
            container.InnerHtml = buttons.ToString();
            footer.InnerHtml = container.ToString();

            return MvcHtmlString.Create(footer.ToString());
        }
        #endregion
    }
}