using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace CompanyAPI.Helper
{
    public class Authentication
    {
        string userName= "Hussain";
        string passWord = "Ruhullah";
        string accessToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsInZlciI6MSwia2lkIjoicXprMW1kYTQifQ.eyJqdGkiOiI4MDM3YzhmMy1mN2Y0LTQwZDAtYjE4NC02YTY1ZWUzYjkxYzUiLCJzdWIiOiIxMjUtMjQ4MzQiLCJ0eXBlIjoxLCJleHAiOiIyMDE4LTA5LTI0VDA3OjE2OjQ5WiIsImlhdCI6IjIwMTgtMDktMjBUMDc6MTY6NDlaIiwiTG9jYXRpb25JRCI6MSwiU2l0ZUlEIjoiNTkxNDAtMDk1MTkiLCJJc0FkbWluIjpmYWxzZSwiVG9iaXRVc2VySUQiOjE1MjYzMjMsIlBlcnNvbklEIjoiMTI1LTI0ODM0IiwiRmlyc3ROYW1lIjoiSHVzc2FpbiIsIkxhc3ROYW1lIjoiUnVob2xsYWgifQ.mtaUkm-pGYGykGRU5hyAVstrUMi-eGWOMgRd3qAvWAoYWuevgbLgpBZVUP0pRqmR-MoKe6e8wnWgo4JRfPlIdzeg2Mjb0pF1lfoHHBFh46rdzf57hGkxI1pi6my-UIquMVXJWRhLuu_AGI_U36aKNOa0nrLNKVgElRMu8fGsdEyxNqv3coo5ynAGldObrusVmyEjGIalSMcRfkvCcriVrUKaVvaR9dqHdJpPhHfuMSuryyogVViD8yWbsN8xuyjZx13_n68QHtOYMVMa3Wb-m6Bkous5-UqgXfEWGW2cp9NdmPnRvga8agQA3e2B0LrYsDubgg6nGAg0GdQeovl_vQ";

        public bool Check( string AuthKey)
        {
            byte[] data = Convert.FromBase64String(AuthKey.Substring(8));
            string[] decodedString = Encoding.UTF8.GetString(data).Split(":");

            if ((userName == decodedString[0]) && (passWord == decodedString[1]))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Second Method
        public bool CheckToken(string AuthKey)
        {
            string token = AuthKey.Substring(7);


            if (token == accessToken)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
