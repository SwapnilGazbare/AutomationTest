/*Author: Kavita Nunse
 * Date : 4-August-2017
 * Desc : File contains fields related to Search Lodgement Page
 */

namespace CCH.Automation.LP.ProductModel
{
    public class SearchLodgementVO
    {
        public string entityName 
        { get; 
            set; 
        }

        public string entityTFN { get; set; }

        public string submissionDate { get; set; }

        public string lodgementTimestamp { get; set; }

        public string workbookName { get; set; }

        public string taxAgentNumber { get; set; }

        public string periodEndDate { get; set; }

        public string status { get; set; }

        public string jobIdentifier { get; set; }

        public bool showDeleted { get; set; }

        public bool SearchButton { get; set; }

        public bool isZeroRecordExpected { get; set; }

        public bool isRecordExpected { get; set; }

        public bool SearchMagnifyingIcon { get; set; }

    }
}
