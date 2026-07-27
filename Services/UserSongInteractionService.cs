using MongoDB_SongManager.Data.Repositories;
using MongoDB_SongManager.Models;
using MongoDB_SongManager.Models.Enums;
using MongoDB_SongManager.Services.DTOs;

namespace MongoDB_SongManager.Services
{
    /// <summary>
    /// Service implementation for managing user-isolated song interactions using the repository layer.
    /// </summary>
    public class UserSongInteractionService : IUserSongInteractionService
    {
        private readonly IUserSongInteractionRepository _interactionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSongInteractionService"/> class.
        /// </summary>
        /// <param name="interactionRepository">The user interaction repository instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interactionRepository"/> is null.</exception>
        public UserSongInteractionService (IUserSongInteractionRepository interactionRepository)
        {
            _interactionRepository = interactionRepository ?? throw new ArgumentNullException(nameof(interactionRepository));
        }

        /// <inheritdoc />
        public Task<UserSongInteractionDto?> GetInteractionAsync (string userId, string songId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(songId))
            {
                return Task.FromResult<UserSongInteractionDto?>(null);
            }

            var entity = _interactionRepository.GetInteraction(userId, songId);
            if (entity == null)
            {
                return Task.FromResult<UserSongInteractionDto?>(null);
            }

            return Task.FromResult<UserSongInteractionDto?>(MapToDto(entity));
        }

        /// <inheritdoc />
        public Task<IEnumerable<UserSongInteractionDto>> GetUserInteractionsAsync (string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(Enumerable.Empty<UserSongInteractionDto>());
            }

            var entities = _interactionRepository.Find(x => x.UserId == userId);
            var dtos = entities.Select(MapToDto);

            return Task.FromResult(dtos);
        }

        /// <inheritdoc />
        public Task<UserSongInteractionDto> SaveInteractionAsync (string userId, SaveUserSongInteractionDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            if (dto == null || string.IsNullOrWhiteSpace(dto.SongId))
            {
                throw new ArgumentException("Invalid song interaction data.", nameof(dto));
            }

            var existingEntity = _interactionRepository.GetInteraction(userId, dto.SongId);

            if (existingEntity != null)
            {
                existingEntity.Ratings = MapToRatingModelList(dto.Ratings);
                existingEntity.Notes = dto.Notes ?? string.Empty;
                existingEntity.UpdatedAt = DateTime.UtcNow;

                _interactionRepository.Update(existingEntity);
                return Task.FromResult(MapToDto(existingEntity));
            }

            var newEntity = new UserSongInteraction
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                SongId = dto.SongId,
                Ratings = MapToRatingModelList(dto.Ratings),
                Notes = dto.Notes ?? string.Empty,
                UpdatedAt = DateTime.UtcNow
            };

            _interactionRepository.Insert(newEntity);
            return Task.FromResult(MapToDto(newEntity));
        }

        /// <inheritdoc />
        public Task<bool> DeleteInteractionAsync (string userId, string songId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(songId))
            {
                return Task.FromResult(false);
            }

            var existingEntity = _interactionRepository.GetInteraction(userId, songId);
            if (existingEntity == null)
            {
                return Task.FromResult(false);
            }

            bool isDeleted = _interactionRepository.Delete(existingEntity.Id);
            return Task.FromResult(isDeleted);
        }

        #region Private Mapping Helpers

        /// <summary>
        /// Maps a <see cref="UserSongInteraction"/> entity to its corresponding DTO.
        /// </summary>
        /// <param name="entity">The source entity.</param>
        /// <returns>The mapped <see cref="UserSongInteractionDto"/>.</returns>
        private static UserSongInteractionDto MapToDto (UserSongInteraction entity)
        {
            return new UserSongInteractionDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SongId = entity.SongId,
                Ratings = entity.Ratings?.Select(r => new RatingDto
                {
                    Category = r.Category.ToString(),
                    Value = r.Value
                }).ToList() ?? new List<RatingDto>(),
                Notes = entity.Notes,
                UpdatedAt = entity.UpdatedAt
            };
        }

        /// <summary>
        /// Maps a list of <see cref="RatingDto"/> instances to a list of domain <see cref="Rating"/> models.
        /// </summary>
        /// <param name="ratingDtos">The source rating DTO collection.</param>
        /// <returns>A list of domain rating models.</returns>
        private static List<Rating> MapToRatingModelList (IEnumerable<RatingDto>? ratingDtos)
        {
            if (ratingDtos == null)
            {
                return new List<Rating>();
            }

            var ratingsList = new List<Rating>();

            foreach (var r in ratingDtos)
            {
                if (Enum.TryParse<RatingType>(r.Category, true, out var ratingType))
                {
                    ratingsList.Add(new Rating
                    {
                        Category = ratingType,
                        Value = r.Value
                    });
                }
            }

            return ratingsList;
        }

        #endregion
    }
}