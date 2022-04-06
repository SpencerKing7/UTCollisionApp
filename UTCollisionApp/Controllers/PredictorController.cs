using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using UTCollisionApp.Models;

namespace UTCollisionApp.Controllers
{
    public class PredictorController : Controller
    {
        private InferenceSession _session;

        public PredictorController(InferenceSession session)
        {
            _session = session;
        }
        
        //[HttpGet]
        //public IActionResult SeverityCalc()
        //{
        //    ViewBag.Button = "Admin Sign In";
        //    ViewBag.Controller = "Admin";
        //    ViewBag.Action = "AdminHome";

        //    ViewBag.PredictionScore = 0;

        //    return View(new SeverityPredictorData());
        //}

        [HttpGet]
        public IActionResult SeverityCalc(Prediction prediction)
        {
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            ViewBag.PredictionScore = prediction.PredictedSeverity;

            return View(new SeverityPredictorData());
        }

        [HttpGet]
        public IActionResult Results(Prediction p)
        {
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            return View(p);
        }

        public IActionResult Predict(SeverityPredictorData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var roundedScore = score.First();

            roundedScore = (float)System.Math.Round(roundedScore, 2);

            var prediction = new Prediction { PredictedSeverity = roundedScore };

            return RedirectToAction("SeverityCalc", prediction);
        }
    }
}