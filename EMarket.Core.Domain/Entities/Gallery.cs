using EMarket.Core.Domain.Common;

namespace EMarket.Core.Domain.Entities
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        //Navigation Properties
        public int AdvertisingId { get; set; }
        public Advertising Advertisings { get; set; }
    }
}
