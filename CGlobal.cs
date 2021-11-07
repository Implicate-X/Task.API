using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.API
{
    public class CGlobal
    {
        public List<string> Countries { get; private set; } = null;

        public CGlobal()
        {
            Countries = new List<string>();
            Countries.Add("Deutschland");
            Countries.Add("Dänemark");
            Countries.Add("Kanada");
        }

        public void AddCountry(string Name)
        {
            Countries.Add(Name);
        }

        public string GetCountry(int index)
        {
            return Countries[index];
        }
    }
}
