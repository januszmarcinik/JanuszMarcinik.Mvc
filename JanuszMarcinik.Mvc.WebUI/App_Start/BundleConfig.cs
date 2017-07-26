﻿using System.Web.Optimization;

namespace JanuszMarcinik.Mvc.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/js/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/js/bootstrap.js",
                      "~/Assets/js/respond.js"));

            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                      "~/Assets/js/application.js"));

            bundles.Add(new StyleBundle("~/Assets/css").Include(
                      "~/Assets/css/bootstrap.css",
                      "~/Assets/css/application.css",
                      "~/Assets/css/font-awesome.min.css"));
        }
    }
}