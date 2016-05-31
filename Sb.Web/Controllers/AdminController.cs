using System;
using System.Web.Mvc;
using Sb.Interfaces.Services;
using Sb.Web.Attributes;
using Sb.Web.Models;

namespace Sb.Web.Controllers
{
    [AdminAuthorize]
    public partial class AdminController : Controller
    {
        private readonly IPersonaManager _personaManager;
        private readonly IQuestionManager _questionManager;
        private readonly IObligationManager _obligationManager;

        public AdminController(IQuestionManager questionManager, IObligationManager obligationManager, IPersonaManager personaManager)
        {
            if (questionManager == null)
            {
                throw new ArgumentNullException(nameof(questionManager));
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
            _obligationManager = obligationManager;
            _personaManager = personaManager;
        }

        public virtual ActionResult QuestionSummary()
        {
            return View(_questionManager.GetAllQuestions());
        }

        public virtual ActionResult QuestionDetail()
        {
            return View(_questionManager.GetAllQuestions());
        }

        public virtual ActionResult ObligationDetail()
        {
            return View(_obligationManager.GetAllObligations());
        }

        public virtual ActionResult AnswerDetail()
        {
            return View(_questionManager.GetAllQuestions());
        }

        public virtual ActionResult PersonaDetail()
        {
            return View(new PersonaDetailModel { Personas = _personaManager.GetAllPersonas(), Questions = _questionManager.GetAllQuestions() });
        }
    }
}