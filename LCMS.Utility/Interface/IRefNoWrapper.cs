namespace LCMS.Utility
{
    public interface IRefNoWrapper
    {
        Task<string> GenerateRefNo(string configValue);
    }
}
