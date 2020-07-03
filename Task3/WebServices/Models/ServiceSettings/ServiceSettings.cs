using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Zeus.Lib.Extensions.Models;
using Zeus.Lib.WebServices.Models.Authorization;

namespace Zeus.Lib.WebServices.Models.ServiceSettings
{
    public class ServiceSettings : ConfigurationSection
    {
        [ConfigurationProperty("ServiceRoutes")]
        public RoutesCollection Routes
        {
            get
            {
                return (RoutesCollection)this["ServiceRoutes"];
            }
        }

        [ConfigurationProperty("ServiceUsers", Options = ConfigurationPropertyOptions.IsRequired)]
        public UsersCollection Users
        {
            get
            {
                return (UsersCollection)this["ServiceUsers"];
            }
        }

        [ConfigurationProperty("ServiceSchedules")]
        public ScheduleCollection Schedules
        {
            get
            {
                return (ScheduleCollection)this["ServiceSchedules"];
            }
        }
    }

    [ConfigurationCollection(typeof(ServiceAddressElement), AddItemName = "ServiceAddress")]
    public class RoutesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceAddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return ((ServiceAddressElement)element).Route;
        }
    }

    public class ServiceAddressElement : ConfigurationElement
    {
        [ConfigurationProperty("Route", IsRequired = true, IsKey = true)]
        public string Route
        {
            get { return (string)base["Route"]; }
        }

        [ConfigurationProperty("DomainID")]
        public int DomainID
        {
            get { return (int)base["DomainID"]; }
        }

        [ConfigurationProperty("DomainName")]
        public string DomainName
        {
            get { return (string)base["DomainName"]; }
        }

        [ConfigurationProperty("WhatLedger")]
        public string WhatLedger
        {
            get { return (string)base["WhatLedger"]; }
        }

        [ConfigurationProperty("BaseURL")]
        public string BaseURL
        {
            get { return (string)base["BaseURL"]; }
        }

        [ConfigurationProperty("User")]
        public string User
        {
            get { return (string)base["User"]; }
        }

        [ConfigurationProperty("Password")]
        public string Password
        {
            get { return (string)base["Password"]; }
        }

        [ConfigurationProperty("InternalOrderOnImport")]
        public bool InternalOrderOnImport
        {
            get { return (bool)base["InternalOrderOnImport"]; }
        }

        [ConfigurationProperty("CheckForMissingProducts")]
        public bool CheckForMissingProducts
        {
            get { return (bool)base["CheckForMissingProducts"]; }
        }
    }

    public class ServiceUserElement : ConfigurationElement
    {
        [ConfigurationProperty("Username", IsRequired = true, IsKey = true)]
        public string Username
        {
            get { return (string)base["Username"]; }
        }

        [ConfigurationProperty("Password", IsRequired = true, IsKey = true)]
        public string Password
        {
            get { return (string)base["Password"]; }
        }

        [ConfigurationProperty("AllowedMethods", IsRequired = false, IsKey = false)]
        public string AllowedMethods
        {
            get { return (string)base["AllowedMethods"]; }
        }

        [ConfigurationProperty("ForbiddenMethods", IsRequired = false, IsKey = false)]
        public string ForbiddenMethods
        {
            get { return (string)base["ForbiddenMethods"]; }
        }

        public WebServiceUser ToUser()
        {
            return new WebServiceUser()
            {
                Username = this.Username,
                Password = this.Password,
                AllowedMethods = this.AllowedMethods,
                ForbiddenMethods = this.ForbiddenMethods,
                ID = Guid.NewGuid(),
            };
        }
    }

    [ConfigurationCollection(typeof(ServiceUserElement), AddItemName = "ServiceUser")]
    public class UsersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceUserElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return ((ServiceUserElement)element).Username;
        }
    }

    public class ServiceScheduleElement : ConfigurationElement
    {
        /*
        [ConfigurationProperty("Interval", IsRequired = true, IsKey = false)]
        public ScheduleType Interval
        {
            get { return (ScheduleType)base["Interval"]; }
        }//*/

        [ConfigurationProperty("IntervalAdjustment", IsRequired = false, IsKey = false)]
        public string IntervalAdjustment
        {
            get { return (string)base["IntervalAdjustment"]; }
        }


        [ConfigurationProperty("MethodsToExecute", IsRequired = true, IsKey = false)]
        public string MethodsToExecute
        {
            get { return (string)base["MethodsToExecute"]; }
        }

        [ConfigurationProperty("TimeOfDay", IsRequired = true, IsKey = false)]
        public DateTime TimeOfDay
        {
            get
            {
                return (DateTime)base["TimeOfDay"];
            }
        }

        [ConfigurationProperty("StartAt", IsRequired = false, IsKey = false)]
        public int StartAt
        {
            get { return (int)base["StartAt"]; }
        }

        [ConfigurationProperty("Email", IsRequired = false, IsKey = false)]
        public string Email
        {
            get { return (string)base["Email"]; }
        }

        public ServiceSchedule ToSchedule()
        {
            // Time of day is today!!!
            var n = DateTime.Now;
            var t = this.TimeOfDay;
            var tod = new DateTime(n.Year, n.Month, n.Day, t.Hour, t.Minute, t.Second);
            var result = new ServiceSchedule()
            {
                //Interval = this.Interval,
                IntervalAdjustment = this.IntervalAdjustment,
                MethodsToExecute = this.MethodsToExecute,
                TimeOfDay = tod,
                StartAt = this.StartAt,
                Email = this.Email,
            };
            return result;
        }
    }

    [ConfigurationCollection(typeof(ServiceUserElement), AddItemName = "ServiceSchedule")]
    public class ScheduleCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceScheduleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return ((ServiceScheduleElement)element).GetHashCode();
        }
    }
}
