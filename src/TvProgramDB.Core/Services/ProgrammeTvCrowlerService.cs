using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Interfaces;
using TvProgramDB.Core.Specifications;

namespace TvProgramDB.Core.Services
{
    public class ProgrammeTvCrowlerService : CrawlerServiceBase
    {
        /// <summary>
        /// example : http://www.programme-tv.net/programme/toutes-les-chaines/2017-11-18/12.html
        /// </summary>
        const string BaseUrl = "http://www.programme-tv.net/programme/toutes-les-chaines/{0}/{1}.html";
        private bool _running;
        private CancellationTokenSource _tokenSource;
        private ILogger _logger;
        private IRepository<Country> _countryRepository;
        private IRepository<Source> _sourceRepository;
        private IRepository<Chanel> _ChanelRepository;
        private IRepository<Program> _programRepository;
        private int _sourceId;
        private List<Chanel> _Chanels;
        private Country _country;
        private readonly Regex RegexThemeDuration = new Regex(@"^(.*) \((\d\d[h]\d\d)\)$");

        public ProgrammeTvCrowlerService(
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
            _logger.LogInformation("Initializing Programme-tv crawler");

            _country = _countryRepository.GetSingleBySpec(new CountryCodeSpecification("FR"));
            if (_country == null)
            {
                _country = _countryRepository.Add(new Country { Code="FR", Name = "France" });
            }

            var source = _sourceRepository.GetSingleBySpec(new SourceCodeSpecification("PTV.NET"));
            if (source == null)
            {
                source = _sourceRepository.Add(new Source {  Code = "PTV.NET", Name = "programme-tv.net" });
            }
            _sourceId = source.Id;

            _Chanels = _ChanelRepository.ListAll(nameof(Chanel.ChanelNames)).ToList();
            
        }

        public override void Start()
        {
            if (_running) return;
            _logger.LogInformation("Starting Programme-tv crawler");
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
            _logger.LogInformation("Programme-tv crawler is started");
        }

        private void Load(DateTime now)
        {
            _logger.LogDebug($"loading from day {now.ToShortDateString()}");
            for (int day = 0; day <= 9; day++)
            {
                LoadDay(now.AddDays(day));
            }
        }

        private void LoadDay(DateTime day)
        {
            _logger.LogDebug($"loading day {day.ToShortDateString()}");
            for (int slot = 1; slot <= 12; slot++)
            {
                LoadDaySlot(day, slot);
            }
        }

        private void LoadDaySlot(DateTime day, int slot)
        {
            string url = string.Format(BaseUrl, day.ToString("yyyy-MM-dd"),slot);
            _logger.LogDebug($"loading slot {slot} ({url})");
            string html = GetHtml(url);
            var doc = new HtmlDocument();
            doc.Load(new StringReader(html));
            HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//div[@class='clearfix p-v-md bgc-white bb-grey-3']");

            foreach (var a in links)
            {
                var text = WebUtility.HtmlDecode(a.InnerText);
                var values = text.Split("\n")
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();
                var ChanelName = values[0].Replace("Programme", "");
                var chanel = SaveChanel(ChanelName);

                for (int i=1;i<values.Count();i=i+3)
                {
                    var programStart = values[i];
                    var programName = values[i + 1];
                    var theme = RegexThemeDuration.Match(values[i + 2]).Groups[1].Value;
                    var duration = RegexThemeDuration.Match(values[i + 2]).Groups[2].Value;

                    SaveProgram(day,slot,chanel, programStart, programName, theme, duration);
                }
            }
        }

        private void SaveProgram(DateTime day,int slot, Chanel chanel, string programStart, string programName, string theme, string duration)
        {
            var start = FormatProgramStart(day, slot, programStart);
            var program = _programRepository.GetSingleBySpec(new ProgramStartNearSpecification(chanel.Id,start));
            if (program != null) return;

            program = new Program
            {
                Name = programName,
                Description = programName,
                ProgramType = GetProgramType(theme),
                StartDate = start,
                EndDate = FormatProgramEnd(start, duration),
                Chanel = chanel
            };

            _programRepository.Add(program);
        }

        private DateTime FormatProgramEnd(DateTime start,  string duration)
        {
            var nbHour = int.Parse(duration.Substring(0, 2));
            var nbMinute = int.Parse(duration.Substring(3, 2));
            return start.AddHours(nbHour).AddMinutes(nbMinute);
        }

        private DateTime FormatProgramStart(DateTime day, int slot, string programStart)
        {
            var nbHour = int.Parse(programStart.Substring(0, 2));
            var nbMinute = int.Parse(programStart.Substring(3, 2));
            if (slot == 1 && nbHour>12)
            {
                return day.AddDays(-1).AddHours(nbHour).AddMinutes(nbMinute);
            }
            else
            {
                return day.AddHours(nbHour).AddMinutes(nbMinute);
            }
        }

        private ProgramType GetProgramType(string theme)
        {
            if ("Téléfilm".Equals(theme, StringComparison.InvariantCultureIgnoreCase))
                return ProgramType.Serie;
            if ("Cinéma".Equals(theme, StringComparison.InvariantCultureIgnoreCase))
                return ProgramType.Serie;
            if ("Série TV".Equals(theme, StringComparison.InvariantCultureIgnoreCase))
                return ProgramType.Serie;
            return ProgramType.Other;
        }

        private Chanel SaveChanel(string ChanelName)
        {
            var chanel = _Chanels.FirstOrDefault(c=>c.ChanelNames.Any(cn=>cn.Name.Contains(ChanelName.ToUpper())));
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

        private string GetHtml(string url)
        {
            string result = "";
            var myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            using (var myResponse = myRequest.GetResponse())
            {
                using (var sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                myResponse.Close();
            }
            return result;
        }

        public override void Stop()
        {
            _running = false;
            _tokenSource.CancelAfter(1000);
        }
    }
}
