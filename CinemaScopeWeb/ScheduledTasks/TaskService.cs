using MovieService.Imdb;
using MovieService.Interfaces;
using MovieService.Interfaces.ServiceInterfaces;
using Quartz;
using System.Configuration;
using System.Threading.Tasks;

namespace CinemaScopeWeb.ScheduledTasks
{
    public class TaskService : IJob
    {
        public static readonly string SchedulingStatus = ConfigurationManager.AppSettings["TaskService"];
        private IUnitOfWork _unitOfWork;
        private IImdbService _imdbService;

        public TaskService(IUnitOfWork unitOfWork, IImdbService imdbService) { _unitOfWork = unitOfWork; _imdbService = imdbService; }

        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                if (SchedulingStatus.Equals("ON")) { Update(); }
            });
            return task;
        }

        public void Update()
        {

            string newMovieId;
            var lastLoadedMovie = _unitOfWork.MovieRepository.GetLastUploadedFromImdb();
            if (lastLoadedMovie != null) 
            { 
                newMovieId = lastLoadedMovie.ImdbId;
            } 
            else 
            {
                newMovieId = ImdbApi.MoiveIdCode + ImdbApi.MovieIdStartNumber;
            }
            for (int i = 0; i < ImdbApi.MaxMovieUpdates; i++)
            {              
                newMovieId = IncrementId(newMovieId);
                var movieAdded = AddNewMovie(newMovieId);
                int tries = 0;
                while(!movieAdded && tries < ImdbApi.MaxMovieUpdateTries)
                {
                    newMovieId = IncrementId(newMovieId);
                    movieAdded = AddNewMovie(newMovieId);
                    tries++;
                }
            }
        }

        private string IncrementId(string id)
        {
            var lastLoadedId = id;
            var lastLoadedIdNumber = int.Parse(lastLoadedId.Replace(ImdbApi.MoiveIdCode, ""));
            lastLoadedIdNumber += 1;
            var newMovieNumber = lastLoadedIdNumber.ToString().PadLeft(ImdbApi.MovieIdStartNumber.Length, '0');
            return ImdbApi.MoiveIdCode + newMovieNumber;
        }

        private bool AddNewMovie(string newMovieId)
        {
            return _imdbService.GetMovieByImdbId(newMovieId);
        }
    }
}
