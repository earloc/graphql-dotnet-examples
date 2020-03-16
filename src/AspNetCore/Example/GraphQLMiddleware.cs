using GraphQL;
using GraphQL.DataLoader;
using GraphQL.NewtonsoftJson;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.Common;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Example
{
    //https://github.com/fiyazbinhasan/GraphQLCore/blob/Part_X_DataLoader/GraphQLAPI/Middleware/GraphQLMiddleware.cs
        public class GraphQLMiddleware

        {

            private readonly RequestDelegate _next;

            private readonly IDocumentWriter _writer;

            private readonly IDocumentExecuter _executor;



            public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)

            {

                _next = next;

                _writer = writer;

                _executor = executor;

            }



            public async Task InvokeAsync(HttpContext httpContext, ISchema schema, IServiceProvider serviceProvider)

            {

                if (httpContext.Request.Path.StartsWithSegments("/graphql") && string.Equals(httpContext.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))

                {

                    string body;

                    using (var streamReader = new StreamReader(httpContext.Request.Body))

                    {

                        body = await streamReader.ReadToEndAsync();



                        var request = JsonConvert.DeserializeObject<GraphQLRequest>(body);



                        var result = await _executor.ExecuteAsync(doc =>

                        {

                            doc.Schema = schema;

                            doc.Query = request.Query;

                            doc.Inputs = request.Variables.ToInputs();
                            //this whole class feels so wrong just to be able to add the DataLoaderDocumentListener
                            doc.Listeners.Add(serviceProvider.GetRequiredService<DataLoaderDocumentListener>());

                        }).ConfigureAwait(false);



                        var json = await _writer.WriteToStringAsync(result);

                        await httpContext.Response.WriteAsync(json);

                    }

                }

                else

                {

                    await _next(httpContext);

                }

            }

        }



        public class GraphQLRequest

        {

            public string Query { get; set; }

            public JObject Variables { get; set; }

        }

    }
