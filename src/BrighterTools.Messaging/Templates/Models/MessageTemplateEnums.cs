namespace BrighterTools.Messaging.Templates.Models;

public enum MessageTemplateChannel
{
    Email = 1,
    Sms = 2
}

public enum MessageTemplateScope
{
    LibraryDefault = 0,
    Host = 1,
    Tenant = 2
}

public enum MessageTemplateSourceFormat
{
    Html = 0,
    ReactEmailEditor = 1
}