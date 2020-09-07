using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PromotionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!!!");
            Cart oCart = new Cart(); 
            oCart.Items = new List<Item>()
            {
                new Item(){  SkuId = "A", Price = 50, Quantity = 3},
                new Item(){  SkuId = "B", Price = 30, Quantity = 5},
                new Item(){  SkuId = "C", Price = 20, Quantity = 1},
                new Item(){  SkuId = "D", Price = 15, Quantity = 1}
            };


            Promotion[] arrPromotions = new Promotion[3];
            arrPromotions[0] = new PromotionType3As();
            arrPromotions[1] = new PromotionType2Bs();
            arrPromotions[2] = new PromotionTypeCAndD();

            PromotionCalculator PC = new PromotionCalculator(oCart.Items);
            double TotalOrderValue = PC.ApplyPromotion(arrPromotions);

            Console.WriteLine("Total Order Value: " + TotalOrderValue.ToString());
        }
    }

    class Cart
    {
        public List<Item> Items = new List<Item>();
        double OrderValue;
        double PromotionAppiedValue;
    }

    public class Item
    {
        public string SkuId;
        public int Quantity;
        public double Price;
    }
    public abstract class Promotion
    {
        public abstract double ApplyPromotion(List<Item> Items);
    }
    public class PromotionType3As : Promotion
    {
        //List<Item> Items = new List<Item>();
        public double PromotionApplied = 0;
        public double RestItemTotalPrice = 0;

        public override double ApplyPromotion(List<Item> Items)
        {
            double TotalPriceWithDiscount = 0;
            int items = Items.Where<Item>(x => x.SkuId == "A").Count();
            if (items > 0)
            {
                var totalItemsQty = Items.Where<Item>(x => x.SkuId == "A").FirstOrDefault().Quantity;
                double itemPrice = Items.Where<Item>(x => x.SkuId == "A").FirstOrDefault().Price;
                int totalItemsEligible = totalItemsQty / 3;
                PromotionApplied = totalItemsEligible * 130;
                TotalPriceWithDiscount = PromotionApplied;

                if (totalItemsQty - (totalItemsEligible * 3) > 0)
                {
                    RestItemTotalPrice = (totalItemsQty - (totalItemsEligible * 3)) * itemPrice;
                    TotalPriceWithDiscount = PromotionApplied + RestItemTotalPrice;
                }
                
            }
            return TotalPriceWithDiscount;
        }
    }

    public class PromotionType2Bs : Promotion
    {
        //List<Item> Items = new List<Item>();
        public double PromotionApplied;
        public double RestItemTotalPrice;

        public override double ApplyPromotion(List<Item> Items)
        {
            double TotalPriceWithDiscount = 0;
            int items = Items.Where<Item>(x => x.SkuId == "B").Count();
            if (items > 0)
            {
                var totalItemsQty = Items.Where<Item>(x => x.SkuId == "B").FirstOrDefault().Quantity;
                double itemPrice = Items.Where<Item>(x => x.SkuId == "B").FirstOrDefault().Price;
                int totalItemsEligible = totalItemsQty / 2;
                PromotionApplied = totalItemsEligible * 45;
                TotalPriceWithDiscount = PromotionApplied;

                if (totalItemsQty - (totalItemsEligible * 2) > 0)
                {
                    RestItemTotalPrice = (totalItemsQty - (totalItemsEligible * 2)) * itemPrice;
                    TotalPriceWithDiscount = PromotionApplied + RestItemTotalPrice;
                }
            }
            return TotalPriceWithDiscount;
        }
    }

    public class PromotionTypeCAndD : Promotion
    {
        //List<Item> Items = new List<Item>();
        public double PromotionApplied = 0;
        public double RestItemTotalPrice = 0;
        int totalItemsEligible = 0;
        int totalItemsOfCQty = 0;
        int totalItemsOfDQty = 0;
        double itemPriceOfC = 0;
        double itemPriceOfD = 0;
        public override double ApplyPromotion(List<Item> Items)
        {
            double TotalPriceWithDiscount = 0;
           
            int itemsOfC = Items.Where<Item>(x => x.SkuId == "C").Count();
            int itemsOfD = Items.Where<Item>(x => x.SkuId == "D").Count();

            if (itemsOfC > 0)
            {
                totalItemsOfCQty = Items.Where<Item>(x => x.SkuId == "C").FirstOrDefault().Quantity;
                itemPriceOfC = Items.Where<Item>(x => x.SkuId == "C").FirstOrDefault().Price;
            }
            if (itemsOfD > 0)
            {
                totalItemsOfDQty = Items.Where<Item>(x => x.SkuId == "D").FirstOrDefault().Quantity;
                itemPriceOfD = Items.Where<Item>(x => x.SkuId == "D").FirstOrDefault().Price;
            }

            if (itemsOfC > 0 && itemsOfD > 0)
            {
                if (totalItemsOfCQty > totalItemsOfDQty)
                {
                    totalItemsEligible = totalItemsOfDQty;
                    PromotionApplied = (totalItemsOfDQty * 30);
                    RestItemTotalPrice = (totalItemsOfCQty - totalItemsOfDQty) * itemPriceOfC;
                }
                else if (totalItemsOfDQty > totalItemsOfCQty)
                {
                    totalItemsEligible = totalItemsOfCQty;
                    PromotionApplied = (totalItemsOfCQty * 30);
                    RestItemTotalPrice = (totalItemsOfDQty - totalItemsOfCQty) * itemPriceOfD;
                }
                else
                {
                    PromotionApplied = (totalItemsOfCQty * 30);
                }

                TotalPriceWithDiscount = PromotionApplied + RestItemTotalPrice;
            }
            else if(itemsOfC > 0 || itemsOfD > 0)
            {
                if (itemsOfC > 0)
                    TotalPriceWithDiscount = itemPriceOfC* totalItemsOfCQty;
                if (itemsOfD > 0)
                    TotalPriceWithDiscount = itemPriceOfD * totalItemsOfDQty;
            }


            return TotalPriceWithDiscount;
        }
    }

    public class PromotionCalculator
    {
        private List<Item> Items = new List<Item>();
        public PromotionCalculator(List<Item> Items)
        {
            this.Items = Items;
        }
        public double ApplyPromotion(Promotion[] arrPromotions)
        {
            double totalOrderValue = 0;
            foreach (var promotion in arrPromotions)
            {
                totalOrderValue += promotion.ApplyPromotion(Items);
            }
            return totalOrderValue;
        }
    }
}
