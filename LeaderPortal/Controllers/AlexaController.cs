using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaderPortal.Controllers
{
    [Produces("application/json")]
    [Route("api/alexa")]
    public class AlexaController : Controller
    {
        [HttpPost]
        public dynamic LeaderPortal([FromBody] AlexaRequest alexaRequest)
        {
            AlexaResponse response = null;

            switch (alexaRequest.Request.Type)
            {
                case "LaunchRequest":
                    response = LaunchRequestHandler(alexaRequest);
                    break;
                case "IntentRequest":
                    response = IntentRequestHandler(alexaRequest);
                    break;
                case "SessionEndRequest":
                    response = SessionEndedRequestHandler(alexaRequest);
                    break;
            }

            return response;
        }

        private AlexaResponse SessionEndedRequestHandler(object request)
        {
            return null;
        }

        private AlexaResponse IntentRequestHandler(AlexaRequest request)
        {
            AlexaResponse response = null;

            switch(request.Request.Intent.Name)
            {
                case "AMAZON.CancelIntent":
                case "AMAZON.StopIntent":
                    response = CancelOrStopIntentHandler(request);
                    break;
                case "AMAZON.HelpIntent":
                    response = HelpIntent(request);
                    break;
                case "TemplateIntent":
                    response = TemplateIntent(request);
                    break;
                case "SearchIntent":
                    response = SearchIntent(request);
                    break;
            }

            return response;
        }

        private AlexaResponse SearchIntent(AlexaRequest request)
        {
            AlexaResponse response = null;

            var slots = request.Request.Intent.GetSlots();
            var value = slots.FirstOrDefault(s => s.Key == "QUERY").Value;

            switch (value.ToUpper())
            {
                case "ADMIN":
                    response = new AlexaResponse("Portal has 5 admin templates", false);
                    break;
                case "USER":
                    response = new AlexaResponse("Portal has 3 user templates", false);
                    break;
            }

            return response;
        }

        private AlexaResponse TemplateIntent(AlexaRequest request)
        {
            return new AlexaResponse("There are 3 different templates available.", false);
        }

        private AlexaResponse HelpIntent(AlexaRequest request)
        {
            var response = new AlexaResponse("To use the Leadership Portal skill, you can say, Alexa, ask Leadership Portal for tempalte information.", false);
            response.Response.Reprompt.OutputSpeech.Text = "Please go ahead and ask your questions";
            return response;
        }

        private AlexaResponse CancelOrStopIntentHandler(AlexaRequest request)
        {
            return new AlexaResponse("Thanks for listening, let's talk again soon.", true);
        }

        private AlexaResponse LaunchRequestHandler(AlexaRequest alexaRequest)
        {
            var response = new AlexaResponse("Welcome to leadership portal. Please ask any leadership portal related questions");
            response.Session.MemberId = (alexaRequest.Session.Attributes == null) ? 0 : alexaRequest.Session.Attributes.MemberId;
            response.Response.Card.Title = "Leadership Portal";
            response.Response.Card.Content = "Welcome!";
            response.Response.Reprompt.OutputSpeech.Text = "You can proceed with answer questions related to leadership portal?";
            response.Response.ShouldEndSession = false;

            return response;
        }
    }
}