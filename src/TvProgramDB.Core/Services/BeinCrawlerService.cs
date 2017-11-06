using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Helpers;
using TvProgramDB.Core.Interfaces;
using TvProgramDB.Core.Specifications;

namespace TvProgramDB.Core.Services
{
    public class BeinCrawlerService : CrawlerServiceBase
    {
        /// <summary>
        /// example : https://epg.beinsports.com/utctime.php?cdate=2017-11-09&offset=+1&mins=00&category=sports&serviceidentity=beinsports.com&id=123
        /// </summary>
        const string BaseUrl = "https://epg.beinsports.com/utctime.php?cdate={0}&offset=+1&mins=00&category=sports&serviceidentity=beinsports.com&id=123";
        private bool _running;
        private CancellationTokenSource _tokenSource;
        private ILogger _logger;
        private IRepository<Country> _countryRepository;
        private IRepository<Source> _sourceRepository;
        private IRepository<Chanel> _ChanelRepository;
        private IRepository<Program> _programRepository;
        private Country _country;
        private int _sourceId;
        private List<Chanel> _Chanels;
        private readonly Regex RegexChanelName = new Regex(@"^.*\/(\w*)\..*");

        public BeinCrawlerService(
            ILoggerFactory logger,
            IRepository<Country> countryRepository,
            IRepository<Source> sourceRepository,
            IRepository<Program> programRepository,
            IRepository<Chanel> ChanelRepository)
        {
            _logger = logger.CreateLogger<ProgrammeTvCrowlerService>();
            _countryRepository = countryRepository;
            _sourceRepository = sourceRepository;
            _ChanelRepository = ChanelRepository;
            _programRepository = programRepository;
        }

        public override void Initialize()
        {
            _logger.LogInformation("Initializing BeinSport crawler");

            _country = _countryRepository.GetSingleBySpec(new CountryCodeSpecification("FR"));
            if (_country == null)
            {
                _country = _countryRepository.Add(new Country { Code = "FR", Name = "France" });
            }

            var source = _sourceRepository.GetSingleBySpec(new SourceCodeSpecification("BeinSport"));
            if (source == null)
            {
                source = _sourceRepository.Add(new Source { Code = "BeinSport", Name = "beinsports.com" });
            }
            _sourceId = source.Id;

            _Chanels = _ChanelRepository.ListAll(nameof(Chanel.ChanelNames)).ToList();

        }

        public override void Start()
        {
            if (_running) return;
            _logger.LogInformation("Starting BeinSport crawler");
            _running = true;

            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                while (_running)
                {
                    Load(DateTime.Now.Date);
                    Thread.Sleep(60 * 60 * 1000);
                }
            }, _tokenSource.Token);
            _logger.LogInformation("BeinSport crawler is started");
        }

        private void Load(DateTime date)
        {
            _logger.LogDebug($"loading from day {date.ToShortDateString()}");
            for (int day = 0; day <= 9; day++)
            {
                var url = string.Format(BaseUrl, date.ToString("yyyy-MM-dd"));
                var html = HttpHelper.GetHtml(url);
                var doc = new HtmlDocument();
                doc.Load(new StringReader(html));
                HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//div[starts-with(@id, 'channels')]");
                foreach (var channelItem in links)
                {
                    var doc2 = new HtmlDocument();
                    doc2.Load(new StringReader(channelItem.InnerHtml));
                    var linkImg = doc2.DocumentNode.SelectNodes("//img")?.FirstOrDefault();
                    var chanelName = GetChanelName(linkImg.Attributes["src"].Value);
                    SaveChanel(chanelName);
                }
            }
        }

        private string GetChanelName(string value)
        {
            return "bein " + RegexChanelName.Match(value).Groups[1].Value;
        }
        private Chanel SaveChanel(string ChanelName)
        {
            var chanel = _Chanels.FirstOrDefault(c => c.ChanelNames.Any(cn => cn.Name.Contains(ChanelName.ToUpper())));
            if (chanel == null)
            {
                chanel = _ChanelRepository.Add(new Chanel
                {
                    Name = ChanelName,
                    Country = _country,
                    ChanelNames = new List<ChanelName>
                        {
                            new ChanelName{Name =ChanelName.ToUpper()}
                        }
                });
                _Chanels.Add(chanel);
            }
            return chanel;
        }


        public override void Stop()
        {
            _running = false;
            _tokenSource.CancelAfter(1000);
        }
    }
}
