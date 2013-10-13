namespace Naru.Log4Net
{
    public interface ILog4NetConfiguration
    {
        string LogDirectoryPath { get; set; }
        string LogFileName { get; set; }
    }
}