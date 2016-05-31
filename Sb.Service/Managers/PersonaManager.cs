using System;
using System.Collections.Generic;
using System.Linq;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Managers
{
    public class PersonaManager : IPersonaManager
    {
        private readonly IPersonaLoader _personaLoader;

        public PersonaManager(IPersonaLoader personaLoader)
        {
            if (personaLoader == null)
            {
                throw new ArgumentNullException(nameof(personaLoader));
            }

            _personaLoader = personaLoader;
        }

        public OutcomeType GetOutcomeType(IDictionary<string, IAnswer[]> answers)
        {
            var persona = GetPersona(answers);
            return persona?.OutcomeType ?? OutcomeType.Unsure;
        }

        public IPersona GetPersona(IDictionary<string, IAnswer[]> visitorAnswers)
        {
            return _personaLoader.GetPersonas().FirstOrDefault(persona => DoesPersonaMatch(persona, visitorAnswers));
        }

        public IList<IPersona> GetAllPersonas()
        {
            return _personaLoader.GetPersonas();
        }

        private static bool DoesPersonaMatch(IPersona persona, IDictionary<string, IAnswer[]> visitorAnswers)
        {
            foreach (var questionAnswerPair in persona.IncludedAnswerList)
            {
                if (!visitorAnswers.ContainsKey(questionAnswerPair.QuestionId))
                {
                    return false;
                }

                var pa = questionAnswerPair.Answer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var va = visitorAnswers[questionAnswerPair.QuestionId].Select(x => x.Id).ToArray();

                if (!DoAnswersMatch(pa, va))
                {
                    return false;
                }
            }

            foreach (var questionAnswerPair in persona.ExcludedAnswerList)
            {
                if (!visitorAnswers.ContainsKey(questionAnswerPair.QuestionId))
                {
                    continue;
                }

                var pa = questionAnswerPair.Answer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var va = visitorAnswers[questionAnswerPair.QuestionId].Select(x => x.Id).ToArray();

                if (DoAnswersMatch(pa, va))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DoAnswersMatch(ICollection<string> personaAnswers, ICollection<string> visitorAnswers)
        {
            if (!visitorAnswers.Any() && !personaAnswers.Any())
            {
                return true;
            }

            return personaAnswers.Any(visitorAnswers.Contains);
        }
    }
}