using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FunctionApp6.Model;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace FunctionApp6.DAL
{
    public class GetMarmPartnerData
    {

        public List<MARMPartner> getAllPartners(ILogger log)
        {
            DBHelper dbHelper = new DBHelper();
            List<MARMPartner> partners = new List<MARMPartner>();
            List<MARMPartner> sortedPartners = new List<MARMPartner>();

            try
            {

                Hashtable ht = new Hashtable();
                DataTable dt = new DataTable();

                dt = dbHelper.GetDataTable("sp_pmt_GetAllPartners", ht, log);
                foreach (DataRow row in dt.Rows)
                {
                    partners.Add(new MARMPartner()
                    {
                        MarmsPartnersId = row["MarmsPartnersId"].ToString(),
                        partnerName = row["partnerName"].ToString(),
                        partnerCode = row["partnerCode"].ToString(),
                        partnerType = row["partnerType"].ToString()
                    });
                }
                sortedPartners = partners.OrderBy(e => e.MarmsPartnersId).ToList();

            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching Cost center master data : " + ex);
            }
            return sortedPartners;

        }

        public List<MARMPartner> getPartner(string partnerId, ILogger log)
        {
            DBHelper dbHelper = new DBHelper();
            List<MARMPartner> partners = new List<MARMPartner>();
            List<MARMPartner> sortedPartners = new List<MARMPartner>();

            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("@MamrsPartnersId", partnerId);
                DataTable dt = dbHelper.GetDataTable("sp_pmt_GetPartnerById", ht, log);
                log.LogInformation("Requestor Name : " + dt);

                foreach (DataRow row in dt.Rows)
                {
                    partners.Add(new MARMPartner()
                    {
                        MarmsPartnersId = row["MarmsPartnersId"].ToString(),
                        partnerName = row["partnerName"].ToString(),
                        partnerCode = row["partnerCode"].ToString(),
                        partnerType = row["partnerType"].ToString()
                    });
                }

                sortedPartners = partners.OrderBy(e => e.MarmsPartnersId).ToList();
            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching Cost center master data : " + ex);
            }

            return sortedPartners;
        }
    }
}
