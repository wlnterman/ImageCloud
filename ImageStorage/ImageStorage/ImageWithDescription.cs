using Microsoft.AspNetCore.Http;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace ImageStorage
{
    [Serializable]
    public class ImageWithDescription
    {
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public IFormFile Image { get; set; }

        public string Description { get; set; }
    }

    public class SerializedDataPath
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}