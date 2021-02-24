using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using dataSite.model;
using HtmlAgilityPack;
using Newtonsoft.Json;
//this is my final
namespace dataSite
{
    class Program
    {
        static void Main(string[] args)
        {
            model.DIGIKALAEntities1 db = new DIGIKALAEntities1();
            
            //start from Supermarket 
            string supermarketPath = "https://www.digikala.com/main/food-beverage";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document = web.Load(supermarketPath);

            var categories = document.DocumentNode.SelectNodes("//a[@class='c-catalog__plain-list-link']");
            //already got every category in Supermarket's page
            List<string> urls = new List<string>();
            foreach(var item in categories)
            {
                string temp = item.GetAttributeValue("href", "");
                temp = temp.Replace("?fresh=1", " ");
                urls.Add("https://www.digikala.com" + temp);
               
            }

            foreach(var item in urls)
            {
                string mainCategory =item.Replace("https://www.digikala.com/search/", " ");
                mainCategory = mainCategory.Replace("/", "");
                mainCategory = mainCategory.Replace("category-", "");
                document = web.Load(item);
                var page = document.DocumentNode.SelectSingleNode("//a[@class='c-pager__next']");
                string page_num = page.GetAttributeValue("data-page", "");
                 int num;
                Int32.TryParse(page_num.Trim(), out num);
                
                for ( int i=1;i<=num;i++)

                { 
                    string temp = item;
                    temp = temp + "?pageno=" + i + "&sortby=4&fresh=1";
                    document = web.Load(temp);
                    var product = document.DocumentNode.SelectNodes("//div[@class='c-product-box__title']/a[@class='js-product-url']");
                    List<string> product_urls = new List<string>();

                    foreach (var item1 in product)
                    {
                        string temp1 = item1.GetAttributeValue("href", "");
                        temp1 = temp1.Replace("?fresh=1", " ");
                        
                        product_urls.Add("https://www.digikala.com" + temp1);

                    }
                    //find the max in pages

                    //استخراج داده‌های محصولات
                    foreach (string item2 in product_urls)
                    {
                        Product productList = new Product();
                        //string FilePath = @"D:\DigiKala\";

                        document = web.Load(item2);
                        IDictionary<string, string> productInfo = new Dictionary<string, string>();

                        var name = document.DocumentNode.SelectSingleNode("//h1[@class='c-product__title']/text()");
                        var nodes1 = document.DocumentNode.SelectNodes("//div[@class='c-params__list-key']/span/text()");
                        var nodes2 = document.DocumentNode.SelectNodes("//div[@class='c-params__list-value']/span/text()");
                        string CreateText = name.InnerText.TrimStart();
                        productList.Name = name.InnerText.Trim();
                        int permission0 = 0;
                        permission0 = productList.Name.IndexOf("کراستینو");
                        if (permission0 <=0)
                        {
                            productList.Category = mainCategory;

                            var node2_counter = nodes2.First();
                            foreach (var node in nodes1)
                            {
                                productInfo.Add(node.InnerText.Trim(), node2_counter.InnerText.Trim());
                                CreateText = CreateText + "\n" + node.InnerText.Trim() + ":" + node2_counter.InnerText.Trim();
                                nodes2.Remove(node2_counter);
                                if (nodes2.Count == 0)
                                {
                                    break;
                                }
                                node2_counter = nodes2.First();

                            }

                            foreach (var node2 in nodes2)
                            {    //بخش توضیحات در دیجی کالا
                                string total = node2.InnerText.ToString();
                                total = total.Replace("-", "");
                                if (total.IndexOf(":") > 0)
                                {
                                    string[] words = total.Split(':');
                                    CreateText = CreateText + "\n" + total.Trim();
                                    productInfo.Add(words[0].Trim(), words[1].Trim());
                                }

                            }
                            //این جا چک می‌کنیم که آیا در مشخصات محصول توضیحات انرژی و قند وجود دارند،
                            //اگر ندارند از ذخیره محصول امتناع می‌کنیم
                            int permisssion1 = 0, permisssion2 = 0;
                            permisssion1 = CreateText.IndexOf("انرژی");
                            permisssion2 = CreateText.IndexOf("قند");
                            if (permisssion1 > 0 || permisssion2 > 0)
                            {
                                string jsonString = JsonConvert.SerializeObject(productInfo);
                                productList.data = jsonString;
                                db.Products.Add(productList);
                                db.SaveChanges();
                                Console.Write("one row added. \n");
                                //File.WriteAllText(FilePath + name.InnerText.Trim() + ".txt", CreateText);
                                //این‌جا در db ذخیره می‌کنیم
                            }

                        }
                    }
                }


            }

            // string urlAddress = "https://www.digikala.com/product/dkp-674799/%D9%88%DB%8C%D9%81%D8%B1-%D8%A8%D8%A7-%D8%B1%D9%88%DA%A9%D8%B4-%D8%B4%DA%A9%D9%84%D8%A7%D8%AA-%D8%AA%D9%84%D8%AE-%DA%A9%D9%88%D9%BE%D8%A7-%D8%A8%D8%B3%D8%AA%D9%87-30-%D8%B9%D8%AF%D8%AF%DB%8C#/tab-params";


            //       HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            //       document = web.Load(urlAddress);

            //string FilePath = @"D:\DigiKala\";



            //var name = document.DocumentNode.SelectSingleNode("//h1[@class='c-product__title']/text()");
            //var nodes1 = document.DocumentNode.SelectNodes("//div[@class='c-params__list-key']/span/text()");
            //var nodes2 = document.DocumentNode.SelectNodes("//div[@class='c-params__list-value']/span/text()");
            //string CreateText = name.InnerText.Trim();
            //var node2_counter = nodes2.First();
            //foreach (var node in nodes1)
            //{

            //    CreateText = CreateText + "\n" + node.InnerText.Trim() + " \t: \t" + node2_counter.InnerText.Trim();
            //    nodes2.Remove(node2_counter);
            //    node2_counter = nodes2.First();
            //}

            //foreach (var node2 in nodes2)
            //{
            //    string total = node2.InnerText.ToString();
               

            //}
            //File.WriteAllText(FilePath + " first.txt", CreateText);





        }
    }
}
