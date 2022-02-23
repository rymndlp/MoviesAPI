using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<MovieCreationDTO, Movie>()
              .ForMember(x => x.Poster, options => options.Ignore())
              .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres));

            CreateMap<Movie, MovieDTO>()
               .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres));
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO moviedto)
        {
            var result = new List<GenreDTO>();

            if (movie.MoviesGenres != null)
            {
                foreach (var genre in movie.MoviesGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }

            return result;
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();

            if (movieCreationDTO.GenresIds == null) { return result; }

            foreach (var id in movieCreationDTO.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }
    }
}
