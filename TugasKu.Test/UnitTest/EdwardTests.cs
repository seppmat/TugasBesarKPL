<<<<<<< HEAD
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TugasKu_TUBES_KPL;
=======
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TugasKu_TUBES_KPL;
using TugasKu_TUBES_KPL.Core;
using TaskStatus = TugasKu_TUBES_KPL.TaskStatus;
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e

namespace TugasKu.Tests
{
    [TestClass]
    public class EdwardTests
    {
        [TestMethod]
        public void TestRepository_AddAndCount()
        {
            var repo = new GenericRepository<TaskItem>();
            repo.Add(new TaskItem { Name = "Test" });
            Assert.AreEqual(1, repo.GetAll().Count);
        }

        [TestMethod]
<<<<<<< HEAD
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestRepository_Update_InvalidIndex()
        {
            var repo = new GenericRepository<TaskItem>();
            repo.Update(99, new TaskItem());
=======
        public void TestRepository_Update_InvalidIndex_Throws()
        {
            var repo = new GenericRepository<TaskItem>();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                repo.Update(99, new TaskItem());
            });
>>>>>>> 61b2bf139511f48402b05b01c025f9093f9b809e
        }

        [TestMethod]
        public void TestTableDriven_TryGetValueSuccess()
        {
            var table = new Dictionary<int, TaskStatus> { { 1, TaskStatus.InProgress } };
            bool found = table.TryGetValue(1, out var result);
            Assert.IsTrue(found);
            Assert.AreEqual(TaskStatus.InProgress, result);
        }

        [TestMethod]
        public void TestTableDriven_TryGetValueFail()
        {
            var table = new Dictionary<int, TaskStatus> { { 1, TaskStatus.InProgress } };
            bool found = table.TryGetValue(99, out var result);
            Assert.IsFalse(found);
        }
    }
}