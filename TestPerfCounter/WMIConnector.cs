using System.Collections.Generic;
using System.Management;

namespace TestPerfCounter
{
    class WMIConnector
    {
        public List<string> MakeQuery(string wqlQuery)
        {
            WqlObjectQuery woq = new WqlObjectQuery(wqlQuery);
            ManagementObjectSearcher mos = new ManagementObjectSearcher(woq);

            List<string> results = new List<string>();

            foreach (ManagementObject result in mos.Get())
            {
                foreach (PropertyData property in result.Properties)
                {
                    results.Add(property.Value.ToString());
                }
            }

            return results;
        }
    }
}
