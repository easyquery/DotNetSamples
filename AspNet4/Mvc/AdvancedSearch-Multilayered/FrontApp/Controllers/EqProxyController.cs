using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EqDemo.Controllers
{
    [RoutePrefix("eqproxy")]
    public class EqProxyController : Controller
    {

        private readonly HttpClient _httpClient;

        public EqProxyController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44331/api/easyquery/"); // service app path
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("lck")]
        public async Task<ActionResult> GetLicenseKeyAsync(CancellationToken ct)
        {
            var response = await _httpClient.GetAsync("lck", ct);
            var key = await response.Content.ReadAsStringAsync();
            return Content(key.Substring(1, key.Length - 2));
        }

        [HttpGet]
        [Route("models/{modelId}")]
        public async Task<ActionResult> GetModelAsync(string modelId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"models/{modelId}", ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpGet]
        [Route("models/{modelId}/queries/{queryId}")]
        public async Task<ActionResult> GetQueryAsync(string modelId, string queryId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"models/{modelId}/queries/{queryId}", ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpGet]
        [Route("models/{modelId}/queries")]
        public async Task<ActionResult> GetQueryListAsync(string modelId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"models/{modelId}/queries", ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpPost]
        [Route("models/{modelId}/queries")]
        public async Task<ActionResult> NewQueryAsync(string modelId, CancellationToken ct)
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyJson = await new StreamReader(stream).ReadToEndAsync();

            var requestContent = new StringContent(bodyJson);
            var response = await _httpClient.PostAsync($"models/{modelId}/queries", requestContent, ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpPut]
        [Route("models/{modelId}/queries/{queryId}")]
        public async Task<ActionResult> SaveQueryAsync(string modelId, string queryId, CancellationToken ct)
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyJson = await new StreamReader(stream).ReadToEndAsync();

            var requestContent = new StringContent(bodyJson);
            var response = await _httpClient.PutAsync($"models/{modelId}/queries/{queryId}", requestContent, ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpDelete]
        [Route("models/{modelId}/queries/{queryId}")]
        public async Task<ActionResult> RemoveQueryAsync(string modelId, string queryId, CancellationToken ct)
        {
            var response = await _httpClient.DeleteAsync($"models/{modelId}/queries/{queryId}", ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpPost]
        [Route("models/{modelId}/queries/{queryId}/sync")]
        public async Task<ActionResult> SyncQueryAsync(string modelId, string queryId, CancellationToken ct)
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyJson = await new StreamReader(stream).ReadToEndAsync();

            var requestContent = new StringContent(bodyJson);
            var response = await _httpClient.PostAsync($"models/{modelId}/queries/{queryId}/sync", requestContent, ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpPost]
        [Route("models/{modelId}/fetch")]
        public async Task<ActionResult> FetchDataAsync(string modelId, CancellationToken ct)
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyJson = await new StreamReader(stream).ReadToEndAsync();

            var requestContent = new StringContent(bodyJson);
            var response = await _httpClient.PostAsync($"models/{modelId}/fetch", requestContent, ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpGet]
        [Route("models/{modelId}/valuelists/{editorId}")]
        public async Task<ActionResult> GetValueListAsync(string modelId, string editorId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"models/{modelId}/valuelists/{editorId}", ct);
            var content = await response.Content.ReadAsStringAsync();
            return JsonContent(content, response.StatusCode);
        }

        [HttpPost]
        [Route("models/{modelId}/export/{formatType}")]
        public async Task<ActionResult> ExportResultAsync(string modelId, string formatType, CancellationToken ct)
        {
            var stream = Request.InputStream;
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyJson = await new StreamReader(stream).ReadToEndAsync();

            var requestContent = new StringContent(bodyJson);
            var response = await _httpClient.PostAsync($"models/{modelId}/export/{formatType}", requestContent, ct);
            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType.MediaType;
                var fileName = response.Content.Headers.ContentDisposition.FileName;
                return File(content, contentType, fileName.Substring(1, fileName.Length - 2));
            }
            else {
                var content = await response.Content.ReadAsStringAsync();
                return JsonContent(content, response.StatusCode);
            }
        }
    
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _httpClient.Dispose();
        }

        private static UTF8Encoding _utf8NoBomEncoding = new UTF8Encoding(false);
        protected ContentResult JsonContent(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            HttpContext.Response.StatusCode = (int)statusCode;
            return Content(json, "application/json", _utf8NoBomEncoding);
        }
    }
}