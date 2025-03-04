namespace OrderService.Models.Requests
{
    /// <summary>
    /// Represents the details for user registration.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// The desired username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The desired password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; set; }
    }
}