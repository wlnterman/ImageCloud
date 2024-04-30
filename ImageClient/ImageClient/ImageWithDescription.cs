using System;
using Microsoft.AspNetCore.Http;

namespace ImageClient
{
    public class ImageWithDescription
    {
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public String Image { get; set; }

        public string Description { get; set; }
    }
}
