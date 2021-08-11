﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMCBA.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LoginID { get; set; }
        public virtual Login Login { get; set; }
    }
}
