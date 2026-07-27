using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service contract responsible for converting domain entities into presentation DTOs and vice versa.
    /// </summary>
    public interface IDtoService
    {
        /// <summary>
        /// Maps a single <see cref="Artist"/> domain model to an <see cref="ArtistDto"/>.
        /// </summary>
        /// <param name="artist">The artist domain model.</param>
        /// <returns>A mapped <see cref="ArtistDto"/> instance, or null if source is null.</returns>
        ArtistDto? MapToArtistDto (Artist? artist);

        /// <summary>
        /// Maps a collection of <see cref="Artist"/> domain models to a collection of <see cref="ArtistDto"/> instances.
        /// </summary>
        /// <param name="artists">The collection of artist domain models.</param>
        /// <returns>A collection of mapped <see cref="ArtistDto"/> instances.</returns>
        IEnumerable<ArtistDto> MapToArtistDtos (IEnumerable<Artist> artists);

        /// <summary>
        /// Maps an <see cref="ArtistDto"/> back to an <see cref="Artist"/> domain model.
        /// </summary>
        /// <param name="artistDto">The artist DTO.</param>
        /// <returns>A mapped <see cref="Artist"/> domain model, or null if source is null.</returns>
        Artist? MapToArtistEntity (ArtistDto? artistDto);

        /// <summary>
        /// Maps a single <see cref="Song"/> domain model to an editable <see cref="SongDto"/>.
        /// </summary>
        /// <param name="song">The song domain model.</param>
        /// <returns>A mapped <see cref="SongDto"/> instance, or null if source is null.</returns>
        SongDto? MapToSongDto (Song? song);

        /// <summary>
        /// Maps a <see cref="SongDto"/> back to a <see cref="Song"/> domain entity.
        /// </summary>
        /// <param name="songDto">The song DTO.</param>
        /// <returns>A mapped <see cref="Song"/> domain model, or null if source is null.</returns>
        Song? MapToSongEntity (SongDto? songDto);

        /// <summary>
        /// Maps a single <see cref="Song"/> model to a presentation <see cref="SongDisplayDto"/>.
        /// </summary>
        /// <param name="song">The song domain model.</param>
        /// <param name="artistNames">A lookup dictionary mapping artist IDs to artist names.</param>
        /// <param name="interaction">Optional user interaction entity containing personal ratings and notes.</param>
        /// <returns>A mapped <see cref="SongDisplayDto"/> instance.</returns>
        SongDisplayDto MapToSongDisplayDto (Song song, IReadOnlyDictionary<string, string> artistNames, UserSongInteraction? interaction = null);

        /// <summary>
        /// Maps a collection of <see cref="Song"/> models to a collection of presentation <see cref="SongDisplayDto"/> instances.
        /// </summary>
        /// <param name="songs">The collection of song domain models.</param>
        /// <param name="artistNames">A lookup dictionary mapping artist IDs to artist names.</param>
        /// <param name="interactions">Optional dictionary mapping song IDs to user interaction records.</param>
        /// <returns>A collection of mapped <see cref="SongDisplayDto"/> instances.</returns>
        IEnumerable<SongDisplayDto> MapToSongDisplayDtos (IEnumerable<Song> songs, IReadOnlyDictionary<string, string> artistNames, IReadOnlyDictionary<string, UserSongInteraction>? interactions = null);

        /// <summary>
        /// Maps a single <see cref="Songlist"/> domain model to a <see cref="SonglistDto"/>.
        /// </summary>
        /// <param name="songlist">The songlist domain model.</param>
        /// <returns>A mapped <see cref="SonglistDto"/> instance, or null if source is null.</returns>
        SonglistDto? MapToSonglistDto (Songlist? songlist);

        /// <summary>
        /// Maps a collection of <see cref="Songlist"/> domain models to a collection of <see cref="SonglistDto"/> instances.
        /// </summary>
        /// <param name="songlists">The collection of songlist domain models.</param>
        /// <returns>A collection of mapped <see cref="SonglistDto"/> instances.</returns>
        IEnumerable<SonglistDto> MapToSonglistDtos (IEnumerable<Songlist> songlists);

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

        /// <summary>
        /// Maps a <see cref="UserSongInteraction"/> domain model to a <see cref="UserSongInteractionDto"/>.
        /// </summary>
        /// <param name="interaction">The user song interaction domain model.</param>
        /// <returns>A mapped <see cref="UserSongInteractionDto"/> instance, or null if source is null.</returns>
        UserSongInteractionDto? MapToInteractionDto (UserSongInteraction? interaction);

        /// <summary>
        /// Maps a <see cref="UserSongInteractionDto"/> back to a <see cref="UserSongInteraction"/> domain model.
        /// </summary>
        /// <param name="dto">The user song interaction DTO.</param>
        /// <returns>A mapped <see cref="UserSongInteraction"/> domain model, or null if source is null.</returns>
        UserSongInteraction? MapToInteractionEntity (UserSongInteractionDto? dto);

        /// <summary>
        /// Maps a <see cref="SaveUserSongInteractionDto"/> and user ID to a <see cref="UserSongInteraction"/> domain entity.
        /// </summary>
        /// <param name="dto">The input save DTO containing user ratings and notes.</param>
        /// <param name="userId">The ID of the user performing the save action.</param>
        /// <returns>A mapped <see cref="UserSongInteraction"/> domain model, or null if source is null.</returns>
        UserSongInteraction? MapToInteractionEntity (SaveUserSongInteractionDto? dto, string userId);
    }
}