﻿namespace Sas.Identity.Service.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
