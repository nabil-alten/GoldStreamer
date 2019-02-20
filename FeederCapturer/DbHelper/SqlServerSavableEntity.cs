using System.Collections.Generic;

namespace FeederCapturer.DbHelper
{
    public class SqlServerSavableEntity 
    {
        public SqlServerSavableEntity(string entityName)
        {
            ElementName = entityName;
            Attributes = new Dictionary<string, object>();
            OutputParameters = new List<int>();
        }

        public string ElementName { get; set; }
        public Dictionary<string, object> Attributes { get; private set; }
        public List<int> OutputParameters { get; private set; }
    }
}
