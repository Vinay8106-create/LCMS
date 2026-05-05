using Galaxy.Dto;

namespace LCMS.Dto
{
    public class ConfigDto
    {
        public long Id { get; set; }
        public string Description { get; set; }

        public int Version { get; set; }
        public AppMessage? Msg { get; set; } = new AppMessage();
    }

   
}