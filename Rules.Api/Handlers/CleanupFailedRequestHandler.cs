using Dapper;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using Rules.Messages;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rules.Api.Handlers
{
    public class CleanupFailedRequestHandler : IHandleMessages<CleanupFailedRequest>
    {
        readonly string connectionString = null;
        public CleanupFailedRequestHandler(IConfiguration configuration)
        {
            connectionString = configuration["connection-string"];
        }

        public async Task Handle(CleanupFailedRequest message, IMessageHandlerContext context)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var affectedrows = await connection.ExecuteAsync(
                        "delete from [CorrectAnswerRules] where RequestId = @RequestId",
                        param: new { message.RequestId }
                    );
            }
        }
    }
}
