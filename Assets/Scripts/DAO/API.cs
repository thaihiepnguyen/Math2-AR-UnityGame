using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;


public class API
{
    private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(GlobalVariable.server_url) };
    public static Dictionary<string, string> Cookies = new Dictionary<string, string>();
    
    private static void ExtractCookie(IEnumerable<string> requestCookie)
    {
        if (requestCookie == null) return;
        
        var cookieList = requestCookie.ToList();
        
        foreach (var cookie in cookieList)
        {
            var decodeCookie = WebUtility.UrlDecode(cookie);
            var splitCookie = decodeCookie.Split("=");
            
            var key = splitCookie[0];
            var value = splitCookie[1].Split(";")[0];
        
            Cookies[key] = value;
        }
    }
    
    // T: is data type of the data inside response from server
    public static async Task<BaseDTO<T>> Get<T>(string url)
    {
        try
        {
            var httpResponse = await _httpClient.GetAsync(url);
           
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseDTO<T>>(content);

            return response;
        } catch (Exception ex)
        {
            return new BaseDTO<T>
            {
                message = ex.Message,
                isSuccessful = false
            };
        }
    }

    // T: is data type that u wanna post to server
    // U: is data type of the data inside response from server
    public static async Task<BaseDTO<U>> Post<T, U>(string url, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var httpRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(url, httpRequest);

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseDTO<U>>(content);
            var requestCookie = httpResponse.Headers
            .SingleOrDefault(header => header.Key == "Set-Cookie").Value;
            ExtractCookie(requestCookie);
            if (Cookies.ContainsKey("act"))
            {
                // Only set token if there is any cookies returned
                var act = Cookies["act"];
            
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    act
                );

                if (GlobalVariable.IS_REMEMBER_ME) {
                    PlayerPrefs.SetString("token", act);
                }
            }

            return response;
        }
        catch (Exception ex)
        {
            return new BaseDTO<U>
            {
                message = "Internal Error Code",
                isSuccessful = false
            };
        }
    }

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
    
    // Can use above function instead this.
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
            return "";
        }

    }
    
    // Can use above function instead this.
    public static async Task<String> postMethod(string path, string content)
    {
        var data = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(path, data);
        string result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public static void AddToken(string token) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );
    }
}

