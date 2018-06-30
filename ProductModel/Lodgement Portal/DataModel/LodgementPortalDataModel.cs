using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CCH.Automation.LP.ProductModel
{
    [Serializable()]
    [XmlRoot("Root")]
    public class LodgementPortalDataModel
    {
        [XmlArray("OtherDataInfo")]
        [XmlArrayItem("FileInfo", typeof(OtherDataInfo))]
        public OtherDataInfo[] objOtherDataInfo { get; set; }

        [XmlArray("DisclosureDataInfo")]
        [XmlArrayItem("FileInfo", typeof(DisclosureDataInfo))]
        public DisclosureDataInfo[] objDisclosureDataInfo { get; set; }

        [XmlElement("ExportSubmitType")]
        public string ExportSubmitType { get; set; }
    }
    
    public class OtherDataInfo
    {

        [XmlAttribute("FilePath")]
        public string FilePath { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

    }

    public class DisclosureDataInfo
    {

        [XmlAttribute("FilePath")]
        public string FilePath { get; set; }

        [XmlAttribute("ScheduelName")]
        public string ScheduleName { get; set; }

    }
}
