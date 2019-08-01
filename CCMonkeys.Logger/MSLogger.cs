using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCMonkeys.Loggings
{
  public class MSLoggerTrack
  {
    public string Name { get; set; } = string.Empty;
    public double TotalMS { get; set; } = 0.0;
    public double MS { get; set; } = 0.0;

  }

  public class MSLogger
  {
    public DateTime Created = DateTime.Now;
    public List<MSLoggerTrack> Tracks = new List<MSLoggerTrack>();


    public void Track(string key)
    {
      double totalMS = (DateTime.Now - this.Created).TotalMilliseconds;
      double ms = totalMS - Tracks.Sum(t => t.MS);
      this.Tracks.Add(new MSLoggerTrack()
      {
        Name = key,
        TotalMS = totalMS,
        MS = ms
      });
    }


  }
}
