using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Microsoft.Win32;

namespace ForeclosureApp
{
    
    //Parser Class
    #region Parser Class    
    class Parser
    {
        // constructor
        public Parser(ref Cleanup p, ref Parser_DailyReport q)
        {
            p_Cleanup = p;
            p_DailyReport = q;
        }
        
        // private data
        #region private data
        Cleanup p_Cleanup;
        Parser_DailyReport p_DailyReport;
        #endregion

        //public properties
        #region Public Properties

        #endregion

        // WIDE Parse Condtions
        #region WIDE Parse Conditions
        // All Wide Parse Conditions - if you make changes below alse make them here
        public string[] COMMON_Get_Wide_ParseConditions()
        {
            return (new string[] { "COMMON_KNOWN_AS", "COMMON_ENCUMBERED_PR", "COMMON_IS_LOCATED", "COMMON_PR_ADDRESSOF" });
        }

        // Broad Regular Expressions - meant to catch all possible property scenarios
        private string COMMON_Parse_RegularExpressionsWide(string ParseWideParseName)
        {
            switch (ParseWideParseName)
            {
                case "COMMON_KNOWN_AS":
                    return (@"[Kk][Nn][Oo][Ww][Nn]\s*([Aa][Ll][Tt][Ee][Rr][Nn][Aa][Tt][Ee][Ll][Yy])?\s*[Aa][Ss]\s*([Nn][Oo]\.?)?\s*:?\s*(<[Bb]>)?(</[Bb]>)?\s*");
                case "COMMON_ENCUMBERED_PR":
                    return (@"[Ee][Nn][Cc][Uu][Mm][Bb][Ee][Rr][Ee][Dd]\s*[Pp][Rr][Oo][Pp][Ee][Rr][Tt][Yy]\s*:?\s*(<[Bb]>)?(</[Bb]>)?\s*");
                case "COMMON_IS_LOCATED":
                    return (@"[Ii][Ss]\s*[Ll][Oo][Cc][Aa][Tt][Ee][Dd]\s*:?\s*([Aa][Tt])?\s*:?\s*(<[Bb]>)?(</[Bb]>)?\s*");
                case "COMMON_PR_ADDRESSOF":
                    return (@"([Pp][Rr][Oo][Pp][Ee][Rr][Tt][Yy])*\s*[Aa][Dd][Rr][Ee][Ss][Ss]\s*:?\s*([Oo][Ff])?\s*:?\s*(<[Bb]>)?(</[Bb]>)?\s*");
                default:
                    return ("");
            }
        }
        #endregion

        // Narrowing Down Parse Conditions
        #region Narrowing Down Parse Conditions
        // All Narrowing Down Parse Conditions if you make changes below alse make them here
        public string[] COMMON_Get_NarrowingDown_ParseConditions()
        {
            return (new string[] { "ZipCode", "Georgia", "GA", "no Zip or State" });
        }

        // These Regular Expressions are used in conjuction (added with the WIDE ones), to
        // cover full range of possible property scenarious
        private string COMMON_Parse_RegularExpressionsNarrowingDown(string NarrowingDownParseName)
        {
            switch (NarrowingDownParseName)
            {
                case "ZipCode":
                    return (@"(?<compl>\d{2,4}-?\d*\s+.{10,100}\s+\d{5,5})");
                case "Georgia":
                    return (@"(?<compl>\d{2,4}-?\d*\s+.{10,100}\s+,?[Gg][Ee][Oo][Rr][Gg][Ii][Aa])");
                case "GA":
                    return (@"(?<compl>\d{2,4}-?\d*\s+.{10,100}\s+,?[Gg][Aa])");
                case "no Zip or State":
                    return (@"(?<compl>\d{2,4}-?\d*\s+.{10,150})\.");

                default:
                    return ("");
            }
        }

