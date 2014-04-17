namespace PortalScrape.DataAccess.Entities
{
    public class EntityId
    {
        public virtual Portal Portal { get; set; }
        public virtual string ExternalId { get; set; }

        public override bool Equals(object obj)
        {
            var entityId = obj as EntityId;
            if (entityId == null) return false;

            return entityId.Portal == Portal && entityId.ExternalId == ExternalId;
        }

        public override int GetHashCode()
        {
            return Portal.GetHashCode()*23 + ExternalId.GetHashCode();
        }
    }
}