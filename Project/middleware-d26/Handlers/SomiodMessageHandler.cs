using middleware_d26.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

public class SomiodMessageHandler : DelegatingHandler
{
    private readonly DiscoverService discoverService;
    private readonly XmlSchemaSet schemaSet;

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
        this.schemaSet.Add("Middleware-d26", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Schemas", "EntityRequestSchema.xsd"));
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
                return await HandleReceivedContent(request, cancellationToken);
            case "PUT":
                return await HandleReceivedContent(request, cancellationToken);
            case "DELETE":
                return await base.SendAsync(request, cancellationToken);
            default:
                return CreateResponse(HttpStatusCode.BadRequest, "Unknown request method");
        }
    }

    private async Task<HttpResponseMessage> HandleGetRequest(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var routeData = request.GetRouteData();
        var applicationName = routeData.Values["applicationName"] as string;
        var containerName = routeData.Values["containerName"] as string;
        Debug.WriteLine($"Route data: {routeData}");
        Debug.WriteLine($"Application name: {applicationName}");
        Debug.WriteLine($"Container name: {containerName}");
        Debug.WriteLine($"Request URI: {request.RequestUri}");

        // Check header -> Accept = application/xml
        var acceptHeader = request.Headers.Accept.FirstOrDefault();

        if (acceptHeader == null || !acceptHeader.MediaType.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
        {
            return CreateResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported media type");
        }

        // Check header -> somiod-discover = application|container|data|subscription
        if (request.Headers.TryGetValues("somiod-discover", out var discoverValues))
        {
            var discoverType = discoverValues.FirstOrDefault();

            switch (discoverType)
            {
                case "application":
                    var applications = discoverService.DiscoverApplications();
                    return CreateResponse(HttpStatusCode.OK, applications);

                case "container":
                    Debug.WriteLine($"Application name: {applicationName}");

                    if (string.IsNullOrEmpty(applicationName))
                    {
                        // Handle the case where applicationName is not present in the route data
                        return CreateResponse(HttpStatusCode.BadRequest, "Application name not specified in the route.");
                    }

                    var containers = discoverService.DiscoverContainers(applicationName);
                    return CreateResponse(HttpStatusCode.OK, containers);

                case "data":
                    if (string.IsNullOrEmpty(containerName))
                    {
                        return CreateResponse(HttpStatusCode.BadRequest, "Parent name not specified in the route.");
                    }

                    var dataRecords = discoverService.DiscoverDataRecords(applicationName, containerName);
                    return CreateResponse(HttpStatusCode.OK, dataRecords);

                case "subscription":
                    if (string.IsNullOrEmpty(containerName))
                    {
                        return CreateResponse(HttpStatusCode.BadRequest, "Parent name not specified in the route.");
                    }

                    var subscriptions = discoverService.DiscoverSubscriptions(applicationName, containerName);
                    return CreateResponse(HttpStatusCode.OK, subscriptions);

                default:
                    return CreateResponse(HttpStatusCode.BadRequest, "Unknown discover type");
            }
        }
        else
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }

    private async Task<HttpResponseMessage> HandleReceivedContent(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Check header -> accept = application/xml
        var contentType = request.Content.Headers.ContentType?.MediaType;

        if (string.IsNullOrEmpty(contentType) || !contentType.Equals("application/xml", StringComparison.OrdinalIgnoreCase))
        {
            return CreateResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported media type");
        }

        var content = await request.Content.ReadAsStringAsync();

        // Log received XML content
        Debug.WriteLine($"Received XML content:\n{content}");

        var xml = XDocument.Parse(content);
        XNamespace ns = "Middleware-d26";
        var resTypeElement = xml.Descendants(ns + "res_type").FirstOrDefault();

        // Log extracted ResType
        var resType = resTypeElement?.Value;
        Debug.WriteLine($"Extracted ResType: {resType}");

        if (string.IsNullOrEmpty(resType))
        {
            return CreateResponse(HttpStatusCode.BadRequest, "res_type not specified");
        }

        ValidateXml(xml);
        if (!isValid)
        {
            return CreateResponse(HttpStatusCode.BadRequest, validationMessage);
        }

        // Properly await the SendAsync method
        return await base.SendAsync(request, cancellationToken);
    }

    private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object content)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new ObjectContent(content.GetType(), content, new XmlMediaTypeFormatter())
        };
    }

    private bool ValidateXml(XDocument xml)
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
            Debug.WriteLine("ValidateXML" + validationMessage);
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
                Debug.WriteLine("ValidateMethod" + validationMessage);
                break;
            case XmlSeverityType.Warning:
                validationMessage = string.Format("WARNING: {0}", args.Message);
                Debug.WriteLine("ValidateMethod2" + validationMessage);
                break;
            default:
                break;
        }
    }
}
