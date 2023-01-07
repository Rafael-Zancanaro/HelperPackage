namespace PackageRZ.Domain.Entities
{
    public class BaseEntity<TPK>
    {
        public TPK Id { get; protected set; }
    }
}