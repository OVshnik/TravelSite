﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSite.Data.Models
{
	public class User:IdentityUser
	{
		public string FirstName { get; set; }=string.Empty;
		public string LastName { get; set; }=string.Empty;
		public string MiddleName { get; set; }=string.Empty;
		public DateTime BirthDate { get; set; }=DateTime.Now;
		public string GetFullName()
		{
			return FirstName+" "+LastName+" "+MiddleName;
		}
		public List<Booking> Bookings { get; set; } = new List<Booking>();
		public List<BookingNotification> SendNotifications { get; set; } = new List<BookingNotification>();
		public List<BookingNotification> ReceivedNotifications { get; set; } = new List<BookingNotification>();
	}
}
