namespace SagUnderpinings;

internal static class ILoggerExtentions
{
    public static Task<ILogger> LogWebException(this ILogger logger, IWebHostEnvironment environment, int errCode, string exception)
    {
        logger.LogError(
        null,              //exception,
        "{{\"Code\":{Code},\r\n" +
            "\"Exception\":{Exception}\r\n}}",
        errCode,
        exception
        );
        return Task.FromResult(logger);
    }

}
