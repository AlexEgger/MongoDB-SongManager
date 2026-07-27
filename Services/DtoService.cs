using MongoDB_SongManager.Models;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service responsible for transforming domain models into DTOs tailored for UI presentation and editing.
    /// </summary>
    public class DtoService : IDtoService
    {
        /// <inheritdoc />
        public SongDisplayDto MapToSongDisplayDto (
            Song song,
            IReadOnlyDictionary<string, string> artistNames,
            UserSongInteraction? interaction = null)
        {
            ArgumentNullException.ThrowIfNull(song);

            string artistName = "Unknown";
            if (!string.IsNullOrEmpty(song.ArtistId) && artistNames != null && artistNames.TryGetValue(song.ArtistId, out var resolvedName))
            {
                artistName = resolvedName;
            }

            string bookInfo = "-";
            if (song.Liederbuchnummer.HasValue || song.Liederbuchseite.HasValue)
            {
                bookInfo = $"Book #{song.Liederbuchnummer?.ToString() ?? "-"} / Page {song.Liederbuchseite?.ToString() ?? "-"}";
            }

            return new SongDisplayDto
            {
                Id = song.Id ?? string.Empty,
                Title = song.Title ?? string.Empty,
                ArtistName = artistName,
                ChordsUrl = song.ChordsUrl,
                YoutubeUrl = song.YoutubeUrl,
                SongbookInfo = bookInfo,
                Tempo = song.Tempo,
                Notes = interaction?.Notes,
                Ratings = interaction?.Ratings ?? new List<Rating>()
            };
        }

        /// <inheritdoc />
        public IEnumerable<SongDisplayDto> MapToSongDisplayDtos (
            IEnumerable<Song> songs,
            IReadOnlyDictionary<string, string> artistNames,
            IReadOnlyDictionary<string, UserSongInteraction>? interactions = null)
        {
            if (songs == null) return Enumerable.Empty<SongDisplayDto>();

            return songs.Select(song =>
            {
                UserSongInteraction? interaction = null;
                if (interactions != null && interactions.TryGetValue(song.Id, out var userInteraction))
                {
                    interaction = userInteraction;
                }
                return MapToSongDisplayDto(song, artistNames, interaction);
            }).ToList();
        }

        /// <inheritdoc />
        public UserDto? MapToUserDto (User? user)
        {
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id ?? string.Empty,
                Name = user.Name ?? string.Empty
            };
        }

        /// <inheritdoc />
        public IEnumerable<UserDto> MapToUserDtos (IEnumerable<User> users)
        {
            if (users == null) return Enumerable.Empty<UserDto>();

            return users.Select(user => MapToUserDto(user)!)
                        .Where(dto => dto != null)
                        .ToList();
        }
    }
}