using NUnit.Framework;
using System.Collections.Generic;

namespace FluffyGameDev.Escapists.InventorySystem.Tests
{
    public class Inventory_Tests
    {
        [Test]
        public void TestCreateSlots()
        {
            Inventory inventory = new();

            Assert.AreEqual(0, inventory.slotCount);

            InventorySlot slotA = inventory.CreateSlot();
            Assert.AreEqual(1, inventory.slotCount);
            Assert.IsNotNull(slotA);

            InventorySlot slotB = inventory.CreateSlot();
            Assert.AreEqual(2, inventory.slotCount);
            Assert.IsNotNull(slotB);
            Assert.AreNotSame(slotA, slotB);
        }

        [Test]
        public void TestForEach()
        {
            Inventory inventory = new();
            inventory.CreateSlot();
            inventory.CreateSlot();
            inventory.CreateSlot();
            inventory.CreateSlot();
            inventory.CreateSlot();

            int iterationCount = 0;
            inventory.ForEachSlot(slot => ++iterationCount);
            Assert.AreEqual(inventory.slotCount, iterationCount);
        }

        [Test]
        public void TestFindSlot()
        {
            Inventory inventory = new();
            InventoryItem item = new();
            inventory.CreateSlot().StoreItem(item, 10);
            inventory.CreateSlot().StoreItem(item, 20);
            InventorySlot targetSlot = inventory.CreateSlot();
            targetSlot.StoreItem(item, 30);
            inventory.CreateSlot().StoreItem(item, 40);
            inventory.CreateSlot().StoreItem(item, 50);

            InventorySlot foundSlot = inventory.FindSlot(slot => slot.Quantity > 25);
            Assert.AreEqual(targetSlot, foundSlot);

            foundSlot = inventory.FindSlot(slot => slot.Quantity > 100);
            Assert.IsNull(foundSlot);
        }

        [Test]
        public void TestFilterSlots()
        {
            Inventory inventory = new();
            InventoryItem item = new();
            inventory.CreateSlot().StoreItem(item, 10);
            inventory.CreateSlot().StoreItem(item, 20);
            inventory.CreateSlot().StoreItem(item, 30);
            inventory.CreateSlot().StoreItem(item, 40);
            inventory.CreateSlot().StoreItem(item, 50);

            List<InventorySlot> result = new();
            inventory.FilterSlots(slot => slot.Quantity > 25, result);
            Assert.AreEqual(3, result.Count);
        }
    }
}
