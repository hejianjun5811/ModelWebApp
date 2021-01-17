using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApp.Model;
using Newtonsoft.Json;

namespace Application.Controllers
{
    /// <summary>
    /// 天气控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger
            //, IHttpClientFactory httpClientFactory
            )
        {
            _logger = logger;
            //_httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 获取天气信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 不想显示某些接口,[ApiExplorerSettings(IgnoreApi = true)]或者直接对这个方法 private，也可以直接使用obsolete属性
        /// </summary>
        /// <param name="love">model实体类参数</param> 
        [HttpPost("Post")]
        public void Post(Love love)
        {
        }


        /// <summary>
        /// 获取爱
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLoves")]
        public List<Love> GetLoves()
        {
            var loveList = new List<Love>();
            loveList.Add(new Love()
            {
                Id = 1,
                Age = 29,
                Name = "hjj",
            });

            loveList.Add(new Love()
            {
                Id = 2,
                Age = 29,
                Name = "lym",
            });

            loveList.Add(new Love()
            {
                Id = 3,
                Age = 3,
                Name = "hyk",
            });

            return loveList;
        }

        /// <summary>
        /// 获取抽奖结果记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetWinningNumbers")]
        public List<WinNumsDto> GetWinningNumbers()
        {
          
            var winNumList = new List<WinNumsDto>();

            for(int pageInt = 1; pageInt <= 70; pageInt++)
            {
                var url = $"https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=30&isVerify=1&pageNo={pageInt}";
                HttpResponseMessage response = _httpClient.GetAsync(url).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var rt = JsonConvert.DeserializeObject<WinningNumber>(responseBody);

                if (rt.success)
                {
                    if (rt.value != null && rt.value.list != null && rt.value.list.Count > 0)
                    {
                        var resultList = rt.value.list;
                        foreach (var item in resultList)
                        {
                            var winValList = item.lotteryDrawResult.Split(' ');
                            winNumList.Add(new WinNumsDto()
                            {
                                OneNum = Int32.Parse(winValList[0]),
                                TwoNum = Int32.Parse(winValList[1]),
                                ThreeNum = Int32.Parse(winValList[2]),
                                FourNum = Int32.Parse(winValList[3]),
                                FiveNum = Int32.Parse(winValList[4]),
                                SixNum = Int32.Parse(winValList[5]),
                                SevenNum = Int32.Parse(winValList[6]),
                            });
                        }
                    }
                }
            }
          
            //var clent = _httpClientFactory.CreateClient();

            //var httpResult =  clent.GetAsync("https://webapi.sporttery.cn/gateway/lottery/getHistoryPageListV1.qry?gameNo=85&provinceId=0&pageSize=30&isVerify=1&pageNo=70");

            return winNumList;
        }
    }
}
