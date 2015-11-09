﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using Dapper.Extensions.Linq.Core.Repositories;
using Dapper.Extensions.Linq.Test.Entities;
using NUnit.Framework;

namespace Dapper.Extensions.Linq.Test.IntegrationTests.SqlServer
{
    public class Delete : SqlServerBase
    {
        [Test]
        public void UsingKey_DeletesFrom_database()
        {
            var personRepository = Container.Resolve<IRepository<Person>>();

            Person p1 = new Person { Active = true, FirstName = "Foo", LastName = "Bar", DateCreated = DateTime.UtcNow };
            int id = personRepository.Insert(p1);
            bool deleted = personRepository.Delete(p1);
            Assert.IsTrue(deleted);
            Assert.IsNull(personRepository.Get(id));
        }

        [Test]
        public void UsingCompositeKey_DeletesFrom_database()
        {
            var multiKeyRepository = Container.Resolve<IRepository<Multikey>>();

            var m1 = new Multikey { Key2 = "key", Value = "bar" };
            dynamic key = multiKeyRepository.Insert(m1);
            int key1 = key.Key1;
            string key2 = key.Key2;

            Multikey m2 = multiKeyRepository
                .Query(e => e.Key1 == key1 && e.Key2 == key2)
                .Single();

            bool success = multiKeyRepository.Delete(m2);

            Assert.IsTrue(success);
        }
    }
}