namespace TvProgramDB.Core.Services
{
    public abstract class CrawlerServiceBase
    {
        public abstract void Initialize();
        public abstract void Start();
        public abstract void Stop();
    }
}