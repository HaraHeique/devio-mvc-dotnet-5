using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Text;

namespace DevIO.App.Configurations
{
    public static class LogConfig
    {
        public static void UseLogConfiguration(IOptionsBuilder options, IConfiguration configuration)
        {
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            RegisterKissLogListeners(options, configuration);
        }

        private static void RegisterKissLogListeners(IOptionsBuilder options, IConfiguration configuration)
        {
            var appKissLog = new Application(configuration["KissLog.OrganizationId"], configuration["KissLog.ApplicationId"]);

            var requestLogListener = new RequestLogsApiListener(appKissLog)
            {
                ApiUrl = configuration["KissLog.ApiUrl"]
            };

            // register KissLog.net cloud listener
            options.Listeners.Add(requestLogListener);
        }
    }
}
