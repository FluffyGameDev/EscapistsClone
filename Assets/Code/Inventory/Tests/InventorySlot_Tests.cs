using NUnit.Framework;
using FluffyGameDev.Escapists.InventorySystem;

namespace FluffyGameDev.Escapists.InventorySystem.Tests
{
    public class InventorySlot_Tests
    {
        [Test]
        public void TestStoreItem()
        {
            int callbackCount = 0;
            InventorySlot slot = new();
            slot.OnSlotModified += _ => ++callbackCount;

            InventoryItem itemA = new();
            InventoryItem itemB = new();

            // Store items in an empty slot
            callbackCount = 0;
            bool result = slot.StoreItem(itemA, 123);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCount, 1);
            Assert.AreEqual(slot.Item, itemA);
            Assert.AreEqual(slot.Quantity, 123);

            // Store items in a slot with same item
            callbackCount = 0;
            result = slot.StoreItem(itemA, 10);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCount, 1);
            Assert.AreEqual(slot.Item, itemA);
            Assert.AreEqual(slot.Quantity, 133);

            // Store items in a slot with different item
            callbackCount = 0;
            result = slot.StoreItem(itemB, 42);
            Assert.IsFalse(result);
            Assert.AreEqual(callbackCount, 0);
            Assert.AreEqual(slot.Item, itemA);
            Assert.AreEqual(slot.Quantity, 133);
        }

        [Test]
        public void TestClearSlot()
        {
            int callbackCount = 0;
            InventorySlot slot = new();
            slot.OnSlotModified += _ => ++callbackCount;

            InventoryItem itemA = new();

            slot.StoreItem(itemA, 10);

            // Clear a slot with items
            callbackCount = 0;
            slot.ClearSlot();
            Assert.AreEqual(callbackCount, 1);
            Assert.IsNull(slot.Item);
            Assert.AreEqual(slot.Quantity, 0);

            // Clear an empty slot
            callbackCount = 0;
            slot.ClearSlot();
            Assert.AreEqual(callbackCount, 0);
            Assert.IsNull(slot.Item);
            Assert.AreEqual(slot.Quantity, 0);
        }

        [Test]
        public void TestMoveToItem()
        {
            bool result = false;
            int callbackCountSource = 0;
            InventorySlot slotSource = new();
            slotSource.OnSlotModified += _ => ++callbackCountSource;

            int callbackCountDestination = 0;
            InventorySlot slotDestination = new();
            slotDestination.OnSlotModified += _ => ++callbackCountDestination;

            InventoryItem itemA = new();
            InventoryItem itemB = new();

            // Move from empty to empty
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 0);
            Assert.IsFalse(result);
            Assert.AreEqual(callbackCountSource, 0);
            Assert.AreEqual(callbackCountDestination, 0);

            // Move part to empty slot
            slotSource.StoreItem(itemA, 10);
            slotDestination.ClearSlot();
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 5);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCountSource, 1);
            Assert.AreEqual(callbackCountDestination, 1);
            Assert.AreEqual(slotSource.Item, itemA);
            Assert.AreEqual(slotSource.Quantity, 5);
            Assert.AreEqual(slotDestination.Item, itemA);
            Assert.AreEqual(slotDestination.Quantity, 5);

            // Move all to empty slot
            slotSource.ClearSlot();
            slotDestination.ClearSlot();
            slotSource.StoreItem(itemA, 10);
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 10);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCountSource, 1);
            Assert.AreEqual(callbackCountDestination, 1);
            Assert.AreEqual(slotSource.Item, null);
            Assert.AreEqual(slotSource.Quantity, 0);
            Assert.AreEqual(slotDestination.Item, itemA);
            Assert.AreEqual(slotDestination.Quantity, 10);

            // Move too much
            slotSource.ClearSlot();
            slotDestination.ClearSlot();
            slotSource.StoreItem(itemA, 10);
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 11);
            Assert.IsFalse(result);
            Assert.AreEqual(callbackCountSource, 0);
            Assert.AreEqual(callbackCountDestination, 0);
            Assert.AreEqual(slotSource.Item, itemA);
            Assert.AreEqual(slotSource.Quantity, 10);
            Assert.AreEqual(slotDestination.Item, null);
            Assert.AreEqual(slotDestination.Quantity, 0);

            // Move to slot of the same type
            slotSource.ClearSlot();
            slotDestination.ClearSlot();
            slotSource.StoreItem(itemA, 10);
            slotDestination.StoreItem(itemA, 10);
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 5);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCountSource, 1);
            Assert.AreEqual(callbackCountDestination, 1);
            Assert.AreEqual(slotSource.Item, itemA);
            Assert.AreEqual(slotSource.Quantity, 5);
            Assert.AreEqual(slotDestination.Item, itemA);
            Assert.AreEqual(slotDestination.Quantity, 15);

            // Move all to a slot of different type
            slotSource.ClearSlot();
            slotDestination.ClearSlot();
            slotSource.StoreItem(itemA, 10);
            slotDestination.StoreItem(itemB, 20);
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 10);
            Assert.IsTrue(result);
            Assert.AreEqual(callbackCountSource, 1);
            Assert.AreEqual(callbackCountDestination, 1);
            Assert.AreEqual(slotSource.Item, itemB);
            Assert.AreEqual(slotSource.Quantity, 20);
            Assert.AreEqual(slotDestination.Item, itemA);
            Assert.AreEqual(slotDestination.Quantity, 10);

            // Move part to a slot of different type
            slotSource.ClearSlot();
            slotDestination.ClearSlot();
            slotSource.StoreItem(itemA, 10);
            slotDestination.StoreItem(itemB, 20);
            callbackCountSource = 0;
            callbackCountDestination = 0;
            result = slotSource.MoveItemTo(slotDestination, 5);
            Assert.IsFalse(result);
            Assert.AreEqual(callbackCountSource, 0);
            Assert.AreEqual(callbackCountDestination, 0);
            Assert.AreEqual(slotSource.Item, itemA);
            Assert.AreEqual(slotSource.Quantity, 10);
            Assert.AreEqual(slotDestination.Item, itemB);
            Assert.AreEqual(slotDestination.Quantity, 20);
        }
    }
}
