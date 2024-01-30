

namespace ClaimManagement.Services.PolicyManagement;

public interface IPolicyManagement
{
    Task<List<PolicyModel>> GetPolicyByClaims(List<ClaimInputModel> claims);
}
public class PolicyManagement : IPolicyManagement
{
    public PolicyManagement()
    {

    }

    public async Task<List<PolicyModel>?> GetPolicyByClaims(List<ClaimInputModel> claims)
    {
        using (HttpClient client = new HttpClient())
        {
            string baseUrl = "https://localhost:7210/";

            string apiRoute = "Policy/ByNames";

            string apiUrl = baseUrl + apiRoute;

           
            // Array of names
            var names = claims.GroupBy(x=>x.PolicyName).Select(x=>x.Key);

            // Convert the array of names to JSON
            string json = JsonConvert.SerializeObject(names);

                // Create the request content
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request with the request content
           HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Check the response status code
              if (!response.IsSuccessStatusCode)
              {
                       throw new Exception($"Request failed with status code: {response.StatusCode}");

              }
                // Read the response content
                var responseBody = await response.Content.ReadAsStringAsync();
                List<PolicyModel> policyModel = JsonConvert.DeserializeObject<List<PolicyModel>>(responseBody);
                return policyModel;
                
            
        }

    }
}
