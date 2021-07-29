using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApiService
{
    public class ConfigurationDetails
    {
        private String _field;
        private bool _isrequired;
        private Int32 _maxLength;
        private String _source;

        public String Field
        {
            get
            {
                return this._field;
            }
            set
            {
                this._field = value;
            }
        }
        public bool IsRequired
        {
            get
            {
                return this._isrequired;
            }
            set
            {
                this._isrequired = value;
            }
        }

        public Int32 MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }
        public String Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }
        public ArrayList GetMergedDataFrom2Sources()
        {
            ArrayList arr = new ArrayList();
            try
            {
                String str = "{ \"Fields\":";
                str += Getresponsefromsources("http://localhost/Task/api/DefaultFields");
                str += Getresponsefromsources("http://localhost/Task/api/CustomFields");
                str += "}";
                str = str.Replace("][", ",");
                arr.Add(str);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return arr;
        }
        public String Getresponsefromsources(String url)
        {
            String strJsonResponse = String.Empty;
            try
            {
                HttpWebResponse WebResponseObject = null;
                StreamReader sr = null;
                HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(url);
                httpreq.Method = "GET";
                WebResponseObject = (HttpWebResponse)httpreq.GetResponse();
                sr = new StreamReader(WebResponseObject.GetResponseStream());
                strJsonResponse = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return strJsonResponse;
        }
    }
}