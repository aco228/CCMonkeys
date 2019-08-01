
namespace CCMonkeys.Loggings
{
  public class Logger
  {
    private static LoggingBase _instance = null;
    public static LoggingBase Instance
    {
      get
      {
        if (_instance != null)
          return _instance;
        _instance = new LoggingBase();
        return _instance;
      }
    }
  }
}
