using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EDIFACT_XML
{
    public class Location
    {
        public int Number;
        public string Text; 
    }

    public class Reference
    {
        public string Code;
        public string Value;
    }

    public class Program
    {

        public static List<Location> ParseEDIFACT(Stream stream)
        {
            var result = new List<Location>();
            if (stream == null)
                return result;
            StreamReader sr = new StreamReader(stream);
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (line.Trim().ToLower().StartsWith("loc"))
                {
                    var split = line.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (split.Length < 3)
                        throw new Exception("Incorrect format near: '" + line + "'");
                    int num = 0;
                    if (!int.TryParse(split[1], out num))
                        throw new Exception("Incorrect number format: '" + split[1] + "'");
                    // remove the segment terminator from the text
                    var text = split[2].TrimEnd(new char[] { '\'' });
                    result.Add(new Location() { Number = num, Text = text });
                }
            }
            return result;
        }

        /// <summary>
        /// Note: we would be deserializing the whle XML to an object in a real application
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static List<Reference> ParseReferences(Stream stream, string[] lookup_codes)
        {
            var result = new List<Reference>();
            if (stream == null || lookup_codes == null || lookup_codes.Length == 0)
                return result;
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            var nodes = doc.SelectNodes("//Declaration/DeclarationHeader/Reference");
            foreach(XmlElement node in nodes)
            {
                var code = node.GetAttribute("RefCode").Trim();
                if (lookup_codes.Contains(code))
                {
                    result.Add(new Reference() { Code = code, Value = node.InnerText });
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            string FileName = "EDIFACT.txt";

            if (!File.Exists(FileName))
                throw new Exception(FileName + " not found.");

            // Task 1.
            Console.WriteLine("Task 1.");
            var Locations = ParseEDIFACT(File.OpenRead(FileName));
            foreach(var loc in Locations)
            {
                Console.WriteLine("Location: " + loc.Number + " " + loc.Text);
            }

            FileName = "Tracking.xml";
            if (!File.Exists(FileName))
                throw new Exception(FileName + " not found.");

            // Task 2.
            Console.WriteLine("Task 2.");
            var References = ParseReferences(File.OpenRead(FileName), new string[] { "MWB", "TRV", "CAR"});
            foreach (var Ref in References)
            {
                Console.WriteLine("Reference code: " + Ref.Code + " value: " + Ref.Value);
            }

            Console.ReadKey();
        }
    }
}
