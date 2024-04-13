using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


public class API
{
    private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(GlobalVariable.server_url) };

    public static async Task<Tuple<Boolean, String>> testConnection()
    {
        try
        {
            var response = await _httpClient.GetAsync("/");
            var stringResult = await response.Content.ReadAsStringAsync();
            return Tuple.Create<Boolean, String>(true, "Connect web service successfully");
        }
        catch (Exception ex)
        {
            return Tuple.Create<Boolean, String>(false, ex.Message);
        }
    }

    public static async Task<String> getMethod(string path)
    {
        try
        {
            var response = await _httpClient.GetAsync(path); //GET 127.0.0.1:5000/category
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return "";
        }

    }
    public static async Task<String> postMethod(string path, string content)
    {
        var data = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(path, data);
        string result = await response.Content.ReadAsStringAsync();
        return result;
    }
}

