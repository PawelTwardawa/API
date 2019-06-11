﻿using System;
using System.Collections.Generic;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
