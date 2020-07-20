using System;
using System.Collections.Generic;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.Sales;

namespace AdventureWorks.Dal.Initialization
{
    public class SampleData
    {
        public static IEnumerable<BusinessEntity> GetBusinessEntities() => new List<BusinessEntity>
        {
            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Ken",
                    MiddleName = "J",
                    LastName = "Sanchez",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "ken@adventure-works.com"
                    },
                    Phones =
                    {   
                        new PersonPhone { PhoneNumber = "697-555-0142", PhoneNumberTypeID = 1 },
                        new PersonPhone { PhoneNumber = "697-123-8901", PhoneNumberTypeID = 2 },
                        new PersonPhone { PhoneNumber = "697-123-4567", PhoneNumberTypeID = 3 }                                                
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                        PasswordSalt = "bE3XiWw="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "295847284",
                        LoginID = "adventure-works\\ken0",
                        JobTitle = "Chief Executive Officer",
                        BirthDate = new DateTime(1969,1,29),
                        MaritalStatus = "S",
                        Gender = "M",
                        HireDate = new DateTime(2009,1,14),
                        VacationHours = 99,
                        SickLeaveHours = 69,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2009,1,14),
                                Rate = 125.50M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        }
                    }
                }
            },
            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Terri",
                    MiddleName = "Lee",
                    LastName = "Duffy",
                    EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "terri@adventure-works.com"
                    },
                    Phones = 
                    {
                        new PersonPhone{PhoneNumber = "819-555-0175", PhoneNumberTypeID = 3}                        
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "bawRVNrZQYQ05qF05Gz6VLilnviZmrqBReTTAGAudm0=",
                        PasswordSalt = "EjJaC3U="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "245797967",
                        LoginID = "adventure-works\\terri0",
                        JobTitle = "Vice President of Engineering",
                        BirthDate = new DateTime(1971,8,1),
                        MaritalStatus = "S",
                        Gender = "F",
                        HireDate = new DateTime(2008,1,31),
                        VacationHours = 1,
                        SickLeaveHours = 20,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2008,1,31),
                                Rate = 63.4615M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        }
                    }
                }
            },
            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Gail",
                    MiddleName = "A",
                    LastName = "Erickson",
                    EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "gail@adventure-works.com"
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "849-555-0139", PhoneNumberTypeID = 1}                        
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "8FYdAiY6gWuBsgjCFdg0UibtsqOcWHf9TyaHIP7+paA=",
                        PasswordSalt = "qYhZRiM="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "695256908",
                        LoginID = "adventure-works\\gail0",
                        JobTitle = "Design Engineer",
                        BirthDate = new DateTime(1952,9,27),
                        MaritalStatus = "M",
                        Gender = "F",
                        HireDate = new DateTime(2008,1,6),
                        VacationHours = 5,
                        SickLeaveHours = 22,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2008,1,6),
                                Rate = 32.6923M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        }
                    }
                }
            },
            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    Title = "Mr.",
                    FirstName = "Jossef",
                    MiddleName = "H",
                    LastName = "Goldberg",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "jossef@adventure-works.com"
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "122-555-0189", PhoneNumberTypeID = 3}                        
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "u5kbN5n84NRE1h/a+ktdRrXucjgrmfF6wZC4g82rjHM=",
                        PasswordSalt = "a9GiLUA="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "998320692",
                        LoginID = "adventure-works\\jossef0",
                        JobTitle = "Design Engineer",
                        BirthDate = new DateTime(1959,3,11),
                        MaritalStatus = "M",
                        Gender = "M",
                        HireDate = new DateTime(2008,1,24),
                        VacationHours = 6,
                        SickLeaveHours = 23,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2008,1,24),
                                Rate = 32.6923M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        }
                    }
                }
            },
            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Diane",
                    MiddleName = "L",
                    LastName = "Margheim",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "diane1@adventure-works.com"
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "815-555-0138", PhoneNumberTypeID = 1}                        
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "s+FUWADIZzXBKpcbxe4OwL2uiJmjLogJNYXXHvc1X/k=",
                        PasswordSalt = "FlCpzTU="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "811994146",
                        LoginID = "adventure-works\\diane1",
                        JobTitle = "Research and Development Engineer",
                        BirthDate = new DateTime(1986,6,5),
                        MaritalStatus = "S",
                        Gender = "F",
                        HireDate = new DateTime(2008,12,29),
                        VacationHours = 62,
                        SickLeaveHours = 51,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2008,12,29),
                                Rate = 40.8654M,
                                PayFrequency = PayFrequency.Monthly
                            }
                        }
                    }
                },                
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Freddy",
                    MiddleName = "L",
                    LastName = "Krueger",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "mr.mayhem@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "s+FUWADIZzXBKpcbxe4OwL2uiJmjLogJNYXXHvc1X/k=",
                        PasswordSalt = "FlCpzTU="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "147994146",
                        LoginID = "adventure-works\\freddyk",
                        JobTitle = "Research and Development Engineer",
                        BirthDate = new DateTime(1986,6,5),
                        MaritalStatus = "S",
                        Gender = "F",
                        HireDate = new DateTime(2008,12,29),
                        VacationHours = 62,
                        SickLeaveHours = 51,
                        IsActive = true,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2008,12,29),
                                Rate = 40.8654M,
                                PayFrequency = PayFrequency.Monthly
                            }
                        }
                    }
                },                
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Paula",
                    MiddleName = "B.",
                    LastName = "Moberly",
                    EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "paula2@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                        PasswordSalt = "K4sEsXg="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "273-555-0100", PhoneNumberTypeID = 1}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Mr.",
                    FirstName = "William",
                    MiddleName = "J.",
                    LastName = "Monroe",
                    EmailPromotion = EmailPromoPreference.AdventureWorksOnly,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "william3@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "gF+ZjCfYSXM0eyoxiL1+TQR2Ok2GEYIULuT6jM0rSwg=",
                        PasswordSalt = "PYj/pzk="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "315-555-0100", PhoneNumberTypeID = 3}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Marie",
                    MiddleName = "E.",
                    LastName = "Moya",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "marie0@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "ImD3R5/BA0YeT1Yy1LCk3Iuj2QQPz4BybsiOooaka4c=",
                        PasswordSalt = "VDufoKM="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "114-555-0100", PhoneNumberTypeID = 3}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Charlene",
                    MiddleName = "J.",
                    LastName = "Wojcik",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "charelene0@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "B8H846iy+ZWO8Ed1QCuYWXFN8x4zzUGCDhjGsIfRmaA=",
                        PasswordSalt = "DNuHuX0="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "459-555-0100", PhoneNumberTypeID = 1}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Jo",
                    MiddleName = "J.",
                    LastName = "Zimmerman",
                    EmailPromotion = EmailPromoPreference.AdventureWorksOnly,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "jo3@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "8pJsGA+VNldlqxGoEloyXnMv3mSCpZXltUf11tCeVts=",
                        PasswordSalt = "d2tgUmM="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "816-555-0142", PhoneNumberTypeID = 3}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Ms.",
                    FirstName = "Terri",
                    MiddleName = "B.",
                    LastName = "Phide",
                    EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "terri.phide@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                        PasswordSalt = "K4sEsXg="
                    },
                    Phones = 
                    {
                        new PersonPhone {PhoneNumber = "214-555-0100", PhoneNumberTypeID = 1},
                        new PersonPhone {PhoneNumber = "469-987-1001", PhoneNumberTypeID = 3}                        
                    }
                }
            },

            new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "VC",
                    IsEasternNameStyle = false,
                    Title = "Mr.",
                    FirstName = "Johnny",
                    MiddleName = "Dwayne",
                    LastName = "Dough",
                    EmailPromotion = EmailPromoPreference.AdventureWorksAndPartners,
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "j.dough@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "elZhadf87Lkl3cXx4oYxXdV31zWENyCgo3VUn5l5l/c=",
                        PasswordSalt = "K4sEsXg="
                    }
                }
            },

            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "AUSTRALI0001",
                    Name = "Australia Bike Retailer",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true
                }
            },
            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "TRIKES0001",
                    Name = "Trikes",
                    CreditRating = CreditRating.Above_Average,
                    PreferredVendor = true,
                    IsActive = true
                }
            },
            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "LIGHTSP0001",
                    Name = "Light Speed",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true
                }
            },

            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "DFWBIRE0001",
                    Name = "DFW Bike Resellers",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true
                }
            },
            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "DESOTOB0001",
                    Name = "Desoto Bike Mart",
                    CreditRating = CreditRating.Above_Average,
                    PreferredVendor = true,
                    IsActive = true
                }
            },
            new BusinessEntity
            {
                VendorObj = new Vendor
                {
                    AccountNumber = "CYCLERU0001",
                    Name = "Cycles-R-Us",
                    CreditRating = CreditRating.Superior,
                    PreferredVendor = true,
                    IsActive = true
                }
            }            
        };

        public static IEnumerable<Address> GetPeopleAddresses() => new List<Address>
        {
            new Address
            {
                AddressLine1 = "4350 Minute Dr.",
                City = "Newport Hills",
                PostalCode = "98006",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "7559 Worth Ct.",
                City = "Renton",
                PostalCode = "98055",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "9435 Breck Court",
                City = "Bellevue",
                PostalCode = "98004",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "5670 Bel Air Dr.",
                City = "Renton",
                PostalCode = "98055",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "475 Santa Maria",
                City = "Everett",
                PostalCode = "98201",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            }           
        };

        public static IEnumerable<Address> GetVendorAddresses() => new List<Address>
        {
            new Address
            {
                AddressLine1 = "28 San Marino Ct",
                City = "Bellingham",
                PostalCode = "98225",
                StateProvinceID = 79,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 3
                }
            },
            new Address
            {
                AddressLine1 = "90 Sunny Ave",
                City = "Berkeley",
                PostalCode = "94704",
                StateProvinceID = 9,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 3
                }
            },
            new Address
            {
                AddressLine1 = "298 Sunnybrook Drive",
                City = "Spring Valley",
                PostalCode = "91977",
                StateProvinceID = 9,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 3
                }
            },
            new Address
            {
                AddressLine1 = "1900 Desoto Court",
                City = "Desoto",
                PostalCode = "75123",
                StateProvinceID = 73,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "211 East Pleasant Run Rd",
                AddressLine2 = "#A",
                City = "Desoto",
                PostalCode = "75115",
                StateProvinceID = 73,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            },
            new Address
            {
                AddressLine1 = "6266 Melody Lane",
                City = "Dallas",
                PostalCode = "75231",
                StateProvinceID = 73,
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            }             
        };

        public static IEnumerable<SalesTerritory> GetSalesTerritories() => new List<SalesTerritory>
        {
            new SalesTerritory
            {
                Name = "Northwest",
                CountryRegionCode = "US",
                Group = "North America",
                SalesYTD = 7887186.7882M,
                SalesLastYear = 3298694.4938M,
                CostYTD = 0,
                CostLastYear = 0
            },
            new SalesTerritory
            {
                Name = "Northeast",
                CountryRegionCode = "US",
                Group = "North America",
                SalesYTD = 2402176.8476M,
                SalesLastYear = 03607148.9371M,
                CostYTD = 0,
                CostLastYear = 0
            },
            new SalesTerritory
            {
                Name = "Canada",
                CountryRegionCode = "CA",
                Group = "North America",
                SalesYTD = 06771829.1376M,
                SalesLastYear = 5693988.8600M,
                CostYTD = 0,
                CostLastYear = 0
            },
            new SalesTerritory
            {
                Name = "Germany",
                CountryRegionCode = "DE",
                Group = "Europe",
                SalesYTD = 3805202.3478M,
                SalesLastYear = 1307949.7917M,
                CostYTD = 0,
                CostLastYear = 0
            },
            new SalesTerritory
            {
                Name = "Australia",
                CountryRegionCode = "AU",
                Group = "Pacific",
                SalesYTD = 5977814.9154M,
                SalesLastYear = 2278548.9776M,
                CostYTD = 0,
                CostLastYear = 0
            }
        };

        public static IEnumerable<Shift> GetHumanResourceShifts() => new List<Shift>
        {
            new Shift
            {
                Name = "Day",
                StartTime = new TimeSpan(7,0,0),
                EndTime = new TimeSpan(15,0,0)
            },
            new Shift
            {
                Name = "Evening",
                StartTime = new TimeSpan(15,0,0),
                EndTime = new TimeSpan(23,0,0)
            },
            new Shift
            {
                Name = "Night",
                StartTime = new TimeSpan(23,0,0),
                EndTime = new TimeSpan(7,0,0)
            }
        };

        public static IEnumerable<Department> GetHRDepartments() => new List<Department>
        {
            new Department
            {
                Name = "Engineering",
                GroupName = "Research and Development"
            },
            new Department
            {
                Name = "Tool Design",
                GroupName = "Research and Development"
            },
            new Department
            {
                Name = "Sales",
                GroupName = "Sales and Marketing"
            },
            new Department
            {
                Name = "Marketing",
                GroupName = "Sales and Marketing"
            },
            new Department
            {
                Name = "Purchasing",
                GroupName = "Inventory Management"
            },
            new Department
            {
                Name = "Research and Development",
                GroupName = "Research and Development"
            },
            new Department
            {
                Name = "Production",
                GroupName = "Manufacturing"
            },
            new Department
            {
                Name = "Production Control",
                GroupName = "Manufacturing"
            },
            new Department
            {
                Name = "Human Resources",
                GroupName = "Executive General and Administration"
            },
            new Department
            {
                Name = "Finance",
                GroupName = "Executive General and Administration"
            },
            new Department
            {
                Name = "Information Services",
                GroupName = "Executive General and Administration"
            },
            new Department
            {
                Name = "Document Control",
                GroupName = "Quality Assurance"
            },
            new Department
            {
                Name = "Quality Assurance",
                GroupName = "Quality Assurance"
            },
            new Department
            {
                Name = "Facility and Maintenance",
                GroupName = "Executive General and Administration"
            },
            new Department
            {
                Name = "Shipping and Receiving",
                GroupName = "Inventory Management"
            },
            new Department
            {
                Name = "Executive",
                GroupName = "Executive General and Administration"
            },
        };
    }
}