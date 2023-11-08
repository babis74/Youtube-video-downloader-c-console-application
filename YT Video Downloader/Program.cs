using YoutubeExplode;

Console.WriteLine("Δώσε μου το URL απο το βίντεο στο YouTube:");
string videoUrl = Console.ReadLine();

var youtube = new YoutubeClient();
var videoInfo = await youtube.Videos.GetAsync(videoUrl);

if (videoInfo != null)
{
    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);

    // Choose the highest quality video stream with both audio and video
    var streamInfo = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).First();

    if (streamInfo != null)
    {
        string videoTitle = videoInfo.Title;
        string fileName = SanitizeFileName(videoTitle) + ".mp4"; // Ensure a valid file name

        Console.WriteLine("Το Βίντεο κατεβαίνει...λιγάκι υπομονή");
        await youtube.Videos.Streams.DownloadAsync(streamInfo, fileName);

        Console.WriteLine($"Video '{videoTitle}' downloaded as '{fileName}'");
    }
    else
    {
        Console.WriteLine("No video stream available for this URL.");
    }
}
else
{
    Console.WriteLine("Invalid or unavailable YouTube URL.");
}

// Helper function to sanitize file names
string SanitizeFileName(string fileName)
{
    foreach (char c in Path.GetInvalidFileNameChars())
    {
        fileName = fileName.Replace(c, '_');
    }
    return fileName;
}

