using System;
using System.Web.Mvc;
using Sb.Interfaces.Services;
using Sb.Web.Models;

namespace Sb.Web.Controllers
{
    [HandleError]
    public partial class HomeController : Controller
    {
        private const int DayInSeconds = 60 * 60 * 24;

        private readonly IQuestionManager _questionManager;
        private readonly IAnswerManager _answerManager;
        private readonly IObligationManager _obligationManager;
        private readonly IPersonaManager _personaManager;

        public HomeController(IQuestionManager questionManager, IAnswerManager answerManager, IObligationManager obligationManager, IPersonaManager personaManager)
        {
            if (questionManager == null)
            {
                throw new ArgumentNullException(nameof(questionManager));
            }

            if (answerManager == null)
            {
                throw new ArgumentNullException(nameof(answerManager));
            }

            if (obligationManager == null)
            {
                throw new ArgumentNullException(nameof(obligationManager));
            }

            if (personaManager == null)
            {
                throw new ArgumentNullException(nameof(personaManager));
            }

            _questionManager = questionManager;
            _answerManager = answerManager;
            _obligationManager = obligationManager;
            _personaManager = personaManager;
        }

        [OutputCache(VaryByParam = "none", Duration = DayInSeconds)]
        public virtual ActionResult Index()
        {
            return View(new LandingModel { FirstQuestionId = _questionManager.GetNextQuestion().Id });
        }

        public virtual ActionResult Warmup()
        {
            return Json(new
            {
                Questions = _questionManager.GetAllQuestions().Count,
                Answers = _answerManager.GetAnswers().Count,
                Obligations = _obligationManager.GetAllObligations().Count,
                Personas = _personaManager.GetAllPersonas().Count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}