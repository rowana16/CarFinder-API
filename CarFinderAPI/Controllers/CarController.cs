﻿using CarFinderAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CarFinderAPI.Controllers
{
    
    [RoutePrefix("api/Car")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //============================================================================================
        //=============================== Stored Procedures ==========================================
        //============================================================================================

       

        //=============================== Get Unique Model Years ==========================================


        /// <summary>
        /// Get all available years represented in the database
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("Years")]
        public async Task<List<string>> GetYears()
        {
            return await db.GetUniqueModelYears();  //Stored Procedure Names
        }

        //=============================== Get Make By Year ==========================================

        /// <summary>
        /// Get Makes by Year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// 
        [Route("{year}/Makes")]
        public async Task<List<string>> GetMakesByYear(string year)
        {
            return await db.GetMakeByYear(year);
        }

        //=============================== Get Model By Year and Make ==========================================

        /// <summary>
        /// Get Models by Year and Make
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns></returns>
        [Route("{year}/{make}/Models")]
        public async Task<List<string>> GetModelsByYearMake(string year, string make)
        {
            return await db.GetModelByYearAndMake(year, make);
        }


        //=============================== Get Model By Year Make Model ==========================================
        /// <summary>
        /// Get Trims by Year Make Model
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [Route("{year}/{make}/{model}/Trims")]
        public async Task<List<string>> GetTrimsByYearMakeModel(string year, string make, string model)
        {
            return await db.GetTrimsByYearMakeModel(year, make, model);
        }

        //=============================== Get all by Year Make Model Trim ==========================================

        /// <summary>
        /// Get All Cars By Make Model Trim
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns></returns>
        /// 
        [Route("{year}/{make}/{model}/{trim}/CarOnly")]
        public Task<List<Cars>> GetCarByYearMakeModelTrim(string year, string make, string model, string trim)
        {
            return db.GetCarsByYearMakeModelTrim(year, make, model, trim);
        }
        

        //=============================== Get External Data ==========================================


        [Route("{year}/{make}/{model}/{trim}/Car")]        
        public async Task<IHttpActionResult> GetCarData(string year = "", string make = "", string model = "", string trim = "")
        {
            HttpResponseMessage response;
            var content = new carRecall();
            var singleCar = GetCarByYearMakeModelTrim(year, make, model, trim).Result.First();
            
            var car = new CarViewModel
            {
                Car = singleCar,
                Recalls = "",
                Image = ""

            };


            //Get recall Data
            string result1 = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + year + "/make/"
                        + make + "/model/" + model + "?format=json");
                    result1 = await response.Content.ReadAsStringAsync();
                    car.Recalls = (result1);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }



            //////////////////////////////   My Bing Search   //////////////////////////////////////////////////////////

            string query = year + " " + make + " " + model + " " + trim;

            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            var accountKey = ConfigurationManager.AppSettings["searchKey"];

            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);


            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            var imageResults = imageQuery.Execute().ToList();


            car.Image = imageResults.First().MediaUrl;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            return Ok(car);

        }
    }
}
