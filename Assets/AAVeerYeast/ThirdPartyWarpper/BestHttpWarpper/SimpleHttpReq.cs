using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 简单的 HttpRequest 需求应使用 SimpleHttpReq 完成
// 目的在于减少工程复杂性
// 1.限制 HttpRequest 的使用方式
// 2.统一工程中对 HttpRequestResult 处理的方式
// 3.增加 Retry 功能

namespace BestHTTP
{
    public enum SimpleHttpResult
    {
        Success,
        FailedForNetwork,
        FailedFor400,
        Abort
    }

    // 之后可能会加入 Error 信息输出
    public delegate void SimpleHttpCallback(SimpleHttpResult result, HTTPResponse res, int reqId);

    public class SimpleHttpReq : IAbortable
    {
        public int ReqId = int.MinValue;
        public HTTPRequest Req = null;

        // 默认 simple http request 会 重试 3 次
        private int _RetryCount = 3;
        private int _CurrentRetryCount = 0;

        #region Ctor

        public SimpleHttpReq(System.Uri uri, int reqId = int.MinValue)
        {
            ReqId = reqId;
            Req = new HTTPRequest(uri);
        }

        public SimpleHttpReq(System.Uri uri, SimpleHttpCallback callback, int reqId = int.MinValue)
        {
            ReqId = reqId;
            Req = new HTTPRequest(uri, (req, res) =>
            {
                CallbackConvert(this, res, callback);
            });
        }

        public SimpleHttpReq(System.Uri uri, bool isKeepAlive, SimpleHttpCallback callback, int reqId = int.MinValue)
        {
            ReqId = reqId;
            Req = new HTTPRequest(uri, isKeepAlive, (req, res) =>
            {
                CallbackConvert(this, res, callback);
            });
        }

        public SimpleHttpReq(System.Uri uri, bool isKeepAlive, bool disableCache, SimpleHttpCallback callback, int reqId = int.MinValue)
        {
            ReqId = reqId;
            Req = new HTTPRequest(uri, isKeepAlive, disableCache, (req, res) =>
            {
                CallbackConvert(this, res, callback);
            });
        }

        #endregion

        public bool IsProcessing()
        {
            return Req.State == HTTPRequestStates.Queued || Req.State == HTTPRequestStates.Processing;
        }

        public void SetRetryCount(int retryCount)
        {
            _RetryCount = retryCount;
        }

        public SimpleHttpReq SetMethodType(HTTPMethods methodType)
        {
            Req.MethodType = methodType;
            return this;
        }

        public SimpleHttpReq AddHeader(string name, string value)
        {
            Req.AddHeader(name, value);
            return this;
        }

        public SimpleHttpReq AddField(string name, string value)
        {
            // songlingyi 很诡异的 Bug
            // 对 GET_LEADERBOARD_VIDEO_BY_PAGE AddField 失效

            // old function
            //Req.AddField(name, value);

            // new function
            string url = Req.Uri.ToString();
            if (url.Contains("?"))
                url += "&" + name + "=" + value;
            else
                url += "?" + name + "=" + value;
            Req.Uri = url.ToUrl();

            return this;
        }

        public SimpleHttpReq SetRawData(byte[] rawData)
        {
            Req.RawData = rawData;
            return this;
        }

        public SimpleHttpReq Send(bool bRetry = false)
        {
            if (!bRetry)
            {
                _CurrentRetryCount = 0;
            }

            Req.Send();
            return this;
        }

        public void Abort()
        {
            if (Req.State < HTTPRequestStates.Finished && Req.State != HTTPRequestStates.Initial)
            {
                Req.Abort();
            }
            //Req.Dispose();
        }

        public void Dispose()
        {
            if (Req.State < HTTPRequestStates.Finished && Req.State != HTTPRequestStates.Initial)
            {
                Req.Abort();
            }
            Req.Dispose();
        }

        public static void CallbackConvert(SimpleHttpReq simpleReq, HTTPResponse res, SimpleHttpCallback callback)
        {
            // 0.如果 callbacck 为 null 则直接 return
            if (callback == null)
            {
                return;
            }

            // 1.请求成功则直接 callback
            if (res != null)
            {
                if (res.StatusCode == 200 || res.StatusCode == 304)
                {
                    callback(SimpleHttpResult.Success, res, simpleReq.ReqId);
                    return;
                }
                else if (res.StatusCode == 400)
                {
                    // code 400 说明服务器正常 但是 数据有错误 所以 不需要再 retry 了
                    VeerDebug.LogWarning(" req failed with code 400 : " + simpleReq.Req.Uri + " error msg : " + res.Message);
                    callback(SimpleHttpResult.FailedFor400, res, simpleReq.ReqId);
                    return;
                }
                else if (res.StatusCode == 404)
                {
                    // code 404 地址不存在 不需要再 retry 了
                    //VeerDebug.LogWarning(" req failed with code 404 : " + simpleReq.Req.Uri + " error msg : " + res.Message);
                    callback(SimpleHttpResult.FailedForNetwork, res, simpleReq.ReqId);
                    return;
                }
            }

            // 2.Abort 则 直接 callback
            if (simpleReq.Req.State == HTTPRequestStates.Aborted)
            {
                callback(SimpleHttpResult.Abort, res, simpleReq.ReqId);
                return;
            }

            // 3.其它情况需要尝试 retry
            if (simpleReq._CurrentRetryCount < simpleReq._RetryCount)
            {
                simpleReq._CurrentRetryCount++;
                VeerDebug.Log(" retry http req time : " + simpleReq._CurrentRetryCount + " api : " + simpleReq.Req.Uri + " code : " + res.StatusCode);
                simpleReq.Send(true);
                return;
            }

            // 4.retry 结束后返回 callback
            if (simpleReq.Req.State == HTTPRequestStates.Error)
            {
                VeerDebug.Log(" request error : " + simpleReq.Req.Exception);

                callback(SimpleHttpResult.FailedForNetwork, res, simpleReq.ReqId);
                return;
            }

            if (simpleReq.Req.State == HTTPRequestStates.ConnectionTimedOut || simpleReq.Req.State == HTTPRequestStates.TimedOut)
            {
                VeerDebug.Log(" request time out ...");

                callback(SimpleHttpResult.FailedForNetwork, res, simpleReq.ReqId);
                return;
            }

            callback(SimpleHttpResult.FailedForNetwork, res, simpleReq.ReqId);
        }
    }
}