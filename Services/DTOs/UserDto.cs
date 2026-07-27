namespace MongoDB_SongManager.Services.DTOs
{
    /// <summary>
    /// Data Transfer Object representing lightweight user information for UI selection and binding.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Returns the display name for UI control formatting.
        /// </summary>
        /// <returns>The display name of the user.</returns>
        public override string ToString () => Name;
    }
}