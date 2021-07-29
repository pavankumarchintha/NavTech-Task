using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiService
{
    public class WebApiController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Index()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            String HtmlContent = "<!DOCTYPE html><html><body><h1>Web API Service</h1><p>To Read/Save Entity Configuration</p></body></html>";
            response.Content = new StringContent(HtmlContent, System.Text.Encoding.UTF8, "text/html");
            return response;
        }
        [HttpGet]
        [Route("api/DefaultFields")]
        public ArrayList DefaultFields()
        {
            ArrayList arr = GetDefaultFieldsData();
            return arr;
        }
        [NonAction]
        public ArrayList GetDefaultFieldsData()
        {
            ArrayList arrList = new ArrayList();
            for (int i = 1; i <= 4; i++)
            {
                ConfigurationDetails cd = new ConfigurationDetails();
                cd.Field = "Field" + i;
                if (i % 2 == 0)
                {
                    cd.IsRequired = true;
                }
                else
                {
                    cd.IsRequired = false;
                }
                cd.MaxLength = Convert.ToInt32(i + "0");
                cd.Source = "Source1";
                arrList.Add(cd);
            }
            return arrList;
        }
        [HttpGet]
        [Route("api/CustomFields")]
        public ArrayList CustomFields()
        {
            ArrayList arr = GetCustomFieldsData();
            return arr;
        }
        [NonAction]
        public ArrayList GetCustomFieldsData()
        {
            ArrayList arrList = new ArrayList();
            for (int i = 1; i <= 4; i++)
            {
                ConfigurationDetails cd = new ConfigurationDetails();
                cd.Field = "CField" + i;
                if (i % 2 == 0)
                {
                    cd.IsRequired = false;
                }
                else
                {
                    cd.IsRequired = true;
                }
                cd.MaxLength = Convert.ToInt32(i + "0");
                cd.Source = "Source2";
                arrList.Add(cd);
            }
            return arrList;
        }
        [HttpGet]
        [Route("api/Read")]
        public HttpResponseMessage ReadConfiguration()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                ConfigurationDetails cd = new ConfigurationDetails();
                DbProcedures db = new DbProcedures();
                ArrayList arr = cd.GetMergedDataFrom2Sources();
                CombinedFileds fields = JsonConvert.DeserializeObject<CombinedFileds>(Convert.ToString(arr[0]));
                foreach (Fields f in fields.Fields)
                {
                    bool isrecordExists = db.CheckFiledExists(f);
                    if (isrecordExists)
                    {
                        fields.EntityName = db.Getentityconfig(f);
                    }
                    else
                    {
                        db.InsertEntityDetails(f);
                    }
                }
                String jsonString = JsonConvert.SerializeObject(fields);
                response.Content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            }
            catch(Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message);
            }
            return response;
        }
        [HttpPost]
        [Route("api/Save")]
        public HttpResponseMessage SaveConfiguration(List<CombinedFileds> lstObj)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DbProcedures db = new DbProcedures();
                foreach (CombinedFileds combinedFileds in lstObj)
                {
                    foreach (Fields f in combinedFileds.Fields)
                    {
                        bool isrecordexists = db.CheckFiledExists(f);
                        if (isrecordexists)
                        {
                            db.UpdateEntityDetails(combinedFileds.EntityName,f);
                        }
                        else
                        {
                            db.InsertEntityDetails(f);
                        }
                    }
                }
                response.Content = new StringContent("Configuration Has been saved Successfully", System.Text.Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.Message);
            }
            return response;
        }
    }
}
