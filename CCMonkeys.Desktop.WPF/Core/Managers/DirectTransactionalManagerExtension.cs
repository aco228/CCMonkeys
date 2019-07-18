using Direct.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMonkeys.Wpf.Desktop.Core.Manager
{
    public class DirectTransactionalManagerExtension
    {
        private DirectDatabaseBase _database = null;
        private List<string> _queries = new List<string>();

        public DirectTransactionalManagerExtension(DirectDatabaseBase db)
        {
            this._database = db;
        }

        public void Execute(string query, params object[] parameters) => Execute(this._database.Construct(query, parameters));
        public void Execute(string command) => this._queries.Add(command);

        public void Start() => this._queries = new List<string>();
        public int? Run()
        {
            if (this._queries.Count == 0)
                return null;

            string mainQuery = "";
            foreach (string query in this._queries)
            {
                string qq = query.Trim();
                mainQuery += qq + (qq.EndsWith(";") ? "" : ";");
            }

            var result = this._database.Execute(mainQuery);
            this._queries = new List<string>();
            return (int?)result.LastID;
        }
    }
}
