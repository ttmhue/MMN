using System.Collections.Generic;

namespace FunctionApp6.Model
{
    public class PartnerDataModel
    {
        public MARMPartner partner { get; set; }
    }


    public class MARMPartner
    {
        public string MarmsPartnersId { get; set; }
        public string partnerName { get; set; }
        public string partnerCode { get; set; }
        public string partnerType { get; set; }
        
    }
      public class GroupedPartner
        {
            public string GroupPartner { get; set; }
            public List<Partner> Partners { get; set; }
        }

        public class Partner
        {
            public int PartnerID { get; set; }
            public string PartnerName { get; set; }
            public string PartnerCode { get; set; }
            public string Status { get; set; }
            public string KPMInCharge { get; set; }
            public string PartnerSource { get; set; }
            public bool isActive { get; set; }
        }
    public class PartnerRequest
    {
        public int PartnerGroup { get; set; }
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string Status { get; set; }
        public string KPMInCharge { get; set; }
    }
    public class Group
    {
        public string PartnerGroupID { get; set; }
        public string PartnerGroup { get; set; }
    }

}