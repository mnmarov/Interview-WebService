using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EDIFACT_XML;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {

        public static Stream ToStream(string str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        [TestMethod]
        public void TestEDIFAX()
        {
            string correct_format = "LOC+18+SOL'\r\nLOC+35+SE'\r\nLOC+36+TZ'";

            var test = Program.ParseEDIFACT(ToStream(correct_format));
            if (test.Count == 3)
            {
                Assert.AreEqual(test[1].Number, 35);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestEDIFAXFail()
        {
            string wrong_format = "LOC+kkkk+SOL";
            try
            {
                var test = Program.ParseEDIFACT(ToStream(wrong_format));
            } catch(Exception ex)
            {
                if (!ex.Message.StartsWith("Incorrect number format:"))
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void TestMalformedXML()
        {
            string MissingClosingTag = "<InputDocument>" +
"  <DeclarationList>" +
"    <Declaration Command=\"DEFAULT\" Version=\"5.13\">" +
"      <DeclarationHeader>" +
"        <Jurisdiction>IE</Jurisdiction>" +
"        <CWProcedure>IMPORT</CWProcedure>" +
"        <DeclarationDestination>CUSTOMSWAREIE</DeclarationDestination>" +
"        <DocumentRef>71Q0019681</DocumentRef>" +
"        <SiteID>DUB</SiteID>" +
"        <AccountCode>G0779837</AccountCode>" +
"        <Reference RefCode=\"MWB\">" +
"          <RefText>586133622</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"KEY\">" +
"          <RefText>DUB16049</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"CAR\">" +
"          <RefText>71Q0019681</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"COM\">" +
"          <RefText>71Q0019681</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"SRC\">" +
"          <RefText>ECUS</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"TRV\">" +
"          <RefText>1</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"CAS\">" +
"          <RefText>586133622</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"HWB\">" +
"          <RefText>586133622</RefText>" +
"        </Reference>" +
"        <Reference RefCode=\"UCR\">" +
"          <RefText>586133622</RefText>" +
"        </Reference>" +
"        <Country CodeType=\"NUM\" CountryType=\"Destination\">IE</Country>" +
"        <Country CodeType=\"NUM\" CountryType=\"Dispatch\">CN</Country>" +
" " +
"          </DeclarationHeader>" +
"</DeclarationList>" +
"</InputDocument>";

            try
            {
                var test = Program.ParseReferences(ToStream(MissingClosingTag), new string[] { "MWB", "TRV", "CAR" });
                Assert.Fail();
            }
            catch (System.Xml.XmlException ex)
            {
                if (!ex.Message.StartsWith("The 'Declaration' start tag on"))
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void TestXML()
        {
            string xml1 = "<DeclarationList><Declaration><DeclarationHeader><Reference RefCode=\"1\"><RefText>586133622</RefText></Reference></DeclarationHeader></Declaration></DeclarationList>";
            var test = Program.ParseReferences(ToStream(xml1), new string[] { "1", "2", "3" });
            Assert.AreEqual(test.Count, 1);
            test = Program.ParseReferences(ToStream(xml1), new string[] { "3" });
            Assert.AreEqual(test.Count, 0);
        }

    }
}
