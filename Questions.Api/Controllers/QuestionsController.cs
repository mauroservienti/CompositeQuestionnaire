using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Questions.Api.Controllers
{
    [Route("api/questions")]
    public class QuestionsController : Controller
    {
        readonly string connectionString = null;
        public QuestionsController(IConfiguration configuration)
        {
            connectionString = configuration["connection-string"];
        }

        [HttpGet]
        public IEnumerable<dynamic> Get()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var questions = connection.Query<Models.Question>
                (
                    @"select QuestionId, QuestionText 
                        from [Questions] as o
                        where Version = 
                        (
                            select max(i.[Version]) 
                            from [Questions] as i where i.QuestionId = o.QuestionId
                        )"
                );

                return questions;
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<dynamic> Get(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var question = await connection.QuerySingleAsync<Models.Question>
                (
                    @"select QuestionId, QuestionText 
                        from [Questions] as o
                        where Version = 
                        (
                            select max(i.[Version]) 
                            from [Questions] as i where i.QuestionId = o.QuestionId
                        )
                        and QuestionId = @questionId",
                    param: new
                    {
                        questionId = id
                    }
                );

                return question;
            }
        }

        [HttpPut]
        public dynamic Put([FromBody]Models.NewQuestion question)
        {
            var requestId = Request.Headers["composed-request-id"].Single();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute
                (
                    @"insert into [Questions]
                        ([QuestionId]
                        ,[RequestId]
                        ,[Version]
                        ,[QuestionText])
                    values
                        (@QuestionId
                        ,@RequestId
                        ,@Version
                        ,@QuestionText)",
                    new
                    {
                        question.QuestionId,
                        RequestId = requestId,
                        Version = DateTimeOffset.Now.Ticks,
                        question.QuestionText
                    }
                );
            }

            return new
            {
                question.QuestionId,
                RequestId = requestId
            };
        }

        [HttpPost]
        public dynamic Post(Models.EditQuestion question)
        {
            var requestId = Request.Headers["composed-request-id"].Single();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //should validate data already exists

                connection.Execute
                (
                    @"insert into [Questions]
                        ([QuestionId]
                        ,[RequestId]
                        ,[Version]
                        ,[QuestionText])
                    values
                        (@QuestionId
                        ,@RequestId
                        ,@Version
                        ,@QuestionText)",
                    new
                    {
                        question.QuestionId,
                        RequestId = requestId,
                        Version = DateTimeOffset.Now.Ticks,
                        question.QuestionText
                    }
                );

                return new
                {
                    question.QuestionId,
                    RequestId = requestId
                };
            }
        }
    }
}