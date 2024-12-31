using Xunit;
using Moq;
using WebAPI.Repository;
using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _mockMovieRepository;
    private readonly MovieServicelmpl _movieService;

    public MovieServiceTests()
    {
        _mockMovieRepository = new Mock<IMovieRepository>();
        _movieService = new MovieServicelmpl(null, null,null);
    }

    [Fact]
    public async Task GetMoviesPagedAsync_ShouldReturnCorrectPagedData()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = "1", MovieNameEnglish = "Movie 1" },
            new Movie { MovieId = "2", MovieNameEnglish = "Movie 2" },
            new Movie { MovieId = "3", MovieNameEnglish = "Movie 3" }
        };

        _mockMovieRepository.Setup(repo => repo.GetAllMoviesQueryable())
            .Returns(movies.AsQueryable());

        // Act
        var (pagedMovies, totalCount, totalPages) = await _movieService.GetMoviesPagedAsync("", 1, 2,0);

        // Assert
        Assert.Equal(2, pagedMovies.Count);
        Assert.Equal(3, totalCount);
        Assert.Equal(2, totalPages);
    }

    [Fact]
    public async Task GetMoviesPagedAsync_ShouldThrowException_WhenPageExceedsTotalPages()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new Movie { MovieId = "1", MovieNameEnglish = "Movie 1" },
            new Movie { MovieId = "2", MovieNameEnglish = "Movie 2" }
        };

        _mockMovieRepository.Setup(repo => repo.GetAllMoviesQueryable())
            .Returns(movies.AsQueryable());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _movieService.GetMoviesPagedAsync("", 3, 1,0));
    }

    [Fact]
    public async Task GetMoviesPagedAsync_ShouldHandleEmptyResults()
    {
        // Arrange
        _mockMovieRepository.Setup(repo => repo.GetAllMoviesQueryable())
            .Returns(new List<Movie>().AsQueryable());

        // Act
        var (pagedMovies, totalCount, totalPages) = await _movieService.GetMoviesPagedAsync("", 1, 5,0);

        // Assert
        Assert.Empty(pagedMovies);
        Assert.Equal(0, totalCount);
        Assert.Equal(0, totalPages);
    }
}
