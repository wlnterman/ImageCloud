using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ImageStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageStorageController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ImageStorageController> _logger;
        private ApplicationContext _dbContext;
        IWebHostEnvironment _appEnvironment;

        public ImageStorageController(ILogger<ImageStorageController> logger, 
            ApplicationContext context,
            IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _dbContext = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        [Route("get-all")]
        //https://localhost:44315/imagestorage/get-all
        public IEnumerable<ImageWithDescription> GetAllImages()
        {
            var imageList = _dbContext.SerializedDataPaths.ToList<SerializedDataPath>();// .ToListAsync();
            var resultList = new List<ImageWithDescription>();

            foreach (var item in imageList)
            {
                // путь к папке Files
                string path = "/Files/" + item.Path;//"user.dat"
                BinaryFormatter binFormat = new BinaryFormatter();
                using (Stream fStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    resultList.Append((ImageWithDescription)binFormat.Deserialize(fStream));
                }
            }
            return resultList;
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadNewImage(IFormFile file, string description)
        {
            return await UploadImage(file, description);
        }

        private async Task<IActionResult> UploadImage(IFormFile uploadedFile, string fileDescription)
        {
            if (uploadedFile == null || uploadedFile.Length == 0 )
                return BadRequest();
             
            try
            {
                var file = new ImageWithDescription
                {
                    Name = uploadedFile.FileName,
                    CreatedAt = DateTime.Now,
                    Image = uploadedFile,
                    Description = fileDescription
                };

                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;//"user.dat"
                BinaryFormatter binFormat = new BinaryFormatter();
                // Сохранить объект в локальном файле.
                using (Stream fStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    binFormat.Serialize(fStream, file);
                }

                _dbContext.SerializedDataPaths.Add(new SerializedDataPath { Path = path });
                await _dbContext.SaveChangesAsync();

                return Ok(path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
