using System;
using System.Linq;
using System.Web.Mvc;
using Sb.Entities;
using Sb.Entities.Models;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Web.Controllers
{
    [HandleError]
    public partial class QuestionsController : Controller
    {
        private readonly IQuestionManager _questionManager;
        private readonly IAnswerManager _answerManager;
        private readonly IPersonaManager _personaManager;
        private readonly IHttpWrapper _httpWrapper;

        public QuestionsController(IQuestionManager questionManager, IAnswerManager answerManager, IPersonaManager personaManager, IHttpWrapper httpWrapper)
        {
            if (questionManager == null)
            {
                throw new ArgumentNullException(nameof(questionManager));
            }

            if (answerManager == null)
            {
                throw new ArgumentNullException(nameof(answerManager));
            }

            if (personaManager == null)
            {
                throw new ArgumentNullException(nameof(personaManager));
            }

            if (httpWrapper == null)
            {
                throw new ArgumentNullException(nameof(httpWrapper));
            }

            _questionManager = questionManager;
            _answerManager = answerManager;
            _personaManager = personaManager;
            _httpWrapper = httpWrapper;
        }

        public virtual ActionResult Index(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var question = _questionManager.GetQuestion(id);

            if (question == null)
            {
                throw new ArgumentException(nameof(id));
            }

            if (question.QuestionNumber > _answerManager.GetAnswers().Count + 1)
            {
                return RedirectToAction(MVC.Home.Index());
            }

            question.Answer = _answerManager.RetrieveAnswerId(question.Id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Index(Question model, string submit)
        {
            if (submit == Constants.SubmitType.Previous)
            {
                return MoveBackward(model);
            }

            if (submit == Constants.SubmitType.Results)
            {
                return MoveToResults(model);
            }

            return MoveForward(model);
        }

        private ActionResult MoveForward(IQuestion model)
        {
            _answerManager.StoreAnswerId(model.Id, model.Answer ?? _httpWrapper.Form[model.Id]);

            var nextQuestion = _questionManager.GetNextQuestion(model.Id);

            if (nextQuestion == null)
            {
                return RedirectToResults();
            }

            return RedirectToAction(MVC.Questions.ActionNames.Index, new { id = nextQuestion.Id });
        }

        private ActionResult MoveBackward(IQuestion model)
        {
            _answerManager.StoreAnswerId(model.Id, model.Answer ?? _httpWrapper.Form[model.Id]);

            var previousQuestion = _questionManager.GetPreviousQuestion(model.Id);

            if (previousQuestion == null)
            {
                return RedirectToAction(MVC.Home.Index());
            }

            return RedirectToAction(MVC.Questions.ActionNames.Index, new { id = previousQuestion.Id });
        }

        private ActionResult MoveToResults(IQuestion model)
        {
            _answerManager.StoreAnswerId(model.Id, model.Answer ?? _httpWrapper.Form[model.Id]);
            return RedirectToResults();
        }

        private ActionResult RedirectToResults()
        {
            var answers = _answerManager.GetAnswers();
            var outcomeType = _personaManager.GetOutcomeType(answers.ToDictionary(x => x.Key.Id, x => x.Value));

            switch (outcomeType)
            {
                case OutcomeType.Business:
                    return RedirectToAction(MVC.Results.Business().AddRouteValues(_answerManager.GetNameValueCollection()));
                case OutcomeType.Hobby:
                    return RedirectToAction(MVC.Results.Hobby().AddRouteValues(_answerManager.GetNameValueCollection()));
                default:
                    return RedirectToAction(MVC.Results.Unsure().AddRouteValues(_answerManager.GetNameValueCollection()));
            }
        }
    }
}