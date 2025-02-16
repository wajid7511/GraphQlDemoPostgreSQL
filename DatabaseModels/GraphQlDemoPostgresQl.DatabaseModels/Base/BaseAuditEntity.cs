namespace GraphQlDemoPostgresQl.DatabaseModels.Base;

public abstract class BaseAuditEntity
{
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}