        // Very specific Regular Expressions used to get as close as possible - Also Make Changes Here!
        public Regex COMMON_Parse_RegularExpressionsVerySpecific(string NarrowingDownCondition)
        {
            switch (NarrowingDownCondition)
            {
                case "ZipCode":
                    return (new Regex(@"(?<addr>\d+-?\d*\s+.+?),.+,?\s(?<zip>\d{5,5})"));
                case "Georgia":
                    // a very broad match, we rely entirely on cleanup :(
                    return (new Regex(@"(?<addr>\d+-?\d*\s+.+?),(?<city>.+)"));
                case "GA":
                    return (new Regex(@"(?<addr>\d+-?\d*\s+.+?),(?<city>.+?),.*"));
                case "no Zip or State":
                    // a very broad match, we rely entirely on cleanup :( - can't even pull out city!
                    return (new Regex(@"(?<addr>\d+-?\d*\s+.+)"));
                    //return (new Regex(@"(?<addr>\d+-?\d*\s+.+?),(?<city>.+?),.*"));

                default:
                    return (new Regex(""));
            }
        }

        // used ONLY to actualy now finally get the actual values - IMP
        public void ParsePropertyDetail(ref Match tmatch, string WideCondition, string NarrowCondition, ref PropertyDetails pd)
        {
            // property details
            pd.isApt = false;
            pd.City = "";
            pd.State = "Georgia";
            pd.Address = "";
            pd.AptNo = "";
            pd.ZipCode = 0;
            pd.SQLlink = 0;

            // All of them have an address field
            pd.Address = p_Cleanup.AddressCleanup(tmatch.Groups["addr"].ToString());

            // 1) ZipCode Specific Parsing
            if((NarrowCondition == "ZipCode") && (WideCondition == "COMMON_KNOWN_AS" || WideCondition == "COMMON_IS_LOCATED"))
            {
                if (p_DailyReport.isValidStr(tmatch.Groups["zip"]))
                {
                    pd.ZipCode = int.Parse(tmatch.Groups["zip"].ToString());
                }                       
            }
            // 2) 3) 4) Most others fall in here
            else if(((NarrowCondition == "Georgia") || (NarrowCondition == "GA") || (NarrowCondition == "no Zip or State")) &&
                    (WideCondition == "COMMON_KNOWN_AS"))
            {
                // exception state
                if (NarrowCondition == "no Zip or State")
                {
                    // Can't get city out + need to manually get out the Address Info
                    p_Cleanup.isRemovedEverythingAfterStreetName(ref pd.Address);
                }
                else
                {
                    pd.City = p_Cleanup.CityCleanup(tmatch.Groups["city"].ToString(), 5, true);
                }                               
            }
            // 2) exception a few fall in here
            else if(((NarrowCondition == "Georgia")) &&
                    ((WideCondition == "COMMON_IS_LOCATED") || (WideCondition == "COMMON_ENCUMBERED_PR") ))
            {
               pd.City = p_Cleanup.CityCleanup(tmatch.Groups["city"].ToString(), 5, true);
            }

            // All of them try to assign Apartments
            string tno;

            // Some of them could have city info in the city, so check city first 
            // (only in Georgia Condition) - that is where it actually occurs
            //if (NarrowCondition == "Georgia" && pd.City != "")
            //{
            //    pd.isApt = p_Cleanup.isApartment(tmatch.Groups["city"].ToString(), out tno);
            //}            
            // Otherwise just check the address and assign it if neccessary
            //if (!(pd.isApt) && (pd.isApt = p_Cleanup.isApartment(pd.Address, out tno)) && (tno != ""))
            //{
            //    pd.AptNo = tno;
            //}

        }
        #endregion

        //Main Parse Logic
        #region Main Parse Logic
        public bool Iterate_Over_COMMON_Parse_Functions(ref string desc, ref string found, ref PropertyDetails pd)
        {

            // Execute all Parsing!
            foreach (string usingWideType in COMMON_Get_Wide_ParseConditions())
            {
                if (COMMON_Parse_Functions(usingWideType, ref desc, ref found, ref pd))
                {
                    return (true);
                }
            }

            return (false);
        }

