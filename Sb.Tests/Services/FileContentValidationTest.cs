using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sb.Interfaces.Services;
using Sb.Services.Loaders;
using Sb.Services.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Sb.Tests.Services
{
    [TestClass]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FileContentValidationTest
    {
        private const string DirectoryPath = "..\\..\\..\\Sb.Web\\App_Data";

        private static IQuestionLoader _questionLoader;
        private static IRulesetLoader _rulesetLoader;
        private static IObligationLoader _obligationLoader;
        private static IPersonaLoader _personaLoader;


        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var fakeFilePathResolver = new Mock<IFilePathResolver>();
            var cacheManager = new CacheManager();
            fakeFilePathResolver.Setup(x => x.GetWorkingDirectory()).Returns(DirectoryPath);

            _questionLoader = new FileQuestionLoader(cacheManager, fakeFilePathResolver.Object, "Questions", "FileContentValidationQuestions");
            Assert.IsNotNull(_questionLoader);

            _rulesetLoader = new FileRulesetLoader(cacheManager, fakeFilePathResolver.Object, "Rules", "FileContentValidationRules");
            Assert.IsNotNull(_rulesetLoader);

            _personaLoader = new FilePersonaLoader(cacheManager, fakeFilePathResolver.Object, "Personas", "FileContentValidationPersonas");
            Assert.IsNotNull(_personaLoader);

            _obligationLoader = new FileObligationLoader(cacheManager, fakeFilePathResolver.Object, "Obligations", "FileContentValidationObligations");
            Assert.IsNotNull(_obligationLoader);
        }

        [TestMethod]
        public void Content_JsonFilesHaveValidJsonSyntax()
        {
            // Arrange
            // Act
            try
            {
                _questionLoader.GetQuestions();
                _obligationLoader.GetObligations();
                _rulesetLoader.GetRulesets();
                _personaLoader.GetPersonas();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Obligations_HaveContent()
        {
            // Arrange
            var obligations = _obligationLoader
                .GetObligations()
                .ToList();

            // Act
            var results = obligations
                .Where(o => !string.IsNullOrWhiteSpace(o.Id) && !string.IsNullOrWhiteSpace(o.Title))
                .ToList();

            // Assert
            Assert.AreEqual(results.Count, obligations.Count);
        }


        [TestMethod]
        public void Obligations_LinksAreNotBroken()
        {
            // Arrange
            var obligations = _obligationLoader.GetObligations()
                .Where(o => o.Link != null);

            foreach (var obligation in obligations)
            {
                // from alternative answer to http://stackoverflow.com/questions/924679/c-sharp-how-can-i-check-if-a-url-exists-is-valid

                HttpWebResponse response = null;
                try
                {
                    // Act
                    var request = (HttpWebRequest)WebRequest.Create(obligation.Link.Url);
                    request.Method = "HEAD";
                    response = request.GetResponse() as HttpWebResponse;
                    // Assert
                    Assert.IsNotNull(response);
                    Assert.AreEqual(response.StatusCode, HttpStatusCode.OK, "Broken link: " + obligation.Link.Url + " in " + obligation.Id);
                }
                catch
                {
                    Assert.Fail("Bad link in " + obligation.Id);
                }
                finally
                {
                    response?.Close();
                }
            }
        }


        [TestMethod]
        public void Obligations_LinksInDescriptionAreNotBroken()
        {
            // Arrange
            var doc = new HtmlDocument();
            var sb = new StringBuilder();

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var obligations = _obligationLoader.GetObligations()
                .Where(o => !string.IsNullOrWhiteSpace(o.Description));

            foreach (var obligation in obligations)
            {
                doc.LoadHtml(obligation.Description);
                var aNodes = doc.DocumentNode.SelectNodes("//a");
                if (aNodes != null)
                {
                    var hrefList = aNodes
                        .Select(a => a.GetAttributeValue("href", null))
                        .Where(h => h != null);

                    foreach (var link in hrefList)
                    {
                        if (link.StartsWith("tel"))
                        {
                            continue;
                        }

                        HttpWebRequest request;
                        HttpWebResponse response = null;
                        try
                        {
                            // Act
                            request = (HttpWebRequest)WebRequest.Create(link);
                            request.Method = "HEAD";
                            response = request.GetResponse() as HttpWebResponse;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);

                            try
                            {
                                // try GET if HEAD method fails
                                request = (HttpWebRequest)WebRequest.Create(link);
                                request.Method = "GET";
                                response = request.GetResponse() as HttpWebResponse;
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine(ex2);    
                            }
                        }
                        finally
                        {
                            response?.Close();

                            if (response == null)
                            {
                                sb.AppendLine("Broken link: " + link + " in " + obligation.Id);
                            }
                        }
                    }
                }
            }

            // Assert
            var message = sb.ToString();
            Assert.AreEqual(string.Empty, message, message);
        }

        [TestMethod]
        public void Obligations_JsonFilesHaveMatchingHtmlFiles()
        {
            // Arrange
            var path = DirectoryPath + "\\Obligations";

            var jsonFiles = Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories)
                .Select(f => Path.GetFileNameWithoutExtension(f).Substring(4, 5));

            var htmlFiles = Directory.EnumerateFiles(path, "*.html", SearchOption.AllDirectories)
                .Select(Path.GetFileNameWithoutExtension);


            // Act

            var results = jsonFiles.Except(htmlFiles).ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Obligation files don't have html files: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Content_HtmlFilesHaveValidHtmlSyntax()
        {
            // Arrange
            var doc = new HtmlDocument();

            var items = _obligationLoader.GetObligations()
                .Select(o => new[] { o.Description, o.Id })
                .ToList();

            items.AddRange(_questionLoader.GetQuestions()
                .Where(q => !string.IsNullOrWhiteSpace(q.Description))
                .Select(q => new[] { q.Description, q.Id })
                .ToList());

            items.AddRange(_questionLoader.GetQuestions()
                .Where(q => !string.IsNullOrWhiteSpace(q.MoreInfo))
                .Select(q => new[] { q.MoreInfo, q.Id })
                .ToList());

            var parseErrors = new List<string>();

            // Act
            foreach (var item in items)
            {
                doc.LoadHtml(item[0]);
                if (doc.ParseErrors.Any())
                {
                    parseErrors.Add(item[1] + " (" + doc.ParseErrors.First().Reason + ")");
                }
            }

            // Assert
            if (parseErrors.Any())
            {
                Assert.Fail("Invalid html in " + parseErrors.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Obligations_HaveAtLeastOneRuleset()
        {
            // Arrange
            var targetIds = _rulesetLoader.GetRulesets()
                .Select(r => r.ObligationId);

            var obligationIds = _obligationLoader.GetObligations()
                .Select(o => o.Id);

            // Act

            var results = obligationIds.Where(t => targetIds.All(o => o != t)).ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Obligation has no ruleset: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }


        [TestMethod]
        public void Questions_HaveTitlesAndShortTitles()
        {
            // Arrange
            var questions = _questionLoader.GetQuestions();

            // Act
            var results = questions
                .Where(q => string.IsNullOrWhiteSpace(q.Title) || string.IsNullOrWhiteSpace(q.ShortTitle))
                .Select(q => q.Id).ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Question is missing title or short tile: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Questions_AnswersHaveMoreThanOneOption()
        {
            // Arrange
            var questions = _questionLoader.GetQuestions();

            // Act
            var results = questions
                .Where(q => q.AnswerList.Length > 1)
                .ToList();

            // Assert
            Assert.AreEqual(results.Count, questions.Count);
        }

        [TestMethod]
        public void Questions_AnswersHaveContent()
        {
            // Arrange
            var answers = _questionLoader
                .GetQuestions()
                .SelectMany(q => q.AnswerList)
                .ToList();

            // Act
            var results = answers
                .Where(a => !string.IsNullOrWhiteSpace(a.Id) && !string.IsNullOrWhiteSpace(a.Title))
                .ToList();

            // Assert
            Assert.AreEqual(results.Count, answers.Count);
        }

        [TestMethod]
        public void Rulesets_HaveValidObligationTargets()
        {
            // Arrange
            var targetIds = _rulesetLoader.GetRulesets()
                .Select(r => r.ObligationId);

            var obligationIds = _obligationLoader.GetObligations()
                .Select(o => o.Id);

            // Act
            var results = targetIds.Except(obligationIds).ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Obligation has no Ruleset: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }


        [TestMethod]
        public void Rulesets_HaveValidAnswerMemberNames()
        {
            // Arrange
            var answerMembers = _questionLoader
                .GetQuestions()
                .Select(q => q.Id)
                .ToList();

            answerMembers.Add("OutcomeType");
            answerMembers.Add("PersonaId");

            var ruleMembers = _rulesetLoader
                .GetRulesets()
                .SelectMany(r => r.Rules)
                .Select(r => r.MemberName)
                .ToList();


            // Act
            var results = ruleMembers.Except(answerMembers);

            // Assert
            Assert.IsFalse(results.Any());
        }


        [TestMethod]
        public void Rulesets_HaveValidAnswerTargetValues()
        {
            // Arrange
            var answerValues = _questionLoader
                .GetQuestions()
                .SelectMany(a => a.AnswerList)
                .Select(v => v.Id)
                .ToList();

            answerValues.AddRange(Enum.GetNames(typeof(Interfaces.Enums.OutcomeType)).Select(s => s));
            answerValues.AddRange(_personaLoader.GetPersonas().Select(x => x.Id));

            var ruleValues = _rulesetLoader
                .GetRulesets()
                .SelectMany(r => r.Rules)
                .Select(r => r.TargetValue)
                .ToList();

            // Act
            var results = ruleValues.Except(answerValues).ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Answer has invalid value: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Content_ItemsHaveGloballyUniqueId()
        {
            // Arrange
            var ids = _questionLoader.GetQuestions()
                .Select(q => q.Id)
                .ToList();

            ids.AddRange(_questionLoader.GetQuestions()
                .SelectMany(q => q.AnswerList)
                .Select(a => a.Id)
                .ToList()
                );

            ids.AddRange(_obligationLoader.GetObligations()
                .Select(o => o.Id)
                .ToList()
                );

            ids.AddRange(_rulesetLoader.GetRulesets()
                .Select(r => r.Id)
                .ToList()
                );

            ids.AddRange(_personaLoader.GetPersonas()
                .Select(p => p.Id)
                .ToList()
                );

            // Act
            var duplicates = ids.GroupBy(s => s)
                .SelectMany(grp => grp.Skip(1))
                .Distinct()
                .ToList();

            // Assert
            if (duplicates.Any())
            {
                Assert.Fail("Duplicate ids detected: " + duplicates.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Personas_HaveValidQuestions()
        {

            // Arrange
            var questions = _questionLoader
                .GetQuestions()
                .Select(q => q.Id)
                .ToList();

            var personaQuestions = _personaLoader
                .GetPersonas()
                .SelectMany(p => p.IncludedAnswerList.SelectMany(q => q.QuestionId
                .Select(r => new[] { q.QuestionId, p.Id })))
                .ToList();

            // Act
            var results = personaQuestions
                .Where(x => questions.All(y => y != x[0]))
                .Select(y => y[1] + " (" + y[0] + ")")
                .Distinct()
                .ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Unknown question ID in persona: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }

        [TestMethod]
        public void Personas_HaveValidAnswersToQuestions()
        {
            // Arrange
           var answerValues = _questionLoader
                .GetQuestions()
                .SelectMany(a => a.AnswerList)
                .Select(v => v.Id)
                .ToList();

            var personaAnswers = _personaLoader
                .GetPersonas()
                .SelectMany(p => p.IncludedAnswerList.SelectMany(a => a.Answer.Select(r => new[] { a.Answer, a.QuestionId, p.Id })))
                .ToList();

            // Act
            var results = personaAnswers
                .Where(x => x[0].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Except(answerValues).Any())
                .Select(a => a[2] + " (" + a[0] + " in " + a[1] + ")")
                .Distinct()
                .ToList();

            // Assert
            if (results.Any())
            {
                Assert.Fail("Unknown answer in persona: " + results.Aggregate((i, j) => i + ", " + j));
            }
        }
    }
}
