using MaxProduct_OptimalPlan.Models;
using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaxProduct_OptimalPlan.Controllers
{
    public class HomeController : Controller
    {

        InputData InputData = new InputData()
        {
            A1_output = 25,
            A2_output = 10,
            A3_output = 30,
            A4_output = 20,
            A5_output = 15,
            Workshops = new List<SolverRow>() {
                new SolverRow() { xId = 1, A1_DeliveryCost = 1, A2_DeliveryCost=3, A3_DeliveryCost=2, A4_DeliveryCost=5, A5_DeliveryCost=4, Needs = 30},
                new SolverRow() { xId = 2, A1_DeliveryCost = 3, A2_DeliveryCost=5, A3_DeliveryCost=7, A4_DeliveryCost=3, A5_DeliveryCost=2, Needs = 20},
                new SolverRow() { xId = 3, A1_DeliveryCost = 2, A2_DeliveryCost=8, A3_DeliveryCost=4, A4_DeliveryCost=6, A5_DeliveryCost=5, Needs = 25},
                new SolverRow() { xId = 4, A1_DeliveryCost = 5, A2_DeliveryCost=8, A3_DeliveryCost=6, A4_DeliveryCost=1, A5_DeliveryCost=3, Needs = 25},
            }
        };
        [HttpGet]
        public ActionResult Index()
        {
            return View(InputData);
        }

        public ActionResult Calc()
		{
            return RedirectToAction(nameof(Index));
		}


        [HttpPost]

        public object Calc(double[][] products, double[][] pr)
        {
            double[] avg = new double[products.Length], avg1 = new double[products[0].Length];

            for(int i = 0; i < products.Length; i++)
			{
                avg[i] = products[i].Sum();
			}
            for (int i = 0; i < products[0].Length; i++)
            {
                double sum = 0;
                for(int i1 = 0; i1 < products.Length; i1++)
				{
                    sum += products[i1][i];
				}

                avg1[i] = sum;
            }



            double[][] array = new double[products.Length][];

            for(int i = 0; i < array.Length; i++)
			{
                array[i] = new double[products[i].Length];
                for(int i1 = 0; i1 < products[i].Length; i1++)
				{
                    array[i][i1] = products[i][i1] * pr[i][i1];
				}
			}

            double[] avg2 = new double[array[0].Length];
            for (int i = 0; i < array[0].Length; i++)
            {
                double sum = 0;
                for (int i1 = 0; i1 < array.Length; i1++)
                {
                    sum += array[i1][i];
                }

                avg2[i] = sum;
            }


            return View(
                new OutputData()
                {
                    avg = avg,
                    avg1 = avg1,
                    avg2 = avg2,
                    pr = products,
                    array = array
                }
                );

        }
    }

}


