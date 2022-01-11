using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TestNinja.Mocking
{
    public class VideoService
    {
        private readonly IFileReader _fileReader;

        public VideoService(IFileReader fileReader = null)
        {
            /*
            What this does is create a default constructor for other uses
            of this code where a fake file reader isn't needed and a constructor
            for unit testing where we want to pass a fake file reader.
            */
            _fileReader = fileReader ?? new FileReader(); // Use the file reader provided if the parameter isn't null
        }
        
        public string ReadVideoTitle()
        {
            var str = _fileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";
            return video.Title;
        }

        public string GetUnprocessedVideosAsCsv()
        {
            var videoIds = new List<int>();
            
            using (var context = new VideoContext())
            {
                var videos = 
                    (from video in context.Videos
                    where !video.IsProcessed
                    select video).ToList();
                
                foreach (var v in videos)
                    videoIds.Add(v.Id);

                return String.Join(",", videoIds);
            }
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }

    public class VideoContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }
}