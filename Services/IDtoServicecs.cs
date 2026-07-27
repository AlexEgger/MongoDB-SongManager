using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service contract responsible for converting domain entities into presentation DTOs.
    /// </summary>
    public interface IDtoService
    {
        /// <summary>
        /// Maps a single <see cref="Song"/> model to a presentation <see cref="SongDisplayDto"/>.
        /// </summary>
        /// <param name="song">The song domain model.</param>
        /// <param name="artistNames">A lookup dictionary mapping artist IDs to artist names.</param>
        /// <returns>A mapped <see cref="SongDisplayDto"/> instance.</returns>
        SongDisplayDto MapToSongDisplayDto (Song song, IReadOnlyDictionary<string, string> artistNames);

        /// <summary>
        /// Maps a collection of <see cref="Song"/> models to a collection of presentation <see cref="SongDisplayDto"/> instances.
        /// </summary>
        /// <param name="songs">The collection of song domain models.</param>
        /// <param name="artistNames">A lookup dictionary mapping artist IDs to artist names.</param>
        /// <returns>A collection of mapped <see cref="SongDisplayDto"/> instances.</returns>
        IEnumerable<SongDisplayDto> MapToSongDisplayDtos (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames);

        /// <summary>
        /// Maps a single <see cref="User"/> domain model to a presentation <see cref="UserDto"/>.
        /// </summary>
        /// <param name="user">The user domain model.</param>
        /// <returns>A mapped <see cref="UserDto"/> instance, or null if source is null.</returns>
        UserDto? MapToUserDto (User? user);

        /// <summary>
        /// Maps a collection of <see cref="User"/> domain models to a collection of presentation <see cref="UserDto"/> instances.
        /// </summary>
        /// <param name="users">The collection of user domain models.</param>
        /// <returns>A collection of mapped <see cref="UserDto"/> instances.</returns>
        IEnumerable<UserDto> MapToUserDtos (IEnumerable<User> users);
    }
}