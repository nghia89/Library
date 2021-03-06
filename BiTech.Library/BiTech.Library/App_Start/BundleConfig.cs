﻿using System.Web;
using System.Web.Optimization;

namespace BiTech.Library
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/unobtrusive-ajax.min.js*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/main.js"
                      ));
            //bundles.Add(new ScriptBundle("~/bundles/Content/fonts").Include(
            //          "~/Content/fonts/fontawesome-webfont.eot",
            //          "~/Content/fonts/fontawesome-webfont.woff",
            //          "~/Content/fonts/fontawesome-webfont.woff2"
            //          ));
            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                      "~/Scripts/main.js",
                      "~/Scripts/jquery.session.js"
                      ));
            bundles.Add(new StyleBundle("~/bundles/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/PagedList.css"));
        }
    }
}
