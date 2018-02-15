using System.Collections.Generic;

namespace Product.API.ApiModels
{
    public class MediaApiModel : ApiModelBase
    { 
        public IEnumerable<string> Images{ get; set; }
        public IEnumerable<string> Videos{ get; set; }
    }
}
