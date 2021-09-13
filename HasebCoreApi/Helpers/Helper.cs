using System;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Net.Http;
using HasebCoreApi.Helpers.Common;
using Newtonsoft.Json;
using MimeKit;
using MailKit.Security;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentFTP;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using HasebCoreApi.Services.Commodities;
using System.Text;
using System.IO;
using System.Net;

namespace HasebCoreApi.Helpers
{
    public static class Helper
    {
        static readonly HttpClient client = new HttpClient();

        public static string GetOTP()
        {
            Random random = new Random();
            int otp = random.Next(10000, 99999);
            return otp.ToString();
        }

        public static string GetUserId(this ClaimsPrincipal User)
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string DeleteFile(FTPServer _ftp, string FileNameAndPath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftp.FTPUrl + FileNameAndPath);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(_ftp.Username, _ftp.Password);

            using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            return response.StatusDescription;
        }

        public static List<string> InsertFileToFtp(this List<IFormFile> files, FTPServer _ftp, string minPath)
        {
            if (files.Count != 0)
            {
                var filePath = new List<string>();
                if (files.Sum(x => x.Length) > files.Count * 1000000) throw new ImageLengthSomuchException();
                foreach (var item in files)
                {

                    //-----------------------------------------------------------              
                    var hashedName = DateTime.Now.Ticks.ToString();
                    var filename = Convert.ToBase64String(Encoding.UTF8.GetBytes(hashedName));
                    string urlForSave = minPath + filename + Path.GetExtension(item.FileName);
                    filePath.Add(urlForSave);
                    //-----------------------------------------------------------
                    var request = (FtpWebRequest)WebRequest.Create(_ftp.FTPUrl + urlForSave);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(_ftp.Username, _ftp.Password);


                    byte[] buffer = new byte[2048];
                    var stream = item.OpenReadStream();
                    byte[] fileContents;
                    //-----------------------------------------------------------
                    using (var ms = new MemoryStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        fileContents = ms.ToArray();
                    }
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(fileContents, 0, fileContents.Length);
                    }

                }
                return filePath;
            }
            return null;
        }

        public static async Task<int> SendSMS(string message, string mobile)
        {
            string url = string.Format("https://raygansms.com/SendMessageWithUrl.ashx?Username=hasebpasargad1&Password=ASDF@4112&PhoneNumber=50002210003000&MessageBody={0}&RecNumber=0{1}&Smsclass=1", message, mobile);
            HttpResponseMessage response = await client.GetAsync(url);

            return 1;
        }

        [Obsolete]
        public static async Task<int> SendEmail(SmtpConfig smtpConfig, string subject, string emailTo, string otp)
        {
            // Email Body
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtpConfig.From, smtpConfig.From));
            email.To.Add(new MailboxAddress(emailTo));
            email.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = @"Click on The Link: https://www.Google.com and with this code : " + otp;
            email.Body = bodyBuilder.ToMessageBody();

            // Email Connection Properities
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(smtpConfig.SmtpHost, smtpConfig.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpConfig.SmtpUser, smtpConfig.SmtpPassword);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            return 1;
        }

        public static async Task<ApiResult> ToJson(this HttpResponseMessage res)
        {
            if (res.RequestMessage.Content != null)
                Console.WriteLine(await res.RequestMessage.Content.ReadAsStringAsync());
            var content = await res.Content.ReadAsStringAsync();
            var apiResult = new ApiResult();
            JsonConvert.PopulateObject(content, apiResult);

            return apiResult;
        }

        public static DateTime GetOTPRemainTime(double time)
        {
            return DateTime.Now.AddMinutes(-time);
        }

        public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string requestUri, object jsonData) where T : new()
        {
            var result = await httpClient.PostAsync(requestUri,
                new StringContent(JsonConvert.SerializeObject(jsonData))
                {
                    Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json") }
                }
                );
            var content = await result.Content.ReadAsStringAsync();
            var obj = new T();
            JsonConvert.PopulateObject(content, obj);
            return obj;
        }

        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string requestUri) where T : new()
        {
            var result = await httpClient.GetAsync(requestUri);
            var content = await result.Content.ReadAsStringAsync();
            var obj = new T();
            JsonConvert.PopulateObject(content, obj);
            return obj;
        }

        public static Task<List<TSource>> ToListAsyncSafe<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (!(source is IAsyncEnumerable<TSource>))
                return Task.FromResult(source.ToList());
            return source.ToListAsync();
        }

        public static string GetError(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors).FirstOrDefault().ErrorMessage;
        }
    }
}



