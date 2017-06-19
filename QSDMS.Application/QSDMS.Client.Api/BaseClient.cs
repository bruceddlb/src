using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using QSDMS.Util;
using iFramework.Framework;

namespace QSDMS.Client.Api
{
    public class BaseClient
    {
        protected static readonly string host = System.Configuration.ConfigurationManager.AppSettings["ApiHost"];
        protected static readonly string traderHost = System.Configuration.ConfigurationManager.AppSettings["TraderHost"];
        protected static readonly string mallHost = System.Configuration.ConfigurationManager.AppSettings["MallHost"];
        protected static readonly string storeHost = System.Configuration.ConfigurationManager.AppSettings["StoreHost"];
        protected static readonly string appHost = System.Configuration.ConfigurationManager.AppSettings["AppHost"];
        protected static readonly string fileHost = System.Configuration.ConfigurationManager.AppSettings["FileHost"];
        protected static readonly string appID = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
        protected static readonly string appSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"];
        protected static readonly string noticeName = System.Configuration.ConfigurationManager.AppSettings["NoticeName"];

        private string accessToken;

        /// <summary>
        /// http
        /// </summary>
        public HttpMethod http = new HttpMethod();

        /// <summary>
        /// Cookie
        /// </summary>
        public static string Cookie = "";

        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get { return GeneralAccessToken(this.AppID, this.AppSecret); } }
        /// <summary>
        /// 对应微信后台开发者凭证的AppId
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 对应微信后台开发者凭证的AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// API HOST
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appSecret"></param>
        /// <param name="force">是否强制获取AccessToken</param>
        public BaseClient(string appID, string appSecret, bool force)
        {
            AppID = appID;
            AppSecret = appSecret;
            Host = host;
        }

        /// <summary>
        /// redisClient
        /// </summary>
        //public static RedisCache redisClient = null;
        public BaseClient(bool force = false)
        {
            AppID = appID;
            AppSecret = appSecret;
            Host = host;
        }

        /// <summary>
        /// 获取accessToken
        /// </summary>
        /// <param name="appID">微信的AppKey</param>
        /// <param name="appSecret"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public string GeneralAccessToken(string appID, string appSecret, bool force = false)
        {
            string tokenUrl = string.Format("{0}/App/Token?appid={1}&secret={2}", Host, appID, appSecret);

            string key = string.Format("{0}_AccessToken", appID);

            CacheAccessTokenInfo accessToken = null;
            string accessTokenString = CacheHelper.Instance.Get(key) as string;

            if (!string.IsNullOrWhiteSpace(accessTokenString) && !force)
            {
                try
                {
                    accessToken = Newtonsoft.Json.JsonConvert.DeserializeObject<CacheAccessTokenInfo>(accessTokenString);
                }
                catch { }
            }

            if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.AccessToken) || DateTime.Now.Subtract(accessToken.CreateTime).TotalMinutes >= 100)
            {
                accessToken = new CacheAccessTokenInfo();
                accessTokenString = new HttpMethod().HttpGet(tokenUrl, UTF8Encoding.UTF8, ref Cookie);

                accessToken.CreateTime = DateTime.Now;

                try
                {
                    var jObject = JsonConvert.DeserializeObject(accessTokenString) as JObject;

                    if (jObject != null && (bool)jObject["IsSuccess"])
                    {
                        accessToken.AccessToken = jObject["ResultData"].Value<string>("access_token");
                    }
                }
                catch { }
                if (!string.IsNullOrWhiteSpace(accessToken.AccessToken))
                {
                    string cacheString = JsonConvert.SerializeObject(accessToken);
                    CacheHelper.Instance.Update(key, cacheString, 60);
                }
            }

            return accessToken.AccessToken;
        }

        public ReturnMessage Get(string url)
        {
            var result = new ReturnMessage(false) { Message = "获取接口信息失败!" };
            try
            {
                string resp = new HttpMethod().HttpGet(url, UTF8Encoding.UTF8, ref Cookie);
                result = JsonConvert.DeserializeObject<ReturnMessage>(resp);
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = this.ToString() + ">>Get";
                new ExceptionHelper().LogException(ex);
            }
            return result;
        }

        public ReturnMessage Post(string url, string data)
        {
            var result = new ReturnMessage(false) { Message = "获取接口信息失败!" };
            try
            {
                string resp = new HttpMethod().HttpPost(url, data, Encoding.UTF8, ref Cookie);
                result = JsonConvert.DeserializeObject<ReturnMessage>(resp);
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = this.ToString() + "\r\n" + url + "\r\n" + data;
                new ExceptionHelper().LogException(ex);
            }
            return result;
        }

        public ReturnMessage Post(string url, string data, string filePath)
        {
            var result = new ReturnMessage(false) { Message = "获取接口信息失败!" };
            try
            {
                string resp = new HttpMethod().HttpPost(url, filePath, data, ref Cookie, Encoding.UTF8);
                result = JsonConvert.DeserializeObject<ReturnMessage>(resp);
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = this.ToString() + ">>Post";
                new ExceptionHelper().LogException(ex);
            }
            return result;
        }

        public ReturnMessage Post(string url, string data, HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "获取接口信息失败!" };
            try
            {
                byte[] bytes = null;
                if (file != null)
                {
                    bytes = StreamToBytes(file);
                }

                string resp = new HttpMethod().HttpPost(url, file.FileName, bytes, data, ref Cookie, Encoding.UTF8);
                result = JsonConvert.DeserializeObject<ReturnMessage>(resp);
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = this.ToString() + ">>Post";
                new ExceptionHelper().LogException(ex);
            }
            return result;
        }

        public byte[] StreamToBytes(HttpPostedFileBase file)
        {
            byte[] bytes = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                bytes = binaryReader.ReadBytes(file.ContentLength);
            }
            return bytes;
        }
    }

    /// <summary>
    /// CacheAccessTokenInfo
    /// </summary>
    public class CacheAccessTokenInfo
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
