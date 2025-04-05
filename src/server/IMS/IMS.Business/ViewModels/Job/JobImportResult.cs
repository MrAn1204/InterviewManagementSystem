namespace IMS.Business.ViewModels;

public class JobImportResult
{
    public int TotalRows { get; set; }
    public int ImportedRows { get; set; }
    public int SkippedRows { get; set; }
    public List<string> Errors { get; set; } = [];
}