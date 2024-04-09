using System.ComponentModel.DataAnnotations;

namespace DocGen.Core.Entities;

public class DocumentType
{
    [Key]
    public int DocTypeID { get; set; }
    
    public string DocTypeName { get; set; }
    
    public string DocTypeDescription { get; set; }
}