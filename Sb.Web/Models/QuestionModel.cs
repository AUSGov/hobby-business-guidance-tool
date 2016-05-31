using System;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;

namespace Sb.Web.Models
{
    public class QuestionModel : IQuestion
    {
        private readonly IQuestion _question;

        public QuestionModel(IQuestion question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            _question = question;
        }

        public string Id
        {
            get { return _question.Id; }
            set { _question.Id = value; }
        }

        public string Title
        {
            get { return _question.Title; }
            set { _question.Title = value; }
        }

        public string Description
        {
            get { return _question.Description; }
            set { _question.Description = value; }
        }

        public string MoreInfo
        {
            get { return _question.MoreInfo; }
            set { _question.MoreInfo = value; }
        }

        public string Answer
        {
            get { return _question.Answer; }
            set { _question.Answer = value; }
        }
        public QuestionType QuestionType
        {
            get { return _question.QuestionType; }
            set { _question.QuestionType = value; }
        }

        public IAnswer[] AnswerList
        {
            get { return _question.AnswerList; }
            set { _question.AnswerList = value; }
        }

        public bool IsEnabled
        {
            get { return _question.IsEnabled; }
            set { _question.IsEnabled = value; }
        }

        public int QuestionNumber
        {
            get { return _question.QuestionNumber; }
            set { _question.QuestionNumber = value; }
        }

        public int QuestionCount
        {
            get { return _question.QuestionCount; }
            set { _question.QuestionCount = value; }
        }

        public bool HasFinishedQuestions { get; set; }
    }
}