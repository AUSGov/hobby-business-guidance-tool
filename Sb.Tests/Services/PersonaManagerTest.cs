using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities.Models;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PersonaManagerTest
    {
        private Mock<IPersonaLoader> _fakePersonaLoader;
        private IPersonaManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _fakePersonaLoader = new Mock<IPersonaLoader>();
            _fakePersonaLoader.Setup(x => x.GetPersonas()).Returns(GetPersonaList());
            _manager = new PersonaManager(_fakePersonaLoader.Object);
        }

        private static IList<IPersona> GetPersonaList()
        {
            var includedAnswers = GetIncludedAnswers();
            var excludedAnswers = GetExcludedAnswers();

            var result = new List<IPersona>
            {
                new Persona { Id = "1", IsEnabled = true, IncludedAnswerList = includedAnswers[0], OutcomeType = OutcomeType.Hobby },
                new Persona { Id = "2", IsEnabled = true, IncludedAnswerList = includedAnswers[1] , OutcomeType = OutcomeType.Hobby },
                new Persona { Id = "3", IsEnabled = true, IncludedAnswerList = includedAnswers[2], OutcomeType = OutcomeType.Business },
                new Persona { Id = "4", IsEnabled = true, IncludedAnswerList = includedAnswers[3], OutcomeType = OutcomeType.Business },
                new Persona { Id = "5", IsEnabled = true, IncludedAnswerList = includedAnswers[4], ExcludedAnswerList = excludedAnswers[0], OutcomeType = OutcomeType.Hobby }
            };

            return result;
        }

        private static List<IQuestionAnswerPair[]> GetIncludedAnswers()
        {
            return new List<IQuestionAnswerPair[]>
            {
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q1", Answer = "A" },
                    new QuestionAnswerPair { QuestionId = "Q2", Answer = "A" },
                    new QuestionAnswerPair { QuestionId = "Q3", Answer = "A" }
                },
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q1", Answer = "D,E,F" }
                },
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q1", Answer = "B" },
                    new QuestionAnswerPair { QuestionId = "Q2", Answer = "A,B" },
                    new QuestionAnswerPair { QuestionId = "Q3", Answer = "B" }
                },
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q10", Answer = "A" },
                    new QuestionAnswerPair { QuestionId = "Q11", Answer = "" }
                },
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q20", Answer = "A" },
                    new QuestionAnswerPair { QuestionId = "Q21", Answer = "B" }
                }
            };
        }

        private static List<IQuestionAnswerPair[]> GetExcludedAnswers()
        {
            return new List<IQuestionAnswerPair[]>
            {
                new IQuestionAnswerPair[]
                {
                    new QuestionAnswerPair { QuestionId = "Q99", Answer = "A" },
                    new QuestionAnswerPair { QuestionId = "Q20", Answer = "Z" },
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObject()
        {
            // Act
            _manager = new PersonaManager(null);
        }

        [TestMethod]
        public void MatchesPersona1()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q1", new IAnswer[] { new Answer { Id = "A"} });
            answers.Add("Q2", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("Q3", new IAnswer[] { new Answer { Id = "A" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("1", persona.Id);
            Assert.AreEqual(OutcomeType.Hobby, outcomeType);
        }

        [TestMethod]
        public void MatchesPersona2()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q1", new IAnswer[] { new Answer { Id = "D" }, new Answer { Id = "E" }, new Answer { Id = "F" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("2", persona.Id);
            Assert.AreEqual(OutcomeType.Hobby, outcomeType);
        }

        [TestMethod]
        public void MatchesPersona3()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q1", new IAnswer[] { new Answer { Id = "B" } });
            answers.Add("Q2", new IAnswer[] { new Answer { Id = "A" }, new Answer { Id = "B" } });
            answers.Add("Q3", new IAnswer[] { new Answer { Id = "B" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("3", persona.Id);
            Assert.AreEqual(OutcomeType.Business, outcomeType);
        }

        [TestMethod]
        public void Persona3_MoreAnswersForQuestion()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q1", new IAnswer[] { new Answer { Id = "B" } });
            answers.Add("Q2", new IAnswer[] { new Answer { Id = "A" }, new Answer { Id = "B" }, new Answer { Id = "C" } });
            answers.Add("Q3", new IAnswer[] { new Answer { Id = "B" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("3", persona.Id);
            Assert.AreEqual(OutcomeType.Business, outcomeType);
        }

        [TestMethod]
        public void Persona3_LessAnswersForQuestion()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q1", new IAnswer[] { new Answer { Id = "B" } });
            answers.Add("Q2", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("Q3", new IAnswer[] { new Answer { Id = "B" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("3", persona.Id);
            Assert.AreEqual(OutcomeType.Business, outcomeType);
        }

        [TestMethod]
        public void AnswersHaveUnknownQuestions()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("QQQ1", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("QQQ2", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("QQQ3", new IAnswer[] { new Answer { Id = "A" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNull(persona);
            Assert.AreEqual(OutcomeType.Unsure, outcomeType);
        }

        [TestMethod]
        public void MatchesPersona4()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q10", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("Q11", new IAnswer[] { });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("4", persona.Id);
            Assert.AreEqual(OutcomeType.Business, outcomeType);
        }

        [TestMethod]
        public void NotMatchesPersona4WithoutBlankAnswer()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q10", new IAnswer[] { new Answer { Id = "A" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNull(persona);
            Assert.AreEqual(OutcomeType.Unsure, outcomeType);
        }

        [TestMethod]
        public void NotMatchesPersona4WithNonBlankAnswer()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q10", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("Q11", new IAnswer[] { new Answer { Id = "A" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNull(persona);
            Assert.AreEqual(OutcomeType.Unsure, outcomeType);
        }

        [TestMethod]
        public void MatchesPersona5_NoExcludedMatches()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q20", new IAnswer[] { new Answer { Id = "A" } });
            answers.Add("Q21", new IAnswer[] { new Answer { Id = "B" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNotNull(persona);
            Assert.AreEqual("5", persona.Id);
            Assert.AreEqual(OutcomeType.Hobby, outcomeType);
        }

        [TestMethod]
        public void MatchesPersona5_ExcludedMatches()
        {
            // Arrange
            IDictionary<string, IAnswer[]> answers = new Dictionary<string, IAnswer[]>();
            answers.Add("Q20", new IAnswer[] { new Answer { Id = "A" }, new Answer { Id = "Z" } });
            answers.Add("Q21", new IAnswer[] { new Answer { Id = "B" } });

            // Act
            var persona = _manager.GetPersona(answers);
            var outcomeType = _manager.GetOutcomeType(answers);

            //Assert
            Assert.IsNull(persona);
            Assert.AreEqual(OutcomeType.Unsure, outcomeType);
        }
    }
}
