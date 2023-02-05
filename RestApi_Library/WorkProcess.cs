using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using RestApi_Library.Models;

namespace RestApi_Library
{
    public static class WorkProcess
    {
        public static async Task<ModelComic> Load(int num = 0)
        {
            string url = "";

            if (num > 0)
            {
                url = $"https://xkcd.com/{num}/info.0.json";
            }
            else
            {
                url = $"https://xkcd.com/info.0.json";
            }

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    ModelComic modelComic = await response.Content.ReadAsAsync<ModelComic>();

                    return modelComic;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
