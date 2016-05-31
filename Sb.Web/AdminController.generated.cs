// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Sb.Web.Generated;
namespace Sb.Web.Controllers
{
    public partial class AdminController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AdminController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminController Actions { get { return MVC.Admin; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string QuestionSummary = "QuestionSummary";
            public readonly string QuestionDetail = "QuestionDetail";
            public readonly string ObligationDetail = "ObligationDetail";
            public readonly string AnswerDetail = "AnswerDetail";
            public readonly string PersonaDetail = "PersonaDetail";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string QuestionSummary = "QuestionSummary";
            public const string QuestionDetail = "QuestionDetail";
            public const string ObligationDetail = "ObligationDetail";
            public const string AnswerDetail = "AnswerDetail";
            public const string PersonaDetail = "PersonaDetail";
        }


        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string AnswerDetail = "AnswerDetail";
                public readonly string ObligationDetail = "ObligationDetail";
                public readonly string PersonaDetail = "PersonaDetail";
                public readonly string QuestionDetail = "QuestionDetail";
                public readonly string QuestionSummary = "QuestionSummary";
            }
            public readonly string AnswerDetail = "~/Views/Admin/AnswerDetail.cshtml";
            public readonly string ObligationDetail = "~/Views/Admin/ObligationDetail.cshtml";
            public readonly string PersonaDetail = "~/Views/Admin/PersonaDetail.cshtml";
            public readonly string QuestionDetail = "~/Views/Admin/QuestionDetail.cshtml";
            public readonly string QuestionSummary = "~/Views/Admin/QuestionSummary.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AdminController : Sb.Web.Controllers.AdminController
    {
        public T4MVC_AdminController() : base(Dummy.Instance) { }

        [NonAction]
        partial void QuestionSummaryOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult QuestionSummary()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.QuestionSummary);
            QuestionSummaryOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void QuestionDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult QuestionDetail()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.QuestionDetail);
            QuestionDetailOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ObligationDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ObligationDetail()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ObligationDetail);
            ObligationDetailOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AnswerDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult AnswerDetail()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AnswerDetail);
            AnswerDetailOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void PersonaDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult PersonaDetail()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PersonaDetail);
            PersonaDetailOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
