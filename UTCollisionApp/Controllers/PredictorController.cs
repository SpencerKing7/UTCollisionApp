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
    [ApiController]
    [Route("/predict")]

    public class ValuesController : ControllerBase
    {
        private InferenceSession _session;

        public ValuesController(InferenceSession session)
        {
            _session = session;
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult Predict(SeverityPredictorData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedValue = score.First() };
            result.Dispose();
            return Ok(prediction);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public ActionResult PredictForm([FromForm] SeverityPredictorData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedValue = score.First()};
            result.Dispose();
            return Ok(prediction);
        }
    }
}