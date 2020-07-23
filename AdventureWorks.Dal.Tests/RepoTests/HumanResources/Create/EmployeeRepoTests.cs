using System;
using AdventureWorks.Dal.Repositories.Interfaces.HumanResources;
using AdventureWorks.Dal.Repositories.HumanResources;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.HumanResources;
using AdventureWorks.Models.Person;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Tests.RepoTests.Base;
using Xunit;

namespace AdventureWorks.Dal.Tests.RepoTests.HumanResources.Create
{
    [Collection("AdventureWorks.Dal")]
    public class EmployeeRepoTests : RepoTestsBase
    {
        private readonly IEmployeeRepo _employeeRepo;

        public EmployeeRepoTests()
        {
            _employeeRepo = new EmployeeRepo(ctx);
        }

        public override void Dispose()
        {
            _employeeRepo.Dispose();
        }

        [Fact]
        public void ShouldCreateNewEmployee()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var bizEntity = new BusinessEntity
                {
                    PersonObj = new Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Miles",
                        MiddleName = "J",
                        LastName = "Davis",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        Phones =
                        {
                            new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 1}
                        },
                        EmailAddressObj = new EmailAddress
                        {
                            PersonEmailAddress = "m.davis@adventure-works.com"
                        },
                        PasswordObj = new PersonPWord
                        {
                            PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                            PasswordSalt = "bE3XiWw="
                        },
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "999999999",
                            LoginID = "adventure-works\\mdavis",
                            JobTitle = "Supreme Leader",
                            BirthDate = new DateTime(2000, 1, 29),
                            MaritalStatus = "S",
                            Gender = "M",
                            HireDate = new DateTime(2020, 6, 16),
                            VacationHours = 99,
                            SickLeaveHours = 69,
                            PayHistories = {
                                new EmployeePayHistory {
                                    RateChangeDate = new DateTime(2020,6,16),
                                    Rate = 200.00M,
                                    PayFrequency = PayFrequency.Biweekly
                                }
                            },
                            DepartmentHistories = {
                                new EmployeeDepartmentHistory {
                                    DepartmentID = 1,
                                    ShiftID = 1,
                                    StartDate = new DateTime(2020,6,16)
                                }
                            }
                        }
                    }
                };

                var businessEntityID = _employeeRepo.AddEmployee(bizEntity);

                Assert.True(businessEntityID > 0);
                Assert.Equal(businessEntityID, bizEntity.BusinessEntityID);
                Assert.Equal(businessEntityID, bizEntity.PersonObj.BusinessEntityID);
                Assert.Equal(businessEntityID, bizEntity.PersonObj.EmployeeObj.BusinessEntityID);
            }
        }

        [Fact]
        public void ShouldCreateNewEmployeeWithAddress()
        {
            var bizEntity = new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Johnny",
                    MiddleName = "J",
                    LastName = "Dough",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    Phones =
                    {
                        new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 1}
                    },
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "jdough@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                        PasswordSalt = "bE3XiWw="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "999999999",
                        LoginID = "adventure-works\\jdough",
                        JobTitle = "Supreme Leader",
                        BirthDate = new DateTime(2000, 1, 29),
                        MaritalStatus = "S",
                        Gender = "M",
                        HireDate = new DateTime(2020, 6, 16),
                        VacationHours = 99,
                        SickLeaveHours = 69,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2020,6,16),
                                Rate = 200.00M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        },
                        DepartmentHistories = {
                            new EmployeeDepartmentHistory {
                                DepartmentID = 1,
                                ShiftID = 1,
                                StartDate = new DateTime(2020,6,16)
                            }
                        }
                    }
                }
            };

            var eeAddress = new Address
            {
                AddressLine1 = "123 EntityFrameworkCore Plaza",
                AddressLine2 = "Suite 1234",
                City = "Renton",
                StateProvinceID = 79,
                PostalCode = "98055",
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            };

            var businessEntityID = _employeeRepo.AddEmployee(bizEntity, eeAddress);

            Assert.True(businessEntityID > 0);
            Assert.Equal(businessEntityID, bizEntity.BusinessEntityID);
            Assert.Equal(businessEntityID, bizEntity.PersonObj.BusinessEntityID);
            Assert.Equal(businessEntityID, bizEntity.PersonObj.EmployeeObj.BusinessEntityID);
            Assert.Equal(businessEntityID, eeAddress.BusinessEntityAddressObj.BusinessEntityID);
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeLogins()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var bizEntity = new BusinessEntity
                {
                    PersonObj = new Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Miles",
                        MiddleName = "J",
                        LastName = "Davis",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        Phones =
                        {
                            new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 1}
                        },
                        EmailAddressObj = new EmailAddress
                        {
                            PersonEmailAddress = "m.davis@adventure-works.com"
                        },
                        PasswordObj = new PersonPWord
                        {
                            PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                            PasswordSalt = "bE3XiWw="
                        },
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "999999999",
                            LoginID = "adventure-works\\terri0",
                            JobTitle = "Supreme Leader",
                            BirthDate = new DateTime(2000, 1, 29),
                            MaritalStatus = "S",
                            Gender = "M",
                            HireDate = new DateTime(2020, 6, 16),
                            VacationHours = 99,
                            SickLeaveHours = 69,
                            PayHistories = {
                                new EmployeePayHistory {
                                    RateChangeDate = new DateTime(2020,6,16),
                                    Rate = 200.00M,
                                    PayFrequency = PayFrequency.Biweekly
                                }
                            },
                            DepartmentHistories = {
                                new EmployeeDepartmentHistory {
                                    DepartmentID = 1,
                                    ShiftID = 1,
                                    StartDate = new DateTime(2020,6,16)
                                }
                            }
                        }
                    }
                };

                Action testCode = () =>
                {
                    _employeeRepo.AddEmployee(bizEntity);
                };

                var ex = Record.Exception(testCode);

                Assert.NotNull(ex);
                Assert.IsType<AdventureWorksUniqueIndexException>(ex);
                Assert.Equal("Error: This operation would result in a duplicate employee login!", ex.Message);
            }
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateEmployeeNationalIDNumber()
        {
            ExecuteInATransaction(RunTheTest);

            void RunTheTest()
            {
                var bizEntity = new BusinessEntity
                {
                    PersonObj = new Person
                    {
                        PersonType = "EM",
                        IsEasternNameStyle = false,
                        FirstName = "Miles",
                        MiddleName = "J",
                        LastName = "Davis",
                        EmailPromotion = EmailPromoPreference.NoPromotions,
                        Phones =
                        {
                            new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 1}
                        },
                        EmailAddressObj = new EmailAddress
                        {
                            PersonEmailAddress = "m.davis@adventure-works.com"
                        },
                        PasswordObj = new PersonPWord
                        {
                            PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                            PasswordSalt = "bE3XiWw="
                        },
                        EmployeeObj = new Employee
                        {
                            NationalIDNumber = "295847284",
                            LoginID = "adventure-works\\jdough",
                            JobTitle = "Supreme Leader",
                            BirthDate = new DateTime(2000, 1, 29),
                            MaritalStatus = "S",
                            Gender = "M",
                            HireDate = new DateTime(2020, 6, 16),
                            VacationHours = 99,
                            SickLeaveHours = 69,
                            PayHistories = {
                                new EmployeePayHistory {
                                    RateChangeDate = new DateTime(2020,6,16),
                                    Rate = 200.00M,
                                    PayFrequency = PayFrequency.Biweekly
                                }
                            },
                            DepartmentHistories = {
                                new EmployeeDepartmentHistory {
                                    DepartmentID = 1,
                                    ShiftID = 1,
                                    StartDate = new DateTime(2020,6,16)
                                }
                            }
                        }
                    }
                };

                Action testCode = () =>
                {
                    _employeeRepo.AddEmployee(bizEntity);
                };

                var ex = Record.Exception(testCode);

                Assert.NotNull(ex);
                Assert.IsType<AdventureWorksUniqueIndexException>(ex);
                Assert.Equal("Error: There is an existing employee with this National ID number!", ex.Message);
            }
        }

        [Fact]
        public void ShouldRaiseExceptionDuplicateAddress()
        {
            var bizEntity = new BusinessEntity
            {
                PersonObj = new Person
                {
                    PersonType = "EM",
                    IsEasternNameStyle = false,
                    FirstName = "Johnny",
                    MiddleName = "J",
                    LastName = "Dough",
                    EmailPromotion = EmailPromoPreference.NoPromotions,
                    Phones =
                    {
                        new PersonPhone {PhoneNumber = "555-555-5555", PhoneNumberTypeID = 1}
                    },
                    EmailAddressObj = new EmailAddress
                    {
                        PersonEmailAddress = "jdough@adventure-works.com"
                    },
                    PasswordObj = new PersonPWord
                    {
                        PasswordHash = "pbFwXWE99vobT6g+vPWFy93NtUU/orrIWafF01hccfM=",
                        PasswordSalt = "bE3XiWw="
                    },
                    EmployeeObj = new Employee
                    {
                        NationalIDNumber = "999999999",
                        LoginID = "adventure-works\\jdough",
                        JobTitle = "Supreme Leader",
                        BirthDate = new DateTime(2000, 1, 29),
                        MaritalStatus = "S",
                        Gender = "M",
                        HireDate = new DateTime(2020, 6, 16),
                        VacationHours = 99,
                        SickLeaveHours = 69,
                        PayHistories = {
                            new EmployeePayHistory {
                                RateChangeDate = new DateTime(2020,6,16),
                                Rate = 200.00M,
                                PayFrequency = PayFrequency.Biweekly
                            }
                        },
                        DepartmentHistories = {
                            new EmployeeDepartmentHistory {
                                DepartmentID = 1,
                                ShiftID = 1,
                                StartDate = new DateTime(2020,6,16)
                            }
                        }
                    }
                }
            };

            var eeAddress = new Address
            {
                AddressLine1 = "28 San Marino Ct",
                City = "Bellingham",
                StateProvinceID = 79,
                PostalCode = "98225",
                BusinessEntityAddressObj = new BusinessEntityAddress
                {
                    AddressTypeID = 2
                }
            };

            Action testCode = () =>
            {
                _employeeRepo.AddEmployee(bizEntity, eeAddress);
            };

            var ex = Record.Exception(testCode);

            Assert.NotNull(ex);
            Assert.IsType<AdventureWorksUniqueIndexException>(ex);
            Assert.Equal("Error: There is an existing entity with this address!", ex.Message);
        }
    }
}