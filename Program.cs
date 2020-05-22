using System;

using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace computer_vision_quickstart
{
    class Program
    {
        private static string key = "2b58cce2e5b6474da3b6e26dd13b3045";
        private static string endpoint = "https://testcomputervisionaz.cognitiveservices.azure.com/";
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";

        static void Main(string[] args)
        {
            // Add your Computer Vision subscription key and endpoint to your environment variables. 
            // Close/reopen your project for them to take effect.
            if (Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY")!=null)
               key = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
            else
               Environment.SetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY",key);
            if (Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT")!=null)
                endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
            else
                Environment.SetEnvironmentVariable("COMPUTER_VISION_ENDPOINT",endpoint);
            Console.WriteLine(Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY"));
            Console.WriteLine(Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT"));
           // Create a client
           ComputerVisionClient client = Authenticate(endpoint, key);
            // Analyze an image to get features and other properties.
            AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();

        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
            new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            { Endpoint = endpoint };
            return client;
        }

        /* 
        * ANALYZE IMAGE - URL IMAGE
        * Analyze URL image. Extracts captions, categories, tags, objects, faces, racy/adult content,
        * brands, celebrities, landmarks, color scheme, and image types.
        */
        public static async Task AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };

            ImageAnalysis results =  await client.AnalyzeImageAsync(ANALYZE_URL_IMAGE,features);
            Console.WriteLine(results.Description);
            foreach(var category in results.Categories){
                    Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            
            /**
                [JsonProperty(PropertyName = "categories")]
                public IList<Category> Categories { get; set; }
                [JsonProperty(PropertyName = "adult")]
                public AdultInfo Adult { get; set; }
                [JsonProperty(PropertyName = "color")]
                public ColorInfo Color { get; set; }
                [JsonProperty(PropertyName = "imageType")]
                public ImageType ImageType { get; set; }
                [JsonProperty(PropertyName = "tags")]
                public IList<ImageTag> Tags { get; set; }
                [JsonProperty(PropertyName = "description")]
                public ImageDescriptionDetails Description { get; set; }
                [JsonProperty(PropertyName = "faces")]
                public IList<FaceDescription> Faces { get; set; }
                [JsonProperty(PropertyName = "objects")]
                public IList<DetectedObject> Objects { get; set; }
                [JsonProperty(PropertyName = "brands")]
                public IList<DetectedBrand> Brands { get; set; }
                [JsonProperty(PropertyName = "requestId")]
                public string RequestId { get; set; }
                [JsonProperty(PropertyName = "metadata")]
                public ImageMetadata Metadata { get; set; }
            **/
            
        }
    }
}