        private bool COMMON_Parse_Functions(string WIDE_PARSE_NAME, ref string desc, ref string found, ref PropertyDetails pd)
        {

            // temp
            Regex Widerx;
            Match Widematch;
            Regex specificrx;
            Match specificmatch;
            int it = 1;

            // Try Narrowing Down the Wide Search into Specific Types
            foreach (string usingNarrowingDownType in COMMON_Get_NarrowingDown_ParseConditions())
            {

                Widerx = new Regex(COMMON_Parse_RegularExpressionsWide(WIDE_PARSE_NAME) + COMMON_Parse_RegularExpressionsNarrowingDown(usingNarrowingDownType));
                Widematch = Widerx.Match(p_DailyReport.CURRENT_PROPERTY_PAGE);
                if (Widematch.Groups.Count > 1)
                {
                    // description is important
                    desc = it.ToString() + ") " + usingNarrowingDownType + " - " + WIDE_PARSE_NAME;
                    found = Widematch.Groups[0].ToString();

                    // very good but not perfect match 
                    // (a few inconsistencies remain):
                    specificrx = COMMON_Parse_RegularExpressionsVerySpecific(usingNarrowingDownType);
                    specificmatch = specificrx.Match(Widematch.Groups["compl"].ToString());

                    // If Found using Wide + Narrowing down - we now need to match Specific and 
                    // try getting the data
                    ParsePropertyDetail(ref Widematch, WIDE_PARSE_NAME, usingNarrowingDownType, ref pd);

                    return (true);
                }
                ++it;
            }

            return (false);
        }
        #endregion       
    }
    #endregion

    //Cleanup Class
    #region Cleanup Class
    class Cleanup
    {
        private Parser_DailyReport p_DailyReport;

        public Cleanup(ref Parser_DailyReport p)
        {
            p_DailyReport = p;
        }
        
        public String AddressCleanup(string AddressToCleanup)
        {

            // Get rid of Brackets! for example:
            // 123 Main street (formerly Beverly street)
            int i = AddressToCleanup.IndexOf('(');
            if (i > 0)
            {
                bool bHasNoLetter = true;
                int j = 0;

                for (; j < i; ++j)
                {
                    if (char.IsLetter(AddressToCleanup[j]))
                    {
                        bHasNoLetter = false;
                        break;
                    }
                }

                if (bHasNoLetter)
                {
                    int k = AddressToCleanup.IndexOf(')');
                    AddressToCleanup = AddressToCleanup.Substring(0, i - 1) + AddressToCleanup.Substring(k + 1, (AddressToCleanup.Length - k - i));

                }
                else
                {
                    AddressToCleanup = AddressToCleanup.Substring(0, i);
                }
            }

            // Get rid of .<br>
            i = AddressToCleanup.ToUpper().IndexOf(".<BR>");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            // Get rid of </b>
            i = AddressToCleanup.ToUpper().IndexOf("</B>");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }
            
            // Get rid of </b>
            i = AddressToCleanup.ToUpper().IndexOf("</BR>");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            // Get rid of AKA
            i = AddressToCleanup.ToUpper().IndexOf("A/K/A");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }
            
            // Get rid of FKA
            i = AddressToCleanup.ToUpper().IndexOf("FKA");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            // Get rid of NKA
            i = AddressToCleanup.ToUpper().IndexOf("NKA");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            // Get rid of NKA
            i = AddressToCleanup.ToUpper().IndexOf(" ACCORDING ");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            // Get rid of NKA
            i = AddressToCleanup.ToUpper().IndexOf(" PER ");
            if (i > 0)
            {
                AddressToCleanup = AddressToCleanup.Substring(0, i);
            }

