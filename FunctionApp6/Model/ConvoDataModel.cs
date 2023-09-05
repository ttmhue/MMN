using System.Collections.Generic;

namespace FunctionApp6.Model
{
    public class PartnerConvo
    {
        public string PartnerCode { get; set; }
        public List<ConvoDTO> ListConvo { get; set; }
        
    }
    public class ConvoDTO
    {
        public int ConversationId { get; set; }
        public string SubjectLine { get; set; }
        public string ConversationDetail { get; set; }
        public string ShortSummary { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string CategoryName { get; set; }

    }
    public class ConvoRequest
    {
        public int PartnerGroup { get; set; }
        public int PartnerCode { get; set; }
        public int CategoryTag { get; set; }
        public string SubjectLine { get; set; }
        public string ConversationDetail { get; set; }
        public string ShortSummary { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
    }
}