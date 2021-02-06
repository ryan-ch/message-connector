using System.Collections.Generic;

namespace XB.MT.Common.Model.Util
{
    public class HeaderUtils
    {
        public static Dictionary<HeaderId, string> SplitIntoHeaders(string message)
        {
            Dictionary<HeaderId, string> headers = new Dictionary<HeaderId, string>();

            int basicHeaderStartIx = message.IndexOf("{1:");
            int applicationHeaderStartIx = message.IndexOf("{2:");
            int userHeaderStartIx = message.IndexOf("{3:");
            int textHeaderStartIx = message.IndexOf("{4:");
            int trailerHeaderStartIx = message.IndexOf("{5:");
            ExtractBasicHeader(headers, message, basicHeaderStartIx, applicationHeaderStartIx, userHeaderStartIx,
                               textHeaderStartIx, trailerHeaderStartIx);
            ExtractApplicationHeader(headers, message, applicationHeaderStartIx, userHeaderStartIx, textHeaderStartIx, trailerHeaderStartIx);
            ExtractUserHeader(headers, message, userHeaderStartIx, textHeaderStartIx, trailerHeaderStartIx);
            ExtractTextHeader(headers, message, textHeaderStartIx, trailerHeaderStartIx);
            ExtractTrailerHeader(headers, message, trailerHeaderStartIx);


            return headers;
        }

        private static void ExtractBasicHeader(Dictionary<HeaderId, string> headers, string message, int basicHeaderStartIx,
                                               int applicationHeaderStartIx, int userHeaderStartIx, int textHeaderStartIx,
                                               int trailerHeaderStartIx)
        {
            string basicHeader = null;
            if (basicHeaderStartIx > -1)
            {
                int endIx = applicationHeaderStartIx > basicHeaderStartIx ? applicationHeaderStartIx :
                            userHeaderStartIx > basicHeaderStartIx ? userHeaderStartIx :
                            textHeaderStartIx > basicHeaderStartIx ? textHeaderStartIx :
                            trailerHeaderStartIx > basicHeaderStartIx ? trailerHeaderStartIx : message.Length;
                basicHeader = message.Substring(basicHeaderStartIx, endIx - basicHeaderStartIx);
            }
            headers.Add(HeaderId.BasicHeader, basicHeader);
        }

        private static void ExtractApplicationHeader(Dictionary<HeaderId, string> headers, string message, int applicationHeaderStartIx,
                                              int userHeaderStartIx, int textHeaderStartIx, int trailerHeaderStartIx)
        {
            string applicationHeader = null;
            if (applicationHeaderStartIx > -1)
            {
                int endIx = userHeaderStartIx > applicationHeaderStartIx ? userHeaderStartIx :
                            textHeaderStartIx > applicationHeaderStartIx ? textHeaderStartIx :
                            trailerHeaderStartIx > applicationHeaderStartIx ? trailerHeaderStartIx : message.Length;
                applicationHeader = message.Substring(applicationHeaderStartIx, endIx - applicationHeaderStartIx);
            }
            headers.Add(HeaderId.Applicationheader, applicationHeader);
        }

        private static void ExtractUserHeader(Dictionary<HeaderId, string> headers, string message, int userHeaderStartIx,
                                              int textHeaderStartIx, int trailerHeaderStartIx)
        {
            string userHeader = null;
            if (userHeaderStartIx > -1)
            {
                int endIx = textHeaderStartIx > userHeaderStartIx ? textHeaderStartIx :
                            trailerHeaderStartIx > userHeaderStartIx ? trailerHeaderStartIx : message.Length;
                userHeader = message.Substring(userHeaderStartIx, endIx - userHeaderStartIx);
            }
            headers.Add(HeaderId.UserHeader, userHeader);
        }

        private static void ExtractTextHeader(Dictionary<HeaderId, string> headers, string message,
                                              int textHeaderStartIx, int trailerHeaderStartIx)
        {
            string textHeader = null;
            if (textHeaderStartIx > -1)
            {
                int endIx = trailerHeaderStartIx > textHeaderStartIx ? trailerHeaderStartIx : message.Length;
                textHeader = message.Substring(textHeaderStartIx, endIx - textHeaderStartIx);
            }
            headers.Add(HeaderId.TextHeader, textHeader);
        }

        private static void ExtractTrailerHeader(Dictionary<HeaderId, string> headers, string message, int trailerHeaderStartIx)
        {
            string trailerHeader = null;
            if (trailerHeaderStartIx > -1)
            {
                trailerHeader = message.Substring(trailerHeaderStartIx, message.Length - trailerHeaderStartIx);
            }
            headers.Add(HeaderId.TrailerHeader, trailerHeader);
        }



        public enum HeaderId
        {
            BasicHeader,
            Applicationheader,
            UserHeader,
            TextHeader,
            TrailerHeader
        }
    }

}
