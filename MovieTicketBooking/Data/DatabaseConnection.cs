namespace MovieTicketBooking.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseConnection
    {
        /// <summary>
        /// 
        /// </summary>
        string ConnectionString { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        string DatabaseName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
