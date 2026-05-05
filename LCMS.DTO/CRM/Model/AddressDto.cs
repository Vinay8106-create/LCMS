using Galaxy.Dto;

namespace LCMS.Dto
{
    public class AddressDto
    {
        public long Id { get; set; }
        public string Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }
        public int? Level1ConfigId { get; set; }
        public int? Level2ConfigId { get; set; }
        public int? Level3ConfigId { get; set; }
        public string? Level1Config{ get; set; }
        public string? Level2Config { get; set; }
        public string? Level3Config { get; set; }

        public int Version { get; set; }
        public AppMessage? Msg { get; set; } = new AppMessage();
    }

    public class Data
    {
        public string Value { get; set; }
    }

}