using System;
using System.Linq;
using System.Web.Mvc;
using Sb.Entities.Models;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Services;
using Sb.Web.Models;

namespace Sb.Web.Controllers
{
    [HandleError]
    public partial class ResultsController : Controller
    {
        private readonly IObligationManager _obligationManager;
        private readonly IAnswerManager _answerManager;
        private readonly IPersonaManager _personaManager;
        private readonly IQuestionManager _questionManager;
        private readonly IHttpWrapper _httpWrapper;

        public ResultsController(IObligationManager obligationManager, IAnswerManager answerManager, IPersonaManager personaManager, IQuestionManager questionManager, IHttpWrapper httpWrapper)
        {
            if (obligationManager == null)
            {
                throw new ArgumentNullException(nameof(obligationManager));
            }

            if (answerManager == null)
            {
                throw new ArgumentNullException(nameof(answerManager));
            }

            if (personaManager == null)
            {
                throw new ArgumentNullException(nameof(personaManager));
            }

            if (questionManager == null)
            {
                throw new ArgumentNullException(nameof(questionManager));
            }

            if (httpWrapper == null)
            {
                throw new ArgumentNullException(nameof(httpWrapper));
            }

            _obligationManager = obligationManager;
            _answerManager = answerManager;
            _personaManager = personaManager;
            _questionManager = questionManager;
            _httpWrapper = httpWrapper;
        }

        public virtual ActionResult Hobby()
        {
            return ShowResult(OutcomeType.Hobby);
        }

        public virtual ActionResult Business()
        {
            return ShowResult(OutcomeType.Business);
        }

        public virtual ActionResult Unsure()
        {
            return ShowResult(OutcomeType.Unsure);
        }

        private ActionResult ShowResult(OutcomeType expectedOutcomeType)
        {
            var referrer = _httpWrapper.UrlReferrer;

            if (referrer == null || !referrer.IsRouteMatch(_httpWrapper.HttpContext, MVC.Questions.Name, MVC.Questions.ActionNames.Index))
            {
                _answerManager.StoreAnswers(_httpWrapper.QueryString);
            }

            var answers = _answerManager.GetAnswers();
            var questions = _questionManager.GetAllQuestions();

            if (answers.Count < questions.Count)
            {
                return RedirectToAction(MVC.Home.Index());
            }

            var dictionary = answers.ToDictionary(x => x.Key.Id, x => x.Value);

            var persona = _personaManager.GetPersona(dictionary);
            var outcomeType = persona?.OutcomeType ?? OutcomeType.Unsure;

            if (outcomeType != expectedOutcomeType)
            {
                return RedirectToOutcomeType(outcomeType);
            }

            var visitor = new Visitor(outcomeType, persona?.Id, dictionary);
            var obligations = _obligationManager.GetObligationsForVisitor(visitor);

            return View(MVC.Results.Views.ViewNames.Index,  new ResultModel { Obligations = obligations, Answers = answers, OutcomeType = outcomeType });
        }

        private ActionResult RedirectToOutcomeType(OutcomeType outcomeType)
        {
            switch (outcomeType)
            {
                case OutcomeType.Business:
                    return RedirectToAction(MVC.Results.Business().AddRouteValues(_httpWrapper.QueryString));
                case OutcomeType.Hobby:
                    return RedirectToAction(MVC.Results.Hobby().AddRouteValues(_httpWrapper.QueryString));
                default:
                    return RedirectToAction(MVC.Results.Unsure().AddRouteValues(_httpWrapper.QueryString));
            }
        }
    }
}