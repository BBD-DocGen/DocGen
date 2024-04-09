using System.ComponentModel.DataAnnotations;

namespace DocGen.Core.Entities;

public class GeneratedDocument
{
    [Key]
    public int GenDocID { get; set; }
    
    public int DocTypeID { get; set; }
    
    public int UpDocID { get; set; }
    
    public string GenDocName { get; set; }

    public string GenDocURL { get; set; }
}