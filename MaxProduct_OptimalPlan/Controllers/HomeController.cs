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
        [HttpGet]
        public ActionResult Index(InputData inputData)
        {
            var prod = new List<SolverRow>() {
                new SolverRow() { xId = 1, Al = 10, Med=20, Olovo=15, Zink=30, Svin=20, Cost = 30},
                new SolverRow() { xId = 2, Al = 70, Med=50, Olovo=35, Zink=40, Svin=45, Cost = 80},
                new SolverRow() { xId = 3, Al = 50, Med=35, Olovo=40, Zink=25, Svin=60, Cost = 65} };

            return View("Index", new InputData() { products = prod, Al_Constraint = 980, Med_Constraint = 860, Olovo_Constraint = 950, Zink_Constraint = 800, Svin_Constraint = 900 });
        }


        [HttpGet]
        public ActionResult Calc()
        {
            return View(new OutputData());
        }


        [HttpPost]
        public ActionResult Calc(InputData inputData)
        {

            List<SolverRow> solverList = new List<SolverRow>();

            // Исходные данные задачи
            solverList.AddRange(inputData.products);


            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            Set users = new Set(Domain.Any, "users");

            Parameter Cost = new Parameter(Domain.Real, "Cost", users);
            Cost.SetBinding(solverList, "Cost", "xId");

            Parameter Al = new Parameter(Domain.Real, "Al", users);
            Al.SetBinding(solverList, "Al", "xId");
            Parameter Med = new Parameter(Domain.Real, "Med", users);
            Med.SetBinding(solverList, "Med", "xId");
            Parameter Olovo = new Parameter(Domain.Real, "Olovo", users);
            Olovo.SetBinding(solverList, "Olovo", "xId");
            Parameter Zink = new Parameter(Domain.Real, "Zink", users);
            Zink.SetBinding(solverList, "Zink", "xId");
            Parameter Svin = new Parameter(Domain.Real, "Svin", users);
            Svin.SetBinding(solverList, "Svin", "xId");

            model.AddParameters(Cost, Al, Med, Olovo, Zink, Svin);

            Decision choose = new Decision(Domain.RealNonnegative, "choose", users);
            model.AddDecision(choose);

            model.AddGoal("goal", GoalKind.Maximize, Model.Sum(Model.ForEach(users, xId => choose[xId] * Cost[xId])));

            // Ограничения-неравенства
            model.AddConstraint("Nerav1", Model.Sum(Model.ForEach(users, xId => Al[xId] * choose[xId])) <= inputData.Al_Constraint);
            model.AddConstraint("Nerav2", Model.Sum(Model.ForEach(users, xId => Med[xId] * choose[xId])) <= inputData.Med_Constraint);
            model.AddConstraint("Nerav3", Model.Sum(Model.ForEach(users, xId => Olovo[xId] * choose[xId])) <= inputData.Olovo_Constraint);
            model.AddConstraint("Nerav4", Model.Sum(Model.ForEach(users, xId => Zink[xId] * choose[xId])) <= inputData.Zink_Constraint);
            model.AddConstraint("Nerav5", Model.Sum(Model.ForEach(users, xId => Svin[xId] * choose[xId])) <= inputData.Svin_Constraint);

            try
            {
                Solution solution = context.Solve();
                Report report = solution.GetReport();

                OutputData outputData = new OutputData();

                for (int i = 0; i < solverList.Count; i++)
                {
                    outputData.results.Add("X" + (i + 1).ToString(), choose.GetDouble(solverList[i].xId));
                    //reportStr += "Значение X" + (i + 1).ToString() + ": " +  + "\n";
                }

                return View("Calc", outputData);
            }
            catch (Exception ex)
            {
                return View("Index", inputData);
            }
        }
    }


}


