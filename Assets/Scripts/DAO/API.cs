using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;


public class API
{
    private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri(GlobalVariable.server_url) };
    public static Dictionary<string, string> Cookies = null;
    
    private static Dictionary<string, string> ExtractCookie(HttpResponseMessage httpResponse)
    {
        Dictionary<string, string> results = new Dictionary<string, string>();
        var requestCookie = httpResponse.Headers
            .SingleOrDefault(header => header.Key == "Set-Cookie").Value;
        
        var cookieList = requestCookie.ToList();
        if (cookieList.Count == 0) return null;

        foreach (var cookie in cookieList)
        {
            var decodeCookie = WebUtility.UrlDecode(cookie);
            var splitCookie = decodeCookie.Split("=");
            
            var key = splitCookie[0];
            var value = splitCookie[1].Split(";")[0];

            results[key] = value;
        }

        if (results["act"] == null) return null;

        return results;
    }
    
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

    public static async Task<BaseDTO<U>> Post<T, U>(string url, T data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data);
            var httpRequest = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(url, httpRequest);
            
            // if (Cookies == null)
            // {
            //     Cookies = ExtractCookie(httpResponse);
            //     if (Cookies != null && Cookies["act"] != null)
            //     {
            //         // Only set token if there is any cookies returned
            //         var act = Cookies["act"];
            //         Debug.Log("Set Access token Successfully");
            //
            //         _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //             "Beare",
            //             act
            //         );
            //     }
            // }
            
            var content = await httpResponse.Content.ReadAsStringAsync();
            Debug.Log(content);
            var response = JsonConvert.DeserializeObject<BaseDTO<U>>(content);
            Debug.Log(response);

            return response;
        }
        catch (Exception ex)
        {
            return new BaseDTO<U>
            {
                message = ex.Message,
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
    public static async Task<String> postMethod(string path, string content)
    {
        var data = new StringContent(content, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(path, data);
        string result = await response.Content.ReadAsStringAsync();
        return result;
    }
}

