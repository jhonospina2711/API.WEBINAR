using System;

namespace Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string VideoName { get; set; }
        public string ImplementDate { get; set; }
        public string ImpactDescription { get; set; }
        public bool Enabled { get; set; }
        public int DependencyId { get; set; }
        public int ProjectTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual Dependency Dependency { get; set; }
        public virtual ProjectType ProjectType { get; set; }
    }
}