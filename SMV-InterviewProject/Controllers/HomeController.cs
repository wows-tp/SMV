using Microsoft.AspNetCore.Mvc;
using SMV_InterviewProject.Models;
using System.Diagnostics;
using System.Reflection;
// Use sixlabor.imagesharp instead of system.drawing
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace SMV_InterviewProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new ImageProcessViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> ImageProcessor(IFormFile uploadedFile, int testMultiplier = 40)
        {
            // add model file for data calculation sample
            var model = new ImageProcessViewModel();

            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                model.Error = "Please select image to continue...";
                return View("Index", model);
            }

            // First we need to record starting memory
            long startMemory = GC.GetTotalMemory(false);

            // load the upload file into memory stream
            using var memoryStream = new MemoryStream();
            // awalys use async to prevent server from freezing up when multiple user using at same time
            await uploadedFile.CopyToAsync(memoryStream);
            byte[] imageByte = memoryStream.ToArray();

            // purposely blaot the memory by creating multiple copies of imagebyte, this will act as simulation that hold many large objects
            List<byte[]> memoryHog = new List<byte[]>();
            for (int i = 0; i < testMultiplier; i++)
            {
                memoryHog.Add((byte[])imageByte.Clone());
            }

            // after bloat, record the peak down
            long peakMemory = Process.GetCurrentProcess().WorkingSet64;

            // start the actual image processing
            memoryStream.Position = 0;
            using var image = await Image.LoadAsync(memoryStream);

            // apply blur to image with blur radius of 20
            image.Mutate(x => x.GaussianBlur(20));

            // record peak after apply blur
            long peak = GC.GetTotalMemory(false);

            // convert the processed image to a base64 string so able to display in HTML
            string afterImage = image.ToBase64String(JpegFormat.Instance);

            // free up memory and force system to Garbage Collection (GC)
            memoryHog.Clear();
            memoryHog = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();


            long endMemory = GC.GetTotalMemory(true);

            // convert to mb more easier to read
            model.startMemory = startMemory / (1024 * 1024);
            model.peakMemory = peakMemory / (1024 * 1024);
            model.endMemory = endMemory / (1024 * 1024);
            model.processedImage = afterImage;

            // lastly, pass back and return view with model
            return View("Index", model);
        }
    }
}
