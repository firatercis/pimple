using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SupersonicWisdomSDK
{
    internal class SwUnityWebRequestClient : ISwWebRequestClient
    {
        #region --- Public Methods ---

        public IEnumerator Get(string url, SwWebResponse response, int timeout = 0, Dictionary<string, string> headers = null, bool logResponseText = false)
        {
            var webRequest = TryCreateWebRequest(url, timeout, response);

            if (webRequest == null) yield break;

            webRequest.method = UnityWebRequest.kHttpVerbGET;
            SwInfra.Logger.Log(() => $"SwUnityWebRequestClient | {webRequest.method} | url={webRequest.url} | timeout={timeout} | headers = {SwJsonParser.Serialize(headers)}");

            yield return SendWebRequest(webRequest, response, headers, logResponseText);
            webRequest.Dispose();
        }

        public IEnumerator Post(string url, object data, SwWebResponse response, int timeout = 0, Dictionary<string, string> headers = null, bool logResponseText = false)
        {
            var webRequest = TryCreateWebRequest(url, timeout, response);

            if (webRequest == null) yield break;
            var body = data as string ?? JsonUtility.ToJson(data);
            webRequest.method = UnityWebRequest.kHttpVerbPOST;

            if (!string.IsNullOrEmpty(body))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(body));
                webRequest.SetRequestHeader("Content-Type", "application/json");
            }

            SwInfra.Logger.Log(() => $"SwUnityWebRequestClient | {webRequest.method} | url={webRequest.url} | body={body} | timeout={timeout} | headers = {SwJsonParser.Serialize(headers)}");

            yield return SendWebRequest(webRequest, response, headers, logResponseText);
            webRequest.Dispose();
        }

        #endregion


        #region --- Private Methods ---

        private UnityWebRequest CreateWebRequest(string url, int timeout)
        {
            var webRequest = new UnityWebRequest(url);
            webRequest.timeout = timeout;
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            return webRequest;
        }

        private IEnumerator SendWebRequest(UnityWebRequest webRequest, SwWebResponse response, Dictionary<string, string> headers, bool logResponseText = false)
        {
            if (headers != null)
            {
                foreach (var keyValuePair in headers)
                {
                    webRequest.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                }
            }

            response.isPending = true;

            yield return webRequest.SendWebRequest();
            response.isPending = false;
            response.data = webRequest.downloadHandler.data;
            response.code = webRequest.responseCode;
            response.isDone = true;

            if (webRequest.isHttpError)
            {
                response.error = SwWebRequestError.Http;
                SwInfra.Logger.LogError($"SwUnityWebRequestClient | {webRequest.method} {webRequest.url} fail | error={SwWebRequestError.Http} | responseCode={webRequest.responseCode}");
            }
            else if (webRequest.isNetworkError)
            {
                if (!string.IsNullOrEmpty(webRequest.error) && webRequest.error.EndsWith("timeout"))
                {
                    response.error = SwWebRequestError.Timeout;
                }
                else
                {
                    response.error = SwWebRequestError.Network;
                }

                SwInfra.Logger.LogError($"SwUnityWebRequestClient | {webRequest.method} {webRequest.url} fail | error={response.error}");
            }
            else
            {
                SwInfra.Logger.Log(() =>
                {
                    var logMessage = $"SwUnityWebRequestClient | {webRequest.method} {webRequest.url} success | responseCode={webRequest.responseCode}";

                    if (logResponseText)
                    {
                        logMessage += $" | responseText {response.Text}";
                    }

                    return logMessage;
                });
            }
        }

        private UnityWebRequest TryCreateWebRequest(string url, int timeout, SwWebResponse response)
        {
            try
            {
                return CreateWebRequest(url, timeout);
            }
            catch (UriFormatException)
            {
                SwInfra.Logger.LogError(() => $"TryCreateWebRequest | Invalid URL: {url}");
                response.error = SwWebRequestError.InvalidUrl;
                response.isDone = true;
            }

            return null;
        }

        #endregion
    }
}