using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FunctionApp6.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace FunctionApp6.DAL
{
    public class GetPartnerData
    {
        public string GetGroupedPartnersJson(ILogger log)
        {
            string sp = "sp_pmt_GetAllPartnersMapping_Grouped_JSON";
            try
            {
                DBHelper dbHelper = new DBHelper();
                string json = dbHelper.GetSPReturnJson(log, sp);
                List<GroupedPartner> groupedPartners = JsonConvert.DeserializeObject<List<GroupedPartner>>(json);
                return json;
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching data from sp_pmt_GetAllPartnersMapping_Grouped_JSON. Exception message is: " + ex.Message);
                return string.Empty;
            }
        }
        public bool CreatePartner(PartnerRequest partnerRequest, ILogger log)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                Hashtable ht = new Hashtable();
                ht.Add("@PartnerGroup", partnerRequest.PartnerGroup);
                ht.Add("@PartnerCode", partnerRequest.PartnerCode);
                ht.Add("@PartnerName", partnerRequest.PartnerName);
                ht.Add("@Status", partnerRequest.Status);
                ht.Add("@KPMInCharge", partnerRequest.KPMInCharge);

                bool value = dbHelper.GetRowInsert("sp_pmt_CreatePartner", ht, log);
                return value;
            }
            catch (Exception ex)
            {
                log.LogError("Error create data in sp_pmt_CreatePartner  " + ex.Message);
                return false;
            }
        }

        public List<Partner> getPartnerOfAPartner(string groupId, ILogger log)
        {
            DBHelper dbHelper = new DBHelper();
            List<Partner> partners = new List<Partner>();
            List<Partner> sortedPartners = new List<Partner>();

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@PartnerGroup", groupId);
                DataTable dt = dbHelper.GetDataTable("sp_pmt_GetAllPartnerOfGroup", ht, log);
                log.LogInformation("Requestor Name : " + dt);

                foreach (DataRow row in dt.Rows)
                {
                    partners.Add(new Partner()
                    {
                        PartnerID = (int)row["PartnerID"],
                        PartnerName = row["PartnerName"].ToString(),
                        PartnerCode = row["PartnerCode"].ToString(),
                    });
                }

                sortedPartners = partners.OrderBy(e => e.PartnerID).ToList();
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching Partner data : " + ex);
            }

            return sortedPartners;
        }

    }
   
}
