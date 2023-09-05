using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FunctionApp6.Model;

namespace FunctionApp6.DAL
{
    public class GetGroupPartnerData
    {
        public List<Group> GetAllGroup(ILogger log)
        {
            DBHelper dbHelper = new DBHelper();
            List<Group> groups = new List<Group>();
            List<Group> sortedGroups = new List<Group>();

            try
            {

                Hashtable ht = new Hashtable();
                DataTable dt = new DataTable();

                dt = dbHelper.GetDataTable("sp_pmt_GetAllGroup", ht, log);
                foreach (DataRow row in dt.Rows)
                {
                    groups.Add(new Group()
                    {
                        PartnerGroupID = row["PartnerGroupID"].ToString(),
                        PartnerGroup = row["PartnerGroup"].ToString(),
                    });
                }
                sortedGroups = groups.OrderBy(e => e.PartnerGroupID).ToList();

            }
            catch (Exception ex)
            {
                log.LogError("Error in fetching Group partner data : " + ex);
            }
            return sortedGroups;
        }

    }
   
}
