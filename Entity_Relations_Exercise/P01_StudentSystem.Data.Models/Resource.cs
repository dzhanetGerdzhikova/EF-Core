using P01_StudentSystem.Data.Models.ResourceType;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        public Course Course { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public ResourceEnum ResourceType { get; set; }
        public string Url { get; set; }
    }
}