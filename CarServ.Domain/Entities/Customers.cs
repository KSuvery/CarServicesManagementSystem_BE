﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace CarServ.Domain.Entities;

public partial class Customers
{
    public int CustomerId { get; set; }

    public string Address { get; set; }

    public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();

    public virtual Users Customer { get; set; }

    public virtual ICollection<Vehicles> Vehicles { get; set; } = new List<Vehicles>();
}