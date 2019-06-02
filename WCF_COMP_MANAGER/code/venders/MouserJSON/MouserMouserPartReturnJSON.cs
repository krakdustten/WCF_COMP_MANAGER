using System.Collections.Generic;

namespace WCF_COMP_MANAGER.code.venders.MouserJSON
{
    public class MouserMouserPartReturnJSON
    {
        public string Availability;
        public string DataSheetUrl;
        public string Description;
        public string FactoryStock;
        public string ImagePath;
        public string Category;
        public string LeadTime;
        public string LifecycleStatus;
        public string Manufacturer;
        public string ManufacturerPartNumber;
        public string Min;
        public string Mult;
        public string MouserPartNumber;
        public List<MouserProductAttributeReturnJSON> ProductAttributes;
        public List<MouserPricebreakReturnJSON> PriceBreaks;
        public List<MouserAlternatePackagingReturnJSON> AlternatePackagings;
        public string ProductDetailUrl;
        public bool Reeling;
        public string ROHSStatus;
        public string SuggestedReplacement;
        public int MultiSimBlue;
        public List<MouserUnitWeightKgReturnJSON> UnitWeightKg;
        public string RestrictionMessage;
        public string PID;
        public List<MouserProductComplianceReturnJSON> ProductCompliance;
    }
}