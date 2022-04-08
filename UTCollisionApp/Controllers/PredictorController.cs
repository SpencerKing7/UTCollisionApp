using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using UTCollisionApp.Models;

namespace UTCollisionApp.Controllers
{
    [AllowAnonymous]
    public class PredictorController : Controller
    {
        public InferenceSession _severitySession;
        public InferenceSession _citySession;
        public InferenceSession _countySession;
        public PredictorController()
        {
            _severitySession = new InferenceSession("severity_predictor.onnx");
            _citySession = new InferenceSession("city_predictor.onnx");
            _countySession = new InferenceSession("county_predictor.onnx");

            //_severitySession.ModelMetadata.GraphName = 'test';
            //_citySession.ModelMetadata.GraphName = 'test';
            //_countySession.ModelMetadata.GraphName = 'test';
        }

        //For our severity predictor
        [HttpGet]
        public IActionResult SeverityCalc(Prediction prediction)
        {
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            ViewBag.PredictionScore = prediction.PredictedSeverity;

            return View(new SeverityPredictorData());
        }

        //For our crash locator
        [HttpGet]
        public IActionResult CrashLocator(CityPrediction cp)
        {

            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            ViewBag.PredictedCity = cp.PredictedCity;

            return View(new CityPredictorData());
        }

        //For our county locator
        [HttpGet]
        public IActionResult CountyLocator(CountyPrediction cp)
        {

            //Button Viewbags
            ViewBag.Button = "Admin Sign In";
            ViewBag.Controller = "Admin";
            ViewBag.Action = "AdminHome";

            ViewBag.PredictedCounty = cp.PredictedCounty;

            return View(new CountyPredictorData());
        }

        public IActionResult Predict(SeverityPredictorData data)
        {
            var severityResult = _severitySession.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = severityResult.First().AsTensor<float>();
            var roundedScore = score.First();

            roundedScore = (float)System.Math.Round(roundedScore, 2);

            var prediction = new Prediction { PredictedSeverity = roundedScore };

            return RedirectToAction("SeverityCalc", prediction);
        }

        public IActionResult Citypredict(CityPredictorData data)
        {
            var CityResult = _citySession.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<string> score = CityResult.First().AsTensor<string>();
            var Score = score.First();

            //roundedScore = (float)System.Math.Round(roundedScore, 2);

            var prediction = new CityPrediction { PredictedCity = Score };

            return RedirectToAction("CrashLocator", prediction);
        }

        public IActionResult CountyPredict(CountyPredictorData data)
        {
            var countyResult = _countySession.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<string> score = countyResult.First().AsTensor<string>();
            var Score = score.First();

            //roundedScore = (float)System.Math.Round(roundedScore, 2);

            var prediction = new CountyPrediction { PredictedCounty = Score };

            return RedirectToAction("CountyLocator", prediction);
        }
    }
}