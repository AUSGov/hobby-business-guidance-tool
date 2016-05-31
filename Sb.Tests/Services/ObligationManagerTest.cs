using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Entities.Models;
using Sb.Interfaces.Enums;
using Sb.Interfaces.Models;
using Sb.Interfaces.Services;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ObligationManagerTest
    {
        private Mock<IObligationLoader> _fakeObligationLoader;
        private Mock<IRulesetLoader> _fakeRulesetLoader;
        private IObligationManager _manager;

        [TestInitialize]
        public void Setup()
        {
            _fakeObligationLoader = new Mock<IObligationLoader>();
            _fakeRulesetLoader = new Mock<IRulesetLoader>();

            _manager = new ObligationManager(_fakeObligationLoader.Object, _fakeRulesetLoader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullObligationLoader()
        {
            // Act
            _manager = new ObligationManager(null, _fakeRulesetLoader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsErrorOnPassingInNullRulesetLoader()
        {
            // Act
            _manager = new ObligationManager(_fakeObligationLoader.Object,  null);
        }

        [TestMethod]
        public void GetObligationsForVisitor_DuplicateTargets()
        {
            // Arrange
            _fakeRulesetLoader.Setup(x => x.GetRulesets()).Returns(GetDuplicateTargetRuleset());
            _fakeObligationLoader.Setup(x => x.GetObligations()).Returns(GetObligations());

            var answers = new Dictionary<string, IAnswer[]>
            {
                {"AA", new IAnswer[] {new Answer {Id = "11"}}},
                {"BB", new IAnswer[] {new Answer {Id = "22"}}}
            };

            // Act
            var obligations = _manager.GetObligationsForVisitor(new Visitor(OutcomeType.Unknown, null, answers));

            // Assert
            Assert.AreEqual(2, obligations.Count);
            Assert.AreEqual("T1", obligations.ElementAt(0).Id);
            Assert.AreEqual("T2", obligations.ElementAt(1).Id);
        }

        [TestMethod]
        public void GetObligationsForVisitor_OrderMaintained()
        {
            // Arrange
            _fakeRulesetLoader.Setup(x => x.GetRulesets()).Returns(GetReverseOrderTargetRuleset());
            _fakeObligationLoader.Setup(x => x.GetObligations()).Returns(GetObligations());

            var answers = new Dictionary<string, IAnswer[]>
            {
                {"AA", new IAnswer[] {new Answer {Id = "11"}}},
                {"BB", new IAnswer[] {new Answer {Id = "22"}}}
            };

            // Act
            var obligations = _manager.GetObligationsForVisitor(new Visitor(OutcomeType.Unknown, null, answers));

            // Assert
            Assert.AreEqual(3, obligations.Count);
            Assert.AreEqual("T1", obligations.ElementAt(0).Id);
            Assert.AreEqual("T2", obligations.ElementAt(1).Id);
            Assert.AreEqual("T3", obligations.ElementAt(2).Id);
        }

        [TestMethod]
        public void GetObligationsForVisitor_NoMatches()
        {
            // Arrange
            _fakeRulesetLoader.Setup(x => x.GetRulesets()).Returns(GetDuplicateTargetRuleset());
            _fakeObligationLoader.Setup(x => x.GetObligations()).Returns(GetObligations());

            var answers = new Dictionary<string, IAnswer[]>
            {
                {"XX", new IAnswer[] {new Answer {Id = "11"}}},
                {"ZZ", new IAnswer[] {new Answer {Id = "22"}}}
            };

            // Act
            var obligations = _manager.GetObligationsForVisitor(new Visitor(OutcomeType.Unknown, null, answers));

            // Assert
            Assert.AreEqual(0, obligations.Count);
        }

        [TestMethod]
        public void GetAllObligations()
        {
            // Arrange
            _fakeObligationLoader.Setup(x => x.GetObligations()).Returns(GetObligations());

            // Act
            var obligations = _manager.GetAllObligations();

            // Assert
            Assert.IsNotNull(obligations);
            Assert.AreEqual(6, obligations.Count);
        }

        private static IList<IObligation> GetObligations()
        {
            var result = new List<IObligation>
            {
                new Obligation {Id = "T1"},
                new Obligation {Id = "T2"},
                new Obligation {Id = "T3"},
                new Obligation {Id = "T4"},
                new Obligation {Id = "T5"},
                new Obligation {Id = "T6"}
            };

            return result;
        }

        private static IList<IRuleset> GetDuplicateTargetRuleset()
        {
            var result = new List<IRuleset>
            {
                new Ruleset
                {
                    Id = "R1",
                    IsEnabled = true,
                    ObligationId = "T1",
                    Rules = new IRule[] {new Rule {MemberName = "AA", Operator = "Equal", TargetValue = "11"}}
                },
                new Ruleset
                {
                    Id = "R2",
                    IsEnabled = true,
                    ObligationId = "T2",
                    Rules = new IRule[] {new Rule {MemberName = "AA", Operator = "Equal", TargetValue = "11"}}
                },
                new Ruleset
                {
                    Id = "R3",
                    IsEnabled = true,
                    ObligationId = "T1",
                    Rules = new IRule[] {new Rule {MemberName = "BB", Operator = "Equal", TargetValue = "22"}}
                },
            };

            return result;
        }

        private static IList<IRuleset> GetReverseOrderTargetRuleset()
        {
            var result = new List<IRuleset>
            {
                new Ruleset
                {
                    Id = "R1",
                    IsEnabled = true,
                    ObligationId = "T3",
                    Rules = new IRule[] {new Rule {MemberName = "AA", Operator = "Equal", TargetValue = "11"}}
                },
                new Ruleset
                {
                    Id = "R2",
                    IsEnabled = true,
                    ObligationId = "T2",
                    Rules = new IRule[] {new Rule {MemberName = "AA", Operator = "Equal", TargetValue = "11"}}
                },
                new Ruleset
                {
                    Id = "R3",
                    IsEnabled = true,
                    ObligationId = "T1",
                    Rules = new IRule[] {new Rule {MemberName = "BB", Operator = "Equal", TargetValue = "22"}}
                },
            };

            return result;
        }
    }
}
