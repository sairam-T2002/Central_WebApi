﻿using System.ComponentModel.DataAnnotations;

namespace Repository_DAL_.Model
{
    public class ControlMaster
    {
        [Key]
        public int id { get; set; }

        public string devUrl { get; set; } = string.Empty;

        public string prodUrl { get; set; } = string.Empty;

        public string? gmapkey {  get; set; } = string.Empty;

        public int defaultSearchImg {  get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
