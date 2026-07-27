using MongoDB_SongManager.Models;
using MongoDB_SongManager.Models.Enums;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service responsible for transforming domain models into DTOs tailored for UI presentation and editing.
    /// </summary>
    public class DtoService : IDtoService
    {
        /// <inheritdoc />
        public ArtistDto? MapToArtistDto (Artist? artist)
        {
            if (artist == null) return null;

            return new ArtistDto
            {
                Id = artist.Id ?? string.Empty,
                Name = artist.Name ?? string.Empty
            };
        }

        /// <inheritdoc />
        public IEnumerable<ArtistDto> MapToArtistDtos (IEnumerable<Artist> artists)
        {
            if (artists == null) return Enumerable.Empty<ArtistDto>();

            return artists.Select(artist => MapToArtistDto(artist)!)
                          .Where(dto => dto != null)
                          .ToList();
        }

        /// <inheritdoc />
        public Artist? MapToArtistEntity (ArtistDto? artistDto)
        {
            if (artistDto == null) return null;

            return new Artist
            {
                Id = artistDto.Id,
                Name = artistDto.Name
            };
        }

        /// <inheritdoc />
        public SongDto? MapToSongDto (Song? song)
        {
            if (song == null) return null;

            return new SongDto
            {
                Id = song.Id ?? string.Empty,
                Title = song.Title ?? string.Empty,
                ArtistId = song.ArtistId,
                Tempo = song.Tempo,
                ChordsUrl = song.ChordsUrl,
                YoutubeUrl = song.YoutubeUrl,
                Liederbuchnummer = song.Liederbuchnummer,
                Liederbuchseite = song.Liederbuchseite
            };
        }

        /// <inheritdoc />
        public Song? MapToSongEntity (SongDto? songDto)
        {
            if (songDto == null) return null;

            return new Song
            {
                Id = songDto.Id,
                Title = songDto.Title,
                ArtistId = songDto.ArtistId,
                Tempo = songDto.Tempo,
                ChordsUrl = songDto.ChordsUrl,
                YoutubeUrl = songDto.YoutubeUrl,
                Liederbuchnummer = songDto.Liederbuchnummer,
                Liederbuchseite = songDto.Liederbuchseite
            };
        }

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
        public SonglistDto? MapToSonglistDto (Songlist? songlist)
        {
            if (songlist == null) return null;

            return new SonglistDto
            {
                Id = songlist.Id ?? string.Empty,
                Name = songlist.Name ?? string.Empty,
                CreatorId = songlist.CreatorId ?? string.Empty,
                SongIds = songlist.SongIds != null ? new List<string>(songlist.SongIds) : new List<string>()
            };
        }

        /// <inheritdoc />
        public IEnumerable<SonglistDto> MapToSonglistDtos (IEnumerable<Songlist> songlists)
        {
            if (songlists == null) return Enumerable.Empty<SonglistDto>();

            return songlists.Select(sl => MapToSonglistDto(sl)!)
                            .Where(dto => dto != null)
                            .ToList();
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

        /// <inheritdoc />
        public UserSongInteractionDto? MapToInteractionDto (UserSongInteraction? interaction)
        {
            if (interaction == null) return null;

            return new UserSongInteractionDto
            {
                Id = interaction.Id ?? string.Empty,
                UserId = interaction.UserId ?? string.Empty,
                SongId = interaction.SongId ?? string.Empty,
                Ratings = interaction.Ratings?.Select(r => new RatingDto
                {
                    Category = r.Category.ToString(),
                    Value = r.Value
                }).ToList() ?? new List<RatingDto>(),
                Notes = interaction.Notes,
                UpdatedAt = interaction.UpdatedAt
            };
        }

        /// <inheritdoc />
        public UserSongInteraction? MapToInteractionEntity (UserSongInteractionDto? dto)
        {
            if (dto == null) return null;

            return new UserSongInteraction
            {
                Id = string.IsNullOrWhiteSpace(dto.Id) ? MongoDB.Bson.ObjectId.GenerateNewId().ToString() : dto.Id,
                UserId = dto.UserId,
                SongId = dto.SongId,
                Ratings = dto.Ratings?.Select(r => new Rating
                {
                    Category = Enum.TryParse<RatingType>(r.Category, true, out var category) ? category : default,
                    Value = r.Value
                }).ToList() ?? new List<Rating>(),
                Notes = dto.Notes ?? string.Empty,
                UpdatedAt = dto.UpdatedAt
            };
        }

        /// <inheritdoc />
        public UserSongInteraction? MapToInteractionEntity (SaveUserSongInteractionDto? dto, string userId)
        {
            if (dto == null) return null;

            return new UserSongInteraction
            {
                UserId = userId,
                SongId = dto.SongId,
                Ratings = dto.Ratings?.Select(r => new Rating
                {
                    Category = Enum.TryParse<RatingType>(r.Category, true, out var category) ? category : default,
                    Value = r.Value
                }).ToList() ?? new List<Rating>(),
                Notes = dto.Notes ?? string.Empty,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}