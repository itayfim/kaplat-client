using System.Text;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace kaplat_Client
{
    class Program
    {
        public static void Main()
        {
            string baseUrl = "http://localhost:8989";
            string id = "312182009";
            string year = "1994";

            using (HttpClient client = new HttpClient())
            {
                // TASK 1:
                string getEndpoint = "/test_get_method";
                HttpResponseMessage getResponse = client.GetAsync($"{baseUrl}{getEndpoint}?id={id}&year={year}").Result;
                string getResponseStr = getResponse.Content.ReadAsStringAsync().Result;

                // TASK 2:
                string postEndpoint = "/test_post_method";
                JObject postBody = new JObject
                {
                    { "id", id },
                    { "year", year },
                    { "requestId", getResponseStr }
                };
                HttpContent postContent = new StringContent(postBody.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage postResponse = client.PostAsync(baseUrl + postEndpoint, postContent).Result;
                string postResponseStr = postResponse.Content.ReadAsStringAsync().Result;

                // TASK 3:
                string putEndpoint = "/test_put_method";
                JObject putBody = new JObject
                {
                    { "id", (int.Parse(id) - 294234) % 34 }, // 29
                    { "year", (int.Parse(year) + 94) % 13 } // 8
                };
                HttpContent putContent = new StringContent(putBody.ToString(), Encoding.UTF8, "application/json");
                string messageId = JObject.Parse(postResponseStr)["message"].ToString();
                HttpResponseMessage putResponse = client.PutAsync($"{baseUrl}{putEndpoint}?id={messageId}", putContent).Result;
                string putResponseStr = putResponse.Content.ReadAsStringAsync().Result;

                // TASK 4:
                string deleteEndpoint = "/test_delete_method";
                messageId = JObject.Parse(putResponseStr)["message"].ToString();
                HttpResponseMessage deleteResponse = client.DeleteAsync($"{baseUrl}{deleteEndpoint}?id={messageId}").Result;
            }

            Application.Exit();
        }
    }
}