            return (AddressToCleanup);
        }

        public bool isApartment(string toCheck, out string AptNo)
        {
            // temp
            Regex temprx = new Regex("");  // assigning it a default value so the compiler
            // won't complain
            Match tmatch;
            AptNo = "";
            bool isApt = false;

            if (toCheck.Contains("#"))
            {
                isApt = true;
                //temprx = new Regex(@"\s*#\s*(?<aptno>\d{2,4})(?<suffix>\w*)\s");
            }
            else if (toCheck.ToUpper().Contains("APT"))
            {
                isApt = true;
                //temprx = new Regex(@"\s*[Aa][Pp][Tt].?\s*(?<aptno>\d{2,4})(?<suffix>\w*)\s");
            }
            else if (toCheck.ToUpper().Contains("UNIT"))
            {
                isApt = true;
                //temprx = new Regex(@"\s*[Uu][Nn][Ii][Tt].+?\s*(?<aptno>\d{2,4})(?<suffix>\w*)\s)");
            }
            else if (toCheck.ToUpper().Contains("SUITE"))
            {
                isApt = true;
                //temprx = new Regex(@"\s*[Ss][Uu][Ii][Tt][Ee].+?\s*(?<aptno>\d{2,4})(?<suffix>\w*)\s)");
            }
            else
            {
                // If it contains a number at the beginning, a
                // string of chars in between and a digit at the end
                // it is an apartment - this mimiks a regular expression
                bool bBeginningNumber = false;
                bool bMiddleString = false;
                bool bEndNumber = false;

                foreach (char c in toCheck)
                {
                    if (char.IsDigit(c) && !bMiddleString)
                    {
                        bBeginningNumber = true;
                    }
                    else if (char.IsLetter(c) && bBeginningNumber)
                    {
                        bMiddleString = true;
                    }
                    else if (char.IsDigit(c) && bMiddleString)
                    {
                        bEndNumber = true;
                    }
                }

                if (bEndNumber)
                {
                    isApt = true;
                    //temprx = new Regex(@"\d{2,4}-?\d*\s+.{10,100}\s*(?<aptno>\d{2,4})(?<suffix>\w*)\s)");
                }
            }

            // If it is an apartment try parsing out the Apartment Number!
            if (isApt)
            {
                tmatch = temprx.Match(toCheck);
                if (p_DailyReport.isValidStr(tmatch.Groups["aptno"]))
                {
                    int tint;
                    if (int.TryParse(tmatch.Groups["aptno"].ToString(), out tint))
                    {
                        AptNo += (tint.ToString() + tmatch.Groups["suffix"].ToString());
                    }
                }
            }

            return (isApt);
        }

        public string GetOutCounty(string stringThatMayContainCounty, out bool bFoundCounty)
        {
            // Only match words that are on their own not part of another word *Sadly could contain space or
            // coma at end; don't want to turn to RE just for this
            string[] countyNames = new String[]
                {" Fulton "," Fulton,"," Fulton", 
                " East Point ", " East Point,"," East Point",
                " Roswell "," Roswell,"," Roswell",
                " Alpharetta "," Alpharetta,"," Alpharetta",
                " Hapeville "," Hapeville,"," Hapeville",
                " College Park "," College Park,"," College Park",
                " Riverdale "," Riverdale,"," Riverdale",
                " Palmetto "," Palmetto,"," Palmetto",
                " Cobb "," Cobb,"," Cobb",
                " Marrieta ", " Marrieta,"," Marrieta",
                " Dunwoody "," Dunwoody,"," Dunwoody",
                " Duluth "," Duluth,"," Duluth",
                " Clayton "," Clayton,"," Clayton",
                " Union City ", " Union City,"," Union City",
                " Fairburn " , " Fairburn,"," Fairburn"};

            foreach (string str in countyNames)
            {                
                
                if (stringThatMayContainCounty.ToUpper().Contains(str.ToUpper()))                    
                {
                    string retS = str;

                    if (str == " Fulton " || str == " Fulton," || str == " Fulton")
                    {
                        retS = " Atlanta ";
                    }
                    
                    // stripe spaces before sending out                    
                    retS = retS.TrimStart(' ');
                    retS = retS.TrimEnd(' ');
                    if (retS[retS.Length - 1] == ',')
                    {
                        retS = retS.TrimEnd(',');
                    }
                    bFoundCounty = true;
                    return (retS);
                }
            }

            bFoundCounty = false;
            return ("");
        }

        public bool isRemovedEverythingAfterStreetName(ref String strMaybeTrimmed)
        {
            string[] StreetNames = new String[]
                {"Avenue", "Ave.", "Ave",
                 "Road", 
                 "Street",
                 "Place", 
                 "Court",
                 "Lane",
                 "Drive",
                 "Terrace",
                 "Blvd"                    
                };

            foreach (string str in StreetNames)
            {
                if (strMaybeTrimmed.ToUpper().Contains(str.ToUpper()))
                {
                    int i = strMaybeTrimmed.ToUpper().IndexOf(str.ToUpper());
                    strMaybeTrimmed = strMaybeTrimmed.Substring(0, i + str.Length);
                    return (true);
                }
            }

            return (false);
        }

        public String CityCleanup(string CityToCleanup, int iSmallerThenSetToAtlanta, bool bCountyCleanup)
        {
            string tno;

            // Check if it is Apartment!         ---- have to check why i wanted to do this!
            //if (isApartment(CityToCleanup, out tno))
            //{
            //    return ("Atlanta");
            //}
            if ((iSmallerThenSetToAtlanta >= 1) && (CityToCleanup.Length <= iSmallerThenSetToAtlanta))
            {
                return ("Atlanta");
            }
            else if (CityToCleanup.Contains("NE") || CityToCleanup.Contains("NW") ||
               CityToCleanup.Contains("N.W.") || CityToCleanup.Contains("N.E.") ||
               CityToCleanup.Contains("SE") || CityToCleanup.Contains("SW") ||
               CityToCleanup.Contains("S.E.") || CityToCleanup.Contains("S.W.")
            )
            {
                return ("Atlanta");
            }
            else if (CityToCleanup.ToUpper().Contains("ATLANTA"))
            {
                return ("Atlanta");
            }
            else if(bCountyCleanup)
            {
                bool bs; 
                string county = GetOutCounty(CityToCleanup, out bs);
                if (county != "")
                {
                    return (county);
                }
                else
                {
                    // Perfect the way it is
                    return (CityToCleanup);
                }
            }            
            //else if (CityToCleanup.ToUpper().Contains("CITY OF")) 
            //{
            //    int i = CityToCleanup.ToUpper().IndexOf("CITY OF");
            //    // ignore city of and space
            //    return (CityToCleanup.Substring(i + 8, (CityToCleanup.Length - i - 8)));
            //}
            //else if (CityToCleanup.ToUpper().Contains("TOWN OF"))
            //{
            //    int i = CityToCleanup.ToUpper().IndexOf("TOWN OF");
            //    // ignore city of and space
            //    return (CityToCleanup.Substring(i + 8, (CityToCleanup.Length - i - 8)));
            //}
            else
            {
                // Don't modify it, it is perfect as it is
                return (CityToCleanup);
            }
        }
    }
    #endregion

    class Parser_DailyReport
    {
        public const int SLEEP_TIME_OUT_AFTER_EVERY_PROPERTY = 100;
        public const int SLEEP_TIME_OUT_AFTER_EVERY_INDEX = 1000;
        public const string INDEX_URL_TO_PARSE = "http://www.dailyreportonline.com/Public_Notice/Legal_Notices/listLN.asp?userSel=3010200All&search_keywords=&cur_Page=";
        public const string PR_PARSE_URL = "http://www.dailyreportonline.com/Public_Notice/Legal_Notices/singleLN.asp"; // singleLN.asp?userSel=3010200All&individual_SQL=213397&search_Keywords=&cur_Page=12";
        
        // Start Page = URL_PAGE_START_INDEX_DEFAULT
        public const int URL_PAGE_START_INDEX_DEFAULT = 1;
        // keeping track of no. of pages currently being parsed  
        private int URL_PAGE_COUNTER = URL_PAGE_START_INDEX_DEFAULT;
        private string PROPERTY_SELECTED = "";

        // important state html variables
        private string CURRENT_INDEX_PAGE = "";
        public string CURRENT_PROPERTY_PAGE = "";

        // counters
        private int successfully_parsed = 0;
        private int unsuccessfully_parsed = 0;

        // private flags
        private bool bLastPageReached = false;
        private bool bIsFirstTimeCalled = true;
        private bool bForceStop = false;
        private bool bIsRunning = false;
        private bool bGetIndexCalled = false;

        // private data
        private RegistryKey RegKey;
        private DateTime DateLast; // used to keep track of last parsed date
        private string Base_Path = "";

        // Objects Pointers
        object cs_DRLock;
        Parser p_Parser;
        Cleanup p_Cleanup;

        // Constructor
        public Parser_DailyReport(string friendly_name, string base_dir_path)
        {
            RegKey = Registry.CurrentUser.CreateSubKey("Software\\ForeclosureApp\\" + friendly_name, RegistryKeyPermissionCheck.ReadWriteSubTree);
            Base_Path = base_dir_path + "\\" + friendly_name;
            cs_DRLock = new Object();

            // sending out pointers to this object
            Parser_DailyReport dr = this; 
            p_Cleanup = new Cleanup(ref dr);
            p_Parser = new Parser(ref p_Cleanup, ref dr);
        }

        // public methods
        #region Public Methods
        public bool GetIndexPage()
        {
            if (!bLastPageReached)
            {
                try
                {
                    GetIndexImpl();
                }
                catch (Exception msg)
                {
                    // Pass exceptions out to caller
                    throw (new Exception(msg.Message));
                } 
            }
            else
            {
                
               // We Successfully parsed everything - no need to parse before this date again                            
               RegKey.SetValue("LastParsedDateCompletelyParsed", new Byte[] { 1 }, RegistryValueKind.Binary);
                                   
               return (false);
            }
            
            return (true);
        }

        public bool GetIndexPage(int StartIndex)
        {
            if (StartIndex < URL_PAGE_START_INDEX_DEFAULT)
            {
                return (false);
            }            
            else
            {
                if(StartIndex > URL_PAGE_COUNTER)
                {
                   URL_PAGE_COUNTER = StartIndex;                
                }
                return (GetIndexPage());
            }            
        }

        public bool Parse()
        {
            if (CURRENT_INDEX_PAGE == "")
            {
                return (false);
            }

            bool isIndexParsed = false;

            try
            {
                isIndexParsed = ParseImpl();                 
            }
            catch (Exception msg)
            {
                // Pass exceptions out to caller
                throw (new Exception(msg.Message));
            }

            return (isIndexParsed);
        }

        public Regex GetRESubExpression(string NarrowDownCondition)
        {
            return (p_Parser.COMMON_Parse_RegularExpressionsVerySpecific(NarrowDownCondition));
        }
        #endregion
     
        // public properties 
        #region Public Properties
        public Cleanup get_Cleanup_pointer
        {
            get
            {
                return (p_Cleanup);
            }
        }

        public Parser get_Parser_pointer
        {
            get
            {
                return (p_Parser);
            }
        }

        public bool LastPageReached
        {
            get
            {
                return (bLastPageReached);
            }
        }

        public bool Stop
        {
            set
            {
                if (bIsRunning)
                {
                    lock (cs_DRLock)
                    {
                        bForceStop = value;                       
                    }                    
                }
                if (value == true)
                {
                    // must reset here so that we 
                    // can start again
                    bGetIndexCalled = false;
                }
            }
        }

        public bool IsStopped
        {
            get
            {
                return (!bIsRunning);
            }
        }

        public int CurrentPage
        {
            get
            {                
                return (URL_PAGE_COUNTER);                
            }
        }

        public string CurrentProperty
        {
            get
            {
                return (PROPERTY_SELECTED);
            }
        }

        public int SuccessfullyParsed
        {
            get
            {
                return (successfully_parsed);
            }
        }

        public int UnsuccessfullyParsed
        {
            get
            {
                return (unsuccessfully_parsed);
            }
        }
        #endregion

        // private methods
        #region Private Methods
        private void GetIndexImpl()
        {
            
            // If it is NOT the first time GetIndex is called
            // increment the counter
            if (bGetIndexCalled)
            {
                ++URL_PAGE_COUNTER;
            }
            else
            {
                bGetIndexCalled = true;
            }

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(INDEX_URL_TO_PARSE + URL_PAGE_COUNTER.ToString());
            webrequest.AllowAutoRedirect = false;

            // Get Page
            HttpWebResponse webresponse;
            webresponse = (HttpWebResponse)webrequest.GetResponse();

            StreamReader sr = new StreamReader(webresponse.GetResponseStream(), (System.Text.Encoding.GetEncoding(1252)));
            CURRENT_INDEX_PAGE = sr.ReadToEnd();

            int iLeft, iRight, iMax;
            bool bLast;

            if (isPageCorrect(out bLast, out iLeft, out iRight, out iMax))
            {
                if (bLast)
                {
                    // return false for next call                        
                    bLastPageReached = true;
                }

            }
            else
            {
                throw (new Exception("error while parsing index page"));
            }
        }

        private bool ParseImpl()
        {
            MatchCollection matches;
            DateTime currentDate;
            String link;

            // Retrieve all properties on each index
            if (retrieveMatchesFromIndex(out matches))
            {
                // Iterate thru each property
                foreach (Match match in matches)
                {
                    if (!bIsRunning)
                    {
                        bIsRunning = true;
                    }

                    if (bForceStop)
                    {
                        bForceStop = false;
                        bIsRunning = false;
                        return (false);
                    }

                    // Get the Date and link of each property
                    if (isMatchCorrect(match, out currentDate, out link))
                    {

                        if (CheckDateTimeWithRegistry(ref currentDate))
                        {

                            GetPropertyPage(link);

                            string desc = "";
                            string found = "";
                            PropertyDetails pd = new PropertyDetails();

                            if (p_Parser.Iterate_Over_COMMON_Parse_Functions(ref desc, ref found, ref pd))
                            {
                                FileAppend((desc + ".txt"), found, false);
                                ++successfully_parsed;
                            }
                            else
                            {
                                // We want to write out only changed Dates into the HTML
                                WriteNotParsedPropertyToFile(HasDateTimeChanged(ref currentDate), ref currentDate, link);
                                ++unsuccessfully_parsed;
                            }

                            System.Threading.Thread.Sleep(SLEEP_TIME_OUT_AFTER_EVERY_PROPERTY);
                        }
                        else
                        {
                            // we are done NO MORE PARSING IS REQUIRED // exit here SOMEHOW
                            // bDone = true;                              
                            return (false);
                        }
                    }
                }

            }
            return (false);
        }

        private void GetPropertyPage(string propertySQLKey)
        {
            PROPERTY_SELECTED = propertySQLKey;

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(PR_PARSE_URL + "?individual_SQL=" + propertySQLKey);
            webrequest.AllowAutoRedirect = false;

            HttpWebResponse webresponse;
            webresponse = (HttpWebResponse)webrequest.GetResponse();

            StreamReader sr = new StreamReader(webresponse.GetResponseStream(), (System.Text.Encoding.GetEncoding(1252)));
            CURRENT_PROPERTY_PAGE = sr.ReadToEnd();
        }

        private void FileAppend(string FileName, string Line, bool addLineBreak)
        {
            StreamWriter sw;
            sw = File.AppendText(Base_Path + "\\" + FileName);
            sw.WriteLine(Line);
            if (addLineBreak) sw.WriteLine("");
            sw.Flush();
            sw.Close();
        }

        private void WriteNotParsedPropertyToFile(bool bDateChanged, ref DateTime currentDate, string propertyLink)
        {

            if (bIsFirstTimeCalled || bDateChanged)
            {
                bIsFirstTimeCalled = false;
                FileAppend(("NOT_PARSED\\" + "Left.htm"), ("<br><b>" + currentDate.Date.ToShortDateString() + "</b><br>"), false);
            }

            // Write out the link to the property
            FileAppend(("NOT_PARSED\\" + "Left.htm"), ("<a href=\"" + PR_PARSE_URL + "?individual_SQL=" + propertyLink + "\">" + propertyLink + "</a><br>"), false);
        }

        private bool HasDateTimeChanged(ref DateTime currentDate)
        {
           
           // Is DateLast Uninitialized?
           if (DateLast.Year == 1)
           {
                DateLast = currentDate;
           }
           else if (DateLast != currentDate)
           {
               DateLast = currentDate;
               return (true);
           }

           return (false);
        }

        private bool CheckDateTimeWithRegistry(ref DateTime currentDate)
        {

            // Get the Last Parsed Date
            object oLastParsedDate = RegKey.GetValue("LastParsedDate");
            if (oLastParsedDate != null)
            {

                DateTime lastDate = DateTime.FromBinary(BitConverter.ToInt64(((Byte[])oLastParsedDate), 0));

                // Only keep track of the most recent Date!!
                if (currentDate > lastDate)
                {
                    RegKey.SetValue("LastParsedDate", BitConverter.GetBytes(currentDate.ToBinary()), RegistryValueKind.Binary);
                    RegKey.SetValue("LastParsedDateCompletelyParsed", new Byte[] { 0 }, RegistryValueKind.Binary);                    
                }
                else
                {
                    object completelyParsed = RegKey.GetValue("LastParsedDateCompletelyParsed");
                    if (completelyParsed != null && (BitConverter.ToBoolean((Byte[])completelyParsed, 0)))
                    {                        
                        // The only time we don't want to parse if the date is already in the registry
                        // AND it is already marked as being completely parsed
                        return (false);
                    }
                }
            }
            else
            {    
                RegKey.SetValue("LastParsedDate", BitConverter.GetBytes(currentDate.ToBinary()), RegistryValueKind.Binary);                                
                RegKey.SetValue("LastParsedDateCompletelyParsed", new Byte[] { 0 }, RegistryValueKind.Binary);                
            }

            return (true);
        }        

        private bool isPageCorrect(out bool bIsLastPage, out int left, out int right, out int max)
        {
            bIsLastPage = false;
            left = 0;
            right = 0;
            max = 0;

            // Parses: Currently showing 1-40 of 4160 results
            Regex rx = new Regex(@"Currently showing\s*(\d{1,4})-(\d{1,4})\s*of\s*(\d{1,4})\s*results");

            Match match = rx.Match(CURRENT_INDEX_PAGE);

            if (match.Groups.Count == 4)
            {

                left = int.Parse(match.Groups[1].ToString());
                right = int.Parse(match.Groups[2].ToString());
                max = int.Parse(match.Groups[3].ToString());

                if (right == max)
                {
                    bIsLastPage = true;
                }
                return (true);
            }
            else
            {

                // something went wrong                                
                return (false);
            }
        }

        private bool retrieveMatchesFromIndex(out MatchCollection matches)
        {
            Regex rx = new Regex(@"singleLN.asp\?.+individual_SQL=(?<link>\d+).*\s*class=.*(?<month>\d{1,2})/(?<day>\d{1,2})/((?<year>\d{2,4}))");
            matches = rx.Matches(CURRENT_INDEX_PAGE);

            if (matches.Count >= 1)
            {
                return (true);
            }
            
            throw (new Exception("error while parsing index page - couldn't parse link and date rMFI"));              
        }

        private bool isMatchCorrect(Match match, out DateTime currentDate, out string link)
        {
            currentDate = new DateTime();
            link = "";

            if (match.Groups.Count == 6)
            {

                link = match.Groups["link"].ToString();
                Int32 year, month, day;

                if (isValidStr(match.Groups["month"]) && isValidStr(match.Groups["day"])
                         && isValidStr(match.Groups["year"]))
                {
                    month = Int32.Parse(match.Groups["month"].ToString());
                    day = Int32.Parse(match.Groups["day"].ToString());
                    year = Int32.Parse(match.Groups["year"].ToString());

                    currentDate = new DateTime(year, month, day);
                    return (true);
                }

            }
                      
            throw (new Exception("error while parsing index page - couldn't parse link and date iMC"));                                               
        }

        public bool isValidStr(object oToValidate)
        {
            if ((oToValidate != null) && (oToValidate.ToString() != ""))
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }       
            
        #endregion
    }
       
}
