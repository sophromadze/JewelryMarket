﻿namespace JewelryMarket.Models
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }  // Make Password optional
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PersonalId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
