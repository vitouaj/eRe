using System.Reflection.Metadata.Ecma335;

namespace eRe.Repository;

public class Utilities
{
    public static string GenerateUserId() => $"U-{Guid.NewGuid().ToString().Substring(0, 8)}";
    public static string GenerateReportId() => $"R-{Guid.NewGuid().ToString().Substring(0, 10)}";
    public static string GenerateClassId() => $"C-{Guid.NewGuid().ToString().Substring(0, 6)}";
    public static string GenerateSubjectId() => $"sub-{Guid.NewGuid().ToString().Substring(0, 3)}";
    public static string GenerateSubjectItemId() => $"si-{Guid.NewGuid().ToString().Substring(0, 10)}";
}
