using middleware_d26.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Linq;
using System;
using middleware_d26.Services;
using System.Net;
using System.Net.Http.Formatting;
using System.Xml.Schema;
using middleware_d26.Models.DTOs;
using System.Data.Entity.Core.Metadata.Edm;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;

public class SomiodMessageHandler : DelegatingHandler
{
    private readonly DiscoverService discoverService;
    private XmlSchemaSet schemaSet;

    private bool isValid = true;
    private string validationMessage;
    public string ValidationMessage
    {
        get { return validationMessage; }
    }

    public SomiodMessageHandler(DiscoverService discoverService)
    {
        this.discoverService = discoverService ?? throw new ArgumentNullException(nameof(discoverService));
        this.schemaSet = new XmlSchemaSet();
        //this.schemaSet.Add("", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "CreateApplication.xsd"));
        //this.schemaSet.Add("", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "CreateContainer.xsd"));
        //this.schemaSet.Add("", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "CreateData.xsd"));
        this.schemaSet.Add("http://www.middleware-d26.com/schemas/CreateSubscription", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "CreateSubscription.xsd"));
    }

    async protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {     
        var requestMethod = request.Method;

        switch (requestMethod.Method)
        {
            case "GET":
                return await HandleGetRequest(request, cancellationToken);
            case "POST":
                return await HandlePostRequest(request, cancellationToken);
            case "PUT":
                return await HandlePutRequest(request, cancellationToken);
            case "DELETE":
                return await HandleDeleteRequest(request, cancellationToken);
            default:
                return CreateResponse(HttpStatusCode.BadRequest, "Unknown request method");
        }        
    }

    private async Task<HttpResponseMessage> HandleGetRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {      
        if (request.Headers.TryGetValues("somiod-discover", out var discoverValues))
        {
            var discoverType = discoverValues.FirstOrDefault();

            switch (discoverType)
            {
                case "application":
                    var applications = discoverService.GetApplications();
                    return CreateResponse(HttpStatusCode.OK, applications);

                case "container":
                    // Extract application name from route data
                    var routeData = request.GetRouteData();
                    var applicationName = routeData.Values["id"] as string;

                    if (string.IsNullOrEmpty(applicationName))
                    {
                        // Handle the case where applicationName is not present in the route data
                        return CreateResponse(HttpStatusCode.BadRequest, "Application name not specified in the route.");
                    }

                    var containers = discoverService.GetContainers(applicationName);
                    return CreateResponse(HttpStatusCode.OK, containers);

                case "data":
                    // Extract parent name from route data
                    var routeDataData = request.GetRouteData();
                    var parentNameData = routeDataData.Values["id"] as string;

                    if (string.IsNullOrEmpty(parentNameData))
                    {
                        // Handle the case where parentName is not present in the route data
                        return CreateResponse(HttpStatusCode.BadRequest, "Parent name not specified in the route.");
                    }

                    var dataRecords = discoverService.GetDataRecords(parentNameData);
                    return CreateResponse(HttpStatusCode.OK, dataRecords);

                case "subscription":
                    //var subscription = await request.Content.ReadAsAsync<Subscription>();
                    // Logic to handle subscription creation
                    // SetupNotification(subscription);
                    return CreateResponse(HttpStatusCode.OK, "Subscription created successfully");

                default:
                    return CreateResponse(HttpStatusCode.BadRequest, "Unknown discover type");
            }
        }
        else
        {
            // Process the regular HTTP request
            Debug.WriteLine("Processing regular request");

            // Call the inner handler to continue processing the request
            var response = await base.SendAsync(request, cancellationToken);

            // Process the response message as needed
            Debug.WriteLine("Processing response");

            // Return the modified response
            return response;
        }
    }

    private async Task<HttpResponseMessage> HandlePostRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var content = await request.Content.ReadAsStringAsync();
        var xml = XDocument.Parse(content);
        var resType = xml.Root?.Element("res_type")?.Value;

        if(string.IsNullOrEmpty(resType))
        {
            return CreateResponse(HttpStatusCode.BadRequest, "Res_type not specified");
        }

        ValidateXml(xml, resType);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<HttpResponseMessage> HandlePutRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<HttpResponseMessage> HandleDeleteRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
    }

    private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new ObjectContent(content.GetType(), content, new XmlMediaTypeFormatter())
        };
    }

    private bool ValidateXml(XDocument xml, string resType)
    {
        isValid = true;
        try
        {
            ValidationEventHandler eventHandler = new ValidationEventHandler(ValidateMethod);
            xml.Validate(schemaSet, eventHandler);
        }
        catch (Exception ex)
        {
            isValid = false;
            validationMessage = string.Format("ERROR: {0}", ex.ToString());
        }

        return isValid;
    }

    private void ValidateMethod(object sender, ValidationEventArgs args)
    {
        isValid = false;
        switch (args.Severity)
        {
            case XmlSeverityType.Error:
                validationMessage = string.Format("ERROR: {0}", args.Message);
                break;
            case XmlSeverityType.Warning:
                validationMessage = string.Format("WARNING: {0}", args.Message);
                break;
            default:
                break;
        }
    }
}
