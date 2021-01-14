using MT103XUnitTestProject.Common;
using MT103XUnitTestProject.MessageHeader;
using System;
using System.Collections.Generic;
using XB.MT.Parser.Model;
using XB.MT.Parser.Model.Text.MT103;
using XB.MT.Parser.Model.Text.MT103.Fields;
using XB.MT.Parser.Parsers;
using Xunit;

namespace MT103XUnitTestProject.Parser
{
    public class MT103SingleCustomerCreditTransferParserUnitTest : MTParserUnitTest
    {
        [Fact]
        public void TestValidMessage()
        {
            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN1020}";
            MT103SingleCustomerCreditTransferModel mT103Model = MT103SingleCustomerCreditTransferParser.ParseMessage(message);
            BasicHeaderUnitTest.ValidateBasicHeader(mT103Model.BasicHeader, "F", "01", "NKCCXH2NBXXX", "9712", "267472");
            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage, 
                                                  "I", "103", "CD7BNS1A22WC", "N", "1", "020");
        }


        [Fact]
        public void TestValidBasicHeader()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();
            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateBasicHeader", parameters);

            BasicHeaderUnitTest.ValidateBasicHeader(mT103Model.BasicHeader, "F", "01", "NKCCXH2NBXXX", "9712", "267472");
        }

        [Fact]
        public void TestInvalidBasicHeaderMissingStartOfBlockIndicator()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateBasicHeader", parameters);

            Assert.Null(mT103Model.BasicHeader);
        }

        [Fact]
        public void TestInvalidBasicHeaderMissingEndOfBlockIndicator()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateBasicHeader", parameters);

            Assert.Equal("{", mT103Model.BasicHeader.CommonBlockDelimiters.StartOfBlockDelimiter);
            Assert.Equal("1", mT103Model.BasicHeader.CommonBlockDelimiters.BlockIdentifier);
            Assert.Equal(":", mT103Model.BasicHeader.CommonBlockDelimiters.Separator);
            Assert.Null(mT103Model.BasicHeader.AppID);
            Assert.Null(mT103Model.BasicHeader.ServiceID);
            Assert.Null(mT103Model.BasicHeader.LTAddress);
            Assert.Null(mT103Model.BasicHeader.SessionNumber);
            Assert.Null(mT103Model.BasicHeader.SequenceNumber);
            Assert.Null(mT103Model.BasicHeader.CommonBlockDelimiters.EndOfBlockDelimiter); // null!
        }



        [Fact]
        public void TestValidFullApplicationHeaderInputMessage()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN1020}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateApplicationHeaderInputMessage", parameters);

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage, 
                                                  "I", "103", "CD7BNS1A22WC", "N", "1", "020");
        }

        [Fact]
        public void TestValidApplicationHeaderInputMessageMissing1Field()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN1}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateApplicationHeaderInputMessage", parameters);

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage,
                                                  "I", "103", "CD7BNS1A22WC", "N", "1", null);
        }

        [Fact]
        public void TestValidApplicationHeaderInputMessageMissing2Fields()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateApplicationHeaderInputMessage", parameters);

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage,
                                                  "I", "103", "CD7BNS1A22WC", "N", null, null);
        }

        [Fact]
        public void TestValidApplicationHeaderInputMessageMissing3Fields()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WC}";
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateApplicationHeaderInputMessage", parameters);

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage,
                                                  "I", "103", "CD7BNS1A22WC", null, null, null);
        }

        [Fact]
        public void TestValidUserHeader()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}" +
                             "{3:{103:TGT}{113:UNNN}{108:REF0140862562/015}{119:STP}{111:001}" +
                                "{121:6d0de9b0-b69a-4a00-b807-886f5ae9e70d}{423:18071715301204}" +
                                "{106:120811BANKBEBBAXXX2222123456}{424:PQAB1234}{115: 121413 121413 DE BANKDECDA123}" +
                                "{165:/abc/abcdefghijklmnopqrstuvwxyzåäö12345}{433:/AOK/12345678901234567890}" +
                                "{434:/FPO/DLOÄEOPAB76HKÄÄPOCMB}" +
                             "}" // End of block 3
                             ;
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateUserHeader", parameters);

            UserHeaderUnitTest.ValidateUserHeader(mT103Model.UserHeader, 
                              "TGT", "120811BANKBEBBAXXX2222123456", "REF0140862562/015", "001", "UNNN", 
                              " 121413 121413 DE BANKDECDA123", "STP", "6d0de9b0-b69a-4a00-b807-886f5ae9e70d", 
                              "/abc/abcdefghijklmnopqrstuvwxyzåäö12345", "18071715301204", "PQAB1234",
                              "/AOK/12345678901234567890", "/FPO/DLOÄEOPAB76HKÄÄPOCMB");
        }


        [Fact]
        public void TestValidBlockText()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}" +
                             "{3:{103:TGT}{113:UNNN}{108:REF0140862562/015}{119:STP}{111:001}" +
                                "{121:6d0de9b0-b69a-4a00-b807-886f5ae9e70d}{423:18071715301204}" +
                                "{106:120811BANKBEBBAXXX2222123456}{424:PQAB1234}{115: 121413 121413 DE BANKDECDA123}" +
                                "{165:/abc/abcdefghijklmnopqrstuvwxyzåäö12345}{433:/AOK/12345678901234567890}" +
                                "{434:/FPO/DLOÄEOPAB76HKÄÄPOCMB}" +
                             "}" +  // End of block 3
                             "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    //":50K:DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}" + // End of block 4
                             "{5:{CHK:123456789ABC}{TNG:}{PDE:1348120811BANKFRPPAXXX2222123456}{DLM:}" +
                                "{MRF:1806271539180626BANKFRPPAXXX2222123456}{PDM:1213120811BANKFRPPAXXX2222123456}" +
                                "{SYS:1454120811BANKFRPPAXXX2222123456}" +
                             "}";  // End of block 5


            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }

            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123", "SEK", "123", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BMin()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }

            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123", "SEK", "123", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BComma()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123,\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123,", "SEK", "123,", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BComma0()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123,0\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                               "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123,0", "SEK", "123,0", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BComma00()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123,00\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123,00", "SEK", "123,00", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BComma9()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123,9\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123,9", "SEK", "123,9", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidBlockText33BComma99()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{4:\r\n" +
                               ":20:20102900735\r\n" +
                               ":23B:CRED\r\n" +
                               ":23E:INTC\r\n" +
                               ":32A:201029EUR897,01\r\n" +
                               ":33B:SEK123,99\r\n" +
                               ":50K:/DE72512202000051234010\r\n" +
                                    "AUTOMATKUND OCM\r\n" +
                                    "KG1\r\n" +
                                    "106 40  STOCKHOLM\r\n" +
                               ":52A:ESSEDEF0XXX\r\n" +
                               ":59:/SE2850000000054238251849\r\n" +
                                   "MAXFPS AB\r\n" +
                               ":70:INTERN OVERFORING\r\n" +
                               ":71A:SHA\r\n" +
                             "-}"  // End of block 4
                             ;

            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateText", parameters);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText,
                           null, "20102900735", "CRED", "INTC", "201029EUR897,01", "201029", "EUR", "897,01",
                           "SEK123,99", "SEK", "123,99", // 33B
                           "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                           _50_accountCrLF,
                           _50_nameAndAddressCrLfList,
                           "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", null);
        }

        [Fact]
        public void TestValidMessageDOD2()
        {
            string message = "{1:F01ESSEDEF0AXXX3700631882}" +
                             "{2:I103ESSESESSXXXXN3020}" +
                             "{3:{103:TGT}{113:UNNN}{119:STP}{111:001}{121:6d0de9b0-b69a-4a00-b807-886f5ae9e70d}}" +
                             "{4:\r\n" +
                                ":20:20102900735\r\n" +
                                ":23B:CRED\r\n" +
                                ":23E:INTC\r\n" +
                                ":32A:201029EUR897,7\r\n" +
                                ":33B:SEK123,\r\n" +
                                ":50K:/DE72512202000051234010\r\n" +
                                "AUTOMATKUND OCM\r\n" +
                                "KG1\r\n" +
                                "106 40  STOCKHOLM\r\n" +
                                ":52A:ESSEDEF0XXX\r\n" +
                                ":59:/SE2850000000054238251849\r\n" +
                                "MAXFPS AB\r\n" +
                                ":70:INTERN OVERFORING\r\n" +
                                ":71A:SHA\r\n" +
                                ":71F:EUR0,\r\n" +
                             "-}" +
                             "{5:{PDE:}}";

            MT103SingleCustomerCreditTransferModel mT103Model = MT103SingleCustomerCreditTransferParser.ParseMessage(message);

            BasicHeaderUnitTest.ValidateBasicHeader(mT103Model.BasicHeader, "F", "01", "ESSEDEF0AXXX", "3700", "631882");

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage, 
                                                  "I", "103", "ESSESESSXXXX", "N", "3", "020");
            UserHeaderUnitTest.ValidateUserHeader(mT103Model.UserHeader, "TGT", null, null, "001", "UNNN", null, "STP", "6d0de9b0-b69a-4a00-b807-886f5ae9e70d",
                null, null, null, null, null);

            AccountCrLF _50_accountCrLF = new AccountCrLF("/DE72512202000051234010", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "AUTOMATKUND OCM", "KG1", "106 40  STOCKHOLM" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }


            // Block 4
            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText, null, "20102900735", "CRED", "INTC",
                "201029EUR897,7", "201029", "EUR", "897,7",
                "SEK123,", "SEK", "123,",
                "/DE72512202000051234010\r\nAUTOMATKUND OCM\r\nKG1\r\n106 40  STOCKHOLM",
                _50_accountCrLF,
                _50_nameAndAddressCrLfList,
                "ESSEDEF0XXX", "/SE2850000000054238251849\r\nMAXFPS AB", "INTERN OVERFORING", "SHA", "EUR0,");

            // Trailer
            TrailerUnitTest.ValidateTrailer(mT103Model.Trailer, null, null, null, "", null, null, null);
        }


        [Fact]
        public void TestValidMessageDOD()
        {
            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}" +
                             "{3:{103:YYG}{108:5V0OP4RFA66}{119:}{111:}{121:7de11583-e6e8-48b2-b8cd-771a839b7fda}}" +
                             "{4:\r\n" +
                               ":20:cd7z1Lja3\r\n" +
                               ":23B:CRED\r\n" +
                               ":32A:200825SEK3,14\r\n" +
                               ":50K:/SE2880000832790000012345\r\n" +
                                    "Vårgårda \r\n" +
                                    "KromverkLilla Korsgatan 3\r\n" +
                               ":59:/SE3550000000054910000003\r\n" +
                                   "Volvo Personvagnar Ab\r\n" +
                                   "Bernhards Gränd 3, \r\n" +
                                   "418 42 Göteborg\r\n" +
                               ":71A:SHA\r\n" +
                             "-}" +
                             "{5:{CHK:123456789ABC}}";

            MT103SingleCustomerCreditTransferModel mT103Model = MT103SingleCustomerCreditTransferParser.ParseMessage(message);

            BasicHeaderUnitTest.ValidateBasicHeader(mT103Model.BasicHeader, "F", "01", "NKCCXH2NBXXX", "9712", "267472");

            ApplicationHeaderInputMessageUnitTest.ValidateApplicationHeaderInputMessage(mT103Model.ApplicationHeaderInputMessage, "I", "103", "CD7BNS1A22WC", "N", null, null);

            UserHeaderUnitTest.ValidateUserHeader(mT103Model.UserHeader, 
                               "YYG", null, "5V0OP4RFA66", "", null, null, "", "7de11583-e6e8-48b2-b8cd-771a839b7fda", 
                               null, null, null, null, null);

            // Textblock
            AccountCrLF _50_accountCrLF = new AccountCrLF("/SE2880000832790000012345", true);

            List<NameOrAddressCrLf> _50_nameAndAddressCrLfList = new List<NameOrAddressCrLf>();
            string[] adresses = { "Vårgårda ", "KromverkLilla Korsgatan 3" };
            foreach (var adress in adresses)
            {
                _50_nameAndAddressCrLfList.Add(new NameOrAddressCrLf(adress, true));
            }

            ValidateBlock4(mT103Model.MT103SingleCustomerCreditTransferBlockText, null, "cd7z1Lja3", "CRED", null,
                "200825SEK3,14", "200825", "SEK", "3,14", null, null, null,
                "/SE2880000832790000012345\r\nVårgårda \r\nKromverkLilla Korsgatan 3", 
                _50_accountCrLF,
                _50_nameAndAddressCrLfList,
                null,
                "/SE3550000000054910000003\r\nVolvo Personvagnar Ab\r\nBernhards Gränd 3, \r\n418 42 Göteborg", null, "SHA", null);

            // Trailer
            TrailerUnitTest.ValidateTrailer(mT103Model.Trailer, "123456789ABC", null, null, null, null, null, null);
        }





        private void ValidateBlock4(MT103SingleCustomerCreditTransferText block4, string _13C_timeIndication,
                                                                                    string _20_senderReference,
                                                                                    string _23B_bankOperationCode,
                                                                                    string _23E_instructionCode,
                                                                                    string _32A_valueDate_Currency_InterbankSettledAmount,
                                                                                    string _32A_valueDate,
                                                                                    string _32A_currency,
                                                                                    string _32A_interbankSettledAmount,
                                                                                    string _33B_currency_InstructedAmount,
                                                                                    string _33B_currency,
                                                                                    string _33B_instructedAmount,
                                                                                    string _50K_orderingCustomer,
                                                                                    AccountCrLF _50K_accountCrLF,
                                                                                    List<NameOrAddressCrLf> _50K_nameAndAdress,
                                                                                    string _52A_orderingInstitution,
                                                                                    string _59_beneficiaryCustomer,
                                                                                    string _70_remittanceInformation,
                                                                                    string _71A_detailsOfCharges,
                                                                                    string _71f_sendersCharges)
        {
            Assert.NotNull(block4);
            CommonBlockDelimitersUnitTest.ValidateCommonBlockDelimiters(block4.CommonBlockDelimiters, "4");
            if (_13C_timeIndication == null)
            {
                Assert.Null(block4.Field13C);
            }
            else
            {
                TextUnitTest.ValidateField13C(block4.Field13C, _13C_timeIndication);
            }

            if (_20_senderReference == null)
            {
                Assert.Null(block4.Field20);
            }
            else
            {
                TextUnitTest.ValidateField20(block4.Field20, _20_senderReference);
            }

            if (_23B_bankOperationCode == null)
            {
                Assert.Null(block4.Field23B);
            }
            else
            {
                TextUnitTest.ValidateField23B(block4.Field23B, _23B_bankOperationCode);
            }

            if (_23E_instructionCode == null)
            {
                Assert.Null(block4.Field23E);
            }
            else
            {
                TextUnitTest.ValidateField23E(block4.Field23E, _23E_instructionCode);
            }

            if (_32A_valueDate_Currency_InterbankSettledAmount == null)
            {
                Assert.Null(block4.Field32A);
            }
            else
            {
                TextUnitTest.ValidateField32A(block4.Field32A, _32A_valueDate_Currency_InterbankSettledAmount, 
                                                  _32A_valueDate, _32A_currency, _32A_interbankSettledAmount);
            }

            if (_33B_currency_InstructedAmount == null)
            {
                Assert.Null(block4.Field33B);
            }
            else
            {
                TextUnitTest.ValidateField33B(block4.Field33B, _33B_currency_InstructedAmount, _33B_currency, _33B_instructedAmount);
            }

            if (_50K_orderingCustomer == null)
            {
                Assert.Null(block4.Field50K);
            }
            else
            {
                TextUnitTest.ValidateField50K(block4.Field50K, _50K_orderingCustomer, _50K_accountCrLF, _50K_nameAndAdress);
            }

            if (_52A_orderingInstitution == null)
            {
                Assert.Null(block4.Field52A);
            }
            else
            {
                TextUnitTest.ValidateField52A(block4.Field52A, _52A_orderingInstitution);
            }

            if (_59_beneficiaryCustomer == null)
            {
                Assert.Null(block4.Field59);
            }
            else
            {
                TextUnitTest.ValidateField59(block4.Field59, _59_beneficiaryCustomer);
            }

            if (_70_remittanceInformation == null)
            {
                Assert.Null(block4.Field70);
            }
            else
            {
                TextUnitTest.ValidateField70(block4.Field70, _70_remittanceInformation);
            }

            if (_71A_detailsOfCharges == null)
            {
                Assert.Null(block4.Field71A);
            }
            else
            {
                TextUnitTest.ValidateField71A(block4.Field71A, _71A_detailsOfCharges);
            }

            if (_71f_sendersCharges == null)
            {
                Assert.Null(block4.Field71F);
            }
            else
            {
                TextUnitTest.ValidateField71F(block4.Field71F, _71f_sendersCharges);
            }
        }

        [Fact]
        public void TestValidTrailer()
        {
            MT103SingleCustomerCreditTransferModel mT103Model = new MT103SingleCustomerCreditTransferModel();

            string message = "{1:F01NKCCXH2NBXXX9712267472}{2:I103CD7BNS1A22WCN}" +
                             "{3:{103:TGT}{113:UNNN}{108:REF0140862562/015}{119:STP}{111:001}" +
                                "{121:6d0de9b0-b69a-4a00-b807-886f5ae9e70d}{423:18071715301204}" +
                                "{106:120811BANKBEBBAXXX2222123456}{424:PQAB1234}{115: 121413 121413 DE BANKDECDA123}" +
                                "{165:/abc/abcdefghijklmnopqrstuvwxyzåäö12345}{433:/AOK/12345678901234567890}" +
                                "{434:/FPO/DLOÄEOPAB76HKÄÄPOCMB}" +
                             "}" +  // End of block 3
                             "{5:{CHK:123456789ABC}{TNG:}{PDE:1348120811BANKFRPPAXXX2222123456}{DLM:}" +
                                "{MRF:1806271539180626BANKFRPPAXXX2222123456}{PDM:1213120811BANKFRPPAXXX2222123456}" +
                                "{SYS:1454120811BANKFRPPAXXX2222123456}" +
                             "}"  // End of block 5
                             ;
            object[] parameters = new object[2];
            parameters[0] = mT103Model;
            parameters[1] = message;

            Type type = typeof(MT103SingleCustomerCreditTransferParser);
            var result = ReflectionCall(type, "PopulateTrailer", parameters);

            TrailerUnitTest.ValidateTrailer(mT103Model.Trailer, "123456789ABC", "", "1806271539180626BANKFRPPAXXX2222123456",
                "1348120811BANKFRPPAXXX2222123456", "1213120811BANKFRPPAXXX2222123456", "1454120811BANKFRPPAXXX2222123456", "");
        }
    }
}
