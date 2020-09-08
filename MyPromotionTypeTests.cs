using NUnit.Framework;
using PromotionApp;
using System.Collections.Generic;

namespace MyPromoTest
{
    public class Tests
    {
        Promotion[] arrPromotions = new Promotion[3];

        [SetUp]
        public void Setup()
        {
            // Availabale Promotion types
            arrPromotions[0] = new PromotionType3As();
            arrPromotions[1] = new PromotionType2Bs();
            arrPromotions[2] = new PromotionTypeCAndD();

            // Active Promotion Types
            //  3 odf A for 130, 
            //  2 of B's for 45
            //  C & D for 30
            //  Where Unit Price Of SKU's - A=50, B=30, C=20, D=15
        }

        [TestCase]
        public void When_OneQtyOfEachProductExceptD_Expect_NoDiscount()
        {
            //Arrange
            Cart oCart = new Cart();
            oCart.Items = new List<Item>()
            {
                new Item(){  SkuId = "A", Price = 50, Quantity = 1},
                new Item(){  SkuId = "B", Price = 30, Quantity = 1},
                new Item(){  SkuId = "C", Price = 20, Quantity = 1}
            };

            //Act
            PromotionCalculator PC = new PromotionCalculator(oCart.Items);
            double TotalOrderValue = PC.ApplyPromotion(arrPromotions);

            //Assert
            Assert.AreEqual(TotalOrderValue, 100);
        }

        [TestCase]
        public void When_FiveQtyOfAAndOneQtyOfBAndC_Expect_PromotionType3AsDiscount()
        {
            //Arrange
            Cart oCart = new Cart();
            oCart.Items = new List<Item>()
            {
                new Item(){  SkuId = "A", Price = 50, Quantity = 5},
                new Item(){  SkuId = "B", Price = 30, Quantity = 1},
                new Item(){  SkuId = "C", Price = 20, Quantity = 1}
            };

            //Act
            PromotionCalculator PC = new PromotionCalculator(oCart.Items);
            double TotalOrderValue = PC.ApplyPromotion(arrPromotions);

            //Assert
            Assert.AreEqual(TotalOrderValue, 280);
        }

        [TestCase]
        public void When_FiveQtyOfA_FiveQtyOfB_OneQtyOfC_Expect_PromotionType3AsAndPromotionType2BsDiscount()
        {
            //Arrange
            Cart oCart = new Cart();
            oCart.Items = new List<Item>()
            {
                new Item(){  SkuId = "A", Price = 50, Quantity = 5},
                new Item(){  SkuId = "B", Price = 30, Quantity = 5},
                new Item(){  SkuId = "C", Price = 20, Quantity = 1}
            };

            //Act
            PromotionCalculator PC = new PromotionCalculator(oCart.Items);
            double TotalOrderValue = PC.ApplyPromotion(arrPromotions);

            //Assert
            Assert.AreEqual(TotalOrderValue, 370);
        }

        [TestCase]
        public void When_ThreeQtyOfA_FiveQtyOfB_OneQtyOfC_OneQtyOfD_Expect_AllPromotionTypeDiscounts()
        {
            //Arrange
            Cart oCart = new Cart();
            oCart.Items = new List<Item>()
            {
                new Item(){  SkuId = "A", Price = 50, Quantity = 3},
                new Item(){  SkuId = "B", Price = 30, Quantity = 5},
                new Item(){  SkuId = "C", Price = 20, Quantity = 1},
                new Item(){  SkuId = "D", Price = 15, Quantity = 1}
            };

            //Act
            PromotionCalculator PC = new PromotionCalculator(oCart.Items);
            double TotalOrderValue = PC.ApplyPromotion(arrPromotions);

            //Assert
            Assert.AreEqual(TotalOrderValue, 280);
        }
    }
}