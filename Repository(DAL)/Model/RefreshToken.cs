﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository_DAL_.Model;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public string Token { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string DeviceInfo { get; set; }

    [ForeignKey("Users")]
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}