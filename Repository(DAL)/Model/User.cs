using System.ComponentModel.DataAnnotations;
using System;

namespace Repository_DAL_.Model;

public class User
{
    [Key]
    public int id { get; set; }

    public string usr_nam { get; set; }

    public string pwd { get; set; }

    public string e_mail { get; set; }

    public string? refreshtoken { get; set; }

    public string? cart { get; set; }

    public DateOnly createdate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    public DateOnly modifieddate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}