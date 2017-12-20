using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Rules.Api.Controllers
{
    [Route("api/rules")]
    public class RulesController : Controller
    {
        readonly string connectionString = null;
        public RulesController(IConfiguration configuration)
        {
            connectionString = configuration["connection-string"];
        }

        [HttpGet]
        public IEnumerable<dynamic> Get(string questionIds)
        {
            var questionGuids = questionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => Guid.Parse(s)).ToArray();

            using (var connection = new SqlConnection(connectionString))
            {
                var rules = connection.Query<Models.CorrectAnswerRule>
                (
                    @"select RuleId, CorrectAnswerId, QuestionId
                        from [CorrectAnswerRules] as o
                        where Version = 
                        (
                            select max(i.[Version]) 
                            from [CorrectAnswerRules] as i where i.RuleId = o.RuleId
                        )and QuestionId in @questionIds",
                    param: new
                    {
                        questionIds = questionGuids
                    }
                );

                return rules;
            }
        }

        [HttpPut]
        public IEnumerable<dynamic> Put([FromBody]Models.NewCorrectAnswerRule[] rules)
        {
            var requestId = Request.Headers["composed-request-id"].Single();
            var results = new List<dynamic>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                {
                    foreach (var rule in rules)
                    {
                        var ruleId = Guid.NewGuid();

                        connection.Execute
                        (
                            @"insert into [CorrectAnswerRules]
                            ([RuleId]
                            ,[QuestionId]
                            ,[RequestId]
                            ,[Version]
                            ,[CorrectAnswerId])
                        values
                            (@RuleId
                            ,@QuestionId
                            ,@RequestId
                            ,@Version
                            ,@CorrectAnswerId)",
                            param: new
                            {
                                RuleId = ruleId,
                                rule.QuestionId,
                                RequestId = requestId,
                                Version = DateTimeOffset.Now.Ticks,
                                rule.CorrectAnswerId
                            },
                            transaction: tx
                        );

                        results.Add(new
                        {
                            RuleId = ruleId,
                            RequestId = requestId
                        });
                    }

                    tx.Commit();
                }



                return results;
            }
        }

        [HttpPost]
        public IEnumerable<dynamic> Post(Models.EditCorrectAnswerRule[] rules)
        {
            var requestId = Request.Headers["composed-request-id"].Single();
            var results = new List<dynamic>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                {
                    foreach (var rule in rules)
                    {
                        //should validate data already exists

                        connection.Execute
                        (
                            @"insert into [CorrectAnswerRules]
                                ([RuleId]
                                ,[QuestionId]
                                ,[RequestId]
                                ,[Version]
                                ,[CorrectAnswerId])
                            values
                                (@RuleId
                                ,@QuestionId
                                ,@RequestId
                                ,@Version
                                ,@CorrectAnswerId)",
                            param: new
                            {
                                rule.RuleId,
                                rule.QuestionId,
                                RequestId = requestId,
                                Version = DateTimeOffset.Now.Ticks,
                                rule.CorrectAnswerId
                            },
                            transaction: tx
                        );

                        results.Add(new
                        {
                            rule.RuleId,
                            RequestId = requestId
                        });
                    }

                    tx.Commit();
                }

                return results;
            }
        }
    }
}