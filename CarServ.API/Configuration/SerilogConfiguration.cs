﻿using Serilog;

namespace CarServ.API.Configuration
{
    public static class SerilogConfiguration
    {
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder loggingBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            return loggingBuilder.AddSerilog(Log.Logger);
        }
    }
}
