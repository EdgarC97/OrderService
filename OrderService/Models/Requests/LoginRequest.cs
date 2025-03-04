namespace OrderService.Models.Requests
{
    /// <summary>
    /// Represents the credentials for user login.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}