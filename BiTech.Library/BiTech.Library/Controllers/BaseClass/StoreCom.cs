using BiTech.Library.Areas.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BiTech.Library.Controllers.BaseClass
{
    public class StoreCom
    {
        internal async Task<dynamic> GetChildWorkPlace(string wpId, string site, string appCode)
        {
            using (var client = new HttpClient())
            {
                site = site.Replace("http://", "").Replace("https://", "");

                client.BaseAddress = new Uri("http://" + site);

                //var content = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");

                var result = await client.GetAsync("/api/BiTechAppCenter/GetSubWorkPlace" + "?id=" + wpId + "&appCode=" + appCode);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    List<WorkPlaceApiModel> list = JsonConvert.DeserializeObject<List<WorkPlaceApiModel>>(resultContent);

                    return list;
                    //Console.WriteLine(resultContent);
                }
                return null;
            }
        }
    }
}