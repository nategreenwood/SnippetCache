namespace SnippetCache.Data.DTO
{
    public class NotificationTypeDTO : INotificationType
    {
        #region INotificationType Members

        public string Name { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        #endregion
    }
}