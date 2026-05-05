namespace Common.HttpClient
{
    public class HttpServiceRequest
    {
        /// <summary>
        /// The base endpoint URL of the external or internal service.
        /// </summary>
        public string ServiceEndpoint { get; set; } = null!;

        /// <summary>
        /// The specific route or path to access within the service endpoint.
        /// </summary>
        public string RoutePath { get; set; } = null!;

        /// <summary>
        /// Optional query parameters to be appended to the request URL.
        /// </summary>
        public string? QueryString { get; set; }

        /// <summary>
        /// The body payload to be sent with the request (e.g., POST, PUT).
        /// </summary>
        public object? BodyPayload { get; set; }

        /// <summary>
        /// JWT or other token used for authorization.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Code identifying the tenant (used in multi-tenant applications).
        /// </summary>
        public string? TenantCode { get; set; }

        /// <summary>
        /// Unique identifier used to trace or correlate requests across services.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Indicates whether the request should be authorized.
        /// </summary>
        public bool IsAuthorized { get; set; }
        /// <summary>
        /// Indicates whether the solution is multi-tenant.
        /// </summary>
        public bool IsMutiTenentSolution { get; set; }
    }
}

