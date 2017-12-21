using Dapper;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using Questions.Messages;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Questions.Api.Handlers
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
                        "delete from [Questions] where RequestId = @RequestId",
                        param: new { message.RequestId }
                    );
            }
        }
    }
}
