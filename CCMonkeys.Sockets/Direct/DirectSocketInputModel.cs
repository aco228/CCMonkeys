using System;
using System.Collections.Generic;
using System.Text;

namespace CCMonkeys.Sockets.Direct
{
  public class DirectSocketInputModel
  {

    public string ticket { get; set; }
    public string query { get; set; }
    public string from { get; set; }
    public string select { get; set; }
    public string where { get; set; }
    public string orderBy { get; set; }
    public string limit { get; set; }

    public string FromQuery
    {
      get
      {
        if (string.IsNullOrEmpty(from))
          throw new Exception("From is empty");
        return " FROM " + this.from;
      }
    }

    public string SelectQuery
    {
      get
      {
        if (string.IsNullOrEmpty(this.select))
          this.select = "*";
        return this.select;
      }
    }

    public string WhereQuery
    {
      get
      {
        if (string.IsNullOrEmpty(this.where))
          return string.Empty;
        return " WHERE " + this.where;
      }
    }

    public string OrderByQuery
    {
      get
      {
        if (string.IsNullOrEmpty(this.orderBy))
          return string.Empty;
        return " ORDER BY " + this.orderBy;
      }
    }

    public string LimitQuery
    {
      get
      {
        if (string.IsNullOrEmpty(this.limit))
          return string.Empty;
        return " LIMIT " + this.limit;
      }
    }

  }
}
