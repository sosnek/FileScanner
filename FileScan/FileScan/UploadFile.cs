using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileScan
{
    class UploadFile
    {
        public static bool hasBeenUploaded = false;

        /// <summary>
        /// Creates a scan request if the file exists in the virusTotal Database
        /// </summary>
        /// <returns></returns>
        public static async Task<ScanResults> CreateScanReqAsync()
        {
            ScanResults scanResults = null;

            using HttpResponseMessage response = await APIHelper.ApiClient.GetAsync("https://www.virustotal.com/api/v3/files/"+ FileInfo.FileInfoInstance.MD5);
            if (response.IsSuccessStatusCode)
            {

                try
                {
                    //deserialize json into object
                    var jsonString = await response.Content.ReadAsStringAsync();
                    scanResults = ScanResults.FromJson(jsonString);
                    return scanResults;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            else
            {
                //File not found in virus total database.Need to upload it.
                if(!hasBeenUploaded)
                {
                    await UploadFileToVTotalAsync();
                    scanResults = await CreateScanReqAsync();
                }
            }
            return scanResults;
        }

        /// <summary>
        /// Uploads the file stream to the virus total API
        /// </summary>
        /// <returns></returns>
        public static async Task UploadFileToVTotalAsync()
        {
            var stream = File.OpenRead(FileInfo.FileInfoInstance.File_Path);
            HttpContent fileStreamContent = new StreamContent(stream);
            var formData = new MultipartFormDataContent();
            formData.Add(fileStreamContent, "file", FileInfo.FileInfoInstance.File_Path);

            using HttpResponseMessage postResponse = await APIHelper.ApiClient.PostAsync("https://www.virustotal.com/api/v3/files", formData);
            if (postResponse.IsSuccessStatusCode)
            {
                hasBeenUploaded = true;
                //https://www.virustotal.com/api/v3/files/id/analyse
                //Reanalize file to get all scan results
                await Task.Delay(5000);
                using HttpResponseMessage analyzeResponse = await APIHelper.ApiClient.PostAsync("https://www.virustotal.com/api/v3/files/"+FileInfo.FileInfoInstance.MD5+"/analyse", null);
                if (!analyzeResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show(analyzeResponse.ReasonPhrase);
                    throw new Exception(analyzeResponse.ReasonPhrase);
                }

            }
            else
            {
                MessageBox.Show(postResponse.ReasonPhrase);
                throw new Exception(postResponse.ReasonPhrase);
            }
        }

    }
}
