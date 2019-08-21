using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using CsvHelper;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HttpTriggerCSVtoJson
{
    public static class Function1
    {
        [FunctionName("CSVtoJSON")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route =null)] HttpRequest req
        , [Blob("hotfile/{query.filename}", FileAccess.Read, Connection = "AzureWebJobsStorage")]Stream myBlob, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            
            string name = req.Query["name"];
            var json = Convert(myBlob);
            return (ActionResult)new OkObjectResult(json);
           
        }
        public static string Convert(Stream blob)
        {
            // Properties per = new Properties();
            //List<Properties> records = new List<Properties>();
            var sReader = new StreamReader(blob);
            var csv = new CsvReader(sReader);

            csv.Read();
            csv.ReadHeader();

            //*** To add into list without customization ***//
            var csvRecords = csv.GetRecords<object>().ToList();

            return JsonConvert.SerializeObject(csvRecords);
        }
      
    }
}
