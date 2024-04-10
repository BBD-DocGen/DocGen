using System.ComponentModel.DataAnnotations;

namespace DocGen.Core.Entities;

public class UploadDocument
{
    [Key]
    public int UpDocID { get; set; }
    
    public int UserID { get; set; }
    
    public string UpDocName { get; set; }
        
    public string UpDocURL { get; set; }
    
    public virtual ICollection<GeneratedDocument> GeneratedDocuments { get; set; } = new List<GeneratedDocument>();
}