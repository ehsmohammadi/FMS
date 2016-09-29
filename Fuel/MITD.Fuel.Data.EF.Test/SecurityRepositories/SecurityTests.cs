using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Transactions;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Domain.Model.Enums;
using MITD.FuelSecurity.Domain.Model;

namespace MITD.Fuel.Data.EF.Test
{
    [TestClass]
    public class SecurityTests
    {
        private TransactionScope scope;

        [TestInitialize]
        public void InitTest()
        {
            scope = new TransactionScope();
        }

        //================================================================================

        [TestMethod]
        public void TestUserInsertion()
        {
            User user = null;

            using (var ctx = new DataContainer())
            {
                user = new User(1, "Party", "f", "l", "e","fueluser");

                ctx.Parties.Add(user);

                ctx.SaveChanges();
            }

            using (var ctx2 = new DataContainer())
            {
                var user2 = ctx2.Parties.OfType<User>().Single(u => u.Id == 1);

                Assert.AreEqual(user.FirstName, user2.FirstName);
            }
        }

        //================================================================================

        [TestMethod]
        public void TestGroupInsertion()
        {
            Group group = null;

            using (var ctx = new DataContainer())
            {
                group = new Group(1, "Party", "desc");

                ctx.Parties.Add(group);

                ctx.SaveChanges();
            }

            using (var ctx = new DataContainer())
            {
                var user2 = ctx.Parties.OfType<Group>().Single(u => u.Id == 1);

                Assert.AreEqual(group.Description, user2.Description);
            }
        }

        //================================================================================

        [TestMethod]
        public void TestUserGroupsInsertion()
        {
            var user = new User(1, "User", "f", "l", "e", "fueluser");

            using (var ctx = new DataContainer())
            {
                ctx.Parties.Add(user);
                ctx.SaveChanges();

                var group = new Group(2, "Group", "Desc");
                ctx.Parties.Add(group);
                ctx.SaveChanges();

                user.AssignGroup(group);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContainer())
            {
                var insertedUser = ctx.Parties.OfType<User>().Single(u => u.Id == 1);

                Assert.IsTrue(
                    insertedUser.Groups.Count == 1 &&
                    insertedUser.Groups.Count(g => g.Id == 2) == 1);

            }
        }

        //================================================================================

        [TestMethod]
        public void TestAdminUserInsertion()
        {
            User user;

            using (var ctx = new DataContainer())
            {
                user = new AdminUser( "f", "l", "e","fueluser");

                ctx.Parties.Add(user);

                ctx.SaveChanges();
            }

            using (var ctx2 = new DataContainer())
            {
                var user2 = ctx2.Parties.OfType<AdminUser>().Single(u => u.Id == 1);

                Assert.AreEqual(user.FirstName, user2.FirstName);
            }
        }

        //================================================================================

        [TestMethod]
        public void TestActionTypeInsertion()
        {
            ActionType actionType = new ActionType(1, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            using (var ctx = new DataContainer())
            {
                ctx.ActionTypes.Add(actionType);

                ctx.SaveChanges();
            }

            using (var ctx = new DataContainer())
            {
                var insertedAction = ctx.ActionTypes.Single(a => a.Id == 1);

                Assert.AreEqual(insertedAction.Name, actionType.Name);
                Assert.AreEqual(insertedAction.Description, actionType.Description);
            }
        }

        //================================================================================

        [TestMethod]
        public void TestCustomActionToUserAssignmentLowLevel()
        {
            using (var ctx = new DataContainer())
            {
                foreach (var action in ActionType.GetAllActions())
                {
                    ctx.ActionTypes.Add(action);
                }

                ctx.SaveChanges();
            }

            var user = new User(1, "User", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "fueluser");
            
            var customAction = new PartyCustomAction(1, ActionType.AddCharterIn.Id, false);

            using (var ctx = new DataContainer())
            {
                ctx.Parties.Add(user);
                ctx.SaveChanges();

                ctx.PartyCustomActions.Add(customAction);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContainer())
            {
                var insertedUser = ctx.Parties.OfType<User>().Single(u=>u.Id == 1);

                Assert.IsTrue(insertedUser.CustomActions.Count == 1 &&
                    insertedUser.CustomActions.Count(ca => ca.Id == customAction.Id) == 1);

                var addedCustomAction = insertedUser.CustomActions.Single(ca => ca.Id == customAction.Id);

                Assert.IsNotNull(addedCustomAction);

                Assert.IsTrue(addedCustomAction.ActionTypeId == customAction.ActionTypeId &&
                    addedCustomAction.IsGranted == customAction.IsGranted);
            }

        }

        //================================================================================

        [TestMethod]
        public void TestCustomActionToUserAssignment()
        {
            using (var ctx = new DataContainer())
            {
                foreach (var action in ActionType.GetAllActions())
                {
                    ctx.ActionTypes.Add(action);
                }

                ctx.SaveChanges();
            }

            var user = new User(1, "User", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "fueluser");

            var grantState = false;

            using (var ctx = new DataContainer())
            {
                ctx.Parties.Add(user);
                ctx.SaveChanges();

                user.AssignCustomActions(ActionType.AddCharterOut, grantState);
                ctx.SaveChanges();
            }

            using (var ctx = new DataContainer())
            {
                var insertedUser = ctx.Parties.OfType<User>().Single(u => u.Id == 1);

                Assert.IsTrue(insertedUser.CustomActions.Count == 1);

                var addedCustomAction = insertedUser.CustomActions[0];

                Assert.IsTrue(addedCustomAction.PartyId == user.Id &&
                    addedCustomAction.ActionTypeId == ActionType.AddCharterOut.Id &&
                    addedCustomAction.IsGranted == grantState);
            }
        }

        //================================================================================

        [TestCleanup]
        public void Cleanup()
        {
            scope.Dispose();
        }

        //================================================================================
    }
}
