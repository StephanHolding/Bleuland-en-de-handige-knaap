# Usage
This is a simple example of how to use the HttpClient class in C# to make HTTP requests.

## C# HTTPClient
```C#
var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "18.201.105.11:80/email/pdf");
request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzZW5kZXIiOnRydWUsImlhdCI6MTcxNTkwMzE3NH0.0g2OtjLCAcVSHJL8w_PxTL1m2hwUCNmkai7joBQ7UaA");
var content = new StringContent("{\n\t\"name\": \"Bob-test\",\n    \"email\":\"target@outlook.com\",\n    \"language\":\"en\"\n}", null, "application/json");
request.Content = content;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
Console.WriteLine(await response.Content.ReadAsStringAsync());

```

## C# RestSharp
```C#
var options = new RestClientOptions("")
{
  MaxTimeout = -1,
};
var client = new RestClient(options);
var request = new RestRequest("18.201.105.11:80/email/pdf", Method.Post);
request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzZW5kZXIiOnRydWUsImlhdCI6MTcxNTkwMzE3NH0.0g2OtjLCAcVSHJL8w_PxTL1m2hwUCNmkai7joBQ7UaA");
request.AddHeader("Content-Type", "application/json");
var body = @"{" + "\n" +
@"	""name"": ""Bob-test""," + "\n" +
@"    ""email"":""target@outlook.com""," + "\n" +
@"    ""language"":""en""" + "\n" +
@"}";
request.AddStringBody(body, DataFormat.Json);
RestResponse response = await client.ExecuteAsync(request);
Console.WriteLine(response.Content);
```


