using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sb.Entities;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;

namespace Sb.Services.Managers
{
    public class AnswerManager : IAnswerManager
    {
        private readonly ICookieContext _cookieContext;
        private readonly IQuestionManager _questionManager;

        public AnswerManager(ICookieContext cookieContext, IQuestionManager questionManager)
        {
            if (cookieContext == null)
            {
                throw new ArgumentNullException(nameof(cookieContext));
            }

            if (questionManager == null)
            {
                throw new ArgumentNullException(nameof(questionManager));
            }

            _cookieContext = cookieContext;
            _questionManager = questionManager;
        }

        public string RetrieveAnswerId(string questionId)
        {
            var cookie = _cookieContext.GetCookie(Constants.CookieName.Answers);
            return cookie?[questionId];
        }

        public void StoreAnswerId(string questionId, string answer)
        {
            if (answer != null)
            { 
                var question = _questionManager.GetQuestion(questionId);

                if (question != null)
                {
                    var validAnswers = answer.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).Where(a => question.AnswerList.Any(x => x.Id == a)).ToArray();

                    var cookie = _cookieContext.GetCookie(Constants.CookieName.Answers) ?? new HttpCookie(Constants.CookieName.Answers);
                    cookie[questionId] = validAnswers.Any() ? string.Join(",", validAnswers) : null;

                    _cookieContext.SetCookie(cookie);
                }
            }
        }

        public void StoreAnswers(NameValueCollection nameValueCollection)
        {
            foreach (string key in nameValueCollection)
            {
                StoreAnswerId(key, nameValueCollection[key]);
            }
        }

        public IDictionary<IQuestion, IAnswer[]> GetAnswers()
        {
            var cookie = _cookieContext.GetCookie(Constants.CookieName.Answers);
            var nameValueCollection = cookie?.Values;

            var result = new Dictionary<IQuestion, IAnswer[]>();

            if (nameValueCollection == null)
            {
                return result;
            }

            var items = nameValueCollection.AllKeys.SelectMany(nameValueCollection.GetValues, (k, v) => new { QuestionId = k, AnswerId = v });

            foreach (var item in items)
            {
                var question = _questionManager.GetQuestion(item.QuestionId);
                var answerIds = item.AnswerId ?? string.Empty;

                if (question != null)
                {
                    var answers = answerIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    result.Add(question, question.AnswerList.Where(x => answers.Contains(x.Id)).ToArray());
                }
            }

            return result;
        }

        public NameValueCollection GetNameValueCollection()
        {
            var cookie = _cookieContext.GetCookie(Constants.CookieName.Answers);
            return cookie?.Values ?? new NameValueCollection();
        }
    }
}

