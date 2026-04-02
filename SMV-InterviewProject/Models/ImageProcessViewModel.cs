namespace SMV_InterviewProject.Models
{
    public class ImageProcessViewModel
    {
        public string? Error { get; set; }

        public long startMemory { get; set; }

        public long peakMemory { get; set; }

        public long endMemory { get; set; }

        public string? processedImage { get; set; }

        public long increaseMemory => peakMemory - startMemory;

        public long releaseMemory => peakMemory - endMemory;

        public double releaseMemoryPercentage =>
            peakMemory == 0
                ? 0
                : Math.Round((double)releaseMemory / peakMemory * 100, 2);
    }
}