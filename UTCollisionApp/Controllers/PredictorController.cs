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

        [HttpGet]
        public IActionResult Results(Prediction p)
        {
            return View(p);
        }

        public IActionResult Predict(SeverityPredictorData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedSeverity = score.First() };

            return RedirectToAction("Results", prediction);
        }
    }
}