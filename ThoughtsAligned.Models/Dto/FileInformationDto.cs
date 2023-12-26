namespace ThoughtsAligned.Models.Dto
{
    public class FileInformationDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string FilePath { get; set; } = null!;

        public byte FilePathType { get; set; }

        public bool? Status { get; set; }
    }
}