using System.Collections.Generic;

namespace WCF_COMP_MANAGER.code.venders.FarnellJSON
{
    public class FarnellProductJSON
    {
        public string sku;
        public string displayName;
        public string productStatus;
        public string rohsStatusCode;
        public int packSize;
        public string unitOfMeasure;
        public string id;
        //public List<FarnellDatasheetJSON> datasheets;
        public List<FarnellPriceJSON> prices;
        public string vendorId;
        public string vendorName;
        public string brandName;
        public string translatedManufacturerPartNumber;
        public int translatedMinimumOrderQuality;
    }
}