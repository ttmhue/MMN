using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using FunctionApp6.Model;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace FunctionApp6.DAL
{
    public class GetConvoData
    {
        public string GetGroupedConvoJson(ILogger log, int group)
        {
            string sp = "sp_pmt_GetAllConvoPartner_Grouped_JSON";
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PartnersGroup", group)
                };
                string json = dbHelper.GetSPReturnJson(log, sp, parameters);
                List<PartnerConvo> partnerConvo = JsonConvert.DeserializeObject<List<PartnerConvo>>(json);
                return json;
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching data from sp_pmt_GetAllConvoPartner_Grouped_JSON. Exception message is: " + ex.Message);
                return string.Empty;
            }
        }

        public bool CreateConvo(ConvoRequest convoRequest, ILogger log) 
        { 
            try
            {
                DBHelper dbHelper = new DBHelper();
                Hashtable ht = new Hashtable();
                ht.Add("@PartnerGroup", convoRequest.PartnerGroup);
                ht.Add("@PartnerCode", convoRequest.PartnerCode);
                ht.Add("@SubjectLine", convoRequest.SubjectLine);
                ht.Add("@ConversationDetail", convoRequest.ConversationDetail);
                ht.Add("@ShortSummary", convoRequest.ShortSummary);
                ht.Add("@CategoryTag ", convoRequest.CategoryTag);
                ht.Add("@UserEmail", convoRequest.UserEmail);
                ht.Add("@UserName", convoRequest.UserName);
                ht.Add("@Status", convoRequest.Status);
                bool value = dbHelper.GetRowInsert("sp_pmt_CreateConvo", ht, log);
                return value;
            }
            catch (Exception ex)
            {
                log.LogError("Error create data in sp_pmt_CreateConvo  " + ex.Message);
                    return false;
            }
        }




    }
}
