using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Answers.Api.Controllers
{
    [Route("api/answers")]
    public class AnswersController : Controller
    {
        readonly string connectionString = null;
        public AnswersController(IConfiguration configuration)
        {
            connectionString = configuration["connection-string"];
        }

        [HttpGet]
        public IEnumerable<dynamic> Get(string questionIds)
        {
            var questionGuids = questionIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(s => Guid.Parse(s)).ToArray();

            using (var connection = new SqlConnection(connectionString))
            {
                var answers = connection.Query<Models.Answer>
                (
                    @"select AnswerId, AnswerText, QuestionId
                        from [Answers] as o
                        where Version = 
                        (
                            select max(i.[Version]) 
                            from [Answers] as i where i.AnswerId = o.AnswerId
                        ) 
                        and QuestionId in @questionIds",
                    param: new
                    {
                        questionIds = questionGuids
                    }

                );

                return answers;
            }
        }

        [HttpPut]
        public IEnumerable<dynamic> Put([FromBody]Models.NewAnswer[] answers)
        {
            var requestId = Request.Headers["composed-request-id"].Single();
            var results = new List<dynamic>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                {
                    foreach (var answer in answers)
                    {
                        connection.Execute
                        (
                            @"insert into [Answers]
                            ([AnswerId]
                            ,[QuestionId]
                            ,[RequestId]
                            ,[Version]
                            ,[AnswerText])
                        values
                            (@AnswerId
                            ,@QuestionId
                            ,@RequestId
                            ,@Version
                            ,@AnswerText)",
                            param: new
                            {
                                answer.AnswerId,
                                answer.QuestionId,
                                RequestId = requestId,
                                Version = DateTimeOffset.Now.Ticks,
                                answer.AnswerText
                            },
                            transaction: tx
                        );

                        results.Add(new
                        {
                            answer.AnswerId,
                            RequestId = requestId
                        });
                    }

                    tx.Commit();
                }
            }

            return results;
        }

        [HttpPost]
        public IEnumerable<dynamic> Post(Models.EditAnswer[] answers)
        {
            var requestId = Request.Headers["composed-request-id"].Single();
            var results = new List<dynamic>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var tx = connection.BeginTransaction())
                {
                    connection.Open();
                    foreach (var answer in answers)
                    {
                        //should validate data already exists

                        connection.Execute
                        (
                            @"insert into [Answers]
                                ([AnswerId]
                                ,[QuestionId]
                                ,[RequestId]
                                ,[Version]
                                ,[AnswerText])
                            values
                                (@AnswerId
                                ,@QuestionId
                                ,@RequestId
                                ,@Version
                                ,@AnswerText)",
                            param: new
                            {
                                answer.AnswerId,
                                answer.QuestionId,
                                RequestId = requestId,
                                Version = DateTimeOffset.Now.Ticks,
                                answer.AnswerText
                            },
                            transaction: tx
                        );

                        results.Add(new
                        {
                            answer.AnswerId,
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